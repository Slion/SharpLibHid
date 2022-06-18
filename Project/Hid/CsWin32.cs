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


    namespace Devices.Properties
    {
        [Flags]
        public enum DEVPROP_TYPE_FLAGS : uint
        {
            DEVPROP_TYPEMOD_ARRAY = 0x00001000,  // array of fixed-sized data elements
            DEVPROP_TYPEMOD_LIST = 0x00002000,  // list of variable-sized data elements
            // Property data types.
            DEVPROP_TYPE_EMPTY = 0x00000000,  // nothing, no property data
            DEVPROP_TYPE_NULL = 0x00000001,  // null property data
            DEVPROP_TYPE_SBYTE = 0x00000002,  // 8-bit signed int (SBYTE)
            DEVPROP_TYPE_BYTE = 0x00000003,  // 8-bit unsigned int (BYTE)
            DEVPROP_TYPE_INT16 = 0x00000004,  // 16-bit signed int (SHORT)
            DEVPROP_TYPE_UINT16 = 0x00000005,  // 16-bit unsigned int (USHORT)
            DEVPROP_TYPE_INT32 = 0x00000006,  // 32-bit signed int (LONG)
            DEVPROP_TYPE_UINT32 = 0x00000007,  // 32-bit unsigned int (ULONG)
            DEVPROP_TYPE_INT64 = 0x00000008,  // 64-bit signed int (LONG64)
            DEVPROP_TYPE_UINT64 = 0x00000009,  // 64-bit unsigned int (ULONG64)
            DEVPROP_TYPE_FLOAT = 0x0000000A,  // 32-bit floating-point (FLOAT)
            DEVPROP_TYPE_DOUBLE = 0x0000000B,  // 64-bit floating-point (DOUBLE)
            DEVPROP_TYPE_DECIMAL = 0x0000000C,  // 128-bit data (DECIMAL)
            DEVPROP_TYPE_GUID = 0x0000000D,  // 128-bit unique identifier (GUID)
            DEVPROP_TYPE_CURRENCY = 0x0000000E,  // 64 bit signed int currency value (CURRENCY)
            DEVPROP_TYPE_DATE = 0x0000000F,  // date (DATE)
            DEVPROP_TYPE_FILETIME = 0x00000010,  // file time (FILETIME)
            DEVPROP_TYPE_BOOLEAN = 0x00000011,  // 8-bit boolean (DEVPROP_BOOLEAN)
            DEVPROP_TYPE_STRING = 0x00000012,  // null-terminated string
            DEVPROP_TYPE_STRING_LIST = (DEVPROP_TYPE_STRING|DEVPROP_TYPEMOD_LIST), // multi-sz string list
            DEVPROP_TYPE_SECURITY_DESCRIPTOR = 0x00000013,  // self-relative binary SECURITY_DESCRIPTOR
            DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING = 0x00000014,  // security descriptor string (SDDL format)
            DEVPROP_TYPE_DEVPROPKEY = 0x00000015,  // device property key (DEVPROPKEY)
            DEVPROP_TYPE_DEVPROPTYPE = 0x00000016,  // device property type (DEVPROPTYPE)
            DEVPROP_TYPE_BINARY = (DEVPROP_TYPE_BYTE |DEVPROP_TYPEMOD_ARRAY),  // custom binary data
            DEVPROP_TYPE_ERROR = 0x00000017,  // 32-bit Win32 system error code
            DEVPROP_TYPE_NTSTATUS = 0x00000018,  // 32-bit NTSTATUS code
            DEVPROP_TYPE_STRING_INDIRECT = 0x00000019,  // string resource (@[path\]<dllname>,-<strId>)
            // Max base DEVPROP_TYPE_ and DEVPROP_TYPEMOD_ values.
            MAX_DEVPROP_TYPE = 0x00000019,  // max valid DEVPROP_TYPE_ value
            MAX_DEVPROP_TYPEMOD = 0x00002000,  // max valid DEVPROP_TYPEMOD_ value
            // Bitmasks for extracting DEVPROP_TYPE_ and DEVPROP_TYPEMOD_ values.
            DEVPROP_MASK_TYPE = 0x00000FFF,  // range for base DEVPROP_TYPE_ values
            DEVPROP_MASK_TYPEMOD = 0x0000F000  // mask for DEVPROP_TYPEMOD_ type modifiers
        }
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


