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

            ControllerReport report = Utils.ParseControllerInputReport(aHidEvent.InputReport);

            if (report.device_id != 0)
            {
                ControllerState state;
                int position = Console.WindowTop + iControllers.Count * 20;
                if (!iControllers.TryGetValue(report.device_id, out state))
                {
                    state = new ControllerState();
                    state.device_id = report.device_id;
                    iControllers.Add(report.device_id, state);
                    //Console.WriteLine("Found new device: 0x" + report.device_id.ToString("X"));
                    iPosition.Add(report.device_id, position);
                }



                Utils.UpdateControllerState(ref state, report);

                // TODO: Find a way to get a reference of that object so we don't have to update all the time
                // Maybe turn our struct into a class
                iControllers[state.device_id] = state;

            }


            // Don't update our prints too often to avoid lags
            if (iStopWatch.ElapsedMilliseconds >= 33)
            {
                foreach (var ctrl in iControllers.Values)
                {
                    var position = iPosition[ctrl.device_id];
                    Console.SetCursorPosition(Console.WindowLeft, position);
                    Console.WriteLine(ctrl.Dump());
                }
                iStopWatch.Restart();
            }

        }
    }
}
