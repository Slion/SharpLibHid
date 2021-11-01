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
using Oculus.Rift.S;
using GregsStack.InputSimulatorStandard;
using GregsStack.InputSimulatorStandard.Native;
using System.Threading;

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
        InputSimulator iInputSim = new InputSimulator();

        //
        ControllerReport iCtrlReport = new ControllerReport();

        //
        Mutex iMutex = new Mutex();

        // AZERTY mapping
        VirtualKeyCode iLeft = VirtualKeyCode.VK_Q;
        VirtualKeyCode iRight = VirtualKeyCode.VK_D;
        VirtualKeyCode iUp = VirtualKeyCode.VK_Z;
        VirtualKeyCode iDown = VirtualKeyCode.VK_S;


        public FormMain()
        {
            InitializeComponent();
            CreateHandler();
            iStopWatch.Start();
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
        bool isDownButtonStick = false;
        bool isDownButtonMenu = false;





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

            iMutex.WaitOne();

            // Parse our raw input report into our controller report class.
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

            
            // Do not update our prints too often to avoid lags
            bool updatePrints = iStopWatch.ElapsedMilliseconds >= 100;

            foreach (var ctrl in iControllers.Values)
            {
                // Quick and dirty mapping to keyboard input for MWO
                if (ctrl.device_id == 0xCE156735BF2F1676) // // Right controller
                {
                    var position = iPosition[ctrl.device_id];
                    if (updatePrints)
                    {
                        // Print our controller states in our console
                        Console.SetCursorPosition(Console.WindowLeft, position);
                        Console.WriteLine(ctrl.Dump());
                        iStopWatch.Restart();
                    }
                        

                    // Throttle
                    if (ctrl.joystick_y>10000)
                    {
                        // Moving forward
                        if (!isDownForward)
                        {
                            isDownForward = true;
                            isDownBackward = false;
                            //
                            iInputSim.Keyboard.KeyDown(iUp);
                            iInputSim.Keyboard.KeyUp(iDown);
                        }
                    }
                    else if (ctrl.joystick_y < -10000)
                    {
                        // Moving backward
                        if (!isDownBackward)
                        {
                            isDownForward = false;
                            isDownBackward = true;
                            //
                            iInputSim.Keyboard.KeyDown(iDown);
                            iInputSim.Keyboard.KeyUp(iUp);
                        }
                    }
                    else
                    {
                        if (isDownForward || isDownBackward)
                        {
                            isDownForward = false;
                            isDownBackward = false;
                            //
                            iInputSim.Keyboard.KeyUp(iDown);
                            iInputSim.Keyboard.KeyUp(iUp);
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
                            //
                            iInputSim.Keyboard.KeyDown(iRight);
                            iInputSim.Keyboard.KeyUp(iLeft);
                        }
                    }
                    else if (ctrl.joystick_x < -15000)
                    {
                        // Moving left
                        if (!isDownLeft)
                        {
                            // We used those to measure our input lag by comparing the time the event is sent here and the time it is received in HidDemo
                            //Console.WriteLine("\nLeft down: " + aHidEvent.Time.ToString("HH:mm:ss:fff"));
                            isDownLeft = true;
                            isDownRight = false;
                            //
                            iInputSim.Keyboard.KeyDown(iLeft);
                            iInputSim.Keyboard.KeyUp(iRight);
                        }
                    }
                    else
                    {
                        if (isDownRight)
                        {
                            // Use that to debug input lag
                            //Console.WriteLine("Right up: " + aHidEvent.Time.ToString("HH:mm:ss:fff"));
                            isDownRight = false;
                            iInputSim.Keyboard.KeyUp(iRight);
                        }

                        if (isDownLeft)
                        {
                            // Use that to debug input lag
                            //Console.WriteLine("Left up: " + aHidEvent.Time.ToString("HH:mm:ss:fff"));
                            isDownLeft = false;
                            iInputSim.Keyboard.KeyUp(iLeft);
                        }
                    }

    
                    //Target
                    if (ctrl.trigger<3072)
                    {
                        if (!isDownTrigger)
                        {
                            isDownTrigger = true;
                            iInputSim.Keyboard.KeyDown(VirtualKeyCode.VK_R);
                        }
                    }
                    else if (isDownTrigger)
                    {
                        isDownTrigger = false;
                        iInputSim.Keyboard.KeyUp(VirtualKeyCode.VK_R);
                    }

                    // Push-to-talk
                    if (ctrl.grip < 512)
                    {
                        if (!isDownGrip)
                        {
                            //Console.WriteLine("down   ");
                            isDownGrip = true;
                            iInputSim.Keyboard.KeyDown(VirtualKeyCode.CAPITAL);
                        }
                    }
                    else if (isDownGrip)
                    {
                        //Console.WriteLine("up   ");
                        isDownGrip = false;
                        iInputSim.Keyboard.KeyUp(VirtualKeyCode.CAPITAL);
                    }

                    // Third weapon
                    if (ctrl.ButtonStick())
                    {
                        if (!isDownButtonStick)
                        {
                            //Console.WriteLine("down   ");
                            isDownButtonStick = true;
                            iInputSim.Keyboard.KeyDown(VirtualKeyCode.VK_3);
                        }
                    }
                    else if (isDownButtonStick)
                    {
                        //Console.WriteLine("up   ");
                        isDownButtonStick = false;
                        iInputSim.Keyboard.KeyUp(VirtualKeyCode.VK_3);
                    }

                    // Battlegrid
                    if (ctrl.ButtonMenu())
                    {
                        if (!isDownButtonMenu)
                        {
                            //Console.WriteLine("down   ");
                            isDownButtonMenu = true;
                            iInputSim.Keyboard.KeyDown(VirtualKeyCode.VK_B);
                        }
                    }
                    else if (isDownButtonMenu)
                    {
                        //Console.WriteLine("up   ");
                        isDownButtonMenu = false;
                        iInputSim.Keyboard.KeyUp(VirtualKeyCode.VK_B);
                    }
                }
            }

            iMutex.ReleaseMutex();

        }
    }
}
