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

namespace SharpLib.Hid
{
    /// <summary>
    /// Represent a HID device.
    /// Rename to RawInputDevice?
    /// </summary>
    public class Device: IDisposable
    {
        /// <summary>
        /// Unique name of that HID device.
        /// Notably used as input to CreateFile.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Friendly name that people should be able to read.
        /// </summary>
        public string FriendlyName { get; private set; }

        //
        public string Manufacturer { get; private set; }
        public string Product { get; private set; }
        public ushort VendorId { get; private set; }
        public ushort ProductId { get; private set; }
        public ushort Version { get; private set; }
        //Pre-parsed HID descriptor
        public IntPtr PreParsedData {get; private set;}
        //Info
        public RID_DEVICE_INFO Info { get {return iInfo;} }
        private RID_DEVICE_INFO iInfo;
        //Capabilities
        public HIDP_CAPS Capabilities { get { return iCapabilities; } }
        private HIDP_CAPS iCapabilities;
        public string InputCapabilitiesDescription { get; private set; }
        //Input Button Capabilities
        public HIDP_BUTTON_CAPS[] InputButtonCapabilities { get { return iInputButtonCapabilities; } }
        private HIDP_BUTTON_CAPS[] iInputButtonCapabilities;
        //Input Value Capabilities
        public HIDP_VALUE_CAPS[] InputValueCapabilities { get { return iInputValueCapabilities; } }
        private HIDP_VALUE_CAPS[] iInputValueCapabilities;

        //
        public int ButtonCount { get; private set; }

        /// <summary>
        /// Class constructor will fetch this object properties from HID sub system.
        /// </summary>
        /// <param name="hRawInputDevice">Device Handle as provided by RAWINPUTHEADER.hDevice, typically accessed as rawinput.header.hDevice</param>
        public Device(IntPtr hRawInputDevice)
        {
            //Try construct and rollback if needed
            try
            {
                Construct(hRawInputDevice);
            }
            catch (System.Exception ex)
            {
                //Just rollback and propagate
                Dispose();
                throw ex;
            }
        }


        /// <summary>
        /// Make sure dispose is called even if the user forgot about it.
        /// </summary>
        ~Device()
        {
            Dispose();
        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="hRawInputDevice"></param>
        private void Construct(IntPtr hRawInputDevice)
        {
            PreParsedData = IntPtr.Zero;
            iInputButtonCapabilities = null;
            iInputValueCapabilities = null;

            //Fetch various information defining the given HID device
            Name = Win32.RawInput.GetDeviceName(hRawInputDevice);

            //Fetch device info
            iInfo = new RID_DEVICE_INFO();
            if (!Win32.RawInput.GetDeviceInfo(hRawInputDevice, ref iInfo))
            {
                throw new Exception("HidDevice: GetDeviceInfo failed: " + Marshal.GetLastWin32Error().ToString());
            }

            //Open our device from the device name/path
            SafeFileHandle handle = Win32.Function.CreateFile(Name,
                Win32.FileAccess.NONE,
                Win32.FileShare.FILE_SHARE_READ | Win32.FileShare.FILE_SHARE_WRITE,
                IntPtr.Zero,
                Win32.CreationDisposition.OPEN_EXISTING,
                Win32.FileFlagsAttributes.FILE_FLAG_OVERLAPPED,
                IntPtr.Zero
                );

            //Check if CreateFile worked
            if (handle.IsInvalid)
            {
                throw new Exception("HidDevice: CreateFile failed: " + Marshal.GetLastWin32Error().ToString());
            }

            //Get manufacturer string
            StringBuilder manufacturerString = new StringBuilder(256);
            if (Win32.Function.HidD_GetManufacturerString(handle, manufacturerString, manufacturerString.Capacity))
            {
                Manufacturer = manufacturerString.ToString();
            }

            //Get product string
            StringBuilder productString = new StringBuilder(256);
            if (Win32.Function.HidD_GetProductString(handle, productString, productString.Capacity))
            {
                Product = productString.ToString();
            }

            //Get attributes
            Win32.HIDD_ATTRIBUTES attributes = new Win32.HIDD_ATTRIBUTES();
            if (Win32.Function.HidD_GetAttributes(handle, ref attributes))
            {
                VendorId = attributes.VendorID;
                ProductId = attributes.ProductID;
                Version = attributes.VersionNumber;
            }

            handle.Close();

            SetFriendlyName();            

            //Get our HID descriptor pre-parsed data
            PreParsedData = Win32.RawInput.GetPreParsedData(hRawInputDevice);

            if (PreParsedData == IntPtr.Zero)
            {
                //We are done then.
                //Some devices don't have pre-parsed data.
                return;
            }

            //Get capabilities
            HidStatus status = Win32.Function.HidP_GetCaps(PreParsedData, ref iCapabilities);
            if (status != HidStatus.HIDP_STATUS_SUCCESS)
            {
                throw new Exception("HidDevice: HidP_GetCaps failed: " + status.ToString());
            }

            SetInputCapabilitiesDescription();

            //Get input button caps if needed
            if (Capabilities.NumberInputButtonCaps > 0)
            {
                iInputButtonCapabilities = new HIDP_BUTTON_CAPS[Capabilities.NumberInputButtonCaps];
                ushort buttonCapabilitiesLength = Capabilities.NumberInputButtonCaps;
                status = Win32.Function.HidP_GetButtonCaps(HIDP_REPORT_TYPE.HidP_Input, iInputButtonCapabilities, ref buttonCapabilitiesLength, PreParsedData);
                if (status != HidStatus.HIDP_STATUS_SUCCESS || buttonCapabilitiesLength != Capabilities.NumberInputButtonCaps)
                {
                    throw new Exception("HidDevice: HidP_GetButtonCaps failed: " + status.ToString());
                }

                ComputeButtonCount();
            }

            //Get input value caps if needed
            if (Capabilities.NumberInputValueCaps > 0)
            {
                iInputValueCapabilities = new HIDP_VALUE_CAPS[Capabilities.NumberInputValueCaps];
                ushort valueCapabilitiesLength = Capabilities.NumberInputValueCaps;
                status = Win32.Function.HidP_GetValueCaps(HIDP_REPORT_TYPE.HidP_Input, iInputValueCapabilities, ref valueCapabilitiesLength, PreParsedData);
                if (status != HidStatus.HIDP_STATUS_SUCCESS || valueCapabilitiesLength != Capabilities.NumberInputValueCaps)
                {
                    throw new Exception("HidDevice: HidP_GetValueCaps failed: " + status.ToString());
                }
            }
        }


        /// <summary>
        /// Useful for gamepads.
        /// </summary>
        void ComputeButtonCount()
        {
            ButtonCount = 0;
            foreach (HIDP_BUTTON_CAPS bc in iInputButtonCapabilities)
            {
                if (bc.IsRange)
                {
                    ButtonCount += (bc.Range.UsageMax - bc.Range.UsageMin + 1);
                }
            }            
        }


        /// <summary>
        /// 
        /// </summary>
        void SetInputCapabilitiesDescription()
        {
            InputCapabilitiesDescription = "[ Input Capabilities ] Button: " + Capabilities.NumberInputButtonCaps + " - Value: " + Capabilities.NumberInputValueCaps + " - Data indices: " + Capabilities.NumberInputDataIndices;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetFriendlyName()
        {
            //Work out proper suffix for our device root node.
            //That allows users to see in a glance what kind of device this is.
            string suffix = "";
            Type usageCollectionType = null;
            if (Info.dwType == RawInputDeviceType.RIM_TYPEHID)
            {
                //Process usage page
                if (Enum.IsDefined(typeof(UsagePage), Info.hid.usUsagePage))
                {
                    //We know this usage page, add its name
                    UsagePage usagePage = (UsagePage)Info.hid.usUsagePage;
                    suffix += " ( " + usagePage.ToString() + ", ";
                    usageCollectionType = Utils.UsageCollectionType(usagePage);
                }
                else
                {
                    //We don't know this usage page, add its value
                    suffix += " ( 0x" + Info.hid.usUsagePage.ToString("X4") + ", ";
                }

                //Process usage collection
                //We don't know this usage page, add its value
                if (usageCollectionType == null || !Enum.IsDefined(usageCollectionType, Info.hid.usUsage))
                {
                    //Show Hexa
                    suffix += "0x" + Info.hid.usUsage.ToString("X4") + " )";
                }
                else
                {
                    //We know this usage page, add its name
                    suffix += Enum.GetName(usageCollectionType, Info.hid.usUsage) + " )";
                }
            }
            else if (Info.dwType == RawInputDeviceType.RIM_TYPEKEYBOARD)
            {
                suffix = " - Keyboard";
            }
            else if (Info.dwType == RawInputDeviceType.RIM_TYPEMOUSE)
            {
                suffix = " - Mouse";
            }

            if (Product != null && Product.Length > 1)
            {
                //This device as a proper name, use it
                FriendlyName = Product + suffix;
            }
            else
            {   
                //Extract friendly name from name
                char[] delimiterChars = { '#', '&'};
                string[] words = Name.Split(delimiterChars);
                if (words.Length >= 2)
                {
                    //Use our name sub-string to describe this device
                    FriendlyName = words[1] + " - 0x" + ProductId.ToString("X4") + suffix;
                }
                else
                {
                    //No proper name just use the device ID instead
                    FriendlyName = "0x" + ProductId.ToString("X4") + suffix;
                }
            }

        }

        /// <summary>
        /// Dispose is just for unmanaged clean-up.
        /// Make sure calling disposed multiple times does not crash.
        /// See: http://stackoverflow.com/questions/538060/proper-use-of-the-idisposable-interface/538238#538238
        /// </summary>
        public void Dispose()
        {
            Marshal.FreeHGlobal(PreParsedData);
            PreParsedData = IntPtr.Zero;
        }

        /// <summary>
        /// Provide a description for the given capabilities.
        /// Notably describes axis on a gamepad/joystick.
        /// </summary>
        /// <param name="aCaps"></param>
        /// <returns></returns>
        static public string InputValueCapabilityDescription(HIDP_VALUE_CAPS aCaps)
        {
            if (!aCaps.IsRange && Enum.IsDefined(typeof(UsagePage), aCaps.UsagePage))
            {
                Type usageType = Utils.UsageType((UsagePage)aCaps.UsagePage);
                if (usageType==null)
                {
                    return "Input Value: " + Enum.GetName(typeof(UsagePage), aCaps.UsagePage) + " Usage 0x" + aCaps.NotRange.Usage.ToString("X2");
                }
                string name = Enum.GetName(usageType, aCaps.NotRange.Usage);
                if (name == null)
                {
                    //Could not find that usage in our enum.
                    //Provide a relevant warning instead.
                    name = "Usage 0x" + aCaps.NotRange.Usage.ToString("X2") + " not defined in " + usageType.Name;
                }
                else
                {
                    //Prepend our usage type name
                    name = usageType.Name + "." + name;
                }
                return "Input Value: " + name;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsGamePad
        {
            get
            {
                return ((UsagePage)iCapabilities.UsagePage == Hid.UsagePage.GenericDesktopControls && (UsageCollection.GenericDesktop)iCapabilities.Usage == Hid.UsageCollection.GenericDesktop.GamePad);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMouse
        {
            get
            {
                return Info.dwType == RawInputDeviceType.RIM_TYPEMOUSE;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsKeyboard
        {
            get
            {
                return Info.dwType == RawInputDeviceType.RIM_TYPEKEYBOARD;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsHid
        {
            get
            {
                return Info.dwType == RawInputDeviceType.RIM_TYPEHID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ushort UsagePage
        {
            get
            {
                if (Info.dwType == RawInputDeviceType.RIM_TYPEHID)
                {
                    //Generic HID
                    return Info.hid.usUsagePage;
                }
                else if (Info.dwType == RawInputDeviceType.RIM_TYPEKEYBOARD)
                {
                    //Keyboard
                    return (ushort)Hid.UsagePage.GenericDesktopControls;
                }
                else if (Info.dwType == RawInputDeviceType.RIM_TYPEMOUSE)
                {
                    //Mouse
                    return (ushort)Hid.UsagePage.GenericDesktopControls;
                }

                //We should never get there
                Debug.Assert(false);
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ushort UsageCollection
        {
            get
            {
                if (Info.dwType == RawInputDeviceType.RIM_TYPEHID)
                {
                    //Generic HID
                    return Info.hid.usUsage;
                }
                else if (Info.dwType == RawInputDeviceType.RIM_TYPEKEYBOARD)
                {
                    //Keyboard
                    return (ushort)Hid.UsageCollection.GenericDesktop.Keyboard;
                }
                else if (Info.dwType == RawInputDeviceType.RIM_TYPEMOUSE)
                {
                    //Mouse
                    return (ushort)Hid.UsageCollection.GenericDesktop.Mouse;
                }

                //We should never get there
                Debug.Assert(false);
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint UsageId
        {
            get
            {
                return (uint)(UsagePage<<16) | UsageCollection ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        override public string ToString()
        {
            string res = "";
            res += "==================== HID Device ====================" + "\r\n";
            res += "==== Name: " + Name + "\r\n";
            res += "==== Manufacturer: " + Manufacturer + "\r\n";
            res += "==== Product: " + Product + "\r\n";
            res += "==== VendorID: 0x" + VendorId.ToString("X4") + "\r\n";
            res += "==== ProductID: 0x" + ProductId.ToString("X4") + "\r\n";
            res += "==== Version: " + Version.ToString() + "\r\n";
            res += "====================================================" + "\r\n";
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

    }

}