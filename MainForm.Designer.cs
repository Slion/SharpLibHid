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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonClear = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageMessages = new System.Windows.Forms.TabPage();
            this.groupBoxRegistrationFlag = new System.Windows.Forms.GroupBox();
            this.radioButtonNone = new System.Windows.Forms.RadioButton();
            this.radioButtonExInputSink = new System.Windows.Forms.RadioButton();
            this.radioButtonInputSink = new System.Windows.Forms.RadioButton();
            this.checkBoxUseSingleHandler = new System.Windows.Forms.CheckBox();
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
            this.columnBackground = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPageDevices = new System.Windows.Forms.TabPage();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonTreeViewExpandAll = new System.Windows.Forms.Button();
            this.buttonTreeViewCollapseAll = new System.Windows.Forms.Button();
            this.treeViewDevices = new System.Windows.Forms.TreeView();
            this.tabPageTests = new System.Windows.Forms.TabPage();
            this.textBoxTests = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelDevice = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageLogs = new System.Windows.Forms.TabPage();
            this.textBoxLogs = new System.Windows.Forms.TextBox();
            this.tabControl.SuspendLayout();
            this.tabPageMessages.SuspendLayout();
            this.groupBoxRegistrationFlag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericRepeatSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRepeatDelay)).BeginInit();
            this.tabPageDevices.SuspendLayout();
            this.tabPageTests.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.tabPageLogs.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(950, 5);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageMessages);
            this.tabControl.Controls.Add(this.tabPageDevices);
            this.tabControl.Controls.Add(this.tabPageTests);
            this.tabControl.Controls.Add(this.tabPageLogs);
            this.tabControl.Location = new System.Drawing.Point(12, 27);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1039, 594);
            this.tabControl.TabIndex = 4;
            // 
            // tabPageMessages
            // 
            this.tabPageMessages.Controls.Add(this.groupBoxRegistrationFlag);
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
            this.tabPageMessages.Size = new System.Drawing.Size(1031, 568);
            this.tabPageMessages.TabIndex = 0;
            this.tabPageMessages.Text = "Messages";
            this.tabPageMessages.UseVisualStyleBackColor = true;
            // 
            // groupBoxRegistrationFlag
            // 
            this.groupBoxRegistrationFlag.Controls.Add(this.radioButtonNone);
            this.groupBoxRegistrationFlag.Controls.Add(this.radioButtonExInputSink);
            this.groupBoxRegistrationFlag.Controls.Add(this.radioButtonInputSink);
            this.groupBoxRegistrationFlag.Location = new System.Drawing.Point(860, 169);
            this.groupBoxRegistrationFlag.Name = "groupBoxRegistrationFlag";
            this.groupBoxRegistrationFlag.Size = new System.Drawing.Size(165, 102);
            this.groupBoxRegistrationFlag.TabIndex = 10;
            this.groupBoxRegistrationFlag.TabStop = false;
            this.groupBoxRegistrationFlag.Text = "Flag";
            // 
            // radioButtonNone
            // 
            this.radioButtonNone.AutoSize = true;
            this.radioButtonNone.Checked = true;
            this.radioButtonNone.Location = new System.Drawing.Point(7, 66);
            this.radioButtonNone.Name = "radioButtonNone";
            this.radioButtonNone.Size = new System.Drawing.Size(51, 17);
            this.radioButtonNone.TabIndex = 2;
            this.radioButtonNone.TabStop = true;
            this.radioButtonNone.Tag = "0";
            this.radioButtonNone.Text = "None";
            this.radioButtonNone.UseVisualStyleBackColor = true;
            this.radioButtonNone.CheckedChanged += new System.EventHandler(this.radioButtonNone_CheckedChanged);
            // 
            // radioButtonExInputSink
            // 
            this.radioButtonExInputSink.AutoSize = true;
            this.radioButtonExInputSink.Location = new System.Drawing.Point(7, 43);
            this.radioButtonExInputSink.Name = "radioButtonExInputSink";
            this.radioButtonExInputSink.Size = new System.Drawing.Size(97, 17);
            this.radioButtonExInputSink.TabIndex = 1;
            this.radioButtonExInputSink.Tag = "00001000";
            this.radioButtonExInputSink.Text = "EXINPUTSINK";
            this.radioButtonExInputSink.UseVisualStyleBackColor = true;
            this.radioButtonExInputSink.CheckedChanged += new System.EventHandler(this.radioButtonExInputSink_CheckedChanged);
            // 
            // radioButtonInputSink
            // 
            this.radioButtonInputSink.AutoSize = true;
            this.radioButtonInputSink.Location = new System.Drawing.Point(7, 20);
            this.radioButtonInputSink.Name = "radioButtonInputSink";
            this.radioButtonInputSink.Size = new System.Drawing.Size(83, 17);
            this.radioButtonInputSink.TabIndex = 0;
            this.radioButtonInputSink.Tag = "00000100";
            this.radioButtonInputSink.Text = "INPUTSINK";
            this.radioButtonInputSink.UseVisualStyleBackColor = true;
            this.radioButtonInputSink.CheckedChanged += new System.EventHandler(this.radioButtonInputSink_CheckedChanged);
            // 
            // checkBoxUseSingleHandler
            // 
            this.checkBoxUseSingleHandler.AutoSize = true;
            this.checkBoxUseSingleHandler.Checked = true;
            this.checkBoxUseSingleHandler.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseSingleHandler.Location = new System.Drawing.Point(908, 131);
            this.checkBoxUseSingleHandler.Name = "checkBoxUseSingleHandler";
            this.checkBoxUseSingleHandler.Size = new System.Drawing.Size(117, 17);
            this.checkBoxUseSingleHandler.TabIndex = 9;
            this.checkBoxUseSingleHandler.Text = "Use Single Handler";
            this.checkBoxUseSingleHandler.UseVisualStyleBackColor = true;
            this.checkBoxUseSingleHandler.CheckedChanged += new System.EventHandler(this.checkBoxUseSingleHandler_CheckedChanged);
            // 
            // labelRepeatSpeed
            // 
            this.labelRepeatSpeed.AutoSize = true;
            this.labelRepeatSpeed.Location = new System.Drawing.Point(908, 96);
            this.labelRepeatSpeed.Name = "labelRepeatSpeed";
            this.labelRepeatSpeed.Size = new System.Drawing.Size(60, 13);
            this.labelRepeatSpeed.TabIndex = 8;
            this.labelRepeatSpeed.Text = "Speed (ms)";
            // 
            // labelRepeatDelay
            // 
            this.labelRepeatDelay.AutoSize = true;
            this.labelRepeatDelay.Location = new System.Drawing.Point(907, 70);
            this.labelRepeatDelay.Name = "labelRepeatDelay";
            this.labelRepeatDelay.Size = new System.Drawing.Size(56, 13);
            this.labelRepeatDelay.TabIndex = 7;
            this.labelRepeatDelay.Text = "Delay (ms)";
            // 
            // numericRepeatSpeed
            // 
            this.numericRepeatSpeed.Location = new System.Drawing.Point(971, 94);
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
            this.numericRepeatDelay.Location = new System.Drawing.Point(971, 68);
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
            this.checkBoxRepeat.Location = new System.Drawing.Point(950, 45);
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
            this.columnHeaderTime,
            this.columnBackground});
            this.listViewEvents.GridLines = true;
            this.listViewEvents.Location = new System.Drawing.Point(8, 6);
            this.listViewEvents.Name = "listViewEvents";
            this.listViewEvents.Size = new System.Drawing.Size(845, 554);
            this.listViewEvents.TabIndex = 3;
            this.listViewEvents.UseCompatibleStateImageBehavior = false;
            this.listViewEvents.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderUsages
            // 
            this.columnHeaderUsages.Text = "Usages";
            this.columnHeaderUsages.Width = 192;
            // 
            // columnHeaderInputReport
            // 
            this.columnHeaderInputReport.Text = "Input Report";
            this.columnHeaderInputReport.Width = 154;
            // 
            // columnHeaderUsagePage
            // 
            this.columnHeaderUsagePage.Text = "Usage Page";
            this.columnHeaderUsagePage.Width = 87;
            // 
            // columnHeaderUsageCollection
            // 
            this.columnHeaderUsageCollection.Text = "Usage Collection";
            this.columnHeaderUsageCollection.Width = 116;
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
            // columnBackground
            // 
            this.columnBackground.Text = "Background";
            this.columnBackground.Width = 97;
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
            this.tabPageDevices.Size = new System.Drawing.Size(1031, 568);
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
            this.treeViewDevices.Size = new System.Drawing.Size(713, 554);
            this.treeViewDevices.TabIndex = 0;
            // 
            // tabPageTests
            // 
            this.tabPageTests.Controls.Add(this.textBoxTests);
            this.tabPageTests.Location = new System.Drawing.Point(4, 22);
            this.tabPageTests.Name = "tabPageTests";
            this.tabPageTests.Size = new System.Drawing.Size(1031, 568);
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
            this.statusStrip.Location = new System.Drawing.Point(0, 624);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1063, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabelDevice
            // 
            this.toolStripStatusLabelDevice.Name = "toolStripStatusLabelDevice";
            this.toolStripStatusLabelDevice.Size = new System.Drawing.Size(61, 17);
            this.toolStripStatusLabelDevice.Text = "No Device";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1063, 24);
            this.menuStrip.TabIndex = 6;
            this.menuStrip.Text = "menuStrip1";
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.updateToolStripMenuItem.Text = "Update";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tabPageLogs
            // 
            this.tabPageLogs.Controls.Add(this.textBoxLogs);
            this.tabPageLogs.Location = new System.Drawing.Point(4, 22);
            this.tabPageLogs.Name = "tabPageLogs";
            this.tabPageLogs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLogs.Size = new System.Drawing.Size(1031, 568);
            this.tabPageLogs.TabIndex = 3;
            this.tabPageLogs.Text = "Logs";
            this.tabPageLogs.UseVisualStyleBackColor = true;
            // 
            // textBoxLogs
            // 
            this.textBoxLogs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxLogs.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLogs.Location = new System.Drawing.Point(6, 6);
            this.textBoxLogs.Multiline = true;
            this.textBoxLogs.Name = "textBoxLogs";
            this.textBoxLogs.ReadOnly = true;
            this.textBoxLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogs.Size = new System.Drawing.Size(893, 556);
            this.textBoxLogs.TabIndex = 1;
            this.textBoxLogs.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1063, 646);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(960, 640);
            this.Name = "MainForm";
            this.Text = "HID Demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageMessages.ResumeLayout(false);
            this.tabPageMessages.PerformLayout();
            this.groupBoxRegistrationFlag.ResumeLayout(false);
            this.groupBoxRegistrationFlag.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericRepeatSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRepeatDelay)).EndInit();
            this.tabPageDevices.ResumeLayout(false);
            this.tabPageTests.ResumeLayout(false);
            this.tabPageTests.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabPageLogs.ResumeLayout(false);
            this.tabPageLogs.PerformLayout();
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
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnBackground;
        private System.Windows.Forms.GroupBox groupBoxRegistrationFlag;
        private System.Windows.Forms.RadioButton radioButtonNone;
        private System.Windows.Forms.RadioButton radioButtonExInputSink;
        private System.Windows.Forms.RadioButton radioButtonInputSink;
        private System.Windows.Forms.TabPage tabPageLogs;
        private System.Windows.Forms.TextBox textBoxLogs;
    }
}
