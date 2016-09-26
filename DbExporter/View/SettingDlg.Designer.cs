namespace DbExporter.View
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.tbDbFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSaveBrowser = new System.Windows.Forms.Button();
            this.tbExportPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbDbType = new System.Windows.Forms.GroupBox();
            this.dbTypePanel = new System.Windows.Forms.TableLayoutPanel();
            this.rbQS2000 = new System.Windows.Forms.RadioButton();
            this.rbPlatinum = new System.Windows.Forms.RadioButton();
            this.rbSpife4000 = new System.Windows.Forms.RadioButton();
            this.rbAggRAM = new System.Windows.Forms.RadioButton();
            this.numBC0 = new System.Windows.Forms.NumericUpDown();
            this.numBC3 = new System.Windows.Forms.NumericUpDown();
            this.numBC2 = new System.Windows.Forms.NumericUpDown();
            this.numBC1 = new System.Windows.Forms.NumericUpDown();
            this.numBC4 = new System.Windows.Forms.NumericUpDown();
            this.numBC5 = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.gbDbType.SuspendLayout();
            this.dbTypePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBC0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBC3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBC2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBC1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBC4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBC5)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDbFolder
            // 
            this.tbDbFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDbFolder.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbDbFolder.Location = new System.Drawing.Point(17, 43);
            this.tbDbFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbDbFolder.Name = "tbDbFolder";
            this.tbDbFolder.Size = new System.Drawing.Size(497, 29);
            this.tbDbFolder.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "数据存储目录";
            // 
            // btnSaveBrowser
            // 
            this.btnSaveBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveBrowser.Location = new System.Drawing.Point(515, 118);
            this.btnSaveBrowser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSaveBrowser.Name = "btnSaveBrowser";
            this.btnSaveBrowser.Size = new System.Drawing.Size(100, 29);
            this.btnSaveBrowser.TabIndex = 0;
            this.btnSaveBrowser.Text = "浏览...";
            this.btnSaveBrowser.UseVisualStyleBackColor = true;
            this.btnSaveBrowser.Click += new System.EventHandler(this.btnSaveBrowser_Click);
            // 
            // tbExportPath
            // 
            this.tbExportPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbExportPath.Location = new System.Drawing.Point(17, 119);
            this.tbExportPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbExportPath.Name = "tbExportPath";
            this.tbExportPath.Size = new System.Drawing.Size(497, 26);
            this.tbExportPath.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 91);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "数据导出目录";
            // 
            // btnBrowser
            // 
            this.btnBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowser.Location = new System.Drawing.Point(515, 43);
            this.btnBrowser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(100, 29);
            this.btnBrowser.TabIndex = 3;
            this.btnBrowser.Text = "浏览...";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(427, 363);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 38);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnOK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btnOK_KeyPress);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(536, 363);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 38);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBrowser);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbExportPath);
            this.groupBox1.Controls.Add(this.tbDbFolder);
            this.groupBox1.Controls.Add(this.btnSaveBrowser);
            this.groupBox1.Location = new System.Drawing.Point(13, 124);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(623, 166);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // gbDbType
            // 
            this.gbDbType.Controls.Add(this.dbTypePanel);
            this.gbDbType.Location = new System.Drawing.Point(12, 12);
            this.gbDbType.Name = "gbDbType";
            this.gbDbType.Size = new System.Drawing.Size(623, 104);
            this.gbDbType.TabIndex = 7;
            this.gbDbType.TabStop = false;
            this.gbDbType.Text = "设置数据类型";
            // 
            // dbTypePanel
            // 
            this.dbTypePanel.ColumnCount = 2;
            this.dbTypePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.dbTypePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.dbTypePanel.Controls.Add(this.rbQS2000, 1, 1);
            this.dbTypePanel.Controls.Add(this.rbPlatinum, 0, 1);
            this.dbTypePanel.Controls.Add(this.rbSpife4000, 1, 0);
            this.dbTypePanel.Controls.Add(this.rbAggRAM, 0, 0);
            this.dbTypePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbTypePanel.Location = new System.Drawing.Point(3, 22);
            this.dbTypePanel.Name = "dbTypePanel";
            this.dbTypePanel.RowCount = 2;
            this.dbTypePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.dbTypePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.dbTypePanel.Size = new System.Drawing.Size(617, 79);
            this.dbTypePanel.TabIndex = 1;
            // 
            // rbQS2000
            // 
            this.rbQS2000.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rbQS2000.AutoSize = true;
            this.rbQS2000.Location = new System.Drawing.Point(218, 47);
            this.rbQS2000.Name = "rbQS2000";
            this.rbQS2000.Size = new System.Drawing.Size(396, 24);
            this.rbQS2000.TabIndex = 0;
            this.rbQS2000.TabStop = true;
            this.rbQS2000.Text = "QS2000（Quick Scan 2000）";
            this.rbQS2000.UseVisualStyleBackColor = true;
            // 
            // rbPlatinum
            // 
            this.rbPlatinum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rbPlatinum.AutoSize = true;
            this.rbPlatinum.Location = new System.Drawing.Point(3, 47);
            this.rbPlatinum.Name = "rbPlatinum";
            this.rbPlatinum.Size = new System.Drawing.Size(209, 24);
            this.rbPlatinum.TabIndex = 0;
            this.rbPlatinum.TabStop = true;
            this.rbPlatinum.Text = "Platinum";
            this.rbPlatinum.UseVisualStyleBackColor = true;
            // 
            // rbSpife4000
            // 
            this.rbSpife4000.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rbSpife4000.AutoSize = true;
            this.rbSpife4000.Location = new System.Drawing.Point(218, 7);
            this.rbSpife4000.Name = "rbSpife4000";
            this.rbSpife4000.Size = new System.Drawing.Size(396, 24);
            this.rbSpife4000.TabIndex = 0;
            this.rbSpife4000.TabStop = true;
            this.rbSpife4000.Text = "Spife4000";
            this.rbSpife4000.UseVisualStyleBackColor = true;
            // 
            // rbAggRAM
            // 
            this.rbAggRAM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rbAggRAM.AutoSize = true;
            this.rbAggRAM.Location = new System.Drawing.Point(3, 7);
            this.rbAggRAM.Name = "rbAggRAM";
            this.rbAggRAM.Size = new System.Drawing.Size(209, 24);
            this.rbAggRAM.TabIndex = 0;
            this.rbAggRAM.TabStop = true;
            this.rbAggRAM.Text = "AggRAM";
            this.rbAggRAM.UseVisualStyleBackColor = true;
            // 
            // numBC0
            // 
            this.numBC0.Enabled = false;
            this.numBC0.Location = new System.Drawing.Point(18, 25);
            this.numBC0.Name = "numBC0";
            this.numBC0.Size = new System.Drawing.Size(56, 26);
            this.numBC0.TabIndex = 8;
            // 
            // numBC3
            // 
            this.numBC3.Location = new System.Drawing.Point(204, 25);
            this.numBC3.Name = "numBC3";
            this.numBC3.Size = new System.Drawing.Size(56, 26);
            this.numBC3.TabIndex = 10;
            // 
            // numBC2
            // 
            this.numBC2.Location = new System.Drawing.Point(142, 25);
            this.numBC2.Name = "numBC2";
            this.numBC2.Size = new System.Drawing.Size(56, 26);
            this.numBC2.TabIndex = 11;
            // 
            // numBC1
            // 
            this.numBC1.Location = new System.Drawing.Point(80, 25);
            this.numBC1.Name = "numBC1";
            this.numBC1.Size = new System.Drawing.Size(56, 26);
            this.numBC1.TabIndex = 12;
            // 
            // numBC4
            // 
            this.numBC4.Location = new System.Drawing.Point(266, 25);
            this.numBC4.Name = "numBC4";
            this.numBC4.Size = new System.Drawing.Size(56, 26);
            this.numBC4.TabIndex = 13;
            // 
            // numBC5
            // 
            this.numBC5.Enabled = false;
            this.numBC5.Location = new System.Drawing.Point(328, 25);
            this.numBC5.Name = "numBC5";
            this.numBC5.Size = new System.Drawing.Size(56, 26);
            this.numBC5.TabIndex = 14;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numBC4);
            this.groupBox2.Controls.Add(this.numBC5);
            this.groupBox2.Controls.Add(this.numBC1);
            this.groupBox2.Controls.Add(this.numBC0);
            this.groupBox2.Controls.Add(this.numBC2);
            this.groupBox2.Controls.Add(this.numBC3);
            this.groupBox2.Location = new System.Drawing.Point(12, 298);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(623, 57);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "基线纠正";
            this.groupBox2.Visible = false;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 415);
            this.Controls.Add(this.gbDbType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "SettingForm";
            this.Text = "系统设置";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SettingForm_KeyPress);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbDbType.ResumeLayout(false);
            this.dbTypePanel.ResumeLayout(false);
            this.dbTypePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBC0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBC3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBC2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBC1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBC4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBC5)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDbFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSaveBrowser;
        private System.Windows.Forms.TextBox tbExportPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbDbType;
        private System.Windows.Forms.RadioButton rbAggRAM;
        private System.Windows.Forms.RadioButton rbPlatinum;
        private System.Windows.Forms.RadioButton rbQS2000;
        private System.Windows.Forms.RadioButton rbSpife4000;
        private System.Windows.Forms.TableLayoutPanel dbTypePanel;
        private System.Windows.Forms.NumericUpDown numBC0;
        private System.Windows.Forms.NumericUpDown numBC3;
        private System.Windows.Forms.NumericUpDown numBC2;
        private System.Windows.Forms.NumericUpDown numBC1;
        private System.Windows.Forms.NumericUpDown numBC4;
        private System.Windows.Forms.NumericUpDown numBC5;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}