using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// TODO: Move it to another project SharpLibSetup
/// </summary>
namespace SharpLib.Hid.Device
{

    using Windows.Win32;
    using Windows.Win32.Foundation;
    using Windows.Win32.Devices.DeviceAndDriverInstallation;
    using Windows.Win32.Devices.Properties;
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
    public class Base : IDisposable
    {
        private string iInstancePath;
        SetupDiDestroyDeviceInfoListSafeHandle iDevInfo;
        SP_DEVINFO_DATA iDevInfoData = new SP_DEVINFO_DATA(true);
        Dictionary<DEVPROPKEY, Property.Base> iProperties = new Dictionary<DEVPROPKEY, Property.Base>();

        /// <summary>
        /// Instance path uniquely identifies a device.
        /// Can be used as input to SetupDiGetClassDevs to retrieve device handles and from there other properties.
        /// </summary>
        public string InstancePath 
        {
            get { return iInstancePath; }
            protected set
            {
                iInstancePath = value;
                GetDeviceInfoData();
                GetAllProperties();   
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        private unsafe void GetDeviceInfoData()
        {
            // Get our device information set handle, that set though should contain only this device instance really
            iDevInfo = PInvoke.SetupDiGetClassDevs(null, InstancePath, new HWND(0), (uint)(DEVICEINFOGETCLASS_FLAGS.DIGCF_ALLCLASSES | DEVICEINFOGETCLASS_FLAGS.DIGCF_DEVICEINTERFACE));
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



        /// <summary>
        /// TODO: Don't do this by default when loading a device as it will slow things down quite a bit
        /// </summary>
        unsafe void GetAllProperties()
        {            

            Trace.WriteLine("--------------------------------------------------------------------------------");

            fixed (SP_DEVINFO_DATA* ptrDevInfoData = &iDevInfoData) // Needed for data members apparently 
            {
                uint propertyCount;
                // First get our number of properties
                PInvoke.SetupDiGetDevicePropertyKeys(iDevInfo, ptrDevInfoData, null, 0, &propertyCount, 0); // Returns false and ERROR_NO_TOKEN when querying size
                if (propertyCount<=0)
                {
                    return;
                }

                DEVPROPKEY* propertyKeys = stackalloc DEVPROPKEY[(int)propertyCount];
                if (!PInvoke.SetupDiGetDevicePropertyKeys(iDevInfo, ptrDevInfoData, propertyKeys, propertyCount, null, 0))
                {
                    GetLastError.LogAndThrow("SetupDiGetDevicePropertyKeys");
                }

                // Fetch all our properties
                for (int i=0;i<propertyCount;i++)
                {
                    GetProperty(propertyKeys[i]);
                }
            }
        }

        /// <summary>
        /// Get property from our cache or load it if needed.
        /// </summary>
        /// <param name="aKey"></param>
        /// <returns></returns>
        public Property.Base GetProperty(DEVPROPKEY aKey)
        {
            // Check if that property is already in our cache
            Property.Base p;
            if (iProperties.TryGetValue(aKey,out p))
            {
                return p;
            }

            // Property not yet in our cache
            return LoadProperty(aKey);
        }

        /// <summary>
        /// Load the given property from the system and put it in our cache.
        /// TODO: Not sure we actually tested reload.
        /// </summary>
        /// <param name="aKey"></param>
        /// <returns></returns>
        unsafe public Property.Base LoadProperty(DEVPROPKEY aKey)
        {
            fixed (SP_DEVINFO_DATA* ptrDevInfoData = &iDevInfoData) // Needed for data members apparently 
            {
                var p = Property.Base.New(iDevInfo, *ptrDevInfoData, aKey);
                if (p != null)
                {
                    // Add or replace that property to our dictionary
                    iProperties[aKey] = p;
                    //iProperties.Add(aKey, p);
                }
                return p;
            }
        }

        /// <summary>
        /// Obtain this device parent.
        /// TODO: Check what happens when not parent.
        /// I'm guessing it would throw an exception.
        /// </summary>
        /// <returns></returns>
        public Device.Base Parent()
        {
            // TODO: Use our device cache instead?
            var parent = new Device.Base();
            parent.InstancePath = GetProperty(DEVPKEY.Device_Parent).ToString();
            return parent;
        }

        /// <summary>
        /// Make sure dispose is called even if the user forgot about it.
        /// </summary>
        ~Base()
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
