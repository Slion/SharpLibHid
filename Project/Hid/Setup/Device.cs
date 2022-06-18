using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// TODO: Move it to another project SharpLibSetup
/// </summary>
namespace SharpLib.Setup
{

    using Windows.Win32;
    using Windows.Win32.Foundation;
    using Windows.Win32.Devices.DeviceAndDriverInstallation;
    using SharpLib.Win32;
    using System.Diagnostics;
    using System.ComponentModel;

    /// <summary>
    /// A setup device is identified by the set it belongs to (HDEVINFO) and its informations data (SP_DEVINFO_DATA).
    /// <see href="https://docs.microsoft.com/en-us/windows-hardware/drivers/install/device-information-sets"/>
    /// <see cref="Windows.Win32.Devices.DeviceAndDriverInstallation.HDEVINFO"/>
    /// <see cref="Windows.Win32.SetupDiDestroyDeviceInfoListSafeHandle"/> 
    /// <see cref="Windows.Win32.Devices.DeviceAndDriverInstallation.SP_DEVINFO_DATA"/>
    /// </summary>
    public class Device : IDisposable
    {

        SetupDiDestroyDeviceInfoListSafeHandle iDevInfo;
        SP_DEVINFO_DATA iDevInfoData = new SP_DEVINFO_DATA(true);
        string iInstancePath;

        /// <summary>
        /// Instance path uniquely identifies a device.
        /// Can be used as input to SetupDiGetClassDevs to retrieve device handles and from there other properties.
        /// </summary>
        public string InstancePath 
        {
            get { return iInstancePath; }
            protected set
            {
                unsafe
                {
                    iInstancePath = value;
                    // Get our device information set handle, that set though should contain only this device instance really
                    iDevInfo = PInvoke.SetupDiGetClassDevs(null, iInstancePath, new HWND(0), (uint)(DEVICEINFOGETCLASS_FLAGS.DIGCF_ALLCLASSES | DEVICEINFOGETCLASS_FLAGS.DIGCF_DEVICEINTERFACE));
                    if (iDevInfo.IsInvalid)
                    {
                        //GetLastError.Log();
                        //return;
                        GetLastError.LogAndThrow("SetupDiGetClassDevs");
                    }

                    uint index = 0;
                    // Get our device information data
                    fixed (SP_DEVINFO_DATA* ptrDevInfoData = &iDevInfoData) // Needed for data members apparently 
                    {                        
                        if (!PInvoke.SetupDiEnumDeviceInfo(iDevInfo, index++, ptrDevInfoData))
                        {
                            GetLastError.LogAndThrow("SetupDiEnumDeviceInfo");
                        }
                    }

                    // Make sure we only have one item in our list
                    SP_DEVINFO_DATA discard = new SP_DEVINFO_DATA(true);
                    if (PInvoke.SetupDiEnumDeviceInfo(iDevInfo, index++, &discard))
                    {
                        throw new ArgumentException("More than one device in our information set");
                        //GetLastError.LogAndThrow("SetupDiEnumDeviceInfo");
                    }
                }
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
        /// Dispose is just for unmanaged clean-up.
        /// Make sure calling disposed multiple times does not crash.
        /// See: http://stackoverflow.com/questions/538060/proper-use-of-the-idisposable-interface/538238#538238
        /// </summary>
        public virtual void Dispose()
        {
        }




    }
}
