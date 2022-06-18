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
    public class Device : IDisposable
    {
        private string iInstancePath;
        SetupDiDestroyDeviceInfoListSafeHandle iDevInfo;
        SP_DEVINFO_DATA iDevInfoData = new SP_DEVINFO_DATA(true);
        static Dictionary<DEVPROPKEY, string> iPropertyNames = null;
        static Dictionary<string, DEVPROPKEY> iPropertyGuids = null;

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
        /// 
        /// </summary>
        void BuildPropertiesDictionary()
        {
            lock(this)
            {
                if (iPropertyNames != null)
                {
                    // Already built
                    return;
                }

                var properties = typeof(DEVPKEY).GetProperties();
                iPropertyNames = new Dictionary<DEVPROPKEY, string>(properties.Length);
                iPropertyGuids = new Dictionary<string, DEVPROPKEY>(properties.Length);
                foreach (var p in properties)
                {
                    //Trace.WriteLine("Prop: " + p.Name);
                    //var guid = ((DEVPROPKEY)p.GetValue(null, null)).fmtid;
                    var guid = ((DEVPROPKEY)p.GetValue(null, null));
                    iPropertyNames.Add(guid, p.Name);
                    iPropertyGuids.Add(p.Name, guid);
                }
            }
        }

        /// <summary>
        /// TODO: Don't do this by default when loading a device as it will slow things down quite a bit
        /// </summary>
        unsafe void GetAllProperties()
        {
            BuildPropertiesDictionary();

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
                    // Query our property type and size
                    DEVPROP_TYPE_FLAGS type;
                    uint size = 0;
                    PInvoke.SetupDiGetDeviceProperty(iDevInfo, ptrDevInfoData, &propertyKeys[i], (uint*)&type, null, 0, &size, 0); // Returns false and ERROR_NO_TOKEN when querying size and type

                    byte[] buffer = new byte[(int)size];
                    // TODO: Handle property types
                    fixed (byte* ptr = buffer)
                    {
                        if (!PInvoke.SetupDiGetDeviceProperty(iDevInfo, ptrDevInfoData, &propertyKeys[i], (uint*)&type, ptr, size, null, 0))
                        {
                            GetLastError.LogAndThrow("SetupDiGetDeviceProperty");
                        }
                    }

                    //string propertyName = iPropertyNames[propertyKeys[i]];
                    string propertyName = "";
                    if (!iPropertyNames.TryGetValue(propertyKeys[i], out propertyName))
                    {
                        // TODO: If the DEVPROPKEY is not found we could try just finding the GUID
                        propertyName = propertyKeys[i].fmtid.ToString() + " - " + propertyKeys[i].pid.ToString();
                    }


                    if (type == DEVPROP_TYPE_FLAGS.DEVPROP_TYPE_STRING)
                    {
                        var value = System.Text.Encoding.Unicode.GetString(buffer).TrimEnd('\0');
                        Trace.WriteLine(propertyName + ": " + value);
                    }
                    else
                    {
                        Trace.WriteLine(propertyName + ": type not supported" );
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
