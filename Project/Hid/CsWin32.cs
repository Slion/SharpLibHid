using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using global::System.Diagnostics;
using global::System.Runtime.CompilerServices;
using global::System.Runtime.InteropServices;
using winmdroot = global::Windows.Win32;

/// <summary>
/// We put here convenient extensions to work with automatically generate stuff from CsWin32
/// </summary>
namespace Windows.Win32
{
    class K
    {
        public static readonly Foundation.BOOLEAN TRUE = new Foundation.BOOLEAN(1);
        public static readonly Foundation.BOOLEAN FALSE = new Foundation.BOOLEAN(0);
    }


    namespace Devices.DeviceAndDriverInstallation
    {
        /// <summary>
        /// We used that as a fixed size buffer to be cast into SP_DEVICE_INTERFACE_DETAIL_DATA_W
        /// See: https://github.com/microsoft/CsWin32/discussions/589
        /// </summary>
        public partial struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public uint cbSize;
            public __char_1 DevicePath;

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public unsafe partial struct __char_1
            {
                public fixed char _0[512];

                /// <summary>Always <c>1</c>.</summary>
                public readonly int Length => 512;

                /// <summary>
                /// Copies the fixed array to a new string up to the specified length regardless of whether there are null terminating characters.
                /// </summary>
                /// <exception cref="ArgumentOutOfRangeException">
                /// Thrown when <paramref name="length"/> is less than <c>0</c> or greater than <see cref="Length"/>.
                /// </exception>
                public unsafe readonly string ToString(int length)
                {
                    if (length < 0 || length > Length) throw new ArgumentOutOfRangeException(nameof(length), length, "Length must be between 0 and the fixed array length.");
                    fixed (char* p0 = _0)
                        return new string(p0, 0, length);
                }

                /// <summary>
                /// Copies the fixed array to a new string, stopping before the first null terminator character or at the end of the fixed array (whichever is shorter).
                /// </summary>
                public override readonly unsafe string ToString()
                {
                    int length;
                    fixed (char* p = _0)
                    {
                        char* pLastExclusive = p + Length;
                        char* pCh = p;
                        for (;
                        pCh < pLastExclusive && *pCh != '\0';
                        pCh++) ;
                        length = checked((int)(pCh - p));
                    }
                    return ToString(length);
                }
            }
        }
    }

}


