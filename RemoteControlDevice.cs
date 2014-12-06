using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32.SafeHandles;

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
        ConsumerControl iConsumerControl;

        public RemoteControlEventArgs(RemoteControlButton rcb, InputDevice device)
		{
            SetNullButtons();
            //
			_rcb = rcb;
			_device = device;            
		}

        public RemoteControlEventArgs(ConsumerControl aConsumerControl, InputDevice device)
        {
            SetNullButtons();
            //
            iConsumerControl = aConsumerControl;            
            _device = device;
        }


        public RemoteControlEventArgs(MceButton mce, InputDevice device)
        {
            SetNullButtons();
            //
            iMceButton = mce;            
            _device = device;
        }

        private void SetNullButtons()
        {
            iConsumerControl = ConsumerControl.Null;
            iMceButton = MceButton.Null;
            _rcb = RemoteControlButton.Unknown;
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

        public ConsumerControl ConsumerControl
        {
            get { return iConsumerControl; }
            set { iConsumerControl = value; }
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
		public delegate bool RemoteControlDeviceEventHandler(object sender, RemoteControlEventArgs e);
		public event RemoteControlDeviceEventHandler ButtonPressed;

        /// <summary>
        /// Return true if the usage was processed.
        /// </summary>
        /// <param name="aUsage"></param>
        /// <returns></returns>
        public delegate bool HidUsageHandler(ushort aUsage);
        

		//-------------------------------------------------------------
		// constructors
		//-------------------------------------------------------------

		public RemoteControlDevice(IntPtr aHWND)
		{
			// Register the input device to receive the commands from the
			// remote device. See http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwmt/html/remote_control.asp
			// for the vendor defined usage page.

			RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[5];

            int i = 0;
			rid[i].usUsagePage = (ushort)Hid.UsagePage.MceRemote;
            rid[i].usUsage = (ushort)Hid.UsageIdMce.MceRemote;
            rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            rid[i].hwndTarget = aHWND;

            i++;
            rid[i].usUsagePage = (ushort)Hid.UsagePage.Consumer;
            rid[i].usUsage = (ushort)Hid.UsageIdConsumer.ConsumerControl;
            rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            rid[i].hwndTarget = aHWND;

            i++;
            rid[i].usUsagePage = (ushort)Hid.UsagePage.Consumer;
            rid[i].usUsage = (ushort)Hid.UsageIdConsumer.Selection;
            rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            rid[i].hwndTarget = aHWND;

            i++;
            rid[i].usUsagePage = (ushort)Hid.UsagePage.GenericDesktopControl;
            rid[i].usUsage = (ushort)Hid.UsageIdGenericDesktop.SystemControl;
            rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            rid[i].hwndTarget = aHWND;

            i++;
            rid[i].usUsagePage = (ushort)Hid.UsagePage.GenericDesktopControl;
            rid[i].usUsage = (ushort)Hid.UsageIdGenericDesktop.Keyboard;
            rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            rid[i].hwndTarget = aHWND;

            //i++;
            //rid[i].usUsagePage = (ushort)Hid.UsagePage.GenericDesktopControl;
            //rid[i].usUsage = (ushort)Hid.UsageIdGenericDesktop.Mouse;
            //rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            //rid[i].hwndTarget = aHWND;


			if (!Function.RegisterRawInputDevices(rid,(uint) rid.Length,(uint) Marshal.SizeOf(rid[0])))
			{
                throw new ApplicationException("Failed to register raw input devices: " + Marshal.GetLastWin32Error().ToString());
			}
		}


		//-------------------------------------------------------------
		// methods
		//-------------------------------------------------------------

		public void ProcessMessage(Message message)
		{
			switch (message.Msg)
			{
				case Const.WM_KEYDOWN:
                    ProcessKeyDown(message.WParam);
					break;
                case Const.WM_INPUT:
                    //Returning zero means we processed that message.
                    message.Result = new IntPtr(0);
					ProcessInputCommand(ref message);
					break;
			}

		}


		//-------------------------------------------------------------
		// methods (helpers)
		//-------------------------------------------------------------

		private void ProcessKeyDown(IntPtr wParam)
		{
			RemoteControlButton rcb = RemoteControlButton.Unknown;

            switch (wParam.ToInt32())
			{
				case (int) Keys.Escape:
					rcb = RemoteControlButton.Clear;
                    break;
                case (int)Keys.Up:
                    rcb = RemoteControlButton.Up;
                    break;
				case (int) Keys.Down:
					rcb = RemoteControlButton.Down;
					break;
				case (int) Keys.Left:
					rcb = RemoteControlButton.Left;
                    break;
                case (int)Keys.Right:
                    rcb = RemoteControlButton.Right;
                    break;
                case (int)Keys.Enter:
                    rcb = RemoteControlButton.Enter;
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
			}

            if (this.ButtonPressed != null && rcb != RemoteControlButton.Unknown)
            {
                Debug.WriteLine("KeyDown: " + rcb.ToString());
                this.ButtonPressed(this, new RemoteControlEventArgs(rcb, InputDevice.Key));
            }
		}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUsage"></param>
        private bool HidConsumerDeviceHandler(ushort aUsage)
        {
            if (aUsage == 0)
            {
                //Just skip those
                return false;
            }

            if (Enum.IsDefined(typeof(ConsumerControl), aUsage) && aUsage != 0) //Our button is a known consumer control
            {
                if (this.ButtonPressed != null)
                {
                    return this.ButtonPressed(this, new RemoteControlEventArgs((ConsumerControl)aUsage, InputDevice.OEM));
                }
                return false;
            }
            else
            {
                Debug.WriteLine("Unknown Consumer Control!");
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUsage"></param>
        private bool HidMceRemoteHandler(ushort aUsage)
        {
            if (aUsage == 0)
            {
                //Just skip those
                return false;
            }


            if (Enum.IsDefined(typeof(MceButton), aUsage) && aUsage != 0) //Our button is a known MCE button
            {
                if (this.ButtonPressed != null)
                {
                    return this.ButtonPressed(this, new RemoteControlEventArgs((MceButton)aUsage, InputDevice.OEM));                    
                }
                return false;
            }
            else
            {
                Debug.WriteLine("Unknown MCE button!");
                return false;
            }
        }


		private void ProcessInputCommand(ref Message message)
		{
            //We received a WM_INPUT message
            Debug.WriteLine("================WM_INPUT================");

            //Check if we received this message while in background or foreground
            if (Macro.GET_RAWINPUT_CODE_WPARAM(message.WParam) == Const.RIM_INPUT)
            {
                Debug.WriteLine("================FOREGROUND");
            }
            else if (Macro.GET_RAWINPUT_CODE_WPARAM(message.WParam) == Const.RIM_INPUTSINK)
            {
                Debug.WriteLine("================BACKGROUND");
            }

            //Declare some pointers
            IntPtr rawInputBuffer = IntPtr.Zero;
            //My understanding is that this is basically our HID descriptor 
            IntPtr preParsedData = IntPtr.Zero;

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

                //Get various information about this HID device
                Hid.HidDevice device = new Hid.HidDevice(rawInput.header.hDevice);
                device.DebugWrite();
                
               


                if (rawInput.header.dwType == Const.RIM_TYPEHID)  //Check that our raw input is HID                        
                {
                    Debug.WriteLine("WM_INPUT source device is HID.");
                    //Get Usage Page and Usage
                    Debug.WriteLine("Usage Page: 0x" + deviceInfo.hid.usUsagePage.ToString("X4") + " Usage ID: 0x" + deviceInfo.hid.usUsage.ToString("X4"));


                    preParsedData = RawInput.GetPreParsedData(rawInput.header.hDevice);

                    //
                    HidUsageHandler usagePageHandler=null;

                    //Check if this an MCE remote HID message
                    if (deviceInfo.hid.usUsagePage == (ushort)Hid.UsagePage.MceRemote && deviceInfo.hid.usUsage == (ushort)Hid.UsageIdMce.MceRemote)
                    {                        
                        usagePageHandler = HidMceRemoteHandler;
                    }
                    //Check if this is a consumer control HID message
                    else if (deviceInfo.hid.usUsagePage == (ushort)Hid.UsagePage.Consumer && deviceInfo.hid.usUsage == (ushort)Hid.UsageIdConsumer.ConsumerControl)
                    {
                        usagePageHandler = HidConsumerDeviceHandler;
                    }
                    //Unknown HID message
                    else
                    {
                        Debug.WriteLine("Unknown HID message.");
                        return;
                    }

                    if (!(rawInput.hid.dwSizeHid > 1     //Make sure our HID msg size more than 1. In fact the first ushort is irrelevant to us for now
                        && rawInput.hid.dwCount > 0))    //Check that we have at least one HID msg
                    {
                        return;
                    }


                    //Allocate a buffer for one HID input
                    byte[] hidInputReport = new byte[rawInput.hid.dwSizeHid];

                    Debug.WriteLine("Raw input contains " + rawInput.hid.dwCount + " HID input report(s)");

                    //For each HID input report in our raw input
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
                        Marshal.Copy(new IntPtr(hidInputOffset), hidInputReport, 0, (int)rawInput.hid.dwSizeHid);

                        //Print HID input report in our debug output
                        string hidDump = "HID input report: ";
                        foreach (byte b in hidInputReport)
                        {
                            hidDump += b.ToString("X2");
                        }
                        Debug.WriteLine(hidDump);
                        
                        //Proper parsing now
                        uint usageCount = 1; //Assuming a single usage per input report. Is that correct?
                        Win32.USAGE_AND_PAGE[] usages = new Win32.USAGE_AND_PAGE[usageCount];
                        Win32.HidStatus status = Win32.Function.HidP_GetUsagesEx(Win32.HIDP_REPORT_TYPE.HidP_Input, 0, usages, ref usageCount, preParsedData, hidInputReport, (uint)hidInputReport.Length);
                        if (status != Win32.HidStatus.HIDP_STATUS_SUCCESS)
                        {
                            Debug.WriteLine("Could not parse HID data!");
                        }
                        else
                        {
                            Debug.WriteLine("UsagePage: 0x" + usages[0].UsagePage.ToString("X4"));
                            Debug.WriteLine("Usage: 0x" + usages[0].Usage.ToString("X4"));
                            //Call on our Usage Page handler
                            usagePageHandler(usages[0].Usage);
                        }
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
                    Debug.WriteLine("Type: " + deviceInfo.keyboard.dwType.ToString());
                    Debug.WriteLine("SubType: " + deviceInfo.keyboard.dwSubType.ToString());
                    Debug.WriteLine("Mode: " + deviceInfo.keyboard.dwKeyboardMode.ToString());
                    Debug.WriteLine("Number of function keys: " + deviceInfo.keyboard.dwNumberOfFunctionKeys.ToString());
                    Debug.WriteLine("Number of indicators: " + deviceInfo.keyboard.dwNumberOfIndicators.ToString());
                    Debug.WriteLine("Number of keys total: " + deviceInfo.keyboard.dwNumberOfKeysTotal.ToString());
                }
            }
            finally
            {
                //Always executed when leaving our try block
                Marshal.FreeHGlobal(rawInputBuffer);
                Marshal.FreeHGlobal(preParsedData);
            }
		}
	}
}
