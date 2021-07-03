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
using System.Text;

namespace SharpLib.Hid
{
    static class Utils
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
