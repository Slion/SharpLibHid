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

            //For each our device add an node to our treeview
            foreach (RAWINPUTDEVICELIST device in ridList)
            {
                string name=GetDeviceName(device.hDevice);
                aTreeView.Nodes.Add(name, name);
            }

            //TreeNode node = null;

        }

    }
}