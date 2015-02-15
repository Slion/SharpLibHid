using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Text;

namespace Win32
{

    static partial class Function
    {
        [DllImport("hid.dll", CharSet = CharSet.Unicode)]
        public static extern HidStatus HidP_GetUsagesEx(HIDP_REPORT_TYPE ReportType, ushort LinkCollection, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] USAGE_AND_PAGE[] ButtonList, ref uint UsageLength, IntPtr PreparsedData, [MarshalAs(UnmanagedType.LPArray)] byte[] Report, uint ReportLength);

        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean HidD_GetManufacturerString(SafeFileHandle HidDeviceObject, StringBuilder Buffer, Int32 BufferLength);

        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean HidD_GetProductString(SafeFileHandle HidDeviceObject, StringBuilder Buffer, Int32 BufferLength);

        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean HidD_GetAttributes(SafeFileHandle HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

        /// Return Type: NTSTATUS->LONG->int
        ///PreparsedData: PHIDP_PREPARSED_DATA->_HIDP_PREPARSED_DATA*
        ///Capabilities: PHIDP_CAPS->_HIDP_CAPS*
        [DllImportAttribute("hid.dll", EntryPoint = "HidP_GetCaps", CallingConvention = CallingConvention.StdCall)]
        public static extern HidStatus HidP_GetCaps(System.IntPtr PreparsedData, ref HIDP_CAPS Capabilities);

        /// Return Type: NTSTATUS->LONG->int
        ///ReportType: HIDP_REPORT_TYPE->_HIDP_REPORT_TYPE
        ///ButtonCaps: PHIDP_BUTTON_CAPS->_HIDP_BUTTON_CAPS*
        ///ButtonCapsLength: PUSHORT->USHORT*
        ///PreparsedData: PHIDP_PREPARSED_DATA->_HIDP_PREPARSED_DATA*
        [System.Runtime.InteropServices.DllImportAttribute("hid.dll", EntryPoint = "HidP_GetButtonCaps", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern HidStatus HidP_GetButtonCaps(HIDP_REPORT_TYPE ReportType, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] HIDP_BUTTON_CAPS[] ButtonCaps, ref ushort ButtonCapsLength, System.IntPtr PreparsedData);

        /// Return Type: NTSTATUS->LONG->int
        ///ReportType: HIDP_REPORT_TYPE->_HIDP_REPORT_TYPE
        ///ValueCaps: PHIDP_VALUE_CAPS->_HIDP_VALUE_CAPS*
        ///ValueCapsLength: PUSHORT->USHORT*
        ///PreparsedData: PHIDP_PREPARSED_DATA->_HIDP_PREPARSED_DATA*
        [System.Runtime.InteropServices.DllImportAttribute("hid.dll", EntryPoint = "HidP_GetValueCaps", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern HidStatus HidP_GetValueCaps(HIDP_REPORT_TYPE ReportType, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] HIDP_VALUE_CAPS[] ValueCaps, ref ushort ValueCapsLength, System.IntPtr PreparsedData);

    }


    static partial class Macro
    {


    }


    static partial class Const
    {


    }


    public enum HIDP_REPORT_TYPE : ushort
    {
        HidP_Input = 0,
        HidP_Output,
        HidP_Feature
    }


    public enum HidStatus : uint
    {
        HIDP_STATUS_SUCCESS = 0x110000,
        HIDP_STATUS_NULL = 0x80110001,
        HIDP_STATUS_INVALID_PREPARSED_DATA = 0xc0110001,
        HIDP_STATUS_INVALID_REPORT_TYPE = 0xc0110002,
        HIDP_STATUS_INVALID_REPORT_LENGTH = 0xc0110003,
        HIDP_STATUS_USAGE_NOT_FOUND = 0xc0110004,
        HIDP_STATUS_VALUE_OUT_OF_RANGE = 0xc0110005,
        HIDP_STATUS_BAD_LOG_PHY_VALUES = 0xc0110006,
        HIDP_STATUS_BUFFER_TOO_SMALL = 0xc0110007,
        HIDP_STATUS_INTERNAL_ERROR = 0xc0110008,
        HIDP_STATUS_I8042_TRANS_UNKNOWN = 0xc0110009,
        HIDP_STATUS_INCOMPATIBLE_REPORT_ID = 0xc011000a,
        HIDP_STATUS_NOT_VALUE_ARRAY = 0xc011000b,
        HIDP_STATUS_IS_VALUE_ARRAY = 0xc011000c,
        HIDP_STATUS_DATA_INDEX_NOT_FOUND = 0xc011000d,
        HIDP_STATUS_DATA_INDEX_OUT_OF_RANGE = 0xc011000e,
        HIDP_STATUS_BUTTON_NOT_PRESSED = 0xc011000f,
        HIDP_STATUS_REPORT_DOES_NOT_EXIST = 0xc0110010,
        HIDP_STATUS_NOT_IMPLEMENTED = 0xc0110020,
        HIDP_STATUS_I8242_TRANS_UNKNOWN = 0xc0110009
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct USAGE_AND_PAGE
    {
        public ushort Usage;
        public ushort UsagePage;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct HIDD_ATTRIBUTES
    {
        public uint Size;
        public ushort VendorID;
        public ushort ProductID;
        public ushort VersionNumber;
    }


    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct HIDP_CAPS
    {
        /// USAGE->USHORT->unsigned short
        public ushort Usage;

        /// USAGE->USHORT->unsigned short
        public ushort UsagePage;

        /// USHORT->unsigned short
        public ushort InputReportByteLength;

        /// USHORT->unsigned short
        public ushort OutputReportByteLength;

        /// USHORT->unsigned short
        public ushort FeatureReportByteLength;

        /// USHORT[17]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 17, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U2)]
        public ushort[] Reserved;

        /// USHORT->unsigned short
        public ushort NumberLinkCollectionNodes;

        /// USHORT->unsigned short
        public ushort NumberInputButtonCaps;

        /// USHORT->unsigned short
        public ushort NumberInputValueCaps;

        /// USHORT->unsigned short
        public ushort NumberInputDataIndices;

        /// USHORT->unsigned short
        public ushort NumberOutputButtonCaps;

        /// USHORT->unsigned short
        public ushort NumberOutputValueCaps;

        /// USHORT->unsigned short
        public ushort NumberOutputDataIndices;

        /// USHORT->unsigned short
        public ushort NumberFeatureButtonCaps;

        /// USHORT->unsigned short
        public ushort NumberFeatureValueCaps;

        /// USHORT->unsigned short
        public ushort NumberFeatureDataIndices;
    }

    /// <summary>
    /// Type created in place of an anonymous struct
    /// </summary>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct HIDP_BUTTON_CAPS_RANGE
    {

        /// USAGE->USHORT->unsigned short
        public ushort UsageMin;

        /// USAGE->USHORT->unsigned short
        public ushort UsageMax;

        /// USHORT->unsigned short
        public ushort StringMin;

        /// USHORT->unsigned short
        public ushort StringMax;

        /// USHORT->unsigned short
        public ushort DesignatorMin;

        /// USHORT->unsigned short
        public ushort DesignatorMax;

        /// USHORT->unsigned short
        public ushort DataIndexMin;

        /// USHORT->unsigned short
        public ushort DataIndexMax;
    }

    /// <summary>
    /// Type created in place of an anonymous struct
    /// </summary>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct HIDP_BUTTON_CAPS_NOT_RANGE
    {

        /// USAGE->USHORT->unsigned short
        public ushort Usage;

        /// USAGE->USHORT->unsigned short
        public ushort Reserved1;

        /// USHORT->unsigned short
        public ushort StringIndex;

        /// USHORT->unsigned short
        public ushort Reserved2;

        /// USHORT->unsigned short
        public ushort DesignatorIndex;

        /// USHORT->unsigned short
        public ushort Reserved3;

        /// USHORT->unsigned short
        public ushort DataIndex;

        /// USHORT->unsigned short
        public ushort Reserved4;
    }

    /// <summary>
    /// 
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Explicit, Pack = 1)]
    public struct HIDP_BUTTON_CAPS
    {
        /// USAGE->USHORT->unsigned short
        [FieldOffset(0)]
        public ushort UsagePage;

        /// UCHAR->unsigned char
        [FieldOffset(2)]
        public byte ReportID;

        /// BOOLEAN->BYTE->unsigned char
        [FieldOffset(3)]
        [MarshalAsAttribute(UnmanagedType.U1)]
        public bool IsAlias;

        /// USHORT->unsigned short
        [FieldOffset(4)]
        public ushort BitField;

        /// USHORT->unsigned short
        [FieldOffset(6)]
        public ushort LinkCollection;

        /// USAGE->USHORT->unsigned short
        [FieldOffset(8)]
        public ushort LinkUsage;

        /// USAGE->USHORT->unsigned short
        [FieldOffset(10)]
        public ushort LinkUsagePage;

        /// BOOLEAN->BYTE->unsigned char
        [FieldOffset(12)]
        [MarshalAsAttribute(UnmanagedType.U1)]
        public bool IsRange;

        /// BOOLEAN->BYTE->unsigned char
        [FieldOffset(13)]
        [MarshalAsAttribute(UnmanagedType.U1)]
        public bool IsStringRange;

        /// BOOLEAN->BYTE->unsigned char
        [FieldOffset(14)]
        [MarshalAsAttribute(UnmanagedType.U1)]
        public bool IsDesignatorRange;

        /// BOOLEAN->BYTE->unsigned char
        [FieldOffset(15)]
        [MarshalAsAttribute(UnmanagedType.U1)]
        public bool IsAbsolute;

        /// ULONG[10]
        [FieldOffset(16)]
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.U4)]
        public uint[] Reserved;

        /// Union Range/NotRange
        [FieldOffset(56)]
        HIDP_BUTTON_CAPS_RANGE Range;

        [FieldOffset(56)]
        HIDP_BUTTON_CAPS_NOT_RANGE NotRange;       
    }

    /// <summary>
    /// Type created in place of an anonymous struct
    /// </summary>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct HIDP_VALUE_CAPS_RANGE
    {

        /// USAGE->USHORT->unsigned short
        public ushort UsageMin;

        /// USAGE->USHORT->unsigned short
        public ushort UsageMax;

        /// USHORT->unsigned short
        public ushort StringMin;

        /// USHORT->unsigned short
        public ushort StringMax;

        /// USHORT->unsigned short
        public ushort DesignatorMin;

        /// USHORT->unsigned short
        public ushort DesignatorMax;

        /// USHORT->unsigned short
        public ushort DataIndexMin;

        /// USHORT->unsigned short
        public ushort DataIndexMax;
    }

    /// <summary>
    /// Type created in place of an anonymous struct
    /// </summary>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct HIDP_VALUE_CAPS_NOT_RANGE
    {

        /// USAGE->USHORT->unsigned short
        public ushort Usage;

        /// USAGE->USHORT->unsigned short
        public ushort Reserved1;

        /// USHORT->unsigned short
        public ushort StringIndex;

        /// USHORT->unsigned short
        public ushort Reserved2;

        /// USHORT->unsigned short
        public ushort DesignatorIndex;

        /// USHORT->unsigned short
        public ushort Reserved3;

        /// USHORT->unsigned short
        public ushort DataIndex;

        /// USHORT->unsigned short
        public ushort Reserved4;
    }


    /// <summary>
    /// 
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Explicit, Pack = 1)]
    public struct HIDP_VALUE_CAPS
    {

        /// USAGE->USHORT->unsigned short
        [FieldOffset(0)]
        public ushort UsagePage;

        /// UCHAR->unsigned char
        [FieldOffset(2)]
        public byte ReportID;

        /// BOOLEAN->BYTE->unsigned char
        [FieldOffset(3)]
        [MarshalAsAttribute(UnmanagedType.U1)]
        public bool IsAlias;

        /// USHORT->unsigned short
        [FieldOffset(4)]
        public ushort BitField;

        /// USHORT->unsigned short
        [FieldOffset(6)]
        public ushort LinkCollection;

        /// USAGE->USHORT->unsigned short
        [FieldOffset(8)]
        public ushort LinkUsage;

        /// USAGE->USHORT->unsigned short
        [FieldOffset(10)]
        public ushort LinkUsagePage;

        /// BOOLEAN->BYTE->unsigned char
        [FieldOffset(12)]
        [MarshalAsAttribute(UnmanagedType.U1)]
        public bool IsRange;

        /// BOOLEAN->BYTE->unsigned char
        [FieldOffset(13)]
        [MarshalAsAttribute(UnmanagedType.U1)]
        public bool IsStringRange;

        /// BOOLEAN->BYTE->unsigned char
        [FieldOffset(14)]
        [MarshalAsAttribute(UnmanagedType.U1)]
        public bool IsDesignatorRange;

        /// BOOLEAN->BYTE->unsigned char
        [FieldOffset(15)]
        [MarshalAsAttribute(UnmanagedType.U1)]
        public bool IsAbsolute;

        /// BOOLEAN->BYTE->unsigned char
        [FieldOffset(16)]
        [MarshalAsAttribute(UnmanagedType.U1)]
        public bool HasNull;

        /// UCHAR->unsigned char
        [FieldOffset(17)]
        public byte Reserved;

        /// USHORT->unsigned short
        [FieldOffset(18)]
        public ushort BitSize;

        /// USHORT->unsigned short
        [FieldOffset(20)]
        public ushort ReportCount;

        /// USHORT[5]
        /// We had to use 5 ushorts instead of an array to avoid alignment exception issues.
        [FieldOffset(22)]
        public ushort Reserved21;
        [FieldOffset(24)]
        public ushort Reserved22;
        [FieldOffset(26)]
        public ushort Reserved23;
        [FieldOffset(28)]
        public ushort Reserved24;
        [FieldOffset(30)]
        public ushort Reserved25;

        /// ULONG->unsigned int
        [FieldOffset(32)]
        public uint UnitsExp;

        /// ULONG->unsigned int
        [FieldOffset(36)]
        public uint Units;

        /// LONG->int
        [FieldOffset(40)]
        public int LogicalMin;

        /// LONG->int
        [FieldOffset(44)]
        public int LogicalMax;

        /// LONG->int
        [FieldOffset(48)]
        public int PhysicalMin;

        /// LONG->int
        [FieldOffset(52)]
        public int PhysicalMax;

        /// Union Range/NotRange
        [FieldOffset(56)]
        public HIDP_VALUE_CAPS_RANGE Range;

        [FieldOffset(56)]
        public HIDP_VALUE_CAPS_NOT_RANGE NotRange;
    }
    
}


