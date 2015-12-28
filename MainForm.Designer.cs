//
// Copyright (C) 2014-2015 Stéphane Lenclud.
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
using System.Collections.Generic;
using System.Text;

namespace HidDemo
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
            this.buttonClear = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageMessages = new System.Windows.Forms.TabPage();
            this.labelRepeatSpeed = new System.Windows.Forms.Label();
            this.labelRepeatDelay = new System.Windows.Forms.Label();
            this.numericRepeatSpeed = new System.Windows.Forms.NumericUpDown();
            this.numericRepeatDelay = new System.Windows.Forms.NumericUpDown();
            this.checkBoxRepeat = new System.Windows.Forms.CheckBox();
            this.listViewEvents = new System.Windows.Forms.ListView();
            this.columnHeaderUsages = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderInputReport = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUsagePage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUsageCollection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderRepeat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPageDevices = new System.Windows.Forms.TabPage();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonTreeViewExpandAll = new System.Windows.Forms.Button();
            this.buttonTreeViewCollapseAll = new System.Windows.Forms.Button();
            this.treeViewDevices = new System.Windows.Forms.TreeView();
            this.tabPageTests = new System.Windows.Forms.TabPage();
            this.textBoxTests = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelDevice = new System.Windows.Forms.ToolStripStatusLabel();
            this.checkBoxUseSingleHandler = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tabPageMessages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericRepeatSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRepeatDelay)).BeginInit();
            this.tabPageDevices.SuspendLayout();
            this.tabPageTests.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
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
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl.Controls.Add(this.tabPageMessages);
            this.tabControl.Controls.Add(this.tabPageDevices);
            this.tabControl.Controls.Add(this.tabPageTests);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(902, 565);
            this.tabControl.TabIndex = 4;
            // 
            // tabPageMessages
            // 
            this.tabPageMessages.Controls.Add(this.checkBoxUseSingleHandler);
            this.tabPageMessages.Controls.Add(this.labelRepeatSpeed);
            this.tabPageMessages.Controls.Add(this.labelRepeatDelay);
            this.tabPageMessages.Controls.Add(this.numericRepeatSpeed);
            this.tabPageMessages.Controls.Add(this.numericRepeatDelay);
            this.tabPageMessages.Controls.Add(this.checkBoxRepeat);
            this.tabPageMessages.Controls.Add(this.listViewEvents);
            this.tabPageMessages.Controls.Add(this.buttonClear);
            this.tabPageMessages.Location = new System.Drawing.Point(4, 22);
            this.tabPageMessages.Name = "tabPageMessages";
            this.tabPageMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMessages.Size = new System.Drawing.Size(894, 539);
            this.tabPageMessages.TabIndex = 0;
            this.tabPageMessages.Text = "Messages";
            this.tabPageMessages.UseVisualStyleBackColor = true;
            // 
            // labelRepeatSpeed
            // 
            this.labelRepeatSpeed.AutoSize = true;
            this.labelRepeatSpeed.Location = new System.Drawing.Point(771, 97);
            this.labelRepeatSpeed.Name = "labelRepeatSpeed";
            this.labelRepeatSpeed.Size = new System.Drawing.Size(60, 13);
            this.labelRepeatSpeed.TabIndex = 8;
            this.labelRepeatSpeed.Text = "Speed (ms)";
            // 
            // labelRepeatDelay
            // 
            this.labelRepeatDelay.AutoSize = true;
            this.labelRepeatDelay.Location = new System.Drawing.Point(770, 71);
            this.labelRepeatDelay.Name = "labelRepeatDelay";
            this.labelRepeatDelay.Size = new System.Drawing.Size(56, 13);
            this.labelRepeatDelay.TabIndex = 7;
            this.labelRepeatDelay.Text = "Delay (ms)";
            // 
            // numericRepeatSpeed
            // 
            this.numericRepeatSpeed.Location = new System.Drawing.Point(834, 95);
            this.numericRepeatSpeed.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericRepeatSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericRepeatSpeed.Name = "numericRepeatSpeed";
            this.numericRepeatSpeed.Size = new System.Drawing.Size(57, 20);
            this.numericRepeatSpeed.TabIndex = 6;
            this.numericRepeatSpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericRepeatSpeed.ValueChanged += new System.EventHandler(this.numericRepeatSpeed_ValueChanged);
            // 
            // numericRepeatDelay
            // 
            this.numericRepeatDelay.Location = new System.Drawing.Point(834, 69);
            this.numericRepeatDelay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericRepeatDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericRepeatDelay.Name = "numericRepeatDelay";
            this.numericRepeatDelay.Size = new System.Drawing.Size(57, 20);
            this.numericRepeatDelay.TabIndex = 5;
            this.numericRepeatDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericRepeatDelay.ValueChanged += new System.EventHandler(this.numericRepeatDelay_ValueChanged);
            // 
            // checkBoxRepeat
            // 
            this.checkBoxRepeat.AutoSize = true;
            this.checkBoxRepeat.Location = new System.Drawing.Point(813, 46);
            this.checkBoxRepeat.Name = "checkBoxRepeat";
            this.checkBoxRepeat.Size = new System.Drawing.Size(61, 17);
            this.checkBoxRepeat.TabIndex = 4;
            this.checkBoxRepeat.Text = "Repeat";
            this.checkBoxRepeat.UseVisualStyleBackColor = true;
            this.checkBoxRepeat.CheckedChanged += new System.EventHandler(this.checkBoxRepeat_CheckedChanged);
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
            this.listViewEvents.Size = new System.Drawing.Size(744, 525);
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
            this.tabPageDevices.Controls.Add(this.buttonRefresh);
            this.tabPageDevices.Controls.Add(this.buttonTreeViewExpandAll);
            this.tabPageDevices.Controls.Add(this.buttonTreeViewCollapseAll);
            this.tabPageDevices.Controls.Add(this.treeViewDevices);
            this.tabPageDevices.Location = new System.Drawing.Point(4, 22);
            this.tabPageDevices.Name = "tabPageDevices";
            this.tabPageDevices.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDevices.Size = new System.Drawing.Size(894, 539);
            this.tabPageDevices.TabIndex = 1;
            this.tabPageDevices.Text = "Devices";
            this.tabPageDevices.UseVisualStyleBackColor = true;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(813, 64);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 3;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonTreeViewExpandAll
            // 
            this.buttonTreeViewExpandAll.Location = new System.Drawing.Point(813, 6);
            this.buttonTreeViewExpandAll.Name = "buttonTreeViewExpandAll";
            this.buttonTreeViewExpandAll.Size = new System.Drawing.Size(75, 23);
            this.buttonTreeViewExpandAll.TabIndex = 2;
            this.buttonTreeViewExpandAll.Text = "Expand All";
            this.buttonTreeViewExpandAll.UseVisualStyleBackColor = true;
            this.buttonTreeViewExpandAll.Click += new System.EventHandler(this.buttonTreeViewExpandAll_Click);
            // 
            // buttonTreeViewCollapseAll
            // 
            this.buttonTreeViewCollapseAll.Location = new System.Drawing.Point(813, 35);
            this.buttonTreeViewCollapseAll.Name = "buttonTreeViewCollapseAll";
            this.buttonTreeViewCollapseAll.Size = new System.Drawing.Size(75, 23);
            this.buttonTreeViewCollapseAll.TabIndex = 1;
            this.buttonTreeViewCollapseAll.Text = "Collapse All";
            this.buttonTreeViewCollapseAll.UseVisualStyleBackColor = true;
            this.buttonTreeViewCollapseAll.Click += new System.EventHandler(this.buttonTreeViewCollapseAll_Click);
            // 
            // treeViewDevices
            // 
            this.treeViewDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewDevices.Location = new System.Drawing.Point(8, 6);
            this.treeViewDevices.Name = "treeViewDevices";
            this.treeViewDevices.Size = new System.Drawing.Size(713, 525);
            this.treeViewDevices.TabIndex = 0;
            // 
            // tabPageTests
            // 
            this.tabPageTests.Controls.Add(this.textBoxTests);
            this.tabPageTests.Location = new System.Drawing.Point(4, 22);
            this.tabPageTests.Name = "tabPageTests";
            this.tabPageTests.Size = new System.Drawing.Size(894, 539);
            this.tabPageTests.TabIndex = 2;
            this.tabPageTests.Text = "Tests";
            this.tabPageTests.UseVisualStyleBackColor = true;
            // 
            // textBoxTests
            // 
            this.textBoxTests.Location = new System.Drawing.Point(4, 4);
            this.textBoxTests.Multiline = true;
            this.textBoxTests.Name = "textBoxTests";
            this.textBoxTests.Size = new System.Drawing.Size(887, 499);
            this.textBoxTests.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelDevice});
            this.statusStrip.Location = new System.Drawing.Point(0, 580);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(944, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabelDevice
            // 
            this.toolStripStatusLabelDevice.Name = "toolStripStatusLabelDevice";
            this.toolStripStatusLabelDevice.Size = new System.Drawing.Size(61, 17);
            this.toolStripStatusLabelDevice.Text = "No Device";
            // 
            // checkBoxUseSingleHandler
            // 
            this.checkBoxUseSingleHandler.AutoSize = true;
            this.checkBoxUseSingleHandler.Checked = true;
            this.checkBoxUseSingleHandler.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseSingleHandler.Location = new System.Drawing.Point(771, 132);
            this.checkBoxUseSingleHandler.Name = "checkBoxUseSingleHandler";
            this.checkBoxUseSingleHandler.Size = new System.Drawing.Size(117, 17);
            this.checkBoxUseSingleHandler.TabIndex = 9;
            this.checkBoxUseSingleHandler.Text = "Use Single Handler";
            this.checkBoxUseSingleHandler.UseVisualStyleBackColor = true;
            this.checkBoxUseSingleHandler.CheckedChanged += new System.EventHandler(this.checkBoxUseSingleHandler_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(944, 602);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(960, 640);
            this.Name = "MainForm";
            this.Text = "HID Demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageMessages.ResumeLayout(false);
            this.tabPageMessages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericRepeatSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRepeatDelay)).EndInit();
            this.tabPageDevices.ResumeLayout(false);
            this.tabPageTests.ResumeLayout(false);
            this.tabPageTests.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion Windows Form Designer generated code

        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageMessages;
        private System.Windows.Forms.ListView listViewEvents;
        private System.Windows.Forms.ColumnHeader columnHeaderUsages;
        private System.Windows.Forms.ColumnHeader columnHeaderInputReport;
        private System.Windows.Forms.ColumnHeader columnHeaderUsagePage;
        private System.Windows.Forms.ColumnHeader columnHeaderUsageCollection;
        private System.Windows.Forms.ColumnHeader columnHeaderRepeat;
        private System.Windows.Forms.ColumnHeader columnHeaderTime;
        private System.Windows.Forms.TabPage tabPageDevices;
        private System.Windows.Forms.TreeView treeViewDevices;
        private System.Windows.Forms.Button buttonTreeViewExpandAll;
        private System.Windows.Forms.Button buttonTreeViewCollapseAll;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelDevice;
        private System.Windows.Forms.TabPage tabPageTests;
        private System.Windows.Forms.TextBox textBoxTests;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.CheckBox checkBoxRepeat;
        private System.Windows.Forms.Label labelRepeatDelay;
        private System.Windows.Forms.NumericUpDown numericRepeatSpeed;
        private System.Windows.Forms.NumericUpDown numericRepeatDelay;
        private System.Windows.Forms.Label labelRepeatSpeed;
        private System.Windows.Forms.CheckBox checkBoxUseSingleHandler;
    }
}
