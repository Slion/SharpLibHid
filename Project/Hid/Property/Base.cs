using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLib.Hid.Property
{
    using Windows.Win32;
    using Windows.Win32.Foundation;
    using Windows.Win32.Devices.DeviceAndDriverInstallation;
    using Windows.Win32.Devices.Properties;
    using SharpLib.Win32;
    using System.Diagnostics;
    using System.ComponentModel;

    public abstract class Base
    {
        static Dictionary<DEVPROPKEY, string> iPropertyNames = null;
        static Dictionary<string, DEVPROPKEY> iPropertyGuids = null;


        public string Name { get; protected set; }

        /*
        public unsafe static Base New(SetupDiDestroyDeviceInfoListSafeHandle aDevInfo, SP_DEVINFO_DATA aDevInfoData, DEVPROPKEY aKey)
        {
            return New(aDevInfo, &aDevInfoData, &aKey);
        }
        */

        /// <summary>
        /// Our property factor function
        /// </summary>
        /// <param name="aKey"></param>
        /// <returns></returns>
        public unsafe static Base New(SetupDiDestroyDeviceInfoListSafeHandle aDevInfo, SP_DEVINFO_DATA aDevInfoData, DEVPROPKEY aKey)
        {
            BuildDictionaries();

            // Query our property type and size
            DEVPROP_TYPE_FLAGS type;
            uint size = 0;
            PInvoke.SetupDiGetDeviceProperty(aDevInfo, &aDevInfoData, &aKey, (uint*)&type, null, 0, &size, 0); // Returns false and ERROR_NO_TOKEN when querying size and type

            // Read the property data as a byte buffer
            byte[] buffer = new byte[(int)size];                    
            fixed (byte* ptr = buffer)
            {
                if (!PInvoke.SetupDiGetDeviceProperty(aDevInfo, &aDevInfoData, &aKey, (uint*)&type, ptr, size, null, 0))
                {
                    GetLastError.LogAndThrow("SetupDiGetDeviceProperty");
                }
            }

            string name = NameForKey(aKey);
            // Create property object matching that property type 
            if (type == DEVPROP_TYPE_FLAGS.DEVPROP_TYPE_STRING)
            {
                // Create and return that string property
                var p = new Property.String(name, System.Text.Encoding.Unicode.GetString(buffer).TrimEnd('\0'));
                Trace.WriteLine(p.Name + ": " + p.Value);
                return p;
            }
            // TODO: Handle other property types
            else
            {
                string msg = name + ": type not supported";
                Trace.WriteLine(msg);
                // Throwing an exception here makes it really slow to load all our devices
                // Just don't do that for now then
                //throw new NotSupportedException(msg);
            }

            return null;
        }


        static string NameForKey(DEVPROPKEY aKey)
        {
            string name = "";
            if (!iPropertyNames.TryGetValue(aKey, out name))
            {
                // TODO: If the DEVPROPKEY is not found we could try just finding the GUID
                name = aKey.fmtid.ToString() + " - " + aKey.pid.ToString();
            }

            return name;
        }

        //public virtual string ToString();

        /// <summary>
        /// 
        /// </summary>
        static void BuildDictionaries()
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
}
