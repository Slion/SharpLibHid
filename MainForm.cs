using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Devices.RemoteControl;

namespace RemoteControlSample
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public partial class MainForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
        private RemoteControlDevice _remote;
        private Label labelButtonName;
        private Label labelDeviceName;
        private Button buttonClear;
        private TabControl tabControl;
        private TabPage tabPageMessages;
        private ListView listViewEvents;
        private ColumnHeader columnHeaderUsages;
        private ColumnHeader columnHeaderInputReport;
        private ColumnHeader columnHeaderUsagePage;
        private ColumnHeader columnHeaderUsageCollection;
        private ColumnHeader columnHeaderRepeat;
        private ColumnHeader columnHeaderTime;
        private TabPage tabPageDevices;
        private TreeView treeViewDevices;
		private Timer _timer;

        public delegate void OnHidEventDelegate(object aSender, Hid.HidEvent aHidEvent);

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_timer = new Timer();
			_timer.Interval = 3000;
			_timer.Enabled = false;
			_timer.Tick +=new EventHandler(_timer_Tick);            
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
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

        private void MainForm_Load(object sender, System.EventArgs e)
		{
            _remote = new RemoteControlDevice(this.Handle);
            _remote.ButtonPressed += new Devices.RemoteControl.RemoteControlDevice.RemoteControlDeviceEventHandler(_remote_ButtonPressed);
            _remote.iHidHandler.OnHidEvent += HandleHidEventThreadSafe;
            
            //
            Win32.RawInput.PopulateDeviceList(treeViewDevices);

		}

        public void HandleHidEventThreadSafe(object aSender, Hid.HidEvent aHidEvent)
        {
            if (aHidEvent.IsStray)
            {
                //Stray event just ignore it
                return;
            }

            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                OnHidEventDelegate d = new OnHidEventDelegate(HandleHidEventThreadSafe);
                this.Invoke(d, new object[] { aSender, aHidEvent });
            }
            else
            {
                //We are in the proper thread
                listViewEvents.Items.Insert(0, aHidEvent.ToListViewItem());
            }
        }

		protected override void WndProc(ref Message message)
		{
            if (_remote != null)
            {
                _remote.ProcessMessage(message);
            }
			base.WndProc(ref message);
		}

		private bool _remote_ButtonPressed(object sender, RemoteControlEventArgs e)
		{
            //Set text from here was disabled because of threading issues
            //That whole thing should be removed anyway
            bool processed = false;
			_timer.Enabled = false;
            if (e.Button != RemoteControlButton.Unknown)
            {
                //labelButtonName.Text = e.Button.ToString();
                processed = true;
            }
            else if (e.MceButton != Hid.Usage.WindowsMediaCenterRemoteControl.Null)
            {
                //Display MCE button name
                //labelButtonName.Text = e.MceButton.ToString();
                //Check if this is an HP extension
                if (Enum.IsDefined(typeof(Hid.Usage.HpWindowsMediaCenterRemoteControl), (ushort)e.MceButton))
                {
                    //Also display HP button name
                    //labelButtonName.Text += " / HP:" + ((Hid.UsageTables.HpWindowsMediaCenterRemoteControl)e.MceButton).ToString();
                }

                processed = true;                
            }
            else if (e.ConsumerControl != Hid.Usage.ConsumerControl.Null)
            {
                //Display consumer control name
                //labelButtonName.Text = e.ConsumerControl.ToString();
                processed = true;
            }
            else
            {
                //labelButtonName.Text = "Unknown";
            }
			//labelDeviceName.Text = e.Device.ToString();
			_timer.Enabled = true;
            return processed;
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			_timer.Enabled = false;
			labelButtonName.Text = "Ready...";
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

	}
}
