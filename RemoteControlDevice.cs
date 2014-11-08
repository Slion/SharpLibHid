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
		MoreInfo,
        Print,
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

		private const int FAPPCOMMAND_MASK				= 0xF000;
		private const int FAPPCOMMAND_MOUSE				= 0x8000;
		private const int FAPPCOMMAND_KEY				= 0;
		private const int FAPPCOMMAND_OEM				= 0x1000;



		public delegate void RemoteControlDeviceEventHandler(object sender, RemoteControlEventArgs e);
		public event RemoteControlDeviceEventHandler ButtonPressed;

        public delegate void HidUsageHandler(ushort aUsage);
        


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUsage"></param>
        private void HidConsumerDeviceHandler(ushort aUsage)
        {
            if (aUsage == 0)
            {
                //Just skip those
                return;
            }

            if (Enum.IsDefined(typeof(ConsumerControl), aUsage) && aUsage != 0) //Our button is a known consumer control
            {
                if (this.ButtonPressed != null)
                {
                    RemoteControlButton button=RemoteControlButton.Unknown;
                    if (aUsage== (ushort)ConsumerControl.AppCtrlProperties)
                    {
                        button = RemoteControlButton.MoreInfo;
                    }
                    else if (aUsage==(ushort)ConsumerControl.AppCtrlPrint)
                    {
                        button = RemoteControlButton.Print;
                    }
                    else if (aUsage==(ushort)ConsumerControl.MediaSelectProgramGuide)
                    {
                        button = RemoteControlButton.Guide;
                    }
                    this.ButtonPressed(this, new RemoteControlEventArgs(button, InputDevice.OEM));
                }
            }
            else
            {
                Debug.WriteLine("Unknown Consumer Control!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUsage"></param>
        private void HidMceRemoteHandler(ushort aUsage)
        {
            if (aUsage == 0)
            {
                //Just skip those
                return;
            }


            if (Enum.IsDefined(typeof(MceButton), aUsage) && aUsage != 0) //Our button is a known MCE button
            {
                if (this.ButtonPressed != null)
                {
                    this.ButtonPressed(this, new RemoteControlEventArgs((MceButton)aUsage, InputDevice.OEM));
                }
            }
            else
            {
                Debug.WriteLine("Unknown MCE button!");
            }

        }


		private void ProcessInputCommand(ref Message message)
		{
            Debug.WriteLine("================WM_INPUT================");


            //Declare a pointer
            IntPtr rawInputBuffer = IntPtr.Zero;

            try
            {
                //Fetch raw input
                RAWINPUT rawInput = new RAWINPUT();
                if (!RawInput.GetRawInputData(message.LParam, ref rawInput, ref rawInputBuffer))
                {
                    return;
                }

                //Fetch device info
                RID_DEVICE_INFO deviceInfo = new RID_DEVICE_INFO();
                if (!RawInput.GetDeviceInfo(rawInput.header.hDevice, ref deviceInfo))
                {
                    return;
                }
               

                if (rawInput.header.dwType == Const.RIM_TYPEHID)  //Check that our raw input is HID                        
                {
                    Debug.WriteLine("WM_INPUT source device is HID.");
                    //Get Usage Page and Usage
                    Debug.WriteLine("Usage Page: 0x" + deviceInfo.hid.usUsagePage.ToString("X4") + " Usage: 0x" + deviceInfo.hid.usUsage.ToString("X4"));

                    //
                    HidUsageHandler handler=null;

                    //Make sure both usage page and usage are matching MCE remote
                    //TODO: handle more that just MCE usage page.
                    if (deviceInfo.hid.usUsagePage == (ushort)Hid.UsagePage.MceRemote || deviceInfo.hid.usUsage == (ushort)Hid.UsageId.MceRemoteUsage)
                    {                        
                        handler = HidMceRemoteHandler;
                    }
                    else if (deviceInfo.hid.usUsagePage == (ushort)Hid.UsagePage.Consumer || deviceInfo.hid.usUsage == (ushort)Hid.UsageId.ConsumerControl)
                    {
                        handler = HidConsumerDeviceHandler;
                    }
                    else
                    {
                        Debug.WriteLine("Not MCE remote page and usage.");
                        return;
                    }

                    if (!(rawInput.hid.dwSizeHid > 1     //Make sure our HID msg size more than 1. In fact the first ushort is irrelevant to us for now
                        && rawInput.hid.dwCount > 0))    //Check that we have at least one HID msg
                    {
                        return;
                    }


                    //Allocate a buffer for one HID input
                    byte[] hidInput = new byte[rawInput.hid.dwSizeHid];

                    //For each HID input in our raw input
                    for (int i = 0; i < rawInput.hid.dwCount; i++)
                    {
                        //Compute the address from which to copy our HID input
                        int hidInputOffset = 0;
                        unsafe
                        {
                            byte* source = (byte*)rawInputBuffer;
                            source += sizeof(RAWINPUTHEADER) + sizeof(RAWHID) + (rawInput.hid.dwSizeHid * i);
                            hidInputOffset = (int)source;
                        }

                        //Copy HID input into our buffer
                        Marshal.Copy(new IntPtr(hidInputOffset), hidInput, 0, rawInput.hid.dwSizeHid);

                        //Print HID raw input in our debug output
                        string hidDump = "HID " + rawInput.hid.dwCount + "/" + rawInput.hid.dwSizeHid + ":";
                        foreach (byte b in hidInput)
                        {
                            hidDump += b.ToString("X2");
                        }
                        Debug.WriteLine(hidDump);
                        
                        ushort usage = 0;
                        //hidInput[0] //Not sure what's the meaning of the code at offset 0
                        if (hidInput.Length == 2)
                        {
                            //Single byte code
                            usage = hidInput[1]; //Get button code
                        }
                        else if (hidInput.Length > 2) //Defensive
                        {
                            //Assuming double bytes code
                            usage = (ushort)((hidInput[2] << 8) + hidInput[1]);
                        }

                        //
                        handler(usage);
                    }

                }
                else if (rawInput.header.dwType == Const.RIM_TYPEMOUSE)
                {
                    Debug.WriteLine("WM_INPUT source device is Mouse.");
                    // do mouse handling...
                }
                else if (rawInput.header.dwType == Const.RIM_TYPEKEYBOARD)
                {
                    Debug.WriteLine("WM_INPUT source device is Keyboard.");
                    // do keyboard handling...

                }
            }
            finally
            {
                //Always executed when leaving our try block
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
