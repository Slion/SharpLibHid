//
// Copyright (C) 2014-2015 Stéphane Lenclud.
//
// This file is part of SharpLibHid.
//
// SharpDisplayManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SharpDisplayManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SharpDisplayManager.  If not, see <http://www.gnu.org/licenses/>.
//


using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32.SafeHandles;
using SharpLib.Win32;
using System.Collections.Generic;
using System.Timers;
using SharpLib.Hid.Usage;


namespace SharpLib.Hid
{
    /// <summary>
    /// We provide utility functions to interpret gamepad dpad state.
    /// </summary>
    public enum DirectionPadState
    {
        Rest=-1,
        Up=0,
        UpRight=1,
        Right=2,
        DownRight=3,
        Down=4,
        DownLeft=5,
        Left=6,
        UpLeft=7
    }

    /// <summary>
    /// Represent a HID event.
    /// TODO: Rename this into HidRawInput?
    /// </summary>
    public class Event : IDisposable
    {
        public bool IsValid { get; private set; }
        public bool IsForeground { get; private set; }
        public bool IsBackground { get { return !IsForeground; } }
        public bool IsMouse { get; private set; }
        public bool IsKeyboard { get; private set; }
        /// <summary>
        /// If this not a mouse or keyboard event then it's a generic HID event.
        /// </summary>
        public bool IsGeneric { get; private set; }
        public bool IsButtonDown { get { return Usages.Count == 1 && Usages[0] != 0; } }
        public bool IsButtonUp { get { return Usages.Count == 0; } }
        public bool IsRepeat { get { return RepeatCount != 0; } }
        public uint RepeatCount { get; private set; }

        public Device Device { get; private set; }
        public RAWINPUT RawInput { get {return iRawInput;} } 
        private RAWINPUT iRawInput;

        public ushort UsagePage { get; private set; }
        public ushort UsageCollection { get; private set; }
        public uint UsageId { get { return ((uint)UsagePage << 16 | (uint)UsageCollection); } }
        public List<ushort> Usages { get; private set; }
        /// <summary>
        /// Sorted in the same order as Device.InputValueCapabilities.
        /// </summary>
        public Dictionary<HIDP_VALUE_CAPS,uint> UsageValues { get; private set; }
        //TODO: We need a collection of input report
        public byte[] InputReport { get; private set; }
        //
        public delegate void HidEventRepeatDelegate(Event aHidEvent);
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
        public Event(Message aMessage, HidEventRepeatDelegate aRepeatDelegate, bool aRepeat)
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
            UsageValues = new Dictionary<HIDP_VALUE_CAPS,uint>();
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

            try
            {
                //Fetch raw input
                iRawInput = new RAWINPUT();
                if (!Win32.RawInput.GetRawInputData(aMessage.LParam, ref iRawInput, ref rawInputBuffer))
                {
                    Debug.WriteLine("GetRawInputData failed!");
                    return;
                }

                //Our device can actually be null.
                //This is notably happening for some keyboard events
                if (RawInput.header.hDevice != IntPtr.Zero)
                {
                    //Get various information about this HID device
                    Device = new Device(RawInput.header.hDevice);
                }

                if (RawInput.header.dwType == Win32.RawInputDeviceType.RIM_TYPEHID)  //Check that our raw input is HID                        
                {
                    IsGeneric = true;

                    Debug.WriteLine("WM_INPUT source device is HID.");
                    //Get Usage Page and Usage
                    //Debug.WriteLine("Usage Page: 0x" + deviceInfo.hid.usUsagePage.ToString("X4") + " Usage ID: 0x" + deviceInfo.hid.usUsage.ToString("X4"));
                    UsagePage = Device.Info.hid.usUsagePage;
                    UsageCollection = Device.Info.hid.usUsage;

                    if (!(RawInput.hid.dwSizeHid > 1     //Make sure our HID msg size more than 1. In fact the first ushort is irrelevant to us for now
                        && RawInput.hid.dwCount > 0))    //Check that we have at least one HID msg
                    {
                        return;
                    }

                    //Allocate a buffer for one HID input
                    InputReport = new byte[RawInput.hid.dwSizeHid];

                    Debug.WriteLine("Raw input contains " + RawInput.hid.dwCount + " HID input report(s)");

                    //For each HID input report in our raw input
                    for (int i = 0; i < RawInput.hid.dwCount; i++)
                    {
                        //Compute the address from which to copy our HID input
                        int hidInputOffset = 0;
                        unsafe
                        {
                            byte* source = (byte*)rawInputBuffer;
                            source += sizeof(RAWINPUTHEADER) + sizeof(RAWHID) + (RawInput.hid.dwSizeHid * i);
                            hidInputOffset = (int)source;
                        }

                        //Copy HID input into our buffer
                        Marshal.Copy(new IntPtr(hidInputOffset), InputReport, 0, (int)RawInput.hid.dwSizeHid);
                        //
                        ProcessInputReport(InputReport);
                    }
                }
                else if (RawInput.header.dwType == RawInputDeviceType.RIM_TYPEMOUSE)
                {
                    IsMouse = true;

                    Debug.WriteLine("WM_INPUT source device is Mouse.");
                    // do mouse handling...
                }
                else if (RawInput.header.dwType == RawInputDeviceType.RIM_TYPEKEYBOARD)
                {
                    IsKeyboard = true;

                    Debug.WriteLine("WM_INPUT source device is Keyboard.");
                    // do keyboard handling...
                    if (Device != null)
                    {
                        Debug.WriteLine("Type: " + Device.Info.keyboard.dwType.ToString());
                        Debug.WriteLine("SubType: " + Device.Info.keyboard.dwSubType.ToString());
                        Debug.WriteLine("Mode: " + Device.Info.keyboard.dwKeyboardMode.ToString());
                        Debug.WriteLine("Number of function keys: " + Device.Info.keyboard.dwNumberOfFunctionKeys.ToString());
                        Debug.WriteLine("Number of indicators: " + Device.Info.keyboard.dwNumberOfIndicators.ToString());
                        Debug.WriteLine("Number of keys total: " + Device.Info.keyboard.dwNumberOfKeysTotal.ToString());
                    }
                }
            }
            finally
            {
                //Always executed when leaving our try block
                Marshal.FreeHGlobal(rawInputBuffer);
            }

            //
            if (IsButtonDown && aRepeat)
            {
                //TODO: Make this optional
                StartRepeatTimer(iRepeatDelay);
            }

            IsValid = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessInputReport(byte[] aInputReport)
        {
            //Print HID input report in our debug output
            //string hidDump = "HID input report: " + InputReportString();
            //Debug.WriteLine(hidDump);

            //Get all our usages, those are typically the buttons currently pushed on a gamepad.
            //For a remote control it's usually just the one button that was pushed.
            GetUsages(aInputReport);

            //Now process direction pad (d-pad, dpad) and axes
            GetUsageValues(aInputReport);
        }

        /// <summary>
        /// Typically fetches values of a joystick/gamepad axis and dpad directions.
        /// </summary>
        /// <param name="aInputReport"></param>
        private void GetUsageValues(byte[] aInputReport)
        {
            if (Device.InputValueCapabilities == null)
            {
                return;
            }

            foreach (HIDP_VALUE_CAPS caps in Device.InputValueCapabilities)
            {                
                if (caps.IsRange)
                {
                    //What should we do with those guys?
                    continue;
                }

                //Now fetch and add our usage value
                uint usageValue = 0;
                Win32.HidStatus status = Win32.Function.HidP_GetUsageValue(Win32.HIDP_REPORT_TYPE.HidP_Input, caps.UsagePage, caps.LinkCollection, caps.NotRange.Usage, ref usageValue, Device.PreParsedData, aInputReport, (uint)aInputReport.Length);
                if (status == Win32.HidStatus.HIDP_STATUS_SUCCESS)
                {
                    UsageValues[caps]=usageValue;
                }
            }
        }

        /// <summary>
        /// Get all our usages, those are typically the buttons currently pushed on a gamepad.
        /// For a remote control it's usually just the one button that was pushed.
        /// </summary>
        private void GetUsages(byte[] aInputReport)
        {
            //Do proper parsing of our HID report
            //First query our usage count
            uint usageCount = 0;
            Win32.USAGE_AND_PAGE[] usages = null;
            Win32.HidStatus status = Win32.Function.HidP_GetUsagesEx(Win32.HIDP_REPORT_TYPE.HidP_Input, 0, usages, ref usageCount, Device.PreParsedData, aInputReport, (uint)aInputReport.Length);
            if (status == Win32.HidStatus.HIDP_STATUS_BUFFER_TOO_SMALL)
            {
                //Allocate a large enough buffer 
                usages = new Win32.USAGE_AND_PAGE[usageCount];
                //...and fetch our usages
                status = Win32.Function.HidP_GetUsagesEx(Win32.HIDP_REPORT_TYPE.HidP_Input, 0, usages, ref usageCount, Device.PreParsedData, aInputReport, (uint)aInputReport.Length);
                if (status != Win32.HidStatus.HIDP_STATUS_SUCCESS)
                {
                    Debug.WriteLine("Second pass could not parse HID data: " + status.ToString());
                }
            }
            else if (status != Win32.HidStatus.HIDP_STATUS_SUCCESS)
            {
                Debug.WriteLine("First pass could not parse HID data: " + status.ToString());
            }

            Debug.WriteLine("Usage count: " + usageCount.ToString());

            //Copy usages into this event
            if (usages != null)
            {
                foreach (USAGE_AND_PAGE up in usages)
                {
                    //Debug.WriteLine("UsagePage: 0x" + usages[0].UsagePage.ToString("X4"));
                    //Debug.WriteLine("Usage: 0x" + usages[0].Usage.ToString("X4"));
                    //Add this usage to our list
                    Usages.Add(up.Usage);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUsagePage"></param>
        /// <param name="Usage"></param>
        /// <returns></returns>
        public uint GetUsageValue(ushort aUsagePage, ushort aUsage)
        {
            foreach (HIDP_VALUE_CAPS caps in Device.InputValueCapabilities)
            {                
                if (caps.IsRange)
                {
                    //What should we do with those guys?
                    continue;
                }

                //Check if we have a match
                if (caps.UsagePage == aUsagePage && caps.NotRange.Usage == aUsage)
                {
                    return UsageValues[caps];
                }
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUsagePage"></param>
        /// <param name="aUsage"></param>
        /// <returns></returns>
        public int GetValueCapabilitiesIndex(ushort aUsagePage, ushort aUsage)
        {
            int i = -1;
            foreach (HIDP_VALUE_CAPS caps in Device.InputValueCapabilities)
            {
                i++;
                if (caps.IsRange)
                {
                    //What should we do with those guys?
                    continue;
                }

                //Check if we have a match
                if (caps.UsagePage == aUsagePage && caps.NotRange.Usage == aUsage)
                {
                    return i;
                }
            }

            return i;
        }        


        /// <summary>
        /// TODO: Move this to another level?
        /// </summary>
        /// <param name="aInterval"></param>
        public void StartRepeatTimer(double aInterval)
        {
            if (Timer == null)
            {
                return;
            }
            Timer.Enabled = false;
            //Initial delay do not use auto reset
            //After our initial delay however we do setup our timer one more time using auto reset
            Timer.AutoReset = (RepeatCount != 0);
            Timer.Interval = aInterval;
            Timer.Enabled = true;
        }

        static private void OnRepeatTimerElapsed(object sender, ElapsedEventArgs e, Event aHidEvent)
        {
            if (aHidEvent.IsStray)
            {
                //Skip events if canceled
                return;
            }

            aHidEvent.RepeatCount++;
            aHidEvent.Time = DateTime.Now;
            if (aHidEvent.RepeatCount == 1)
            {
                //Re-Start our timer only after the initial delay 
                aHidEvent.StartRepeatTimer(aHidEvent.iRepeatSpeed);
            }

            //Broadcast our repeat event
            aHidEvent.OnHidEventRepeat(aHidEvent);
        }

        /// <summary>
        /// Provide the state of the dpad or hat switch if any.
        /// If no dpad is found we return 'at rest'.
        /// </summary>
        /// <returns></returns>
        public DirectionPadState GetDirectionPadState()
        {
            int index=GetValueCapabilitiesIndex((ushort)Hid.UsagePage.GenericDesktopControls, (ushort)GenericDesktop.HatSwitch);
            if (index < 0)
            {
                //No hat switch found
                return DirectionPadState.Rest;
            }

            HIDP_VALUE_CAPS caps=Device.InputValueCapabilities[index];
            if (caps.IsRange)
            {
                //Defensive
                return DirectionPadState.Rest;
            }

            uint dpadUsageValue = UsageValues[caps]; 

            if (dpadUsageValue < caps.LogicalMin || dpadUsageValue > caps.LogicalMax)
            {
                //Out of range means at rest
                return DirectionPadState.Rest;
            }

            //Normalize value to start at zero
            //TODO: more error check here?
            DirectionPadState res = (DirectionPadState)((int)dpadUsageValue - caps.LogicalMin);            
            return res;
        }

        /// <summary>
        /// Print information about this device to our debug output.
        /// </summary>
        [Conditional("DEBUG")]
        public void DebugWrite()
        {
            if (!IsValid)
            {
                Debug.WriteLine("==== Invalid HidEvent");
                return;
            }

            if (Device!=null)
            {
                Device.DebugWrite();
            }
            
            if (IsGeneric) Debug.WriteLine("==== Generic");
            if (IsKeyboard) Debug.WriteLine("==== Keyboard");
            if (IsMouse) Debug.WriteLine("==== Mouse");
            Debug.WriteLine("==== Foreground: " + IsForeground.ToString());
            Debug.WriteLine("==== UsagePage: 0x" + UsagePage.ToString("X4"));
            Debug.WriteLine("==== UsageCollection: 0x" + UsageCollection.ToString("X4"));
            Debug.WriteLine("==== InputReport: 0x" + InputReportString());
            foreach (ushort usage in Usages)
            {
                Debug.WriteLine("==== Usage: 0x" + usage.ToString("X4"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string InputReportString()
        {
            if (InputReport == null)
            {
                return "null";
            }

            string hidDump = "";
            foreach (byte b in InputReport)
            {
                hidDump += b.ToString("X2");
            }
            return hidDump;
        }


        /// <summary>
        /// Create a list view item describing this HidEvent
        /// </summary>
        /// <returns></returns>
        public ListViewItem ToListViewItem()
        {
            string usageText = "";

            foreach (ushort usage in Usages)
            {
                if (usageText != "")
                {
                    //Add a separator
                    usageText += ", ";
                }

                UsagePage usagePage = (UsagePage)UsagePage;
                string name = Enum.GetName(Utils.UsageType(usagePage), usage);
                if (name == null || Device.IsGamePad) //Gamepad buttons do not belong to Usage enumeration, they are just ordinal
                {
                    name = usage.ToString("X2");
                }
                usageText += name;
            }

            //If we are a gamepad display axis and dpad values
            if (Device.IsGamePad)
            {
                //uint dpadUsageValue = GetUsageValue((ushort)Hid.UsagePage.GenericDesktopControls, (ushort)Hid.Usage.GenericDesktop.HatSwitch);
                //usageText = dpadUsageValue.ToString("X") + " (dpad), " + usageText;
              
                if (usageText != "")
                {
                    //Add a separator
                    usageText += " (Buttons)";
                }

                if (usageText != "")
                {
                    //Add a separator
                    usageText += ", ";
                }

                usageText += GetDirectionPadState().ToString();

                foreach (KeyValuePair<HIDP_VALUE_CAPS, uint> entry in UsageValues)
                {
                    if (entry.Key.IsRange)
                    {
                        continue;
                    }

                    Type usageType = Utils.UsageType((UsagePage)entry.Key.UsagePage);
                    if (usageType == null)
                    {
                        //TODO: check why this is happening on Logitech rumble gamepad 2.
                        //Probably some of our axis are hiding in there.
                        continue;
                    }
                    string name = Enum.GetName(usageType, entry.Key.NotRange.Usage);

                    if (usageText != "")
                    {
                        //Add a separator
                        usageText += ", ";
                    }
                    usageText += entry.Value.ToString("X") + " ("+ name +")";        
                }
            }

            //Now create our list item
            ListViewItem item = new ListViewItem(new[] { usageText, InputReportString(), UsagePage.ToString("X2"), UsageCollection.ToString("X2"), RepeatCount.ToString(), Time.ToString("HH:mm:ss:fff") });
            return item;
        }

    }

}