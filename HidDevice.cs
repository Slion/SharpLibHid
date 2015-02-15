using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Win32;

namespace Hid
{
    /// <summary>
    /// Represent a HID device.
    /// Rename to RawInputDevice?
    /// </summary>
    public class HidDevice: IDisposable
    {
        public string Name { get; private set; }
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
        //Input Button Capabilities
        public HIDP_BUTTON_CAPS[] InputButtonCapabilities { get { return iInputButtonCapabilities; } }
        private HIDP_BUTTON_CAPS[] iInputButtonCapabilities;
        //Input Value Capabilities
        public HIDP_VALUE_CAPS[] InputValueCapabilities { get { return iInputValueCapabilities; } }
        private HIDP_VALUE_CAPS[] iInputValueCapabilities;

        
        

        /// <summary>
        /// Class constructor will fetch this object properties from HID sub system.
        /// </summary>
        /// <param name="hRawInputDevice">Device Handle as provided by RAWINPUTHEADER.hDevice, typically accessed as rawinput.header.hDevice</param>
        public HidDevice(IntPtr hRawInputDevice)
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
        ~HidDevice()
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