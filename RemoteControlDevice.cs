using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace Devices.RemoteControl
{

    public static class Hid
    {
        /// <summary>
        /// From USB HID usage tables.
        /// http://www.usb.org/developers/hidpage#HID_Usage
        /// http://www.usb.org/developers/devclass_docs/Hut1_12v2.pdf
        /// </summary>
        public enum UsagePage : ushort
        {
            Undefined = 0,
            GenericDesktopControl,
            SimulationControl,
            VirtualRealityControl,
            SportControl,
            GameControl,
            GenericDeviceControl,
            Keyboard,
            LightEmittingDiode,
            Button,
            Ordinal,
            Telephony,
            Consumer,
            Digitiser,
            PhysicalInterfaceDevice = 0x0f,
            Unicode = 0x10,
            AlphaNumericDisplay = 0x14,
            MedicalInstruments = 0x40,
            MonitorPage0 = 0x80,
            MonitorPage1,
            MonitorPage2,
            MonitorPage3,
            PowerPage0,
            PowerPage1,
            PowerPage2,
            PowerPage3,
            BarCodeScanner = 0x8c,
            Scale,
            MagneticStripeReader,
            ReservedPointOfSale,
            CameraControl,
            Arcade,
            // http://msdn.microsoft.com/en-us/library/windows/desktop/bb417079.aspx
            MceRemote = 0xffbc,
            TerraTecRemote = 0xffcc
        }

        public const ushort MceRemoteUsage = 0x88;
    }


	public enum InputDevice
	{
		Key,
		Mouse,
		OEM
	}


	public enum RemoteControlButton
	{
		Clear,
		Down,
		Left,
		Digit0,
		Digit1,
		Digit2,
		Digit3,
		Digit4,
		Digit5,
		Digit6,
		Digit7,
		Digit8,
		Digit9,
		Enter,
		Right,
		Up,

		Back,
		ChannelDown,
		ChannelUp,
		FastForward,
		VolumeMute,
		Pause,
		Play,
        PlayPause,
		Record,
		PreviousTrack,
		Rewind,
		NextTrack,
		Stop,
		VolumeDown,
		VolumeUp,

		RecordedTV,
		Guide,
		LiveTV,
		Details,
		DVDMenu,
		DVDAngle,
		DVDAudio,
		DVDSubtitle,
		MyMusic,
		MyPictures,
		MyVideos,
		MyTV,
		OEM1,
		OEM2,
		StandBy,
		TVJump,

		Unknown
	}

    /// <summary>
    ///
    /// </summary>
    public enum MceButton
    {
        /// <summary>
        /// Not defined by the Microsoft specs.
        /// </summary>
        Null                    =   0x00,
        GreenStart              =   0x0D,
        ClosedCaptioning        =   0x2B,
        Teletext                =   0x5A,
        TeletextRed             =   0x5B,
        TeletextGreen           =   0x5C,
        TeletextYellow          =   0x5D,
        TeletextBlue            =   0x5E,
        LiveTv                  =   0x25,
        Music                   =   0x47,
        RecordedTv              =   0x48,
        Pictures                =   0x49,
        Videos                  =   0x4A,
        FmRadio                 =   0x50,
        Extras                  =   0x3C,
        ExtrasApp               =   0x3D,
        DvdMenu                 =   0x24,
        DvdAngle                =   0x4B,
        DvdAudio                =   0x4C,
        DvdSubtitle             =   0x4D,
        /// <summary>
        /// First press action: Ejects a DVD drive.
        /// <para />
        /// Second press action: Repeats first press action.
        /// <para />
        /// Notably issued by XBOX360 remote as defined in irplus - Remote Control - Android application.
        /// </summary>
        Eject                   =   0x28,
        DvdTopMenu              =   0x43,
        /// <summary>
        /// First press action: Generates EXTn HID message in the Media Center Vendor Specific
        /// Collection (page 0xFFBC, usage 0x88).
        /// <para />
        /// Second press action: Repeats message.
        /// <para />
        /// Auto-repeat: No
        /// <para />
        /// Notably sent by the 'Visualization' button of HP Windows Media Center Remote (TSGH-IR08).
        /// <para />
        /// According to HP specs it displays visual imagery that is synchronized to the sound of your music tracks.
        /// </summary>
        Ext0                    =   0x32,
        /// <summary>
        /// First press action: Generates EXTn HID message in the Media Center Vendor Specific
        /// Collection (page 0xFFBC, usage 0x88).
        /// <para />
        /// Second press action: Repeats message.
        /// <para />
        /// Auto-repeat: No
        /// <para />
        /// Notably sent by the 'Slide Show' button of HP Windows Media Center Remote (TSGH-IR08).
        /// <para />
        /// According to HP specs it plays a slide show of all the pictures on your hard disk drive.
        /// </summary>
        Ext1                    =   0x33,
        /// <summary>
        /// First press action: Generates EXTn HID message in the Media Center Vendor Specific
        /// Collection (page 0xFFBC, usage 0x88).
        /// <para />
        /// Second press action: Repeats message.
        /// <para />
        /// Auto-repeat: No
        /// <para />
        /// Notably sent by the 'Eject' button of HP Windows Media Center Remote (TSGH-IR08).
        /// Also interpreted as 'Eject' action by SoundGraph iMON Manager in MCE mode (OrigenAE VF310).
        /// </summary>
        Ext2                    =   0x34,
        /// <summary>
        /// First press action: Generates EXTn HID message in the Media Center Vendor Specific
        /// Collection (page 0xFFBC, usage 0x88).
        /// <para />
        /// Second press action: Repeats message.
        /// <para />
        /// Auto-repeat: No
        /// <para />
        /// Notably sent by the 'Input selection' button of HP Windows Media Center Remote (TSGH-IR08).
        /// </summary>
        Ext3                    =   0x35,
        Ext4                    =   0x36,
        Ext5                    =   0x37,
        Ext6                    =   0x38,
        Ext7                    =   0x39,
        Ext8                    =   0x3A,
        Ext9                    =   0x80,
        Ext10                   =   0x81,
        Ext11                   =   0x6F,
        Zoom                    =   0x27,
        ChannelInput            =   0x42,
        SubAudio                =   0x2D,
        Channel10               =   0x3E,
        Channel11               =   0x3F,
        Channel12               =   0x40,
        /// <summary>
        /// First press action: Generates OEM2 HID message in the Media Center Vendor Specific
        /// Collection. This button is intended to control the front panel display of home entertainment
        /// computers. When this button is pressed, the display could be turned on or off, or the display
        /// mode could change.
        /// <para />
        /// Second press action: Repeats message.
        /// <para />
        /// Auto-repeat: No
        /// <para />
        /// Notably issued by XBOX360 remote as defined in irplus - Remote Control - Android application.
        /// </summary>
        Display                 =   0x4F,
        /// <summary>
        /// First press action: To be determined.
        /// <para />
        /// Second press action: Repeats message.
        /// <para />
        /// Auto-repeat: No
        /// </summary>
        Kiosk                   =   0x6A,
        NetworkSelection        =   0x2C,
        BlueRayTool             =   0x78,
        ChannelInfo             =   0x41,
        VideoSelection          =   0x61
    }

    public enum HpMceButton
    {
        /// <summary>
        /// Displays visual imagery that is synchronized to the sound of your music tracks.
        /// <para />
        /// Second press action: Repeats message.
        /// <para />
        /// Auto-repeat: No
        /// <para />
        /// Notably sent by the 'Visualization' button of HP Windows Media Center Remote (TSGH-IR08).
        /// <para />
        /// According to HP specs it displays visual imagery that is synchronized to the sound of your music tracks.
        /// </summary>
        Visualization = MceButton.Ext0,
        /// <summary>
        /// Plays a slide show of all the pictures on your hard disk drive.
        /// <para />
        /// Second press action: Repeats message.
        /// <para />
        /// Auto-repeat: No
        /// <para />
        /// Notably sent by the 'Slide Show' button of HP Windows Media Center Remote (TSGH-IR08).
        /// <para />
        /// According to HP specs it plays a slide show of all the pictures on your hard disk drive.
        /// </summary>
        SlideShow = MceButton.Ext1,
        /// <summary>
        /// Eject optical drive.
        /// <para />
        /// Second press action: Repeats message.
        /// <para />
        /// Auto-repeat: No
        /// <para />
        /// Notably sent by the 'Eject' button of HP Windows Media Center Remote (TSGH-IR08).
        /// Also interpreted as 'Eject' action by SoundGraph iMON Manager in MCE mode (OrigenAE VF310).
        /// </summary>
        Eject = MceButton.Ext2,
        /// <summary>
        /// Not sure what this should do.
        /// <para />
        /// Second press action: Repeats message.
        /// <para />
        /// Auto-repeat: No
        /// <para />
        /// Notably sent by the 'Input selection' button of HP Windows Media Center Remote (TSGH-IR08).
        /// </summary>
        InputSelection = MceButton.Ext3,
    }



	#region RemoteControlEventArgs

	public class RemoteControlEventArgs : EventArgs
	{
        RemoteControlButton _rcb;
		InputDevice _device;
        MceButton iMceButton;

        public RemoteControlEventArgs(RemoteControlButton rcb, InputDevice device)
		{
            iMceButton = MceButton.Null;
			_rcb = rcb;
			_device = device;
		}

        public RemoteControlEventArgs(MceButton mce, InputDevice device)
        {
            iMceButton = mce;
            _rcb = RemoteControlButton.Unknown;
            _device = device;
        }

		public RemoteControlEventArgs()
		{
            iMceButton = MceButton.Null;
			_rcb = RemoteControlButton.Unknown;
			_device = InputDevice.Key;
		}

		public RemoteControlButton Button
		{
			get { return _rcb;  }
			set { _rcb = value; }
		}

        public MceButton MceButton
        {
            get { return iMceButton; }
            set { iMceButton = value; }
        }

		public InputDevice Device
		{
			get { return _device;  }
			set { _device = value; }
		}
	}

	#endregion RemoteControlEventArgs


	public sealed class RemoteControlDevice
	{

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct RAWINPUTDEVICE
		{
			[MarshalAs(UnmanagedType.U2)]
			public ushort usUsagePage;
			[MarshalAs(UnmanagedType.U2)]
			public ushort usUsage;
			[MarshalAs(UnmanagedType.U4)]
			public int	 dwFlags;
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
			[FieldOffset (0)] public ushort usFlags;
			[MarshalAs(UnmanagedType.U4)]
			[FieldOffset (4)] public uint ulButtons;
			[FieldOffset (4)] public BUTTONSSTR buttonsStr;
			[MarshalAs(UnmanagedType.U4)]
			[FieldOffset (8)] public uint ulRawButtons;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset (12)] public int lLastX;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset (16)] public int lLastY;
			[MarshalAs(UnmanagedType.U4)]
			[FieldOffset (20)] public uint ulExtraInformation;
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


		[StructLayout(LayoutKind.Explicit, Pack=1)]
		internal struct RAWINPUT
		{
			[FieldOffset  (0)] public RAWINPUTHEADER header;
			[FieldOffset (16)] public RAWMOUSE mouse;
			[FieldOffset (16)] public RAWKEYBOARD keyboard;
			[FieldOffset (16)] public RAWHID hid;
		}


        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct RID_DEVICE_INFO_MOUSE
		{
            public uint dwId;
            public uint dwNumberOfButtons;
            public uint dwSampleRate;
            public bool fHasHorizontalWheel;
		}


        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct RID_DEVICE_INFO_KEYBOARD
		{
            public uint dwType;
            public uint dwSubType;
            public uint dwKeyboardMode;
            public uint dwNumberOfFunctionKeys;
            public uint dwNumberOfIndicators;
            public uint dwNumberOfKeysTotal;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct RID_DEVICE_INFO_HID
		{
            public uint dwVendorId;
            public uint dwProductId;
            public uint dwVersionNumber;
            public ushort usUsagePage;
            public ushort usUsage;
        }

        [StructLayout(LayoutKind.Explicit, Pack=1)]
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



		[DllImport("User32.dll")]
		extern static bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);

		[DllImport("User32.dll")]
		extern static uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

   		[DllImport("User32.dll", SetLastError=true)]
		extern static int GetRawInputDeviceInfo(IntPtr hDevice, uint uiCommand, IntPtr pData, ref uint pcbSize);


		private const int WM_KEYDOWN	= 0x0100;
		private const int WM_APPCOMMAND	= 0x0319;
		private const int WM_INPUT		= 0x00FF;

		private const int APPCOMMAND_BROWSER_BACKWARD   = 1;
		private const int APPCOMMAND_VOLUME_MUTE        = 8;
		private const int APPCOMMAND_VOLUME_DOWN        = 9;
		private const int APPCOMMAND_VOLUME_UP          = 10;
		private const int APPCOMMAND_MEDIA_NEXTTRACK    = 11;
		private const int APPCOMMAND_MEDIA_PREVIOUSTRACK = 12;
		private const int APPCOMMAND_MEDIA_STOP         = 13;
		private const int APPCOMMAND_MEDIA_PLAY_PAUSE   = 14;
		private const int APPCOMMAND_MEDIA_PLAY         = 46;
		private const int APPCOMMAND_MEDIA_PAUSE        = 47;
		private const int APPCOMMAND_MEDIA_RECORD       = 48;
		private const int APPCOMMAND_MEDIA_FAST_FORWARD = 49;
		private const int APPCOMMAND_MEDIA_REWIND       = 50;
		private const int APPCOMMAND_MEDIA_CHANNEL_UP   = 51;
		private const int APPCOMMAND_MEDIA_CHANNEL_DOWN = 52;

		private const int RID_INPUT						= 0x10000003;
		private const int RID_HEADER					= 0x10000005;

		private const int FAPPCOMMAND_MASK				= 0xF000;
		private const int FAPPCOMMAND_MOUSE				= 0x8000;
		private const int FAPPCOMMAND_KEY				= 0;
		private const int FAPPCOMMAND_OEM				= 0x1000;

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





		public delegate void RemoteControlDeviceEventHandler(object sender, RemoteControlEventArgs e);
		public event RemoteControlDeviceEventHandler ButtonPressed;


		//-------------------------------------------------------------
		// constructors
		//-------------------------------------------------------------

		public RemoteControlDevice()
		{
			// Register the input device to receive the commands from the
			// remote device. See http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwmt/html/remote_control.asp
			// for the vendor defined usage page.

			RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[3];

			rid[0].usUsagePage = 0xFFBC;
			rid[0].usUsage = 0x88;
			rid[0].dwFlags = 0;

			rid[1].usUsagePage = 0x0C;
			rid[1].usUsage = 0x01;
			rid[1].dwFlags = 0;

			rid[2].usUsagePage = 0x0C;
			rid[2].usUsage = 0x80;
			rid[2].dwFlags = 0;

			if (!RegisterRawInputDevices(rid,
				(uint) rid.Length,
				(uint) Marshal.SizeOf(rid[0]))
				)
			{
				throw new ApplicationException("Failed to register raw input devices.");
			}
		}


		//-------------------------------------------------------------
		// methods
		//-------------------------------------------------------------

		public void ProcessMessage(Message message)
		{
			int param;

			switch (message.Msg)
			{
				case WM_KEYDOWN:
					param = message.WParam.ToInt32();
					ProcessKeyDown(param);
					break;
				case WM_APPCOMMAND:
					param = message.LParam.ToInt32();
					ProcessAppCommand(param);
					break;
				case WM_INPUT:
					ProcessInputCommand(ref message);
                    message.Result = new IntPtr(0);
					break;
			}

		}


		//-------------------------------------------------------------
		// methods (helpers)
		//-------------------------------------------------------------

		private void ProcessKeyDown(int param)
		{
			RemoteControlButton rcb = RemoteControlButton.Unknown;

			switch (param)
			{
				case (int) Keys.Escape:
					rcb = RemoteControlButton.Clear;
					break;
				case (int) Keys.Down:
					rcb = RemoteControlButton.Down;
					break;
				case (int) Keys.Left:
					rcb = RemoteControlButton.Left;
					break;
				case (int) Keys.D0:
					rcb = RemoteControlButton.Digit0;
					break;
				case (int) Keys.D1:
					rcb = RemoteControlButton.Digit1;
					break;
				case (int) Keys.D2:
					rcb = RemoteControlButton.Digit2;
					break;
				case (int) Keys.D3:
					rcb = RemoteControlButton.Digit3;
					break;
				case (int) Keys.D4:
					rcb = RemoteControlButton.Digit4;
					break;
				case (int) Keys.D5:
					rcb = RemoteControlButton.Digit5;
					break;
				case (int) Keys.D6:
					rcb = RemoteControlButton.Digit6;
					break;
				case (int) Keys.D7:
					rcb = RemoteControlButton.Digit7;
					break;
				case (int) Keys.D8:
					rcb = RemoteControlButton.Digit8;
					break;
				case (int) Keys.D9:
					rcb = RemoteControlButton.Digit9;
					break;
				case (int) Keys.Enter:
					rcb = RemoteControlButton.Enter;
					break;
				case (int) Keys.Right:
					rcb = RemoteControlButton.Right;
					break;
				case (int) Keys.Up:
					rcb = RemoteControlButton.Up;
					break;
			}

			if (this.ButtonPressed != null && rcb != RemoteControlButton.Unknown)
				this.ButtonPressed(this, new RemoteControlEventArgs(rcb, GetDevice(param)));
		}


		private void ProcessAppCommand(int param)
		{
			RemoteControlButton rcb = RemoteControlButton.Unknown;

			int cmd	= (int) (((ushort) (param >> 16)) & ~FAPPCOMMAND_MASK);

			switch (cmd)
			{
				case APPCOMMAND_BROWSER_BACKWARD:
					rcb = RemoteControlButton.Back;
					break;
				case APPCOMMAND_MEDIA_CHANNEL_DOWN:
					rcb = RemoteControlButton.ChannelDown;
					break;
				case APPCOMMAND_MEDIA_CHANNEL_UP:
					rcb = RemoteControlButton.ChannelUp;
					break;
				case APPCOMMAND_MEDIA_FAST_FORWARD:
					rcb = RemoteControlButton.FastForward;
					break;
				case APPCOMMAND_VOLUME_MUTE:
					rcb = RemoteControlButton.VolumeMute;
					break;
				case APPCOMMAND_MEDIA_PAUSE:
					rcb = RemoteControlButton.Pause;
					break;
				case APPCOMMAND_MEDIA_PLAY:
					rcb = RemoteControlButton.Play;
					break;
                case APPCOMMAND_MEDIA_PLAY_PAUSE:
                    rcb = RemoteControlButton.PlayPause;
                    break;
				case APPCOMMAND_MEDIA_RECORD:
					rcb = RemoteControlButton.Record;
					break;
				case APPCOMMAND_MEDIA_PREVIOUSTRACK:
					rcb = RemoteControlButton.PreviousTrack;
					break;
				case APPCOMMAND_MEDIA_REWIND:
					rcb = RemoteControlButton.Rewind;
					break;
				case APPCOMMAND_MEDIA_NEXTTRACK:
					rcb = RemoteControlButton.NextTrack;
					break;
				case APPCOMMAND_MEDIA_STOP:
					rcb = RemoteControlButton.Stop;
					break;
				case APPCOMMAND_VOLUME_DOWN:
					rcb = RemoteControlButton.VolumeDown;
					break;
				case APPCOMMAND_VOLUME_UP:
					rcb = RemoteControlButton.VolumeUp;
					break;
			}

			if (this.ButtonPressed != null && rcb != RemoteControlButton.Unknown)
				this.ButtonPressed(this, new RemoteControlEventArgs(rcb, GetDevice(param)));
		}


		private void ProcessInputCommand(ref Message message)
		{



			uint dwSize = 0;

            uint sizeOfHeader=(uint)Marshal.SizeOf(typeof(RAWINPUTHEADER));

            //Get the size of our raw input data.
			GetRawInputData(message.LParam,	RID_INPUT, IntPtr.Zero,	ref dwSize,	sizeOfHeader);

            //Allocate a large enough buffer
			IntPtr rawInputBuffer = Marshal.AllocHGlobal((int) dwSize);
			try
			{
				if(rawInputBuffer == IntPtr.Zero)
					return;

                //Now read our RAWINPUT data
				if (GetRawInputData(message.LParam,	RID_INPUT, rawInputBuffer, ref dwSize, (uint) Marshal.SizeOf(typeof(RAWINPUTHEADER))) != dwSize)
				{
					return;
				}

                //Cast our buffer
                RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(rawInputBuffer, typeof(RAWINPUT));

                //Get Device Info
                uint deviceInfoSize = (uint)Marshal.SizeOf(typeof(RID_DEVICE_INFO));
                IntPtr deviceInfoBuffer = Marshal.AllocHGlobal((int)deviceInfoSize);

                int res = GetRawInputDeviceInfo(raw.header.hDevice, RIDI_DEVICEINFO, deviceInfoBuffer, ref deviceInfoSize);
                if (res <= 0)
                {
                    Debug.WriteLine("WM_INPUT could not read device info: " + Marshal.GetLastWin32Error().ToString());
                    return;
                }

                //Cast our buffer
                RID_DEVICE_INFO deviceInfo = (RID_DEVICE_INFO)Marshal.PtrToStructure(deviceInfoBuffer, typeof(RID_DEVICE_INFO));

                //Check type of input device and quite if we don't like it
                switch (deviceInfo.dwType)
                {
                    case RIM_TYPEHID:
                        Debug.WriteLine("WM_INPUT source device is HID.");
                        break;
                    case RIM_TYPEMOUSE:
                        Debug.WriteLine("WM_INPUT source device is Mouse.");
                        return;
                    case RIM_TYPEKEYBOARD:
                        Debug.WriteLine("WM_INPUT source device is Keyboard.");
                        return;
                    default:
                        Debug.WriteLine("WM_INPUT source device is Unknown.");
                        return;
                }

                //Get Usage Page and Usage
                Debug.WriteLine("Usage Page: 0x" + deviceInfo.hid.usUsagePage.ToString("X4") + " Usage: 0x" + deviceInfo.hid.usUsage.ToString("X4"));

                //Check that our raw input is HID
                if (raw.header.dwType == RIM_TYPEHID && raw.hid.dwSizeHid>0)
				{
                    //Allocate a buffer for one HID message
					byte[] bRawData = new byte[raw.hid.dwSizeHid];

                    //Compute the address from which to copy our HID message
                    int pRawData = 0;
                    unsafe
                    {
                        byte* source = (byte*)rawInputBuffer;
                        source += sizeof(RAWINPUTHEADER) + sizeof(RAWHID);
                        pRawData = (int)source;
                    }

                    //Copy HID message into our buffer
                    Marshal.Copy(new IntPtr(pRawData), bRawData, 0, raw.hid.dwSizeHid);
                    //bRawData[0] //Not sure what's the meaning of the code at offset 0
                    //TODO: check size before access
                    int rawData = bRawData[1]; //Get button code
                    //Print HID codes in our debug output
                    string hidDump = "HID " + raw.hid.dwCount + "/" + raw.hid.dwSizeHid + ":";
                    foreach (byte b in bRawData)
                    {
                        hidDump += b.ToString("X2");
                    }
                    Debug.WriteLine(hidDump);

                    //Make sure both usage page and usage are matching MCE remote
                    if (deviceInfo.hid.usUsagePage != (ushort)Hid.UsagePage.MceRemote || deviceInfo.hid.usUsage != (ushort)Hid.MceRemoteUsage)
                    {
                        Debug.WriteLine("Not MCE remote page and usage.");
                        return;
                    }

                    if (Enum.IsDefined(typeof(MceButton), rawData) && rawData!=0) //Our button is a known MCE button
                    {
                        if (this.ButtonPressed != null) //What's that?
                        {
                            this.ButtonPressed(this, new RemoteControlEventArgs((MceButton)rawData, GetDevice(message.LParam.ToInt32())));
                        }
                    }
				}
				else if(raw.header.dwType == RIM_TYPEMOUSE)
				{
					// do mouse handling...
				}
				else if(raw.header.dwType == RIM_TYPEKEYBOARD)
				{
					// do keyboard handling...
				}
			}
			finally
			{
				Marshal.FreeHGlobal(rawInputBuffer);
			}
		}


		private InputDevice GetDevice(int param)
		{
			InputDevice inputDevice;

			switch ((int) (((ushort) (param >> 16)) & FAPPCOMMAND_MASK))
			{
				case FAPPCOMMAND_OEM:
					inputDevice = InputDevice.OEM;
					break;
				case FAPPCOMMAND_MOUSE:
					inputDevice = InputDevice.Mouse;
					break;
				default:
					inputDevice = InputDevice.Key;
					break;
			}

			return inputDevice;
		}
	}
}
