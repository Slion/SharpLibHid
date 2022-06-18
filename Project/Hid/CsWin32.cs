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
        //public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1L);
        public static readonly uint INVALID_HANDLE_VALUE = uint.MaxValue;
    }




    namespace Devices.DeviceAndDriverInstallation
    {

        [Flags]
        public enum DEVICEINFOGETCLASS_FLAGS : uint
        {
            // Flags controlling what is included in the device information set built by SetupDiGetClassDevs
            DIGCF_DEFAULT = 0x00000001,       // only valid with DIGCF_DEVICEINTERFACE
            DIGCF_PRESENT = 0x00000002,
            DIGCF_ALLCLASSES = 0x00000004,
            DIGCF_PROFILE = 0x00000008,
            DIGCF_DEVICEINTERFACE = 0x00000010
        }

        /// <summary>
        /// 
        /// </summary>
        public partial struct SP_DEVINFO_DATA
        {
            unsafe public SP_DEVINFO_DATA(bool aValid = true)
            {
                cbSize = (uint)sizeof(SP_DEVINFO_DATA);
                ClassGuid = Guid.Empty;
                DevInst = K.INVALID_HANDLE_VALUE;
                //DevInst = 0;
                Reserved = 0;
            }
        }

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


