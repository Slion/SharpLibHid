using SharpLib.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hid = SharpLib.Hid;
using MicroInput = SharpLib.MicroInput;
using Oculus.Rift.S;

namespace OculusDemo
{


    public partial class FormMain : Form
    {
        private Hid.Handler iHandler;

        Dictionary<ulong, ControllerState> iControllers = new Dictionary<ulong, ControllerState>();

        // To position logs on our console
        Dictionary<ulong, int> iPosition = new Dictionary<ulong, int>();

        // To not print stuff too often
        Stopwatch iStopWatch = new Stopwatch();

        // Used to generate actual keyboard inputs
        MicroInput.Client iClient = new MicroInput.Client();

        //
        ControllerReport iCtrlReport = new ControllerReport();


        public FormMain()
        {
            InitializeComponent();
            CreateHandler();
            iStopWatch.Start();
            iClient.Open();
        }

        void CreateHandler()
        {
            int i = 0;
            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[1];



            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.VirtualRealityControls;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.VirtualReality.HeadTracker;
            rid[i].dwFlags = RawInputDeviceFlags.RIDEV_EXINPUTSINK;
            rid[i].hwndTarget = Handle;


            //i++;
            //rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.GenericDesktopControls;
            //rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.GenericDesktop.Keyboard;
            //rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            //rid[i].hwndTarget = Handle;

            //i++;
            //rid[i].usUsagePage = (ushort)Hid.UsagePage.GenericDesktopControls;
            //rid[i].usUsage = (ushort)Hid.UsageCollection.GenericDesktop.Mouse;
            //rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            //rid[i].hwndTarget = aHWND;

            iHandler = new SharpLib.Hid.Handler(rid);
            if (!iHandler.IsRegistered)
            {
                Debug.WriteLine("Failed to register raw input devices: " + Marshal.GetLastWin32Error().ToString());
            }

            iHandler.OnHidEvent += OnHidEvent;


        }

        /// <summary>
        /// Hook in HID handler.
        /// </summary>
        /// <param name="message"></param>
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                //case Const.WM_KEYDOWN:
                //ProcessKeyDown(message.WParam);
                //break;
                case Const.WM_INPUT:
                    //Log that message
                    //richTextBoxLogs.AppendText("WM_INPUT: " + message.ToString() + "\r\n");
                    //Returning zero means we processed that message.
                    message.Result = new IntPtr(0);
                    //iHidHandler.ProcessInput(ref message);
                    iHandler?.ProcessInput(ref message);
                    break;
            }
            //Is that needed? Check the docs.
            base.WndProc(ref message);
        }


        // Avoid sending our micro controllers input when not needed
        bool isDownTrigger = false;
        bool isDownGrip = false;
        bool isDownForward = false;
        bool isDownBackward = false;
        bool isDownLeft = false;
        bool isDownRight = false;





        unsafe public void OnHidEvent(object aSender, SharpLib.Hid.Event aHidEvent)
        {
            if (aHidEvent.IsStray)
            {
                //Stray event just ignore it
                return;
            }

            // Oculus Rift S Touch Controller report starts with 0x67
            if (aHidEvent.InputReport[0] != 0x67)
            {
                return;
                //Console.WriteLine(aHidEvent.InputReportString());
            }


            var size = aHidEvent.InputReport.Length;

            if (size < 62)
            {
                return;
            }

            // Parse our raw input report into our controller report class.
            // TODO: Avoid instantiating a new object every time.
            Utils.ParseControllerInputReport(aHidEvent.InputReport, iCtrlReport);

            // Check if we have a valid device ID
            if (iCtrlReport.device_id != 0)
            {
                // Check if we already meat this device
                ControllerState state;
                int position = Console.WindowTop + iControllers.Count * 20;
                if (!iControllers.TryGetValue(iCtrlReport.device_id, out state))
                {
                    // That's a new device, create a state object for it then and add it to our collection
                    state = new ControllerState();
                    state.device_id = iCtrlReport.device_id;
                    iControllers.Add(iCtrlReport.device_id, state);
                    //Console.WriteLine("Found new device: 0x" + report.device_id.ToString("X"));
                    iPosition.Add(iCtrlReport.device_id, position);
                }

                // Now that we have our state object for this device, we need to update it with the incoming input report
                Utils.UpdateControllerState(ref state, iCtrlReport);
            }

            // Print our controller states in our console
            // However we do not update our prints too often to avoid lags
            if (iStopWatch.ElapsedMilliseconds >= 10) // 33?
            {
                foreach (var ctrl in iControllers.Values)
                {
                    var position = iPosition[ctrl.device_id];
                    //Console.SetCursorPosition(Console.WindowLeft, position);
                    //Console.WriteLine(ctrl.Dump());

                    // Quick and dirty mapping to keyboard input for MWO
                    if (ctrl.device_id == 0xCE156735BF2F1676) // // Right controller
                    {   
                        // Throttle
                        if (ctrl.joystick_y>10000)
                        {
                            // Moving forward
                            if (!isDownForward)
                            {
                                isDownForward = true;
                                isDownBackward = false;
                                iClient.KeyboardPress((ushort)MicroInput.Keyboard.Key.W, 0);
                                iClient.KeyboardRelease((ushort)MicroInput.Keyboard.Key.S, 0);
                            }
                        }
                        else if (ctrl.joystick_y < -10000)
                        {
                            // Moving backward
                            if (!isDownBackward)
                            {
                                isDownForward = false;
                                isDownBackward = true;
                                iClient.KeyboardPress((ushort)MicroInput.Keyboard.Key.S, 0);
                                iClient.KeyboardRelease((ushort)MicroInput.Keyboard.Key.W, 0);
                            }
                        }
                        else
                        {
                            if (isDownForward || isDownBackward)
                            {
                                isDownForward = false;
                                isDownBackward = false;
                                iClient.KeyboardRelease((ushort)MicroInput.Keyboard.Key.S, 0);
                                iClient.KeyboardRelease((ushort)MicroInput.Keyboard.Key.W, 0);
                            }
                        }
                        
                        // Rotation
                        if (ctrl.joystick_x > 15000)
                        {
                            // Moving right
                            if (!isDownRight)
                            {
                                isDownLeft = false;
                                isDownRight = true;
                                iClient.KeyboardPress((ushort)MicroInput.Keyboard.Key.D, 0);
                                iClient.KeyboardRelease((ushort)MicroInput.Keyboard.Key.A, 0);
                            }
                        }
                        else if (ctrl.joystick_x < -15000)
                        {
                            // Moving left
                            if (!isDownLeft)
                            {
                                isDownLeft = true;
                                isDownRight = false;                                
                                iClient.KeyboardPress((ushort)MicroInput.Keyboard.Key.A, 0);
                                iClient.KeyboardRelease((ushort)MicroInput.Keyboard.Key.D, 0);
                            }
                        }
                        else
                        {
                            if (isDownRight || isDownLeft)
                            {
                                isDownLeft = false;
                                isDownRight = false;
                                iClient.KeyboardRelease((ushort)MicroInput.Keyboard.Key.D, 0);
                                iClient.KeyboardRelease((ushort)MicroInput.Keyboard.Key.A, 0);
                            }
                        }

    
                        //Target
                        if (ctrl.trigger<3072 && !isDownTrigger)
                        {
                            isDownTrigger = true;
                            iClient.KeyboardPress((ushort)MicroInput.Keyboard.Key.R, 0);
                        }
                        else if (isDownTrigger)
                        {
                            isDownTrigger = false;
                            iClient.KeyboardRelease((ushort)MicroInput.Keyboard.Key.R, 0);
                        }
                  
                        if (ctrl.grip < 3072 && !isDownGrip)
                        {
                            isDownGrip = true;
                            iClient.KeyboardPress((ushort)MicroInput.Keyboard.Key.CAPS_LOCK, 0);
                        }
                        else if (isDownGrip)
                        {
                            isDownGrip = false;
                            iClient.KeyboardRelease((ushort)MicroInput.Keyboard.Key.CAPS_LOCK, 0);
                        }


                    }

                }
                iStopWatch.Restart();
            }



        }
    }
}
