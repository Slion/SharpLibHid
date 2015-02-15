using System;
using System.Collections.Generic;
using System.Text;

namespace Hid
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
                    return typeof(UsageCollectionGenericDesktop);

                case UsagePage.Consumer:
                    return typeof(UsageCollectionConsumer);

                case UsagePage.WindowsMediaCenterRemoteControl:
                    return typeof(UsageCollectionWindowsMediaCenter);

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
                    return typeof(UsageTables.GenericDesktop);

                case UsagePage.Consumer:
                    return typeof(UsageTables.ConsumerControl);

                case UsagePage.WindowsMediaCenterRemoteControl:
                    return typeof(UsageTables.WindowsMediaCenterRemoteControl);

                case UsagePage.Telephony:
                    return typeof(UsageTables.TelephonyDevice);

                case UsagePage.SimulationControls:
                    return typeof(UsageTables.SimulationControl);

                case UsagePage.GameControls:
                    return typeof(UsageTables.GameControl);

                default:
                    return null;
            }
        }

    }
}
