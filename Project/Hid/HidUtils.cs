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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace SharpLib.Hid
{
    public static class Utils
    {
        /// <summary>
        /// Provide the type for the usage collection corresponding to the given usage page.
        /// </summary>
        /// <param name="aUsagePage"></param>
        /// <returns></returns>
        public static Type UsageCollectionType(UsagePage aUsagePage)
        {
            switch (aUsagePage)
            {
                case UsagePage.GenericDesktopControls:
                    return typeof(UsageCollection.GenericDesktop);

                case UsagePage.Consumer:
                    return typeof(UsageCollection.Consumer);

                case UsagePage.WindowsMediaCenterRemoteControl:
                    return typeof(UsageCollection.WindowsMediaCenter);

                case UsagePage.VirtualRealityControls:
                    return typeof(UsageCollection.VirtualReality);
                  
                default:
                    return null;
            }
        }

        /// <summary>
        /// Provide the type for the usage corresponding to the given usage page.
        /// </summary>
        /// <param name="aUsagePage"></param>
        /// <returns></returns>
        public static Type UsageType(UsagePage aUsagePage)
        {
            switch (aUsagePage)
            {
                case UsagePage.GenericDesktopControls:
                    return typeof(Usage.GenericDesktop);

                case UsagePage.Consumer:
                    return typeof(Usage.ConsumerControl);

                case UsagePage.WindowsMediaCenterRemoteControl:
                    return typeof(Usage.WindowsMediaCenterRemoteControl);

                case UsagePage.Telephony:
                    return typeof(Usage.TelephonyDevice);

                case UsagePage.SimulationControls:
                    return typeof(Usage.SimulationControl);

                case UsagePage.GameControls:
                    return typeof(Usage.GameControl);

                case UsagePage.GenericDeviceControls:
                    return typeof(Usage.GenericDevice);

                default:
                    return null;
            }
        }
    }
}

// From: https://stackoverflow.com/a/38021136/3969362
// TODO: Move those else where
namespace SharpLib.ToEnum
{
    public static class Extensions
    {
        /// <summary>
        /// Extension method to return an enum value of type T for the given string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Extension method to return an enum value of type T for the given int.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value)
        {
            var name = System.Enum.GetName(typeof(T), value);
            return name.ToEnum<T>();
        }

    }
}

//TODO: Move this to SharpLib.Win32 v2
namespace SharpLib.Win32
{
    using SharpLib.ToEnum;
    using Windows.Win32.Foundation;
    using System.ComponentModel;
    using System.Diagnostics;
    public static class GetLastError
    {
        public static string String()
        {
            return Enum().ToString();
        }

        public static WIN32_ERROR Enum()
        {
            int err = Int();
            return err.ToEnum<WIN32_ERROR>();
        }

        public static int Int()
        {
            return Marshal.GetLastWin32Error();
        }

        public static void Throw()
        {            
            throw new Win32Exception(Int(),String());
        }

        public static void Log(string aPrefix="")
        {
            Trace.WriteLine(aPrefix + ".GetLastError: " + String());
        }

        public static void LogAndThrow(string aPrefix = "")
        {
            Log(aPrefix);
            Throw();
        }

    }
}
