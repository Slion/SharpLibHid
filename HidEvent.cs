using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Win32;
using System.Collections.Generic;
using System.Timers;


namespace Hid
{
    /// <summary>
    /// Represent a HID event.
    /// </summary>
    public class HidEvent: IDisposable
    {
        public bool IsValid { get; private set; }
        public bool IsForeground { get; private set; }        
        public bool IsBackground { get{return !IsForeground;} }
        public bool IsMouse { get; private set; }
        public bool IsKeyboard { get; private set; }
        public bool IsGeneric { get; private set; }
        public bool IsButtonDown { get { return Usages.Count == 1 && Usages[0] != 0; } }
        public bool IsButtonUp { get { return Usages.Count == 1 && Usages[0] == 0; } }
        public bool IsRepeat { get { return RepeatCount != 0; } }
        public uint RepeatCount { get; private set; }

        public HidDevice Device { get; private set; }

        public ushort UsagePage { get; private set; }
        public ushort UsageCollection { get; private set; }
        public uint UsageId { get { return ((uint)UsagePage << 16 | (uint)UsageCollection); } }
        public List<ushort> Usages { get; private set; }        
        public delegate void HidEventRepeatDelegate(HidEvent aHidEvent);
        public event HidEventRepeatDelegate OnHidEventRepeat;

        private System.Timers.Timer Timer { get; set; }
        public DateTime Time { get; private set; }
        public DateTime OriginalTime { get; private set; }

        //Compute repeat delay and speed based on system settings
        //Those computations were taken from the Petzold here: ftp://ftp.charlespetzold.com/ProgWinForms/4%20Custom%20Controls/NumericScan/NumericScan/ClickmaticButton.cs
        private int iRepeatDelay = 250 * (1 + SystemInformation.KeyboardDelay);
        private int iRepeatSpeed = 405 - 12 * SystemInformation.KeyboardSpeed;

        /// <summary>
        /// Tells whether this event has already been disposed of.
        /// </summary>
        public bool IsStray { get { return Timer == null; } }

        /// <summary>
        /// We typically dispose of events as soon as we get the corresponding key up signal.
        /// </summary>
        public void Dispose()
        {
            Timer.Enabled = false;
            Timer.Dispose();
            //Mark this event as a stray
            Timer = null;
        }

        /// <summary>
        /// Initialize an HidEvent from a WM_INPUT message
        /// </summary>
        /// <param name="hRawInputDevice">Device Handle as provided by RAWINPUTHEADER.hDevice, typically accessed as rawinput.header.hDevice</param>
        public HidEvent(Message aMessage, HidEventRepeatDelegate aRepeatDelegate)
        {
            RepeatCount = 0;
            IsValid = false;
            IsKeyboard = false;
            IsGeneric = false;
            

            Time = DateTime.Now;
            OriginalTime = DateTime.Now;
            Timer = new System.Timers.Timer();
            Timer.Elapsed += (sender, e) => OnRepeatTimerElapsed(sender, e, this);
            Usages = new List<ushort>();
            OnHidEventRepeat += aRepeatDelegate;

            if (aMessage.Msg != Const.WM_INPUT)
            {
                //Has to be a WM_INPUT message
                return;
            }

            if (Macro.GET_RAWINPUT_CODE_WPARAM(aMessage.WParam) == Const.RIM_INPUT)
            {
                IsForeground = true;
            }
            else if (Macro.GET_RAWINPUT_CODE_WPARAM(aMessage.WParam) == Const.RIM_INPUTSINK)
            {
                IsForeground = false;
            }

            //Declare some pointers
            IntPtr rawInputBuffer = IntPtr.Zero;
            //My understanding is that this is basically our HID descriptor 
            IntPtr preParsedData = IntPtr.Zero;

            try
            {
                //Fetch raw input
                RAWINPUT rawInput = new RAWINPUT();
                if (!Win32.Utils.RawInput.GetRawInputData(aMessage.LParam, ref rawInput, ref rawInputBuffer))
                {
                    return;
                }

                //Fetch device info
                RID_DEVICE_INFO deviceInfo = new RID_DEVICE_INFO();
                if (!Win32.Utils.RawInput.GetDeviceInfo(rawInput.header.hDevice, ref deviceInfo))
                {
                    return;
                }

                //Get various information about this HID device
                Device = new Hid.HidDevice(rawInput.header.hDevice);                

                if (rawInput.header.dwType == Const.RIM_TYPEHID)  //Check that our raw input is HID                        
                {
                    IsGeneric = true;

                    Debug.WriteLine("WM_INPUT source device is HID.");
                    //Get Usage Page and Usage
                    //Debug.WriteLine("Usage Page: 0x" + deviceInfo.hid.usUsagePage.ToString("X4") + " Usage ID: 0x" + deviceInfo.hid.usUsage.ToString("X4"));
                    UsagePage = deviceInfo.hid.usUsagePage;
                    UsageCollection = deviceInfo.hid.usUsage;

                    preParsedData = Win32.Utils.RawInput.GetPreParsedData(rawInput.header.hDevice);

                    if (!(rawInput.hid.dwSizeHid > 1     //Make sure our HID msg size more than 1. In fact the first ushort is irrelevant to us for now
                        && rawInput.hid.dwCount > 0))    //Check that we have at least one HID msg
                    {
                        return;
                    }

                    //Allocate a buffer for one HID input
                    byte[] hidInputReport = new byte[rawInput.hid.dwSizeHid];

                    Debug.WriteLine("Raw input contains " + rawInput.hid.dwCount + " HID input report(s)");

                    //For each HID input report in our raw input
                    for (int i = 0; i < rawInput.hid.dwCount; i++)
                    {
                        //Compute the address from which to copy our HID input
                        int hidInputOffset = 0;
                        unsafe
                        {
                            byte* source = (byte*)rawInputBuffer;
                            source += sizeof(RAWINPUTHEADER) + sizeof(RAWHID) + (rawInput.hid.dwSizeHid * i);
                            hidInputOffset = (int)source;
                        }

                        //Copy HID input into our buffer
                        Marshal.Copy(new IntPtr(hidInputOffset), hidInputReport, 0, (int)rawInput.hid.dwSizeHid);

                        //Print HID input report in our debug output
                        string hidDump = "HID input report: ";
                        foreach (byte b in hidInputReport)
                        {
                            hidDump += b.ToString("X2");
                        }
                        Debug.WriteLine(hidDump);

                        //Proper parsing now
                        uint usageCount = 1; //Assuming a single usage per input report. Is that correct?
                        Win32.USAGE_AND_PAGE[] usages = new Win32.USAGE_AND_PAGE[usageCount];
                        Win32.HidStatus status = Win32.Function.HidP_GetUsagesEx(Win32.HIDP_REPORT_TYPE.HidP_Input, 0, usages, ref usageCount, preParsedData, hidInputReport, (uint)hidInputReport.Length);
                        if (status != Win32.HidStatus.HIDP_STATUS_SUCCESS)
                        {
                            Debug.WriteLine("Could not parse HID data!");
                        }
                        else
                        {
                            //Debug.WriteLine("UsagePage: 0x" + usages[0].UsagePage.ToString("X4"));
                            //Debug.WriteLine("Usage: 0x" + usages[0].Usage.ToString("X4"));
                            //Add this usage to our list
                            Usages.Add(usages[0].Usage);
                        }
                    }

                }
                else if (rawInput.header.dwType == Const.RIM_TYPEMOUSE)
                {
                    IsMouse = true;

                    Debug.WriteLine("WM_INPUT source device is Mouse.");                    
                    // do mouse handling...
                }
                else if (rawInput.header.dwType == Const.RIM_TYPEKEYBOARD)
                {
                    IsKeyboard = true;

                    Debug.WriteLine("WM_INPUT source device is Keyboard.");
                    // do keyboard handling...
                    Debug.WriteLine("Type: " + deviceInfo.keyboard.dwType.ToString());
                    Debug.WriteLine("SubType: " + deviceInfo.keyboard.dwSubType.ToString());
                    Debug.WriteLine("Mode: " + deviceInfo.keyboard.dwKeyboardMode.ToString());
                    Debug.WriteLine("Number of function keys: " + deviceInfo.keyboard.dwNumberOfFunctionKeys.ToString());
                    Debug.WriteLine("Number of indicators: " + deviceInfo.keyboard.dwNumberOfIndicators.ToString());
                    Debug.WriteLine("Number of keys total: " + deviceInfo.keyboard.dwNumberOfKeysTotal.ToString());
                }
            }
            finally
            {
                //Always executed when leaving our try block
                Marshal.FreeHGlobal(rawInputBuffer);
                Marshal.FreeHGlobal(preParsedData);
            }

            //
            if (IsButtonDown)
            {
                StartRepeatTimer(iRepeatDelay);
            }
            
            IsValid = true;
        }

        /// <summary>
        /// Print information about this device to our debug output.
        /// </summary>
        public void DebugWrite()
        {
            if (!IsValid)
            {
                Debug.WriteLine("==== Invalid HidEvent");
                return;
            }
            Device.DebugWrite();
            if (IsGeneric) Debug.WriteLine("==== Generic");
            if (IsKeyboard) Debug.WriteLine("==== Keyboard");
            if (IsMouse) Debug.WriteLine("==== Mouse");
            Debug.WriteLine("==== Foreground: " + IsForeground.ToString());
            Debug.WriteLine("==== UsagePage: 0x" + UsagePage.ToString("X4"));
            Debug.WriteLine("==== UsageCollection: 0x" + UsageCollection.ToString("X4"));
            foreach (ushort usage in Usages)
            {
                Debug.WriteLine("==== Usage: 0x" + usage.ToString("X4"));
            }
        }

        public void StartRepeatTimer(double aInterval)
        {
            if (Timer == null)
            {
                return;
            }
            Timer.Enabled = false;
            //Initial delay do not use auto reset
            //After our initial delay however we do setup our timer one more time using auto reset
            Timer.AutoReset = (RepeatCount!=0);
            Timer.Interval = aInterval;         
            Timer.Enabled = true;            
        }

        static private void OnRepeatTimerElapsed(object sender, ElapsedEventArgs e, HidEvent aHidEvent)
        {
            if (aHidEvent.IsStray)
            {
                //Skip events if canceled
                return;
            }

            aHidEvent.RepeatCount++;
            aHidEvent.Time = DateTime.Now;
            if (aHidEvent.RepeatCount==1)
            {
                //Re-Start our timer only after the initial delay 
                aHidEvent.StartRepeatTimer(aHidEvent.iRepeatSpeed);
            }

            //Broadcast our repeat event
            aHidEvent.OnHidEventRepeat(aHidEvent);
        }

        public ListViewItem ToListViewItem()
        {
            //TODO: What to do with multiple usage
            string usage = "";
            UsagePage usagePage = (UsagePage)UsagePage;
            switch (usagePage)
            {
                case Hid.UsagePage.Consumer:
                    usage = ((Hid.UsageTables.ConsumerControl)Usages[0]).ToString();
                    break;

                case Hid.UsagePage.WindowsMediaCenterRemoteControl:
                    usage = ((Hid.UsageTables.WindowsMediaCenterRemoteControl)Usages[0]).ToString();
                    break;

            }

            ListViewItem item = new ListViewItem(new[] { usage, UsagePage.ToString("X2"), UsageCollection.ToString("X2"), RepeatCount.ToString(), Time.ToString("HH:mm:ss:fff") });
            return item;
        }

    }

}