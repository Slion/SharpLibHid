﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControlSample
{
    public partial class MainForm
    {
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelButtonName = new System.Windows.Forms.Label();
            this.labelDeviceName = new System.Windows.Forms.Label();
            this.buttonClear = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageMessages = new System.Windows.Forms.TabPage();
            this.listViewEvents = new System.Windows.Forms.ListView();
            this.columnHeaderUsages = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderInputReport = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUsagePage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUsageCollection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderRepeat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPageDevices = new System.Windows.Forms.TabPage();
            this.treeViewDevices = new System.Windows.Forms.TreeView();
            this.tabControl.SuspendLayout();
            this.tabPageMessages.SuspendLayout();
            this.tabPageDevices.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelButtonName
            // 
            this.labelButtonName.AutoSize = true;
            this.labelButtonName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelButtonName.Location = new System.Drawing.Point(785, 53);
            this.labelButtonName.Name = "labelButtonName";
            this.labelButtonName.Size = new System.Drawing.Size(103, 20);
            this.labelButtonName.TabIndex = 0;
            this.labelButtonName.Text = "Button Name";
            // 
            // labelDeviceName
            // 
            this.labelDeviceName.AutoSize = true;
            this.labelDeviceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDeviceName.Location = new System.Drawing.Point(785, 33);
            this.labelDeviceName.Name = "labelDeviceName";
            this.labelDeviceName.Size = new System.Drawing.Size(103, 20);
            this.labelDeviceName.TabIndex = 1;
            this.labelDeviceName.Text = "Device Name";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(813, 6);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageMessages);
            this.tabControl.Controls.Add(this.tabPageDevices);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(902, 514);
            this.tabControl.TabIndex = 4;
            // 
            // tabPageMessages
            // 
            this.tabPageMessages.Controls.Add(this.listViewEvents);
            this.tabPageMessages.Controls.Add(this.buttonClear);
            this.tabPageMessages.Controls.Add(this.labelDeviceName);
            this.tabPageMessages.Controls.Add(this.labelButtonName);
            this.tabPageMessages.Location = new System.Drawing.Point(4, 22);
            this.tabPageMessages.Name = "tabPageMessages";
            this.tabPageMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMessages.Size = new System.Drawing.Size(894, 488);
            this.tabPageMessages.TabIndex = 0;
            this.tabPageMessages.Text = "Messages";
            this.tabPageMessages.UseVisualStyleBackColor = true;
            // 
            // listViewEvents
            // 
            this.listViewEvents.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listViewEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewEvents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUsages,
            this.columnHeaderInputReport,
            this.columnHeaderUsagePage,
            this.columnHeaderUsageCollection,
            this.columnHeaderRepeat,
            this.columnHeaderTime});
            this.listViewEvents.GridLines = true;
            this.listViewEvents.Location = new System.Drawing.Point(8, 6);
            this.listViewEvents.Name = "listViewEvents";
            this.listViewEvents.Size = new System.Drawing.Size(744, 474);
            this.listViewEvents.TabIndex = 3;
            this.listViewEvents.UseCompatibleStateImageBehavior = false;
            this.listViewEvents.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderUsages
            // 
            this.columnHeaderUsages.Text = "Usages";
            this.columnHeaderUsages.Width = 180;
            // 
            // columnHeaderInputReport
            // 
            this.columnHeaderInputReport.Text = "Input Report";
            this.columnHeaderInputReport.Width = 176;
            // 
            // columnHeaderUsagePage
            // 
            this.columnHeaderUsagePage.Text = "Usage Page";
            this.columnHeaderUsagePage.Width = 87;
            // 
            // columnHeaderUsageCollection
            // 
            this.columnHeaderUsageCollection.Text = "Usage Collection";
            this.columnHeaderUsageCollection.Width = 134;
            // 
            // columnHeaderRepeat
            // 
            this.columnHeaderRepeat.Text = "Repeat";
            this.columnHeaderRepeat.Width = 68;
            // 
            // columnHeaderTime
            // 
            this.columnHeaderTime.Text = "Time";
            this.columnHeaderTime.Width = 76;
            // 
            // tabPageDevices
            // 
            this.tabPageDevices.Controls.Add(this.treeViewDevices);
            this.tabPageDevices.Location = new System.Drawing.Point(4, 22);
            this.tabPageDevices.Name = "tabPageDevices";
            this.tabPageDevices.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDevices.Size = new System.Drawing.Size(918, 512);
            this.tabPageDevices.TabIndex = 1;
            this.tabPageDevices.Text = "Devices";
            this.tabPageDevices.UseVisualStyleBackColor = true;
            // 
            // treeViewDevices
            // 
            this.treeViewDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewDevices.Location = new System.Drawing.Point(8, 6);
            this.treeViewDevices.Name = "treeViewDevices";
            this.treeViewDevices.Size = new System.Drawing.Size(713, 498);
            this.treeViewDevices.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(926, 538);
            this.Controls.Add(this.tabControl);
            this.Name = "MainForm";
            this.Text = "Remote Control Sample";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageMessages.ResumeLayout(false);
            this.tabPageMessages.PerformLayout();
            this.tabPageDevices.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion Windows Form Designer generated code

    }
}