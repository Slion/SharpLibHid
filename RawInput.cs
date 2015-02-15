using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;


namespace Win32
{
    /// <summary>
    /// Provide some utility functions for raw input handling.
    /// </summary>
    static class RawInput
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aRawInputHandle"></param>
        /// <param name="aRawInput"></param>
        /// <param name="rawInputBuffer">Caller must free up memory on the pointer using Marshal.FreeHGlobal</param>
        /// <returns></returns>
        public static bool GetRawInputData(IntPtr aRawInputHandle, ref RAWINPUT aRawInput, ref IntPtr rawInputBuffer)
        {
            bool success = true;
            rawInputBuffer = IntPtr.Zero;

            try
            {
                uint dwSize = 0;
                uint sizeOfHeader = (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER));

                //Get the size of our raw input data.
                Win32.Function.GetRawInputData(aRawInputHandle, Const.RID_INPUT, IntPtr.Zero, ref dwSize, sizeOfHeader);

                //Allocate a large enough buffer
                 rawInputBuffer = Marshal.AllocHGlobal((int)dwSize);

                //Now read our RAWINPUT data
                if (Win32.Function.GetRawInputData(aRawInputHandle, Const.RID_INPUT, rawInputBuffer, ref dwSize, (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) != dwSize)
                {
                    return false;
                }

                //Cast our buffer
                aRawInput = (RAWINPUT)Marshal.PtrToStructure(rawInputBuffer, typeof(RAWINPUT));
            }
            catch
            {
                Debug.WriteLine("GetRawInputData failed!");
                success = false;
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDevice"></param>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        public static bool GetDeviceInfo(IntPtr hDevice, ref RID_DEVICE_INFO deviceInfo)
        {
            bool success = true;
            IntPtr deviceInfoBuffer = IntPtr.Zero;
            try
            {
                //Get Device Info
                uint deviceInfoSize = (uint)Marshal.SizeOf(typeof(RID_DEVICE_INFO));
                deviceInfoBuffer = Marshal.AllocHGlobal((int)deviceInfoSize);

                int res = Win32.Function.GetRawInputDeviceInfo(hDevice, Win32.RawInputDeviceInfoType.RIDI_DEVICEINFO, deviceInfoBuffer, ref deviceInfoSize);
                if (res <= 0)
                {
                    Debug.WriteLine("WM_INPUT could not read device info: " + Marshal.GetLastWin32Error().ToString());
                    return false;
                }

                //Cast our buffer
                deviceInfo = (RID_DEVICE_INFO)Marshal.PtrToStructure(deviceInfoBuffer, typeof(RID_DEVICE_INFO));
            }
            catch
            {
                Debug.WriteLine("GetRawInputData failed!");
                success = false;
            }
            finally
            {
                //Always executes, prevents memory leak
                Marshal.FreeHGlobal(deviceInfoBuffer);
            }

            
            return success;
        }

        /// <summary>
        /// Fetch pre-parsed data corresponding to HID descriptor for the given HID device.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static IntPtr GetPreParsedData(IntPtr hDevice)
        {
            uint ppDataSize = 0;
            int result = Win32.Function.GetRawInputDeviceInfo(hDevice, RawInputDeviceInfoType.RIDI_PREPARSEDDATA, IntPtr.Zero, ref ppDataSize);
            if (result != 0)
            {
                Debug.WriteLine("Failed to get raw input pre-parsed data size" + result + Marshal.GetLastWin32Error());
                return IntPtr.Zero;
            }

            IntPtr ppData = Marshal.AllocHGlobal((int)ppDataSize);
            result = Win32.Function.GetRawInputDeviceInfo(hDevice, RawInputDeviceInfoType.RIDI_PREPARSEDDATA, ppData, ref ppDataSize);
            if (result <= 0)
            {
                Debug.WriteLine("Failed to get raw input pre-parsed data" + result + Marshal.GetLastWin32Error());
                return IntPtr.Zero;
            }
            return ppData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static string GetDeviceName(IntPtr device)
        {
            uint deviceNameSize = 256;
            int result = Win32.Function.GetRawInputDeviceInfo(device, RawInputDeviceInfoType.RIDI_DEVICENAME, IntPtr.Zero, ref deviceNameSize);
            if (result != 0)
            {
                return string.Empty;
            }

            IntPtr deviceName = Marshal.AllocHGlobal((int)deviceNameSize * 2);  // size is the character count not byte count
            try
            {
                result = Win32.Function.GetRawInputDeviceInfo(device, RawInputDeviceInfoType.RIDI_DEVICENAME, deviceName, ref deviceNameSize);
                if (result > 0)
                {
                    return Marshal.PtrToStringAnsi(deviceName, result - 1); // -1 for NULL termination
                }

                return string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(deviceName);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aTreeView"></param>
        public static void PopulateDeviceList(TreeView aTreeView)
        {

            //Get our list of devices
            RAWINPUTDEVICELIST[] ridList = null;
            uint deviceCount = 0;
            int res = Win32.Function.GetRawInputDeviceList(ridList, ref deviceCount,(uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            if (res == -1)
            {
                //Just give up then
                return;
            }

            ridList = new RAWINPUTDEVICELIST[deviceCount];
            res = Win32.Function.GetRawInputDeviceList(ridList, ref deviceCount, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            if (res != deviceCount)
            {
                //Just give up then
                return;
            }

            //For each our device add a node to our treeview
            foreach (RAWINPUTDEVICELIST device in ridList)
            {
                Hid.HidDevice hidDevice=new Hid.HidDevice(device.hDevice);

                //Work out proper suffix for our device root node.
                //That allows users to see in a glance what kind of device this is.
                string suffix = "";
                Type usageCollectionType=null; 
                if (hidDevice.Info.dwType == RawInputDeviceType.RIM_TYPEHID)
                {
                    //Process usage page
                    if (Enum.IsDefined(typeof(Hid.UsagePage), hidDevice.Info.hid.usUsagePage))
                    {
                        //We know this usage page, add its name
                        Hid.UsagePage usagePage = (Hid.UsagePage)hidDevice.Info.hid.usUsagePage;
                        suffix += " ( " + usagePage.ToString() + ", ";
                        usageCollectionType = Hid.Utils.UsageCollectionType(usagePage);
                    }
                    else
                    {
                        //We don't know this usage page, add its value
                        suffix += " ( 0x" + hidDevice.Info.hid.usUsagePage.ToString("X4") + ", ";
                    }

                    //Process usage collection
                    //We don't know this usage page, add its value
                    if (usageCollectionType == null || !Enum.IsDefined(usageCollectionType, hidDevice.Info.hid.usUsage))
                    {
                        //Show Hexa
                        suffix += "0x" + hidDevice.Info.hid.usUsage.ToString("X4") + " )";
                    }
                    else
                    {
                        //We know this usage page, add its name
                        suffix += Enum.GetName(usageCollectionType, hidDevice.Info.hid.usUsage) + " )";                        
                    }
                }
                else if (hidDevice.Info.dwType == RawInputDeviceType.RIM_TYPEKEYBOARD)
                {
                    suffix = " - Keyboard";
                }
                else if (hidDevice.Info.dwType == RawInputDeviceType.RIM_TYPEMOUSE)
                {
                    suffix = " - Mouse";
                }

                TreeNode node = null;
                if (hidDevice.Product != null && hidDevice.Product.Length > 1)
                {
                    //Add the devices with a proper name at the top
                    node = aTreeView.Nodes.Insert(0, hidDevice.Name, hidDevice.Product + suffix);
                }
                else
                {
                    //Add other once at the bottom
                    node = aTreeView.Nodes.Add(hidDevice.Name, "0x" + hidDevice.ProductId.ToString("X4") + suffix);
                }

                node.Nodes.Add("Manufacturer: " + hidDevice.Manufacturer);
                node.Nodes.Add("Product ID: 0x" + hidDevice.ProductId.ToString("X4"));
                node.Nodes.Add("Vendor ID: 0x" + hidDevice.VendorId.ToString("X4"));
                node.Nodes.Add("Version: " + hidDevice.Version);
                node.Nodes.Add(hidDevice.Info.dwType.ToString());
                if (hidDevice.Info.dwType == RawInputDeviceType.RIM_TYPEHID)
                {
                    node.Nodes.Add("UsagePage / UsageCollection: 0x" + hidDevice.Info.hid.usUsagePage.ToString("X4") + " / 0x" + hidDevice.Info.hid.usUsage.ToString("X4"));
                }

                node.Nodes.Add(hidDevice.Name);
            }
        }

    }
}