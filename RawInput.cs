using System;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace Win32
{
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
        /// <param name="aRawInputHandle"></param>
        /// <param name="aUsagePage"></param>
        /// <param name="aUsage"></param>
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

                int res = Win32.Function.GetRawInputDeviceInfo(hDevice, Const.RIDI_DEVICEINFO, deviceInfoBuffer, ref deviceInfoSize);
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

    }
}