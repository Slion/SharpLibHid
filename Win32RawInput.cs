using System;
using System.Runtime.InteropServices;

namespace Win32
{

    static partial class Function
    {
        [DllImport("User32.dll", SetLastError = true)]
		public extern static bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);

        [DllImport("User32.dll", SetLastError = true)]
		public extern static uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

   		[DllImport("User32.dll", SetLastError=true)]
		public extern static int GetRawInputDeviceInfo(IntPtr hDevice, uint uiCommand, IntPtr pData, ref uint pcbSize);
    }

    static partial class Const
    {
        /// <summary>
        /// GetRawInputDeviceInfo pData points to a string that contains the device name.
        /// </summary>
        public const uint RIDI_DEVICENAME = 0x20000007;
        /// <summary>
        /// GetRawInputDeviceInfo For this uiCommand only, the value in pcbSize is the character count (not the byte count).
        /// </summary>
        public const uint RIDI_DEVICEINFO = 0x2000000b;
        /// <summary>
        /// GetRawInputDeviceInfo pData points to an RID_DEVICE_INFO structure.
        /// </summary>
        public const uint RIDI_PREPARSEDDATA = 0x20000005;


        /// <summary>
        /// Data comes from a mouse.
        /// </summary>
        public const uint RIM_TYPEMOUSE = 0;
        /// <summary>
        /// Data comes from a keyboard.
        /// </summary>
        public const uint RIM_TYPEKEYBOARD = 1;
        /// <summary>
        /// Data comes from an HID that is not a keyboard or a mouse.
        /// </summary>
        public const uint RIM_TYPEHID = 2;

    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct RAWINPUTDEVICE
    {
        [MarshalAs(UnmanagedType.U2)]
        public ushort usUsagePage;
        [MarshalAs(UnmanagedType.U2)]
        public ushort usUsage;
        [MarshalAs(UnmanagedType.U4)]
        public int dwFlags;
        public IntPtr hwndTarget;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct RAWINPUTHEADER
    {
        [MarshalAs(UnmanagedType.U4)]
        public int dwType;
        [MarshalAs(UnmanagedType.U4)]
        public int dwSize;
        public IntPtr hDevice;
        [MarshalAs(UnmanagedType.U4)]
        public int wParam;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct RAWHID
    {
        [MarshalAs(UnmanagedType.U4)]
        public int dwSizeHid;
        [MarshalAs(UnmanagedType.U4)]
        public int dwCount;
        //
        //BYTE  bRawData[1];
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct BUTTONSSTR
    {
        [MarshalAs(UnmanagedType.U2)]
        public ushort usButtonFlags;
        [MarshalAs(UnmanagedType.U2)]
        public ushort usButtonData;
    }


    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct RAWMOUSE
    {
        [MarshalAs(UnmanagedType.U2)]
        [FieldOffset(0)]
        public ushort usFlags;
        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(4)]
        public uint ulButtons;
        [FieldOffset(4)]
        public BUTTONSSTR buttonsStr;
        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(8)]
        public uint ulRawButtons;
        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(12)]
        public int lLastX;
        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(16)]
        public int lLastY;
        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(20)]
        public uint ulExtraInformation;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct RAWKEYBOARD
    {
        [MarshalAs(UnmanagedType.U2)]
        public ushort MakeCode;
        [MarshalAs(UnmanagedType.U2)]
        public ushort Flags;
        [MarshalAs(UnmanagedType.U2)]
        public ushort Reserved;
        [MarshalAs(UnmanagedType.U2)]
        public ushort VKey;
        [MarshalAs(UnmanagedType.U4)]
        public uint Message;
        [MarshalAs(UnmanagedType.U4)]
        public uint ExtraInformation;
    }


    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct RAWINPUT
    {
        [FieldOffset(0)]
        public RAWINPUTHEADER header;
        [FieldOffset(16)]
        public RAWMOUSE mouse;
        [FieldOffset(16)]
        public RAWKEYBOARD keyboard;
        [FieldOffset(16)]
        public RAWHID hid;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct RID_DEVICE_INFO_MOUSE
    {
        public uint dwId;
        public uint dwNumberOfButtons;
        public uint dwSampleRate;
        public bool fHasHorizontalWheel;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct RID_DEVICE_INFO_KEYBOARD
    {
        public uint dwType;
        public uint dwSubType;
        public uint dwKeyboardMode;
        public uint dwNumberOfFunctionKeys;
        public uint dwNumberOfIndicators;
        public uint dwNumberOfKeysTotal;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct RID_DEVICE_INFO_HID
    {
        public uint dwVendorId;
        public uint dwProductId;
        public uint dwVersionNumber;
        public ushort usUsagePage;
        public ushort usUsage;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct RID_DEVICE_INFO
    {
        [FieldOffset(0)]
        public uint cbSize;
        [FieldOffset(4)]
        public uint dwType;
        [FieldOffset(8)]
        public RID_DEVICE_INFO_MOUSE mouse;
        [FieldOffset(8)]
        public RID_DEVICE_INFO_KEYBOARD keyboard;
        [FieldOffset(8)]
        public RID_DEVICE_INFO_HID hid;
    }


}