﻿namespace excel2json.GUI {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnImportExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCopyJSON = new System.Windows.Forms.ToolStripButton();
            this.btnSaveJson = new System.Windows.Forms.ToolStripButton();
            this.btnCopyCSharp = new System.Windows.Forms.ToolStripButton();
            this.btnSaveCSharp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelExcelDropBox = new System.Windows.Forms.Panel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBoxExcel = new System.Windows.Forms.PictureBox();
            this.labelExcelFile = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxExculdePrefix = new System.Windows.Forms.TextBox();
            this.comboBoxSheetName = new System.Windows.Forms.ComboBox();
            this.comboBoxDateFormat = new System.Windows.Forms.ComboBox();
            this.btnReimport = new System.Windows.Forms.Button();
            this.comboBoxLowcase = new System.Windows.Forms.ComboBox();
            this.comboBoxHeader = new System.Windows.Forms.ComboBox();
            this.comboBoxEncoding = new System.Windows.Forms.ComboBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.tabControlCode = new System.Windows.Forms.TabControl();
            this.tabPageJSON = new System.Windows.Forms.TabPage();
            this.tabCSharp = new System.Windows.Forms.TabPage();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.checkBoxCellJson = new System.Windows.Forms.CheckBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelExcelDropBox.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExcel)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControlCode.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(6, 49);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(102, 12);
            label2.TabIndex = 1;
            label2.Text = "Encoding:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(31, 23);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(77, 12);
            label1.TabIndex = 1;
            label1.Text = "Export Type:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(6, 75);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(102, 12);
            label4.TabIndex = 6;
            label4.Text = "Lowcase:";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(6, 101);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(102, 12);
            label3.TabIndex = 4;
            label3.Text = "Header:";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(6, 127);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(102, 12);
            label5.TabIndex = 9;
            label5.Text = "Date Format:";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(6, 153);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(102, 12);
            label6.TabIndex = 11;
            label6.Text = "SheetName:";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.Location = new System.Drawing.Point(6, 179);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(102, 12);
            label7.TabIndex = 13;
            label7.Text = "ExculdePrefix:";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 540);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "Ready";
            // 
            // statusLabel
            // 
            this.statusLabel.IsLink = true;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusLabel.Size = new System.Drawing.Size(130, 17);
            this.statusLabel.Text = "https://neil3d.github.io";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.statusLabel.Click += new System.EventHandler(this.statusLabel_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnImportExcel,
            this.toolStripSeparator1,
            this.btnCopyJSON,
            this.btnSaveJson,
            this.btnCopyCSharp,
            this.btnSaveCSharp,
            this.toolStripSeparator2,
            this.btnHelp});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(784, 46);
            this.toolStrip.TabIndex = 4;
            this.toolStrip.Text = "Import excel file and export as JSON";
            // 
            // btnImportExcel
            // 
            this.btnImportExcel.Image = global::excel2json.Properties.Resources.excel;
            this.btnImportExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImportExcel.Name = "btnImportExcel";
            this.btnImportExcel.Size = new System.Drawing.Size(77, 43);
            this.btnImportExcel.Text = "Import Excel";
            this.btnImportExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnImportExcel.ToolTipText = "Import Excel .xlsx file";
            this.btnImportExcel.Click += new System.EventHandler(this.btnImportExcel_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 46);
            // 
            // btnCopyJSON
            // 
            this.btnCopyJSON.Image = global::excel2json.Properties.Resources.clipboard;
            this.btnCopyJSON.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyJSON.Name = "btnCopyJSON";
            this.btnCopyJSON.Size = new System.Drawing.Size(70, 43);
            this.btnCopyJSON.Text = "Copy JSON";
            this.btnCopyJSON.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCopyJSON.ToolTipText = "Copy JSON string to clipboard";
            this.btnCopyJSON.Click += new System.EventHandler(this.btnCopyJSON_Click);
            // 
            // btnSaveJson
            // 
            this.btnSaveJson.Image = global::excel2json.Properties.Resources.json;
            this.btnSaveJson.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveJson.Name = "btnSaveJson";
            this.btnSaveJson.Size = new System.Drawing.Size(66, 43);
            this.btnSaveJson.Text = "Save JSON";
            this.btnSaveJson.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSaveJson.ToolTipText = "Save JSON file";
            this.btnSaveJson.Click += new System.EventHandler(this.btnSaveJson_Click);
            // 
            // btnCopyCSharp
            // 
            this.btnCopyCSharp.Image = global::excel2json.Properties.Resources.clipboard;
            this.btnCopyCSharp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyCSharp.Name = "btnCopyCSharp";
            this.btnCopyCSharp.Size = new System.Drawing.Size(57, 43);
            this.btnCopyCSharp.Text = "Copy C#";
            this.btnCopyCSharp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCopyCSharp.ToolTipText = "Save JSON file";
            this.btnCopyCSharp.Click += new System.EventHandler(this.btnCopyCSharp_Click);
            // 
            // btnSaveCSharp
            // 
            this.btnSaveCSharp.Image = global::excel2json.Properties.Resources.code;
            this.btnSaveCSharp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveCSharp.Name = "btnSaveCSharp";
            this.btnSaveCSharp.Size = new System.Drawing.Size(53, 43);
            this.btnSaveCSharp.Text = "Save C#";
            this.btnSaveCSharp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSaveCSharp.ToolTipText = "Save JSON file";
            this.btnSaveCSharp.Click += new System.EventHandler(this.btnSaveCSharp_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 46);
            // 
            // btnHelp
            // 
            this.btnHelp.Image = global::excel2json.Properties.Resources.about;
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(36, 43);
            this.btnHelp.Text = "Help";
            this.btnHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnHelp.ToolTipText = "Help Document on web";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 46);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlCode);
            this.splitContainer1.Size = new System.Drawing.Size(784, 494);
            this.splitContainer1.SplitterDistance = 288;
            this.splitContainer1.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panelExcelDropBox);
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(286, 492);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // panelExcelDropBox
            // 
            this.panelExcelDropBox.AllowDrop = true;
            this.panelExcelDropBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelExcelDropBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelExcelDropBox.Controls.Add(this.flowLayoutPanel2);
            this.panelExcelDropBox.Location = new System.Drawing.Point(8, 8);
            this.panelExcelDropBox.Margin = new System.Windows.Forms.Padding(8);
            this.panelExcelDropBox.Name = "panelExcelDropBox";
            this.panelExcelDropBox.Size = new System.Drawing.Size(270, 176);
            this.panelExcelDropBox.TabIndex = 1;
            this.panelExcelDropBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelExcelDropBox_DragDrop);
            this.panelExcelDropBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelExcelDropBox_DragEnter);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.pictureBoxExcel);
            this.flowLayoutPanel2.Controls.Add(this.labelExcelFile);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(268, 174);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // pictureBoxExcel
            // 
            this.pictureBoxExcel.Image = global::excel2json.Properties.Resources.excel_64;
            this.pictureBoxExcel.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxExcel.Name = "pictureBoxExcel";
            this.pictureBoxExcel.Size = new System.Drawing.Size(262, 130);
            this.pictureBoxExcel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxExcel.TabIndex = 0;
            this.pictureBoxExcel.TabStop = false;
            // 
            // labelExcelFile
            // 
            this.labelExcelFile.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelExcelFile.Location = new System.Drawing.Point(3, 136);
            this.labelExcelFile.Name = "labelExcelFile";
            this.labelExcelFile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelExcelFile.Size = new System.Drawing.Size(260, 35);
            this.labelExcelFile.TabIndex = 1;
            this.labelExcelFile.Text = "Drop you .xlsx file here!";
            this.labelExcelFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxCellJson);
            this.groupBox1.Controls.Add(this.textBoxExculdePrefix);
            this.groupBox1.Controls.Add(label7);
            this.groupBox1.Controls.Add(label6);
            this.groupBox1.Controls.Add(this.comboBoxSheetName);
            this.groupBox1.Controls.Add(label5);
            this.groupBox1.Controls.Add(this.comboBoxDateFormat);
            this.groupBox1.Controls.Add(this.btnReimport);
            this.groupBox1.Controls.Add(label4);
            this.groupBox1.Controls.Add(this.comboBoxLowcase);
            this.groupBox1.Controls.Add(label3);
            this.groupBox1.Controls.Add(this.comboBoxHeader);
            this.groupBox1.Controls.Add(label2);
            this.groupBox1.Controls.Add(label1);
            this.groupBox1.Controls.Add(this.comboBoxEncoding);
            this.groupBox1.Controls.Add(this.comboBoxType);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(8, 200);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 280);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // textBoxExculdePrefix
            // 
            this.textBoxExculdePrefix.Location = new System.Drawing.Point(114, 180);
            this.textBoxExculdePrefix.Name = "textBoxExculdePrefix";
            this.textBoxExculdePrefix.Size = new System.Drawing.Size(150, 21);
            this.textBoxExculdePrefix.TabIndex = 14;
            // 
            // comboBoxSheetName
            // 
            this.comboBoxSheetName.DisplayMember = "0";
            this.comboBoxSheetName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSheetName.FormattingEnabled = true;
            this.comboBoxSheetName.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.comboBoxSheetName.Location = new System.Drawing.Point(114, 153);
            this.comboBoxSheetName.Name = "comboBoxSheetName";
            this.comboBoxSheetName.Size = new System.Drawing.Size(150, 20);
            this.comboBoxSheetName.TabIndex = 10;
            this.comboBoxSheetName.ValueMember = "0";
            // 
            // comboBoxDateFormat
            // 
            this.comboBoxDateFormat.DisplayMember = "0";
            this.comboBoxDateFormat.FormattingEnabled = true;
            this.comboBoxDateFormat.Items.AddRange(new object[] {
            "yyyy/MM/dd",
            "yyyy/MM/dd hh:mm:ss"});
            this.comboBoxDateFormat.Location = new System.Drawing.Point(114, 127);
            this.comboBoxDateFormat.Name = "comboBoxDateFormat";
            this.comboBoxDateFormat.Size = new System.Drawing.Size(150, 20);
            this.comboBoxDateFormat.TabIndex = 8;
            this.comboBoxDateFormat.ValueMember = "0";
            // 
            // btnReimport
            // 
            this.btnReimport.Location = new System.Drawing.Point(85, 238);
            this.btnReimport.Name = "btnReimport";
            this.btnReimport.Size = new System.Drawing.Size(75, 23);
            this.btnReimport.TabIndex = 7;
            this.btnReimport.Text = "Reimport";
            this.btnReimport.UseVisualStyleBackColor = true;
            this.btnReimport.Click += new System.EventHandler(this.btnReimport_Click);
            // 
            // comboBoxLowcase
            // 
            this.comboBoxLowcase.DisplayMember = "0";
            this.comboBoxLowcase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLowcase.FormattingEnabled = true;
            this.comboBoxLowcase.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.comboBoxLowcase.Location = new System.Drawing.Point(114, 75);
            this.comboBoxLowcase.Name = "comboBoxLowcase";
            this.comboBoxLowcase.Size = new System.Drawing.Size(150, 20);
            this.comboBoxLowcase.TabIndex = 5;
            this.comboBoxLowcase.ValueMember = "0";
            // 
            // comboBoxHeader
            // 
            this.comboBoxHeader.DisplayMember = "0";
            this.comboBoxHeader.FormattingEnabled = true;
            this.comboBoxHeader.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.comboBoxHeader.Location = new System.Drawing.Point(114, 101);
            this.comboBoxHeader.Name = "comboBoxHeader";
            this.comboBoxHeader.Size = new System.Drawing.Size(150, 20);
            this.comboBoxHeader.TabIndex = 3;
            this.comboBoxHeader.ValueMember = "0";
            // 
            // comboBoxEncoding
            // 
            this.comboBoxEncoding.DisplayMember = "0";
            this.comboBoxEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEncoding.FormattingEnabled = true;
            this.comboBoxEncoding.Location = new System.Drawing.Point(114, 49);
            this.comboBoxEncoding.Name = "comboBoxEncoding";
            this.comboBoxEncoding.Size = new System.Drawing.Size(150, 20);
            this.comboBoxEncoding.TabIndex = 0;
            this.comboBoxEncoding.ValueMember = "0";
            // 
            // comboBoxType
            // 
            this.comboBoxType.DisplayMember = "0";
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "Array",
            "Dict Object"});
            this.comboBoxType.Location = new System.Drawing.Point(114, 23);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(150, 20);
            this.comboBoxType.TabIndex = 0;
            this.comboBoxType.ValueMember = "0";
            // 
            // tabControlCode
            // 
            this.tabControlCode.Controls.Add(this.tabPageJSON);
            this.tabControlCode.Controls.Add(this.tabCSharp);
            this.tabControlCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCode.Location = new System.Drawing.Point(0, 0);
            this.tabControlCode.Name = "tabControlCode";
            this.tabControlCode.SelectedIndex = 0;
            this.tabControlCode.Size = new System.Drawing.Size(490, 492);
            this.tabControlCode.TabIndex = 0;
            // 
            // tabPageJSON
            // 
            this.tabPageJSON.Location = new System.Drawing.Point(4, 22);
            this.tabPageJSON.Name = "tabPageJSON";
            this.tabPageJSON.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageJSON.Size = new System.Drawing.Size(482, 466);
            this.tabPageJSON.TabIndex = 0;
            this.tabPageJSON.Text = "JSON";
            this.tabPageJSON.UseVisualStyleBackColor = true;
            // 
            // tabCSharp
            // 
            this.tabCSharp.Location = new System.Drawing.Point(4, 22);
            this.tabCSharp.Name = "tabCSharp";
            this.tabCSharp.Padding = new System.Windows.Forms.Padding(3);
            this.tabCSharp.Size = new System.Drawing.Size(482, 466);
            this.tabCSharp.TabIndex = 1;
            this.tabCSharp.Text = "C#";
            this.tabCSharp.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // checkBoxCellJson
            // 
            this.checkBoxCellJson.AutoSize = true;
            this.checkBoxCellJson.Location = new System.Drawing.Point(54, 207);
            this.checkBoxCellJson.Name = "checkBoxCellJson";
            this.checkBoxCellJson.Size = new System.Drawing.Size(186, 16);
            this.checkBoxCellJson.TabIndex = 15;
            this.checkBoxCellJson.Text = "Convert Json String in Cell";
            this.checkBoxCellJson.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "excel2json";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panelExcelDropBox.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExcel)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControlCode.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnImportExcel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnHelp;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton btnCopyJSON;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panelExcelDropBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.PictureBox pictureBoxExcel;
        private System.Windows.Forms.Label labelExcelFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxEncoding;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.ComboBox comboBoxLowcase;
        private System.Windows.Forms.ComboBox comboBoxHeader;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ToolStripButton btnSaveJson;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button btnReimport;
        private System.Windows.Forms.TabControl tabControlCode;
        private System.Windows.Forms.TabPage tabPageJSON;
        private System.Windows.Forms.ComboBox comboBoxDateFormat;
        private System.Windows.Forms.ComboBox comboBoxSheetName;
        private System.Windows.Forms.TabPage tabCSharp;
        private System.Windows.Forms.ToolStripButton btnCopyCSharp;
        private System.Windows.Forms.ToolStripButton btnSaveCSharp;
        private System.Windows.Forms.TextBox textBoxExculdePrefix;
        private System.Windows.Forms.CheckBox checkBoxCellJson;
    }
}