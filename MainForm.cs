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
		private Timer _timer;

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
            this.SuspendLayout();
            // 
            // labelButtonName
            // 
            this.labelButtonName.AutoSize = true;
            this.labelButtonName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelButtonName.Location = new System.Drawing.Point(257, 97);
            this.labelButtonName.Name = "labelButtonName";
            this.labelButtonName.Size = new System.Drawing.Size(103, 20);
            this.labelButtonName.TabIndex = 0;
            this.labelButtonName.Text = "Button Name";
            // 
            // labelDeviceName
            // 
            this.labelDeviceName.AutoSize = true;
            this.labelDeviceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDeviceName.Location = new System.Drawing.Point(257, 67);
            this.labelDeviceName.Name = "labelDeviceName";
            this.labelDeviceName.Size = new System.Drawing.Size(103, 20);
            this.labelDeviceName.TabIndex = 1;
            this.labelDeviceName.Text = "Device Name";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(784, 393);
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
            bool processed = false;
			_timer.Enabled = false;
            if (e.Button != RemoteControlButton.Unknown)
            {
                labelButtonName.Text = e.Button.ToString();
                processed = true;
            }
            else if (e.MceButton != Hid.UsageTables.WindowsMediaCenterRemoteControl.Null)
            {
                //Display MCE button name
                labelButtonName.Text = e.MceButton.ToString();
                //Check if this is an HP extension
                if (Enum.IsDefined(typeof(Hid.UsageTables.HpWindowsMediaCenterRemoteControl), (ushort)e.MceButton))
                {
                    //Also display HP button name
                    labelButtonName.Text += " / HP:" + ((Hid.UsageTables.HpWindowsMediaCenterRemoteControl)e.MceButton).ToString();
                }

                processed = true;
            }
            else if (e.ConsumerControl != Hid.UsageTables.ConsumerControl.Null)
            {
                //Display consumer control name
                labelButtonName.Text = e.ConsumerControl.ToString();
                processed = true;
            }
            else
            {
                labelButtonName.Text = "Unknown";
            }
			labelDeviceName.Text = e.Device.ToString();
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
