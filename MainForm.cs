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
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            Application.EnableVisualStyles();
			Application.Run(new MainForm());
		}

        /// <summary>
        /// Get a ClickOnce version string if any otherwise otherwise return a default string.
        /// </summary>
        /// <returns>ClickOnce version string or place holder instead.</returns>
        public static string ClickOnceVersion()
        {
            //Check if we are running a Click Once deployed application
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                //This is a proper Click Once installation, fetch and show our version number
                return "v" + ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            else
            {
                //Not a proper Click Once installation, assuming development build then
                return "development";
            }
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            this.Text += " - " + ClickOnceVersion();
            
            PopulateDeviceList();
            RegisterHidDevices();
            CheckDefaultDevices();

            // Register for key down event
            KeyDown += OnKeyDown;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {

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
                
                richTextBoxLogs.AppendText(aHidEvent.ToString());
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

        /// <summary>
        /// ClickOnce install update.
        /// </summary>
        private void InstallUpdateSyncWithInfo()
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
                        if (!(DialogResult.OK == dr))
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " +
                            "version to version " + info.MinimumRequiredVersion.ToString() +
                            ". The application will now install the update and restart.",
                            "Update Available", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You are already running the latest version.", "Application up-to-date");
                }
            }
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
            //Check for ClickOnce update.
            InstallUpdateSyncWithInfo();
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
    }
}
