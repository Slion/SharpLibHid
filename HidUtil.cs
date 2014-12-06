using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Hid
{
    /// <summary>
    /// Represent a HID device.
    /// </summary>
    class HidDevice
    {
        public string Name { get; private set; }
        public string Manufacturer { get; private set; }
        public string Product { get; private set; }
        public ushort VendorId { get; private set; }
        public ushort ProductId { get; private set; }
        public ushort Version { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hRawInputDevice"></param>
        public HidDevice(IntPtr hRawInputDevice)
        {
            //Fetch various information defining the given HID device
            Name = Win32.RawInput.GetDeviceName(hRawInputDevice);            
                
            //Open our device from the device name/path
            SafeFileHandle handle=Win32.Function.CreateFile(Name,
                Win32.FileAccess.NONE,
                Win32.FileShare.FILE_SHARE_READ|Win32.FileShare.FILE_SHARE_WRITE,
                IntPtr.Zero,
                Win32.CreationDisposition.OPEN_EXISTING,
                Win32.FileFlagsAttributes.FILE_FLAG_OVERLAPPED,
                IntPtr.Zero
                );

            if (handle.IsInvalid)
            {
                Debug.WriteLine("Failed to CreateFile from device name " + Marshal.GetLastWin32Error().ToString());
            }
            else
            {
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
                Win32.HIDD_ATTRIBUTES attributes=new Win32.HIDD_ATTRIBUTES();
                if (Win32.Function.HidD_GetAttributes(handle, ref attributes))
                {
                    VendorId = attributes.VendorID;
                    ProductId = attributes.ProductID;
                    Version = attributes.VersionNumber;
                }

                handle.Close();
            }
        }

        /// <summary>
        /// Print information about this device to our debug output.
        /// </summary>
        public void DebugWrite()
        {
            Debug.WriteLine("================ HID =========================================================================================");
            Debug.WriteLine("==== Name: " + Name);
            Debug.WriteLine("==== Manufacturer: " + Manufacturer);
            Debug.WriteLine("==== Product: " + Product);
            Debug.WriteLine("==== VendorID: 0x" + VendorId.ToString("X4"));
            Debug.WriteLine("==== ProductID: 0x" + ProductId.ToString("X4"));
            Debug.WriteLine("==== Version: " + Version.ToString());
            Debug.WriteLine("==============================================================================================================");
        }




    }

}