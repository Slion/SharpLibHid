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

            RegisterHidDevices();
            //Create our list of HID devices
            SharpLib.Win32.RawInput.PopulateDeviceList(treeViewDevices);
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

        void RegisterHidDevices()
	    {
            // Register the input device to receive the commands from the
            // remote device. See http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwmt/html/remote_control.asp
            // for the vendor defined usage page.

            DisposeHandlers();

            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[5];

            int i = 0;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.WindowsMediaCenterRemoteControl;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.WindowsMediaCenter.WindowsMediaCenterRemoteControl;
            rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.Consumer;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.Consumer.ConsumerControl;
            rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.Consumer;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.Consumer.Selection;
            rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.GenericDesktopControls;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.GenericDesktop.SystemControl;
            rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.GenericDesktopControls;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.GenericDesktop.GamePad;
            rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
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
                toolStripStatusLabelDevice.Text = aHidEvent.Device.FriendlyName;
            }
        }

		protected override void WndProc(ref Message message)
		{
            switch (message.Msg)
            {
                case Const.WM_KEYDOWN:
                    //ProcessKeyDown(message.WParam);
                    break;
                case Const.WM_INPUT:
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
            treeViewDevices.Nodes.Clear();
            SharpLib.Win32.RawInput.PopulateDeviceList(treeViewDevices);
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
    }
}
