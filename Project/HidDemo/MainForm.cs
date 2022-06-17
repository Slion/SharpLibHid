//
// Copyright (C) 2014-2016 Stéphane Lenclud.
//
// This file is part of SharpLibHid.
//
// SharpDisplayManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SharpDisplayManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SharpDisplayManager.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Hid = SharpLib.Hid;
using SharpLib.ToEnum; // Needed for ToEnum extensions. Move it somewhere else and remove it.
using SharpLib.Win32;

//For ClickOnce support
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

// Win32
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Devices.DeviceAndDriverInstallation;

namespace HidDemo
{

    /// <summary>
    /// MainForm for our HID demo.
    /// </summary>
    public partial class MainForm : System.Windows.Forms.Form
	{
        /// <summary>
        /// Can be used to register for WM_INPUT messages and parse them.
        /// For testing purposes it can also be used to solely register for WM_INPUT messages.
        /// </summary>
        private Hid.Handler iHidHandler;

        /// <summary>
        /// Just using another handler to check that one can use the parser without registering.
        /// That's useful cause only one windows per application can register for a range of WM_INPUT apparently.
        /// See: http://stackoverflow.com/a/9756322/3288206
        /// </summary>
        private Hid.Handler iHidParser;

        public delegate void OnHidEventDelegate(object aSender, Hid.Event aHidEvent);

        // Avoid logging too many events otherwise we just hang when testing high frequency device like Virpil joysticks and Oculus Rift S
        DateTime iLogPeriodStartTime;
        int iEventCount;
        const int KMaxEventPerPeriod = 10;
        const int KPeriodDurationInMs = 500;

		public MainForm()
		{
			// Required for Windows Form Designer support
			InitializeComponent();           
		}


        /// <summary>
        /// Provides version number as a string.
        /// </summary>
        /// <returns>Version string.</returns>
        public static string Version()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return "v" + versionInfo.ProductVersion;
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            this.Text += " - " + Version();
            
            PopulateDeviceList();
            RegisterHidDevices();
            //CheckDefaultDevices();

            // Register for key down event
            KeyDown += OnKeyDown;

        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            TrySquirrelUpdate(true);
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeHandlers();
        }


        private void DisposeHandlers()
        {
            if (iHidHandler != null)
            {
                //First de-register
                iHidHandler.Dispose();
                iHidHandler = null;
            }

            if (iHidParser != null)
            {
                //First de-register
                iHidParser.Dispose();
                iHidParser = null;
            }

        }

        /// <summary>
        /// Populate our tree view with our devices.
        /// </summary>
        private void PopulateDeviceList()
        {
            treeViewDevices.Nodes.Clear();
            //Create our list of HID devices
            SharpLib.Win32.RawInput.PopulateDeviceList(treeViewDevices);
            treeViewDevices.CollapseAll();
            //Hide check boxes
            //treeViewDevices.Nodes.OfType<TreeNode>().ToList().ForEach(n => TreeViewUtils.HideCheckBox(treeViewDevices,n));
            //Do it twice because of that bug where the first node we hit is not hiding its checkbox as it should.
            treeViewDevices.Nodes.OfType<TreeNode>().ToList().ForEach(n => n.Nodes.OfType<TreeNode>().ToList().ForEach(on => TreeViewUtils.HideCheckBox(treeViewDevices, on)));
            treeViewDevices.Nodes.OfType<TreeNode>().ToList().ForEach(n => n.Nodes.OfType<TreeNode>().ToList().ForEach(on => TreeViewUtils.HideCheckBox(treeViewDevices, on)));

            foreach (TreeNode node in treeViewDevices.Nodes)
            {
                Hid.Device device = (Hid.Device)node.Tag;
                if (!device.IsHid)
                {
                    //Now allowing mouse and keyboard to register too
                    //Do not allow registering mice and keyboards for now
                    //TreeViewUtils.HideCheckBox(treeViewDevices, node);
                }
            }

            //Dump our devices to our logs
            richTextBoxLogs.AppendText(TreeViewUtils.TreeViewToText(treeViewDevices));
        }


        /// <summary>
        /// Open system properties dialog for specified device
        /// </summary>
        /// <param name="aDevicePath"></param>
        /// <returns></returns>
        public static bool OpenPropertiesDialog(Hid.Device aDevice)
        {
            // We could not find a better way to do this other than just trying with the various classes: HID, Keyboard and Mouse
            // Get the GUID of the HID class

            bool ok = false;

            if (aDevice.IsHid)
            {
                Guid guid;
                Extra.Function.HidD_GetHidGuid(out guid); // Could also use the hard coded value I guess
                ok = OpenPropertiesDialog(aDevice.Name, guid);
            }

            if (aDevice.IsMouse)
            {
                ok = OpenPropertiesDialog(aDevice.Name, Extra.Const.GUID_DEVINTERFACE_MOUSE);
            }

            if (aDevice.IsKeyboard)
            {
                ok = OpenPropertiesDialog(aDevice.Name, Extra.Const.GUID_DEVINTERFACE_KEYBOARD);
            }
            
            return ok;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDevicePath"></param>
        /// <param name="aClass"></param>
        public unsafe static bool OpenPropertiesDialog(string aDevicePath, Guid aClass)
        {
            uint index = 0;

            // Get a handle to a list of devices matching the given class GUID.            
            SetupDiDestroyDeviceInfoListSafeHandle hDevInfo = PInvoke.SetupDiGetClassDevs(&aClass, (string)null, new HWND(0), Extra.Const.DIGCF_DEVICEINTERFACE);
            //IntPtr hDevInfo = HIDImports.SetupDiGetClassDevs(IntPtr.Zero, null, IntPtr.Zero, HIDImports.DIGCF_DEVICEINTERFACE | HIDImports.DIGCF_PRESENT);
            //IntPtr hDevInfo = HIDImports.SetupDiCreateDeviceInfoList(IntPtr.Zero, IntPtr.Zero);

            // Alloc and init
            SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
            deviceInterfaceData.cbSize = (uint)sizeof(SP_DEVICE_INTERFACE_DATA);

            // Alloc and init
            SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA();
            deviceInfoData.cbSize = (uint)sizeof(SP_DEVINFO_DATA);

            // get a device interface to a single device (enumerate all devices)
            bool keepGoing = false;
            do
            {
                /*
                keepGoing = HIDImports.SetupDiEnumDeviceInfo(hDevInfo, index, ref deviceInfoData);
                if (!keepGoing)
                {
                    Debug.Print(Marshal.GetLastWin32Error().ToString());
                    break;
                }
                */

                // Get interface data for device at the given index and matching that class
                keepGoing = PInvoke.SetupDiEnumDeviceInterfaces(hDevInfo, null, &aClass, index, &deviceInterfaceData);
                if (!keepGoing)
                {
                    //Debug.Print(Marshal.GetLastWin32Error().ToString());
                    break;
                }

                uint size = 0;

                // Get the buffer size for this device detail instance (returned in the size parameter)
                PInvoke.SetupDiGetDeviceInterfaceDetail(hDevInfo, &deviceInterfaceData, null, 0, &size, null);

                // Create a detail struct and set its size
                var deviceInterfaceDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                //var deviceInterfaceDetail = new Extra.SP_DEVICE_INTERFACE_DETAIL_DATA();
                //SL: does not make sense, fix it
                //On Win x86, cbSize = 5, On x64, cbSize = 8 i
                deviceInterfaceDetail.cbSize = (IntPtr.Size == 8) ? (uint)8 : (uint)5;

                // TODO: Fixed that fixed size buffer size defined in SP_DEVICE_INTERFACE_DETAIL_DATA
                // Actually get the detail struct
                if (PInvoke.SetupDiGetDeviceInterfaceDetail(hDevInfo, &deviceInterfaceData, (SP_DEVICE_INTERFACE_DETAIL_DATA_W*) &deviceInterfaceDetail, size, &size, &deviceInfoData))
                {
                    // Check that current device is matching requested device
                    if (deviceInterfaceDetail.DevicePath.ToString().ToLower() == aDevicePath.ToLower())
                    {
                        // It's a match, just open that system dialog. 
                        // Get Device instance path          
                        char[] buffer = new char[512];
                        fixed (char* ptr = buffer)
                        {
                            var deviceInstanceId = new PWSTR(ptr);


                            uint requiredSize = 0;
                            PInvoke.SetupDiGetDeviceInstanceId(hDevInfo, &deviceInfoData, deviceInstanceId, 512, &requiredSize);

                            // Define command line parameters before running our process
                            string parameters = "devmgr.dll,DeviceProperties_RunDLL /MachineName \"\"  /DeviceID ";
                            // Add double quote
                            parameters += "\"" + deviceInstanceId + "\"";
                            //Debug.Print(parameters);
                            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("rundll32.exe", parameters);

                            // The following commands are needed to redirect the standard output.
                            // This means that it will be redirected to the Process.StandardOutput StreamReader.
                            //procStartInfo.RedirectStandardOutput = true;
                            //procStartInfo.UseShellExecute = false;
                            // Do not create the black window.
                            //procStartInfo.CreateNoWindow = true;
                            //
                            Process.Start(procStartInfo);

                            return true;
                        }

                    }
                }
                else
                {
                    //Debug.Print(Marshal.GetLastWin32Error().ToString());
                }
                index++;
            } while (keepGoing);


            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDevicePath"></param>
        /// <returns></returns>
        public unsafe static bool OpenPropertiesDialog(string aDevicePath)
        {
            uint index = 0;

            // Get a handle to a list of devices matching the given device path/id.            
            SetupDiDestroyDeviceInfoListSafeHandle hDevInfo = PInvoke.SetupDiGetClassDevs(null, aDevicePath, new HWND(0), Extra.Const.DIGCF_ALLCLASSES | Extra.Const.DIGCF_DEVICEINTERFACE);

            // List all devices present
            // See: https://stackoverflow.com/questions/956669/does-setupdigetclassdevs-work-with-device-instance-ids-as-documented
            //SetupDiDestroyDeviceInfoListSafeHandle hDevInfo = PInvoke.SetupDiGetClassDevs(null, (string)null, new HWND(0), Extra.Const.DIGCF_ALLCLASSES | Extra.Const.DIGCF_PRESENT);
            if (hDevInfo.IsInvalid)
            {
                Trace.WriteLine("SetupDiGetClassDevs error: " + GetLastError.String());
                return false;
            }
            

            //IntPtr hDevInfo = HIDImports.SetupDiGetClassDevs(IntPtr.Zero, null, IntPtr.Zero, HIDImports.DIGCF_DEVICEINTERFACE | HIDImports.DIGCF_PRESENT);
            //IntPtr hDevInfo = HIDImports.SetupDiCreateDeviceInfoList(IntPtr.Zero, IntPtr.Zero);

            // Alloc and init
            SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
            deviceInterfaceData.cbSize = (uint)sizeof(SP_DEVICE_INTERFACE_DATA);

            // Alloc and init
            SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA();
            deviceInfoData.cbSize = (uint)sizeof(SP_DEVINFO_DATA);

            // get a device interface to a single device (enumerate all devices)
            bool keepGoing = false;
            do
            {
                Trace.WriteLine("SetupDiEnumDeviceInfo index: " + index);

                // Get interface data for device at the given index and matching that class
                keepGoing = PInvoke.SetupDiEnumDeviceInfo(hDevInfo, index, &deviceInfoData);
                if (!keepGoing)
                {
                    Trace.WriteLine("SetupDiEnumDeviceInfo error: " + GetLastError.String());
                    break;
                }

                // It's a match, just open that system dialog. 
                // Get Device instance path          
                char[] buffer = new char[512];
                fixed (char* ptr = buffer)
                {
                    var deviceInstanceId = new PWSTR(ptr);
                        

                    uint requiredSize = 0;
                    PInvoke.SetupDiGetDeviceInstanceId(hDevInfo, &deviceInfoData, deviceInstanceId, 512, &requiredSize);

                    if (deviceInstanceId.ToString().ToLower() == aDevicePath.ToLower())
                    {
                        // Define command line parameters before running our process
                        string parameters = "devmgr.dll,DeviceProperties_RunDLL /MachineName \"\"  /DeviceID ";
                        // Add double quote
                        parameters += "\"" + deviceInstanceId + "\"";
                        //Debug.Print(parameters);
                        ProcessStartInfo procStartInfo = new ProcessStartInfo("rundll32.exe", parameters);

                        // The following commands are needed to redirect the standard output.
                        // This means that it will be redirected to the Process.StandardOutput StreamReader.
                        //procStartInfo.RedirectStandardOutput = true;
                        //procStartInfo.UseShellExecute = false;
                        // Do not create the black window.
                        //procStartInfo.CreateNoWindow = true;
                        //
                        Process.Start(procStartInfo);

                        return true;
                    }                                           
                }
                     
                index++;
            } while (keepGoing);


            return false;
        }




        /// <summary>
        /// Check some devices for registration
        /// </summary>
        void CheckDefaultDevices()
        {
            uint mceUsageId = (uint)Hid.UsagePage.WindowsMediaCenterRemoteControl << 16 | (uint)Hid.UsageCollection.WindowsMediaCenter.WindowsMediaCenterRemoteControl;
            uint consumerUsageId = (uint)Hid.UsagePage.Consumer << 16 | (uint)Hid.UsageCollection.Consumer.ConsumerControl;
            uint gamepadUsageId = (uint)Hid.UsagePage.GenericDesktopControls << 16 | (uint)Hid.UsageCollection.GenericDesktop.GamePad;

            foreach (TreeNode node in treeViewDevices.Nodes)
            {
                Hid.Device device = (Hid.Device)node.Tag;
                if (device.IsKeyboard || (device.IsHid && 
                    (device.UsageId == mceUsageId || device.UsageId == consumerUsageId || device.UsageId == gamepadUsageId)))
                {
                    node.Checked = true;
                }
            }
        }

        /// <summary>
        /// Go through our device list and register the devices that are checked.
        /// </summary>
        void RegisterHidDevices()
	    {
            // Register the input device to receive the commands from the
            // remote device. See http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwmt/html/remote_control.asp
            // for the vendor defined usage page.

            DisposeHandlers();

            //Get our flags
            RadioButton checkedRadioButton = groupBoxRegistrationFlag.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            uint flags = uint.Parse((string)checkedRadioButton.Tag, System.Globalization.NumberStyles.HexNumber);
            // See: Const.RIDEV_EXINPUTSINK and Const.RIDEV_INPUTSINK

            //We collect our devices in a dictionary to remove duplicates
            Dictionary<uint, Hid.Device>  devices = new Dictionary<uint, Hid.Device>();
            foreach (TreeNode node in treeViewDevices.Nodes)
            {
                Hid.Device device = (Hid.Device)node.Tag;
                //Now allowing mouse and keyboard to register too
                if (node.Checked /*&& device.IsHid*/) 
                {
                    try
                    {
                        devices.Add(device.UsageId, device);
                    }
                    catch //(Exception ex)
                    {
                        //Just ignore duplicate UsageId
                        Debug.WriteLine("Similar device already registered!");
                    }
                }
            }

            if (devices.Count==0)
            {
                //No device to register for, nothing to do here
                return;
            }

            int i = 0;
            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[devices.Count];

            foreach (KeyValuePair<uint,Hid.Device> entry in devices)
            {                
                rid[i].usUsagePage = entry.Value.UsagePage;
                rid[i].usUsage = entry.Value.UsageCollection;
                rid[i].dwFlags = (RawInputDeviceFlags)flags;
                rid[i].hwndTarget = Handle;
                i++;
            }

            /*
            int i = 0;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.WindowsMediaCenterRemoteControl;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.WindowsMediaCenter.WindowsMediaCenterRemoteControl;
            rid[i].dwFlags = flags;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.WindowsMediaCenterRemoteControl;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.WindowsMediaCenter.WindowsMediaCenterLowLevel;
            rid[i].dwFlags = flags;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.Consumer;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.Consumer.ConsumerControl;
            rid[i].dwFlags = flags;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.Consumer;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.Consumer.Selection;
            rid[i].dwFlags = flags;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.GenericDesktopControls;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.GenericDesktop.SystemControl;
            rid[i].dwFlags = flags;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.GenericDesktopControls;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.GenericDesktop.GamePad;
            rid[i].dwFlags = flags;
            rid[i].hwndTarget = Handle;
            */

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

            iHidHandler = new SharpLib.Hid.Handler(rid, checkBoxRepeat.Checked, (int)numericRepeatDelay.Value, (int)numericRepeatSpeed.Value);
            if (!iHidHandler.IsRegistered)
            {
                Debug.WriteLine("Failed to register raw input devices: " + Marshal.GetLastWin32Error().ToString());
            }

            if (checkBoxUseSingleHandler.Checked)
            {
                iHidParser = iHidHandler;
            }
            else
            {
                //For testing purposes we parse WM_INPUT messages from another Handler instance.
                iHidParser = new SharpLib.Hid.Handler(checkBoxRepeat.Checked, (int)numericRepeatDelay.Value, (int)numericRepeatSpeed.Value);
            }
            
            iHidParser.OnHidEvent += HandleHidEventThreadSafe;            
        }

        /// <summary>
        /// Process HID events.
        /// </summary>
        /// <param name="aSender"></param>
        /// <param name="aHidEvent"></param>
        public void HandleHidEventThreadSafe(object aSender, SharpLib.Hid.Event aHidEvent)
        {
            if (aHidEvent.IsStray)
            {
                //Stray event just ignore it
                return;
            }

            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves.
                //Repeat events usually come from another thread.
                OnHidEventDelegate d = new OnHidEventDelegate(HandleHidEventThreadSafe);
                this.Invoke(d, new object[] { aSender, aHidEvent });
            }
            else
            {
                //We are in the proper thread                

                // Check if our log period has expired
                if (aHidEvent.Time > iLogPeriodStartTime.AddMilliseconds(KPeriodDurationInMs))
                {
                    // Our period has expired, reset our period start time and number of allowed event logs in that period
                    iLogPeriodStartTime = aHidEvent.Time; // Mark the time of the last event we logged
                    iEventCount = 0;
                }

                // Status label show last device no matter what
                if (aHidEvent.Device != null)
                {
                    toolStripStatusLabelDevice.Text = aHidEvent.Device.FriendlyName;
                }

                // Check if we are still allowed to log events in that period of time
                if (iEventCount < KMaxEventPerPeriod) // Avoid spamming too many events
                {
                    // We can log that event
                    iEventCount++; // Make sure we count it
                    listViewEvents.Items.Insert(0, aHidEvent.ToListViewItem());
                    richTextBoxLogs.AppendText(aHidEvent.ToLog());
                }
                else 
                {
                    // Status label also tells us if we are receiving high amount of events
                    toolStripStatusLabelDevice.Text += $" - More than {KMaxEventPerPeriod} events per {KPeriodDurationInMs}ms, skipping some!";
                }
            }
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
                    richTextBoxLogs.AppendText("WM_INPUT: " + message.ToString() + "\r\n");
                    //Returning zero means we processed that message.
                    message.Result = new IntPtr(0);
                    //iHidHandler.ProcessInput(ref message);
                    if (iHidParser!=null) // Can be the case when disabling with high event frequency device
                    {
                        iHidParser.ProcessInput(ref message);
                    }
                    
                    break;
            }
            //Is that needed? Check the docs.
			base.WndProc(ref message);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {

        }


        private void buttonClear_Click(object sender, EventArgs e)
		{
			listViewEvents.Items.Clear();
		}

        private void buttonTreeViewCollapseAll_Click(object sender, EventArgs e)
        {
            treeViewDevices.CollapseAll();            
        }

        private void buttonTreeViewExpandAll_Click(object sender, EventArgs e)
        {
            treeViewDevices.ExpandAll();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            PopulateDeviceList();            
        }

        private void checkBoxRepeat_CheckedChanged(object sender, EventArgs e)
        {
            RegisterHidDevices();
        }

        private void numericRepeatDelay_ValueChanged(object sender, EventArgs e)
        {
            RegisterHidDevices();
        }

        private void numericRepeatSpeed_ValueChanged(object sender, EventArgs e)
        {
            RegisterHidDevices();
        }

        private void checkBoxUseSingleHandler_CheckedChanged(object sender, EventArgs e)
        {
            RegisterHidDevices();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check for Squirrel update.
            TrySquirrelUpdate(false);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox box = new AboutBox();
            box.ShowDialog();
        }

        private void radioButtonInputSink_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                RegisterHidDevices();
            }
        }

        private void radioButtonExInputSink_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                RegisterHidDevices();
            }

        }

        private void radioButtonNone_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                RegisterHidDevices();
            }
        }

        private void buttonClearLogs_Click(object sender, EventArgs e)
        {
            richTextBoxLogs.Text = "";
        }

        private void treeViewDevices_AfterCheck(object sender, TreeViewEventArgs e)
        {
            RegisterHidDevices();
        }

        private void treeViewDevices_KeyDown(object sender, KeyEventArgs e)
        {
            // Allow copying text from node
            /*
            if (e.KeyData == (Keys.Control | Keys.C))
            {
                if (treeViewDevices.SelectedNode != null)
                {
                    Clipboard.SetText(treeViewDevices.SelectedNode.Text);
                }
                e.SuppressKeyPress = true;
            }

            // Launch property dialog
            if (e.KeyData == (Keys.Control | Keys.P))
            {
                if (treeViewDevices.SelectedNode != null)
                {
                    OpenPropertiesDialog(treeViewDevices.SelectedNode.Text);
                }
                e.SuppressKeyPress = true;
            }
            */
         
        }

        private void treeViewDevices_MouseUp(object sender, MouseEventArgs e)
        {
            // Show menu only if the right mouse button is clicked.
            if (e.Button == MouseButtons.Right)
            {

                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                // Get the node that the user has clicked.
                TreeNode node = treeViewDevices.GetNodeAt(p);
                if (node != null)
                {

                    // Select the node the user has clicked.
                    // The node appears selected until the menu is displayed on the screen.
                    //m_OldSelectNode = treeView1.SelectedNode;
                    treeViewDevices.SelectedNode = node;

                    contextMenuStripDevice.Show(treeViewDevices,p);

                    // Find the appropriate ContextMenu depending on the selected node.
                    /*
                    switch (Convert.ToString(node.Tag))
                    {
                        case "TextFile":
                            mnuTextFile.Show(treeView1, p);
                            break;
                        case "File":
                            mnuFile.Show(treeView1, p);
                            break;
                    }
                    */

                    // Highlight the selected node.
                    //treeView1.SelectedNode = m_OldSelectNode;
                    //m_OldSelectNode = null;
                }
            }
        }

        private void deviceNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Walk up the tree until we find our device
            TreeNode node = treeViewDevices.SelectedNode;
            while (!(node.Tag is Hid.Device))
            {
                node = node.Parent;
            }
            Hid.Device device = (Hid.Device)node.Tag;

            // We were testing this trying to get a device from its instance path or instance id without joy really.
            // The goal was to get to the friendly name of the Razor Junglecat bluetooth device which we should be able to get from its grandparent I believe.
            // See:
            // https://stackoverflow.com/questions/956669/does-setupdigetclassdevs-work-with-device-instance-ids-as-documented
            // https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetclassdevsw
            // https://docs.microsoft.com/en-us/windows-hardware/drivers/install/determining-the-parent-of-a-device
            // https://docs.microsoft.com/en-us/windows-hardware/drivers/install/retrieving-device-relations
            // https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetdevicepropertyw
            string parent = "BTHLEDevice\\{00001812-0000-1000-8000-00805f9b34fb}_Dev_VID&021532_PID&070a_REV&0001_e889516bc56f\\8&ab05448&1&0017";
            string deviceInstanceId = "8&ab05448&1&0017";
            Guid GUID_DEVINTERFACE_BT = new Guid("E0CBF06C-CD8B-4647-BB8A-263B43F0F974");
            OpenPropertiesDialog(parent);

            // Then show our properties dialog
            OpenPropertiesDialog(device);
        }

        private void copyNodeTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Just copy current node text
            Clipboard.SetText(treeViewDevices.SelectedNode.Text);
        }
    }
}
