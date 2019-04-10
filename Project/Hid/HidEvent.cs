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
using System.ComponentModel;
using System.Timers;
using SharpLib.Hid.Usage;
using System.Reflection;

namespace SharpLib.Hid
{
    /// <summary>
    /// We provide utility functions to interpret gamepad dpad state.
    /// </summary>
    public enum DirectionPadState
    {
        Rest = -1,
        Up = 0,
        UpRight = 1,
        Right = 2,
        DownRight = 3,
        Down = 4,
        DownLeft = 5,
        Left = 6,
        UpLeft = 7
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

        public Keys VirtualKey {
            get { return (Keys) RawInput.data.keyboard.VKey; }
        }

        public bool HasModifierShift;
        public bool HasModifierControl;
        public bool HasModifierAlt;
        public bool HasModifierWindows;

        /// <summary>
        /// Tells whether this event is a SHIFT modifier.
        /// </summary>
        public bool IsModifierShift
        {
            get
            {
                return IsKeyboard &&
                       (VirtualKey == Keys.ShiftKey || VirtualKey == Keys.LShiftKey || VirtualKey == Keys.RShiftKey);
            }
        }

        /// <summary>
        /// Tells whether this event is a CONTROL modifier.
        /// </summary>
        public bool IsModifierControl
        {
            get
            {
                return IsKeyboard &&
                       (VirtualKey == Keys.ControlKey || VirtualKey == Keys.LControlKey ||
                        VirtualKey == Keys.RControlKey);
            }
        }

        /// <summary>
        /// Tells whether this event is a ALT modifier.
        /// </summary>
        public bool IsModifierAlt
        {
            get
            {
                return IsKeyboard && (VirtualKey == Keys.Menu || VirtualKey == Keys.LMenu || VirtualKey == Keys.RMenu);
            }
        }

        /// <summary>
        /// Tells whether this event is a WINDOWS modifier.
        /// </summary>
        public bool IsModifierWindows
        {
            get { return IsKeyboard && (VirtualKey == Keys.LWin || VirtualKey == Keys.RWin); }
        }

        /// <summary>
        /// Tells whether this is a modifier key.
        /// </summary>
        public bool IsModifier
        {
            get { return IsModifierShift || IsModifierControl || IsModifierAlt || IsModifierWindows; }
        }

        /// <summary>
        /// Check if this event correspond to a button or a key being pushed down.
        /// </summary>
        public bool IsButtonDown
        {
            get
            {
                if (IsGeneric)
                {
                    // For generic HID device check that our first usage is not zero
                    return Usages.Count == 1 && Usages[0] != 0;
                }
                else if (IsKeyboard)
                {
                    return !IsButtonUp;
                }
                //TODO: mouse handling
                Debug.Assert(false);
                return false;
            }
        }

        /// <summary>
        /// Check if this event correspond to a button or a key being released.
        /// </summary>
        public bool IsButtonUp
        {
            get
            {
                if (IsGeneric)
                {
                    // Button up event if we do not have any usages
                    return Usages.Count == 0;
                } 
                else if (IsKeyboard)
                {
                    // This is a key up event if our break flag is set
                    return RawInput.data.keyboard.Flags.HasFlag(RawInputKeyFlags.RI_KEY_BREAK);
                }
                //TODO: mouse handling
                Debug.Assert(false);
                return false;
            }
        }

        public bool IsRepeat { get { return RepeatCount != 0; } }
        public uint RepeatCount { get; set; }

        /// <summary>
        /// Uniquely identify keyboard events.
        /// Key down and up event will return the same ID.
        /// </summary>
        public ulong KeyId
        {
            get
            {
                return (ulong)RawInput.data.keyboard.VKey << 32 | (ulong)RawInput.data.keyboard.MakeCode << 16 | (ulong)(RawInput.data.keyboard.Flags & ~RawInputKeyFlags.RI_KEY_BREAK);
            }
        }

        public Device Device { get; private set; }
        public RAWINPUT RawInput { get {return iRawInput;} } 
        private RAWINPUT iRawInput;

        /// <summary>
        /// Usage Page
        /// </summary>
        public ushort UsagePage { get; private set; }

        /// <summary>
        /// Usage Page as enumeration.
        /// </summary>
        public UsagePage UsagePageEnum {get { return (UsagePage) UsagePage; } }

        /// <summary>
        /// Provides the name of our usage page.
        /// </summary>
        public string UsagePageName()
        {            
            return UsagePageEnum.ToString();
        }

        /// <summary>
        /// Provides name and value of our usage page as a string.
        /// </summary>
        public string UsagePageNameAndValue()
        {
            return string.Format("{0} (0x{1})", UsagePageName(), UsagePage.ToString("X4"));
        }

        /// <summary>
        /// Usage Collection
        /// </summary>
        public ushort UsageCollection { get; private set; }

        /// <summary>
        /// Provides the name of our usage collection.
        /// </summary>
        public string UsageCollectionName()
        {
                Type collectionType = Utils.UsageCollectionType(UsagePageEnum);
                return Enum.GetName(collectionType, UsageCollection);                
        }

        /// <summary>
        /// Provides name and value of our usage collection as a string.
        /// </summary>
        public string UsageCollectionNameAndValue()
        {
                return string.Format("{0} (0x{1})", UsageCollectionName(), UsageCollection.ToString("X4"));
        }

        public List<ushort> Usages { get; private set; }

        /// <summary>
        /// Provides name of the usage at the given index.
        /// </summary>
        /// <param name="aIndex">Index of the usage concerned.</param>
        /// <returns></returns>
        public string UsageName(int aIndex)
        {
            Type usageType = Utils.UsageType(UsagePageEnum);
            return Enum.GetName(usageType, Usages[aIndex]);
        }

        /// <summary>
        /// Provides name and value of the usage at the given index.
        /// </summary>
        /// <param name="aIndex">Index of the usage concerned.</param>
        /// <returns></returns>
        public string UsageNameAndValue(int aIndex)
        {
            return string.Format("{0} (0x{1})", UsageName(aIndex), Usages[aIndex].ToString("X4"));
        }

        public uint UsageId { get { return ((uint)UsagePage << 16 | (uint)UsageCollection); } }
        
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
        private int iRepeatDelayInMs = 250 * (1 + SystemInformation.KeyboardDelay);
        private int iRepeatSpeedInMs = 405 - 12 * SystemInformation.KeyboardSpeed;

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
        /// <param name="aMessage"></param>
        /// <param name="aRepeatDelegate"></param>
        /// <param name="aRepeat"></param>
        /// <param name="aRepeatDelayInMs"></param>
        /// <param name="aRepeatSpeedInMs"></param>
        public Event(Message aMessage, HidEventRepeatDelegate aRepeatDelegate, bool aRepeat, int aRepeatDelayInMs=-1, int aRepeatSpeedInMs=-1 )
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

            //Use custom repeat delay if specified
            if (aRepeatDelayInMs >= 0)
            {
                iRepeatDelayInMs = aRepeatDelayInMs;
            }

            //Use custom repeat speed if specified
            if (aRepeatSpeedInMs >= 0)
            {
                iRepeatSpeedInMs = aRepeatSpeedInMs;
            }

            //
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

                    if (!(RawInput.data.hid.dwSizeHid > 1     //Make sure our HID msg size more than 1. In fact the first ushort is irrelevant to us for now
                        && RawInput.data.hid.dwCount > 0))    //Check that we have at least one HID msg
                    {
                        return;
                    }

                    //Allocate a buffer for one HID input
                    InputReport = new byte[RawInput.data.hid.dwSizeHid];

                    Debug.WriteLine("Raw input contains " + RawInput.data.hid.dwCount + " HID input report(s)");

                    //For each HID input report in our raw input
                    for (int i = 0; i < RawInput.data.hid.dwCount; i++)
                    {
                        //Compute the address from which to copy our HID input
                        int hidInputOffset = 0;
                        unsafe
                        {
                            byte* source = (byte*)rawInputBuffer;
                            source += sizeof(RAWINPUTHEADER) + sizeof(RAWHID) + (RawInput.data.hid.dwSizeHid * i);
                            hidInputOffset = (int)source;
                        }
                        
                        //Copy HID input into our buffer
                        Marshal.Copy(new IntPtr(hidInputOffset), InputReport, 0, (int)RawInput.data.hid.dwSizeHid);
                        //
                        ProcessInputReport(InputReport);
                    }
                }
                else if (RawInput.header.dwType == RawInputDeviceType.RIM_TYPEMOUSE)
                {
                    IsMouse = true;
                    UsagePage = (ushort)Hid.UsagePage.GenericDesktopControls;
                    UsageCollection = (ushort)Hid.UsageCollection.GenericDesktop.Mouse;

                    Debug.WriteLine("WM_INPUT source device is Mouse.");
                    // do mouse handling...
                }
                else if (RawInput.header.dwType == RawInputDeviceType.RIM_TYPEKEYBOARD)
                {
                    IsKeyboard = true;
                    UsagePage = (ushort)Hid.UsagePage.GenericDesktopControls;
                    UsageCollection = (ushort)Hid.UsageCollection.GenericDesktop.Keyboard;

                    // Precise ALT key - work out if we are left or right ALT
                    if (iRawInput.data.keyboard.VKey == (ushort) Keys.Menu)
                    {
                        if (RawInput.data.keyboard.Flags.HasFlag(RawInputKeyFlags.RI_KEY_E0))
                        {
                            iRawInput.data.keyboard.VKey = (ushort) Keys.RMenu;
                        }
                        else
                        {
                            iRawInput.data.keyboard.VKey = (ushort) Keys.LMenu;
                        }
                    }

                    // Precise CTRL key - work out if we are left or right CTRL
                    if (iRawInput.data.keyboard.VKey == (ushort)Keys.ControlKey)
                    {
                        if (RawInput.data.keyboard.Flags.HasFlag(RawInputKeyFlags.RI_KEY_E0))
                        {
                            iRawInput.data.keyboard.VKey = (ushort)Keys.RControlKey;
                        }
                        else
                        {
                            iRawInput.data.keyboard.VKey = (ushort)Keys.LControlKey;
                        }
                    }

                    // Precise SHIFT key - work out if we are left or right SHIFT
                    if (iRawInput.data.keyboard.VKey == (ushort)Keys.ShiftKey)
                    {
                        if (RawInput.data.keyboard.MakeCode == 0x0036)
                        {
                            iRawInput.data.keyboard.VKey = (ushort)Keys.RShiftKey;
                        }
                        else
                        {
                            Debug.Assert(RawInput.data.keyboard.MakeCode == 0x002A);
                            iRawInput.data.keyboard.VKey = (ushort)Keys.LShiftKey;
                        }

                    }


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

            //We don't want to repeat every events. Key up events for instance must not be repeated.
            if ( aRepeat &&         // If repeat is enabled
                 IsGeneric &&       // Only generic HID event need repeat management, keyboards repeats are managed by the drivers
                (IsButtonDown ||    // and if we are dealing with a button down event
                (GetDirectionPadState() != DirectionPadState.Rest))) //or our dpad is not at rest
            {
                StartRepeatTimer(iRepeatDelayInMs);
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
            //Make sure we have a device with input value capabilities
            if (Device == null || Device.InputValueCapabilities == null)
            {
                return i;
            }

            //Search our value capabilities for the first one matching usage and usage page
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
        /// Start the repeat timer for this event. 
        /// </summary>
        /// <param name="aInterval">The time, in milliseconds, after which the timer elapsed event is triggered.</param>
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

        /// <summary>
        /// Callback from our repeat timer.
        /// </summary>
        /// <seealso cref="System.Timers.ElapsedEventHandler"/>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.Timers.ElapsedEventArgs object that contains the event data.</param>
        /// <param name="aHidEvent">The HID Event from which our repeat is coming from.</param>
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
                aHidEvent.StartRepeatTimer(aHidEvent.iRepeatSpeedInMs);
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
            Debug.Write(ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        override public string ToString()
        {
            string res = "";            
            if (!IsValid)
            {
                res += "HID Event Invalid";
                return res;
            }

            res += "HID Event";
            if (IsButtonDown) res += ", DOWN";
            if (IsButtonUp) res += ", UP";

            if (IsGeneric)
            {
                res += ", Generic";
                for (int i = 0; i < Usages.Count; i++)
                {
                    res += ", Usage: " + UsageNameAndValue(i);
                }
                res += ", UsagePage: " + UsagePageNameAndValue();
                res += ", UsageCollection: " + UsageCollectionNameAndValue();
                res += ", Input Report: 0x" + InputReportString();
            }
            else if (IsKeyboard)
            {
                res += ", Keyboard";
                res += ", Virtual Key: " + VirtualKey;
            }
            else if (IsMouse)
            {
                res += ", Mouse";
            }

            if (IsBackground) res += ", Background";
            if (IsRepeat) res += ", Repeat: " + RepeatCount;

            return res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToLog(uint aDepth=0)
        {
            string res = "";
            if (!IsValid)
            {
                res += "WARNING: Invalid HID Event";
            }

            //Compute our prefix and indent
            const string KPrefix = "   ";
            string prefix = "";
            for (uint i = 0; i<aDepth; i++)
            {
                prefix += KPrefix;
            }
            string indent = prefix + KPrefix;
            //

            //Open our HID Event block
            res += "[HID Event]" + "\r\n";

            // Log our debug string too to give a quick overview
            res += indent + "String: " + ToString() + "\r\n";

            //For each properties of our HID Event
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this))
            {
                string propertyName = property.Name;
                object value = property.GetValue(this);

                if (value != null)
                {
                    //Check if our property value object type has a method of the same name has this one.
                    Type type = value.GetType();
                    MethodInfo method = type.GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (method != null && method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == aDepth.GetType())
                    {
                        object[] parameters = new object[] { aDepth+1 };
                        res += method.Invoke(value, parameters);
                    }
                    //Make special case for some properties
                    else if (propertyName.Equals("InputReport"))
                    {
                        res += string.Format("{0}{1}: {2}\r\n", indent, propertyName, InputReportString());
                    }
                    else if (propertyName.Equals("UsagePage"))
                    {
                        res += string.Format("{0}{1}: {2} (0x{3})\r\n", indent, propertyName, UsagePageEnum, UsagePage.ToString("X4"));
                    }
                    else if (propertyName.Equals("UsageCollection"))
                    {
                        res += string.Format("{0}{1}: {2}\r\n", indent, propertyName, UsageCollectionNameAndValue());
                    }
                    else if (propertyName.Equals("Usages"))
                    {
                        for (int i=0;i<Usages.Count;i++)
                        {                            
                            res += string.Format("{0}{1}: {2}\r\n", indent, propertyName, UsageNameAndValue(i));
                        }
                    }
                    else if (propertyName.Equals("UsageValues"))
                    {
                        //Just give the count
                        res += string.Format("{0}{1}: {2}\r\n", indent, "UsageValues count", UsageValues.Count);
                    }
                    else if (   propertyName.Equals("RawInput")
                                || propertyName.Equals("UsagePageEnum"))
                    {
                        //Skip those
                    }
                    else
                    {
                        res += string.Format("{0}{1}: {2}\r\n", indent, propertyName, value);
                    }
                }
                else
                {
                    // Special case for NULL values
                    res += string.Format("{0}{1}: NULL\r\n", indent, propertyName);
                }
            }
            //Close our HID Event block
            res += "[/HID Event]" + "\r\n";
            return res;
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
            string inputReport = null;

            foreach (ushort usage in Usages)
            {
                if (usageText != "")
                {
                    //Add a separator
                    usageText += ", ";
                }

                //Try to get a name for that usage
                string name = "";
                if (Enum.IsDefined(typeof(Hid.UsagePage), UsagePage))
                {
                    UsagePage usagePage = (UsagePage)UsagePage;

                    try
                    {
                        name = Enum.GetName(Utils.UsageType(usagePage), usage);
                    }
                    catch
                    {

                    }
                }

                if (name == null || name.Equals("") || Device.IsGamePad) //Gamepad buttons do not belong to Usage enumeration, they are just ordinal
                {
                    name = usage.ToString("X2");
                }
                usageText += name;
            }

            // Get input report for generic HID events
            if (IsGeneric)
            {
                inputReport = InputReportString();
            }

            //If we are a gamepad display axis and dpad values
            if (Device != null && Device.IsGamePad)
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

                //For each axis
                foreach (KeyValuePair<HIDP_VALUE_CAPS, uint> entry in UsageValues)
                {
                    if (entry.Key.IsRange)
                    {
                        continue;
                    }

                    //Get our usage type
                    Type usageType = Utils.UsageType((UsagePage)entry.Key.UsagePage);
                    if (usageType == null)
                    {
                        //Unknown usage type
                        //TODO: check why this is happening on Logitech rumble gamepad 2.
                        //Probably some of our axis are hiding in there.
                        continue;
                    }

                    //Get the name of our axis
                    string name = Enum.GetName(usageType, entry.Key.NotRange.Usage);

                    if (usageText != "")
                    {
                        //Add a separator
                        usageText += ", ";
                    }
                    usageText += entry.Value.ToString("X") + " ("+ name +")";        
                }
            }
            //Handle keyboard events
            else if (IsKeyboard)
            {
                //Get the virtual key
                System.Windows.Forms.Keys vKey = (Keys)RawInput.data.keyboard.VKey;
                usageText = vKey.ToString() + " -";

                //Get the key flag
                if (IsButtonUp)
                {
                    usageText += " UP";
                }
                else if (IsButtonDown)
                {
                    usageText += " DOWN";
                }

                if (RawInput.data.keyboard.Flags.HasFlag(RawInputKeyFlags.RI_KEY_E0))
                {
                    usageText += " E0";
                }

                if (RawInput.data.keyboard.Flags.HasFlag(RawInputKeyFlags.RI_KEY_E1))
                {
                    usageText += " E1";
                }

                if (HasModifierShift)
                {
                    usageText += " SHIFT";
                }

                if (HasModifierControl)
                {
                    usageText += " CTRL";
                }

                if (HasModifierAlt)
                {
                    usageText += " ALT";
                }

                if (HasModifierWindows)
                {
                    usageText += " WIN";
                }


                //Put our scan code into our input report field
                inputReport = "0x" + RawInput.data.keyboard.MakeCode.ToString("X4");
            }

            //Now create our list item
            ListViewItem item = new ListViewItem(new[] { usageText, inputReport, UsagePageNameAndValue(), UsageCollectionNameAndValue(), RepeatCount.ToString(), Time.ToString("HH:mm:ss:fff"), IsBackground.ToString()});
            return item;
        }

    }

}