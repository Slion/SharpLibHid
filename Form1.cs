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
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private RemoteControlDevice _remote;
		private System.Windows.Forms.Label label2;
		private Timer _timer;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_timer = new Timer();
			_timer.Interval = 3000;
			_timer.Enabled = false;
			_timer.Tick +=new EventHandler(_timer_Tick);
			_remote = new RemoteControlDevice();
			_remote.ButtonPressed +=new Devices.RemoteControl.RemoteControlDevice.RemoteControlDeviceEventHandler(_remote_ButtonPressed);
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			//
			// label1
			//
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(736, 266);
			this.label1.TabIndex = 0;
			this.label1.Text = "Ready...";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label2
			//
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(72, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(576, 23);
			this.label2.TabIndex = 1;
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// Form1
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.LightSteelBlue;
			this.ClientSize = new System.Drawing.Size(736, 266);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "Remote Control Sample";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion Windows Form Designer generated code

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{

		}


		protected override void WndProc(ref Message message)
		{
			_remote.ProcessMessage(message);
			base.WndProc(ref message);
		}

		private void _remote_ButtonPressed(object sender, RemoteControlEventArgs e)
		{
			_timer.Enabled = false;
            if (e.Button != RemoteControlButton.Unknown)
            {
                label1.Text = e.Button.ToString();
            }
            else if (e.MceButton != Hid.UsageTables.MceButton.Null)
            {
                //Display MCE button name
                label1.Text = e.MceButton.ToString();
                //Check if this is an HP extension
                if (Enum.IsDefined(typeof(Hid.UsageTables.HpMceButton), (ushort)e.MceButton))
                {
                    //Also display HP button name
                    label1.Text += " / HP:" + ((Hid.UsageTables.HpMceButton)e.MceButton).ToString();
                }
            }
            else
            {
                label1.Text = "Unknown";
            }
			label2.Text = e.Device.ToString();
			_timer.Enabled = true;
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			_timer.Enabled = false;
			label1.Text = "Ready...";
		}
	}
}
