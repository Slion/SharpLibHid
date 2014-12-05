using System;
using System.Runtime.InteropServices;
using System.Diagnostics;


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static IntPtr GetPreParsedData(IntPtr hDevice)
        {
            uint ppDataSize = 256;
            int result = Win32.Function.GetRawInputDeviceInfo(hDevice, Win32.Const.RIDI_PREPARSEDDATA, IntPtr.Zero, ref ppDataSize);
            if (result != 0)
            {
                Debug.WriteLine("Failed to get raw input pre-parsed data size" + result + Marshal.GetLastWin32Error());
                return IntPtr.Zero;
            }

            IntPtr ppData = Marshal.AllocHGlobal((int)ppDataSize);
            result = Win32.Function.GetRawInputDeviceInfo(hDevice, Win32.Const.RIDI_PREPARSEDDATA, ppData, ref ppDataSize);
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
        /// <param name="driver"></param>
        /// <param name="device"></param>
        /// <param name="input"></param>
        /// <param name="rawInput"></param>
        /// <param name="usageType"></param>
        /// <param name="usage"></param>
        /// <param name="usageName"></param>
        /// <returns></returns>
        /// 
        /*
        private bool GetUsageFromRawInput(TwinhanHidDriver driver, TwinhanHid device, NativeMethods.RAWINPUT input, IntPtr rawInput, out UsageType usageType, out int usage, out string usageName)
        {
            usageType = 0;
            usage = 0;
            usageName = string.Empty;
            if (input.header.dwType == NativeMethods.RawInputDeviceType.RIM_TYPEKEYBOARD)
            {
                if (input.data.keyboard.Flags == NativeMethods.RawInputKeyboardFlag.RI_KEY_BREAK)
                {
                    _modifiers = 0;
                    // Key up event. We don't handle repeats, so ignore this.
                    return false;
                }
                NativeMethods.VirtualKey vk = input.data.keyboard.VKey;
                if (vk == NativeMethods.VirtualKey.VK_CONTROL)
                {
                    _modifiers |= VirtualKeyModifier.Control;
                    return false;
                }
                if (vk == NativeMethods.VirtualKey.VK_SHIFT)
                {
                    _modifiers |= VirtualKeyModifier.Shift;
                    return false;
                }
                if (vk == NativeMethods.VirtualKey.VK_MENU)
                {
                    _modifiers |= VirtualKeyModifier.Alt;
                    return false;
                }
                usageType = UsageType.Keyboard;
                usage = (int)vk | (int)_modifiers;
                usageName = vk.ToString();
                if (_modifiers != 0)
                {
                    usageName += string.Format(", modifiers = {0}", _modifiers);
                }
            }
            else if (input.header.dwType == NativeMethods.RawInputDeviceType.RIM_TYPEHID)
            {
                if ((!driver.IsTerraTec && device.Name.Contains("Col03")) || (driver.IsTerraTec && device.Name.Contains("Col02")))
                {
                    usageType = UsageType.Raw;
                    usage = Marshal.ReadByte(rawInput, HID_INPUT_DATA_OFFSET + 1);
                    usageName = string.Format("0x{0:x2}", usage);
                }
                else if (device.Name.Contains("Col05"))
                {
                    usageType = UsageType.Ascii;
                    usage = Marshal.ReadByte(rawInput, HID_INPUT_DATA_OFFSET + 1);
                    usageName = string.Format("0x{0:x2}", usage);
                }
                else
                {
                    byte[] report = new byte[input.data.hid.dwSizeHid];
                    Marshal.Copy(IntPtr.Add(rawInput, HID_INPUT_DATA_OFFSET), report, 0, report.Length);
                    uint usageCount = input.data.hid.dwCount;
                    NativeMethods.USAGE_AND_PAGE[] usages = new NativeMethods.USAGE_AND_PAGE[usageCount];
                    NativeMethods.HidStatus status = NativeMethods.HidP_GetUsagesEx(NativeMethods.HIDP_REPORT_TYPE.HidP_Input, 0, usages, ref usageCount, device.PreParsedData, report, (uint)report.Length);
                    if (status != NativeMethods.HidStatus.HIDP_STATUS_SUCCESS)
                    {
                        this.LogError("Twinhan HID remote: failed to get raw input HID usages, device = {0}, status = {1}", device.Name, status);
                        Dump.DumpBinary(rawInput, (int)input.header.dwSize);
                        return false;
                    }
                    if (usageCount > 1)
                    {
                        this.LogWarn("Twinhan HID remote: multiple simultaneous HID usages not supported");
                    }
                    NativeMethods.USAGE_AND_PAGE up = usages[0];
                    HidUsagePage page = (HidUsagePage)up.UsagePage;
                    usage = up.Usage;
                    if (page != HidUsagePage.MceRemote && usage == 0)
                    {
                        // Key up event. We don't handle repeats, so ignore this.
                        return false;
                    }
                    if (page == HidUsagePage.Consumer)
                    {
                        usageType = UsageType.Consumer;
                        usageName = Enum.GetName(typeof(HidConsumerUsage), up.Usage);
                    }
                    else if (page == HidUsagePage.MceRemote)
                    {
                        usageType = UsageType.Mce;
                        usageName = Enum.GetName(typeof(MceRemoteUsage), up.Usage);
                    }
                    else
                    {
                        this.LogError("Twinhan HID remote: unexpected usage page, device = {0}, page = {1}, usage = {2}", device.Name, page, up.Usage);
                        return false;
                    }
                }
            }
            else
            {
                this.LogError("Twinhan HID remote: received input from unsupported input device type, device = {0}, type = {1}", device.Name, input.header.dwType);
                return false;
            }
            return true;
        }
         * */

    }
}