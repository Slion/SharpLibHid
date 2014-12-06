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
            get { return _rcb; }
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
            get { return _device; }
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

            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[4];

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

            //i++;
            //rid[i].usUsagePage = (ushort)Hid.UsagePage.GenericDesktopControl;
            //rid[i].usUsage = (ushort)Hid.UsageIdGenericDesktop.Keyboard;
            //rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            //rid[i].hwndTarget = aHWND;

            //i++;
            //rid[i].usUsagePage = (ushort)Hid.UsagePage.GenericDesktopControl;
            //rid[i].usUsage = (ushort)Hid.UsageIdGenericDesktop.Mouse;
            //rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            //rid[i].hwndTarget = aHWND;


            if (!Function.RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0])))
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
                case (int)Keys.Escape:
                    rcb = RemoteControlButton.Clear;
                    break;
                case (int)Keys.Up:
                    rcb = RemoteControlButton.Up;
                    break;
                case (int)Keys.Down:
                    rcb = RemoteControlButton.Down;
                    break;
                case (int)Keys.Left:
                    rcb = RemoteControlButton.Left;
                    break;
                case (int)Keys.Right:
                    rcb = RemoteControlButton.Right;
                    break;
                case (int)Keys.Enter:
                    rcb = RemoteControlButton.Enter;
                    break;
                case (int)Keys.D0:
                    rcb = RemoteControlButton.Digit0;
                    break;
                case (int)Keys.D1:
                    rcb = RemoteControlButton.Digit1;
                    break;
                case (int)Keys.D2:
                    rcb = RemoteControlButton.Digit2;
                    break;
                case (int)Keys.D3:
                    rcb = RemoteControlButton.Digit3;
                    break;
                case (int)Keys.D4:
                    rcb = RemoteControlButton.Digit4;
                    break;
                case (int)Keys.D5:
                    rcb = RemoteControlButton.Digit5;
                    break;
                case (int)Keys.D6:
                    rcb = RemoteControlButton.Digit6;
                    break;
                case (int)Keys.D7:
                    rcb = RemoteControlButton.Digit7;
                    break;
                case (int)Keys.D8:
                    rcb = RemoteControlButton.Digit8;
                    break;
                case (int)Keys.D9:
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

            Hid.HidEvent hidEvent = new Hid.HidEvent(message);
            hidEvent.DebugWrite();

            if (!hidEvent.IsValid || !hidEvent.IsGeneric)
            {
                Debug.WriteLine("Skipping HID message.");
                return;
            }


            HidUsageHandler usagePageHandler = null;

            //Check if this an MCE remote HID message
            if (hidEvent.UsagePage == (ushort)Hid.UsagePage.MceRemote && hidEvent.UsageCollection == (ushort)Hid.UsageIdMce.MceRemote)
            {
                usagePageHandler = HidMceRemoteHandler;
            }
            //Check if this is a consumer control HID message
            else if (hidEvent.UsagePage == (ushort)Hid.UsagePage.Consumer && hidEvent.UsageCollection == (ushort)Hid.UsageIdConsumer.ConsumerControl)
            {
                usagePageHandler = HidConsumerDeviceHandler;
            }
            //Unknown HID message
            else
            {
                Debug.WriteLine("Unknown HID message.");
                return;
            }

            foreach (ushort usage in hidEvent.Usages)
            {
                usagePageHandler(usage);
            }
        }
    }
}

