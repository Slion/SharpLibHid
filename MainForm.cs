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
	public class MainForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
        private RemoteControlDevice _remote;
        private Label labelButtonName;
        private Label labelDeviceName;
        private ListView listViewEvents;
        private ColumnHeader columnHeaderUsage;
        private ColumnHeader columnHeaderUsagePage;
        private ColumnHeader columnHeaderUsageCollection;
        private ColumnHeader columnHeaderRepeat;
        private ColumnHeader columnHeaderTime;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.labelButtonName = new System.Windows.Forms.Label();
            this.labelDeviceName = new System.Windows.Forms.Label();
            this.listViewEvents = new System.Windows.Forms.ListView();
            this.columnHeaderUsage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUsagePage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUsageCollection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderRepeat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // labelButtonName
            // 
            this.labelButtonName.AutoSize = true;
            this.labelButtonName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelButtonName.Location = new System.Drawing.Point(600, 32);
            this.labelButtonName.Name = "labelButtonName";
            this.labelButtonName.Size = new System.Drawing.Size(103, 20);
            this.labelButtonName.TabIndex = 0;
            this.labelButtonName.Text = "Button Name";
            // 
            // labelDeviceName
            // 
            this.labelDeviceName.AutoSize = true;
            this.labelDeviceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDeviceName.Location = new System.Drawing.Point(600, 12);
            this.labelDeviceName.Name = "labelDeviceName";
            this.labelDeviceName.Size = new System.Drawing.Size(103, 20);
            this.labelDeviceName.TabIndex = 1;
            this.labelDeviceName.Text = "Device Name";
            // 
            // listViewEvents
            // 
            this.listViewEvents.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listViewEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewEvents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUsage,
            this.columnHeaderUsagePage,
            this.columnHeaderUsageCollection,
            this.columnHeaderRepeat,
            this.columnHeaderTime});
            this.listViewEvents.GridLines = true;
            this.listViewEvents.Location = new System.Drawing.Point(12, 12);
            this.listViewEvents.Name = "listViewEvents";
            this.listViewEvents.Size = new System.Drawing.Size(582, 369);
            this.listViewEvents.TabIndex = 2;
            this.listViewEvents.UseCompatibleStateImageBehavior = false;
            this.listViewEvents.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderUsage
            // 
            this.columnHeaderUsage.Text = "Usage";
            this.columnHeaderUsage.Width = 180;
            // 
            // columnHeaderUsagePage
            // 
            this.columnHeaderUsagePage.Text = "Usage Page";
            this.columnHeaderUsagePage.Width = 120;
            // 
            // columnHeaderUsageCollection
            // 
            this.columnHeaderUsageCollection.Text = "Usage Collection";
            this.columnHeaderUsageCollection.Width = 120;
            // 
            // columnHeaderRepeat
            // 
            this.columnHeaderRepeat.Text = "Repeat";
            // 
            // columnHeaderTime
            // 
            this.columnHeaderTime.Text = "Time";
            this.columnHeaderTime.Width = 76;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(926, 393);
            this.Controls.Add(this.listViewEvents);
            this.Controls.Add(this.labelDeviceName);
            this.Controls.Add(this.labelButtonName);
            this.Name = "MainForm";
            this.Text = "Remote Control Sample";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion Windows Form Designer generated code

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.Run(new MainForm());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
            _remote = new RemoteControlDevice(this.Handle);
            _remote.ButtonPressed += new Devices.RemoteControl.RemoteControlDevice.RemoteControlDeviceEventHandler(_remote_ButtonPressed);
            _remote.iHidHandler.OnHidEvent += HandleHidEventThreadSafe;             
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
            else if (e.MceButton != Hid.UsageTables.WindowsMediaCenterRemoteControl.Null)
            {
                //Display MCE button name
                //labelButtonName.Text = e.MceButton.ToString();
                //Check if this is an HP extension
                if (Enum.IsDefined(typeof(Hid.UsageTables.HpWindowsMediaCenterRemoteControl), (ushort)e.MceButton))
                {
                    //Also display HP button name
                    //labelButtonName.Text += " / HP:" + ((Hid.UsageTables.HpWindowsMediaCenterRemoteControl)e.MceButton).ToString();
                }

                processed = true;                
            }
            else if (e.ConsumerControl != Hid.UsageTables.ConsumerControl.Null)
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

	}
}
