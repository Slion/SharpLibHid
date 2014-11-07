using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Hid.UsageTables;
using Win32;

namespace Devices.RemoteControl
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

			if (!Function.RegisterRawInputDevices(rid,
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
            Debug.WriteLine("================WM_INPUT================");

			uint dwSize = 0;

            uint sizeOfHeader=(uint)Marshal.SizeOf(typeof(RAWINPUTHEADER));

            //Get the size of our raw input data.
			Win32.Function.GetRawInputData(message.LParam,	RID_INPUT, IntPtr.Zero,	ref dwSize,	sizeOfHeader);

            //Allocate a large enough buffer
			IntPtr rawInputBuffer = Marshal.AllocHGlobal((int) dwSize);
			try
			{
				if(rawInputBuffer == IntPtr.Zero)
					return;

                //Now read our RAWINPUT data
                if (Win32.Function.GetRawInputData(message.LParam, RID_INPUT, rawInputBuffer, ref dwSize, (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) != dwSize)
				{
					return;
				}

                //Cast our buffer
                RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(rawInputBuffer, typeof(RAWINPUT));

                //Get Device Info
                uint deviceInfoSize = (uint)Marshal.SizeOf(typeof(RID_DEVICE_INFO));
                IntPtr deviceInfoBuffer = Marshal.AllocHGlobal((int)deviceInfoSize);

                int res = Win32.Function.GetRawInputDeviceInfo(raw.header.hDevice, Const.RIDI_DEVICEINFO, deviceInfoBuffer, ref deviceInfoSize);
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
                    case Const.RIM_TYPEHID:
                        Debug.WriteLine("WM_INPUT source device is HID.");
                        break;
                    case Const.RIM_TYPEMOUSE:
                        Debug.WriteLine("WM_INPUT source device is Mouse.");
                        return;
                    case Const.RIM_TYPEKEYBOARD:
                        Debug.WriteLine("WM_INPUT source device is Keyboard.");
                        return;
                    default:
                        Debug.WriteLine("WM_INPUT source device is Unknown.");
                        return;
                }

                //Get Usage Page and Usage
                Debug.WriteLine("Usage Page: 0x" + deviceInfo.hid.usUsagePage.ToString("X4") + " Usage: 0x" + deviceInfo.hid.usUsage.ToString("X4"));

                //Check that our raw input is HID
                if (raw.header.dwType == Const.RIM_TYPEHID && raw.hid.dwSizeHid>0)
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
                    if (deviceInfo.hid.usUsagePage != (ushort)Hid.UsagePage.MceRemote || deviceInfo.hid.usUsage != (ushort)Hid.UsageId.MceRemoteUsage)
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
				else if(raw.header.dwType == Const.RIM_TYPEMOUSE)
				{
					// do mouse handling...
				}
				else if(raw.header.dwType == Const.RIM_TYPEKEYBOARD)
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
