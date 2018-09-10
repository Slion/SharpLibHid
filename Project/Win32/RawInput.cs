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
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace SharpLib.Win32
{
    /// <summary>
    /// Provide some utility functions for raw input handling.
    /// </summary>
    static public class RawInput
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
                Debug.WriteLine("Failed to get raw input pre-parsed data size: " + result + " : " + Marshal.GetLastWin32Error());
                return IntPtr.Zero;
            }

            IntPtr ppData = Marshal.AllocHGlobal((int)ppDataSize);
            result = Win32.Function.GetRawInputDeviceInfo(hDevice, RawInputDeviceInfoType.RIDI_PREPARSEDDATA, ppData, ref ppDataSize);
            if (result <= 0)
            {
                Debug.WriteLine("Failed to get raw input pre-parsed data: " + result + " : " + Marshal.GetLastWin32Error());
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
        /// Populate the given tree-view control with our Raw Input Devices.
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
                SharpLib.Hid.Device hidDevice;

                //Try create our HID device.
                try
                {
                    hidDevice = new SharpLib.Hid.Device(device.hDevice);
                }
                catch /*(System.Exception ex)*/
                {
                    //Just skip that device then
                    continue;
                }

                TreeNode node = null;
                if (hidDevice.Product != null && hidDevice.Product.Length > 1)
                {
                    //Add the devices with a proper name at the top
                    node = aTreeView.Nodes.Insert(0, hidDevice.Name, hidDevice.FriendlyName);
                }
                else
                {
                    //Add other once at the bottom
                    node = aTreeView.Nodes.Add(hidDevice.Name, hidDevice.FriendlyName);
                }

                //Each device root node keeps a reference to our HID device
                node.Tag = hidDevice;

                //Populate device properties
                node.Nodes.Add("Manufacturer: " + hidDevice.Manufacturer);
                node.Nodes.Add("Product ID: 0x" + hidDevice.ProductId.ToString("X4"));
                node.Nodes.Add("Vendor ID: 0x" + hidDevice.VendorId.ToString("X4"));
                node.Nodes.Add("Version: " + hidDevice.Version);
                node.Nodes.Add(hidDevice.Info.dwType.ToString());
                if (hidDevice.Info.dwType == RawInputDeviceType.RIM_TYPEHID)
                {
                    node.Nodes.Add("UsagePage / UsageCollection: 0x" + hidDevice.Info.hid.usUsagePage.ToString("X4") + " / 0x" + hidDevice.Info.hid.usUsage.ToString("X4"));
                }

                if (hidDevice.InputCapabilitiesDescription != null)
                {
                    node.Nodes.Add(hidDevice.InputCapabilitiesDescription);
                }

                //Add button count
                node.Nodes.Add("Button Count: " + hidDevice.ButtonCount);

                //Those can be joystick/gamepad axis
                if (hidDevice.InputValueCapabilities != null)
                {
                    foreach (HIDP_VALUE_CAPS caps in hidDevice.InputValueCapabilities)
                    {
                        string des = SharpLib.Hid.Device.InputValueCapabilityDescription(caps);
                        if (des != null)
                        {
                            node.Nodes.Add(des);
                        }
                    }

                }

                node.Nodes.Add(hidDevice.Name);
            }
        }

    }
}