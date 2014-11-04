using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace BruceThomas.Devices.RemoteControl
{
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
        Null                    =   0x00, //Not defined by the specs
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
        Eject                   =   0x28,
        DvdTopMenu              =   0x43,
        Ext0                    =   0x32,
        Ext1                    =   0x33,
        Ext2                    =   0x34,
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
        Display                 =   0x4F,
        Kiosk                   =   0x6A,
        NetworkSelection        =   0x2C,
        BlueRayTool             =   0x78,
        ChannelInfo             =   0x41,
        VideoSelection          =   0x61                
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


		[DllImport("User32.dll")]
		extern static bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);

		[DllImport("User32.dll")]
		extern static uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);


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

		private const int RIM_TYPEMOUSE					= 0;
		private const int RIM_TYPEKEYBOARD				= 1;
		private const int RIM_TYPEHID					= 2;

		private const int RID_INPUT						= 0x10000003;
		private const int RID_HEADER					= 0x10000005;

		private const int FAPPCOMMAND_MASK				= 0xF000;
		private const int FAPPCOMMAND_MOUSE				= 0x8000;
		private const int FAPPCOMMAND_KEY				= 0;
		private const int FAPPCOMMAND_OEM				= 0x1000;

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
			RemoteControlButton rcb = RemoteControlButton.Unknown;
			uint dwSize = 0;

            uint sizeOfHeader=(uint)Marshal.SizeOf(typeof(RAWINPUTHEADER));

            //Get the size of our raw input data.
			GetRawInputData(message.LParam,	RID_INPUT, IntPtr.Zero,	ref dwSize,	sizeOfHeader);

            //Allocate a large enough buffer
			IntPtr buffer = Marshal.AllocHGlobal((int) dwSize);
			try
			{
				if(buffer == IntPtr.Zero)
					return;

                //Now read our RAWINPUT data
				if (GetRawInputData(message.LParam,	RID_INPUT, buffer, ref dwSize, (uint) Marshal.SizeOf(typeof(RAWINPUTHEADER))) != dwSize)
				{
					return;
				}

                //Cast our buffer
                RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RAWINPUT));

                //Check that our raw input is HID
                if (raw.header.dwType == RIM_TYPEHID && raw.hid.dwSizeHid>0)
				{
                    //Allocate a buffer for one HID message
					byte[] bRawData = new byte[raw.hid.dwSizeHid];

                    //Compute the address from which to copy our HID message
                    int pRawData = 0;
                    unsafe
                    {
                        byte* source = (byte*)buffer;
                        source += sizeof(RAWINPUTHEADER) + sizeof(RAWHID);
                        //source += 1;
                        pRawData = (int)source;
                    }
                    //int pRawData = buffer.ToUint32() + Marshal.SizeOf(typeof(RAWINPUT)) + 1;

					//Marshal.Copy(new IntPtr(pRawData), bRawData, 0, raw.hid.dwSizeHid - 1);
                    Marshal.Copy(new IntPtr(pRawData), bRawData, 0, raw.hid.dwSizeHid);
					//int rawData = bRawData[0] | bRawData[1] << 8;
                    int rawData = bRawData[1]; //Get button code
                    Debug.WriteLine("HID " + raw.hid.dwCount + "/" + raw.hid.dwSizeHid + ":" + bRawData[0].ToString("X2") + bRawData[1].ToString("X2"));

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
				Marshal.FreeHGlobal(buffer);
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
