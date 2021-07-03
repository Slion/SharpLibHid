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
using SharpLib.Win32;

//For ClickOnce support
using System.Deployment.Application;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

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
        public static Boolean OpenPropertiesDialog(string aDevicePath)
        {
            // We could not find a better way to do this other than just trying with the various classes: HID, Keyboard and Mouse
            // Get the GUID of the HID class
            Guid guid;
            Extra.Function.HidD_GetHidGuid(out guid);
            // Try HID
            Boolean ok=OpenPropertiesDialog(aDevicePath, guid);
            if (ok) return ok;
            // Try mouse
            ok = OpenPropertiesDialog(aDevicePath, Extra.Const.GUID_DEVINTERFACE_MOUSE);
            if (ok) return ok;
            // Try keyboard
            ok = OpenPropertiesDialog(aDevicePath, Extra.Const.GUID_DEVINTERFACE_KEYBOARD);
            return ok;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDevicePath"></param>
        /// <param name="aClass"></param>
        public static Boolean OpenPropertiesDialog(string aDevicePath, Guid aClass)
        {
            uint index = 0;

            // Get a handle to a list of devices matching the given class GUID.
            IntPtr hDevInfo = Extra.Function.SetupDiGetClassDevs(ref aClass, null, IntPtr.Zero, Extra.Const.DIGCF_DEVICEINTERFACE);
            //IntPtr hDevInfo = HIDImports.SetupDiGetClassDevs(IntPtr.Zero, null, IntPtr.Zero, HIDImports.DIGCF_DEVICEINTERFACE | HIDImports.DIGCF_PRESENT);
            //IntPtr hDevInfo = HIDImports.SetupDiCreateDeviceInfoList(IntPtr.Zero, IntPtr.Zero);

            // Alloc and init
            Extra.SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new Extra.SP_DEVICE_INTERFACE_DATA();
            deviceInterfaceData.cbSize = Marshal.SizeOf(deviceInterfaceData);

            // Alloc and init
            Extra.SP_DEVINFO_DATA deviceInfoData = new Extra.SP_DEVINFO_DATA();
            deviceInfoData.cbSize = (uint)Marshal.SizeOf(deviceInfoData);

            // get a device interface to a single device (enumerate all devices)
            Boolean keepGoing = false;
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
                keepGoing = Extra.Function.SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref aClass, index, ref deviceInterfaceData);
                if (!keepGoing)
                {
                    //Debug.Print(Marshal.GetLastWin32Error().ToString());
                    break;
                }

                UInt32 size = 0;

                // Get the buffer size for this device detail instance (returned in the size parameter)
                Extra.Function.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref deviceInterfaceData, IntPtr.Zero, 0, out size, IntPtr.Zero);

                // Create a detail struct and set its size
                Extra.SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetail = new Extra.SP_DEVICE_INTERFACE_DETAIL_DATA();
                //SL: does not make sense, fix it
                //On Win x86, cbSize = 5, On x64, cbSize = 8 i
                deviceInterfaceDetail.cbSize = (IntPtr.Size == 8) ? (uint)8 : (uint)5;

                // TODO: Fixed that fixed size buffer size defined in SP_DEVICE_INTERFACE_DETAIL_DATA
                // Actually get the detail struct
                if (Extra.Function.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref deviceInterfaceData, ref deviceInterfaceDetail, size, out size, ref deviceInfoData))
                {
                    // Check that current device is matching requested device
                    if (deviceInterfaceDetail.DevicePath.ToLower() == aDevicePath.ToLower())
                    {
                        // It's a match, just open that system dialog. 
                        // Get Device intance path
                        StringBuilder deviceInstanceId = new StringBuilder(512);
                        int requiredSize = 0;
                        Extra.Function.SetupDiGetDeviceInstanceId(hDevInfo, ref deviceInfoData, deviceInstanceId, 512, out requiredSize);

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
                else
                {
                    //Debug.Print(Marshal.GetLastWin32Error().ToString());
                }
                index++;
            } while (keepGoing);


            Extra.Function.SetupDiDestroyDeviceInfoList(hDevInfo);


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
                listViewEvents.Items.Insert(0, aHidEvent.ToListViewItem());
                if (aHidEvent.Device != null)
                {
                    toolStripStatusLabelDevice.Text = aHidEvent.Device.FriendlyName;
                }
                
                richTextBoxLogs.AppendText(aHidEvent.ToLog());
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
                    iHidParser.ProcessInput(ref message);
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

            // Then show our porpeties dialog
            OpenPropertiesDialog(device.Name);
        }

        private void copyNodeTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Just copy current node text
            Clipboard.SetText(treeViewDevices.SelectedNode.Text);
        }
    }
}
