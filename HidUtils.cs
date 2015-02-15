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
    }
}
