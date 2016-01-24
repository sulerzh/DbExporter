namespace DbExporter.View
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.cklbSampleId = new System.Windows.Forms.CheckedListBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.ckAll = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckReverse = new System.Windows.Forms.CheckBox();
            this.lbSelectedSampleId = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSetting = new System.Windows.Forms.Button();
            this.datePicker1 = new CustomControls.DatePicker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cklbSampleId
            // 
            this.cklbSampleId.CheckOnClick = true;
            this.cklbSampleId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cklbSampleId.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cklbSampleId.FormattingEnabled = true;
            this.cklbSampleId.Location = new System.Drawing.Point(3, 19);
            this.cklbSampleId.Margin = new System.Windows.Forms.Padding(6);
            this.cklbSampleId.Name = "cklbSampleId";
            this.cklbSampleId.Size = new System.Drawing.Size(408, 340);
            this.cklbSampleId.TabIndex = 1;
            this.cklbSampleId.SelectedValueChanged += new System.EventHandler(this.cklbSampleId_SelectedValueChanged);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(548, 28);
            this.btnExport.Margin = new System.Windows.Forms.Padding(6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(147, 49);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ckAll
            // 
            this.ckAll.AutoSize = true;
            this.ckAll.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ckAll.Location = new System.Drawing.Point(255, 0);
            this.ckAll.Name = "ckAll";
            this.ckAll.Size = new System.Drawing.Size(75, 21);
            this.ckAll.TabIndex = 3;
            this.ckAll.Text = "全部选择";
            this.ckAll.UseVisualStyleBackColor = true;
            this.ckAll.CheckedChanged += new System.EventHandler(this.ckAll_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.cklbSampleId);
            this.groupBox1.Controls.Add(this.ckAll);
            this.groupBox1.Controls.Add(this.ckReverse);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.groupBox1.Location = new System.Drawing.Point(12, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 362);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "样本列表(胶片号 序号 ： 样本号)";
            // 
            // ckReverse
            // 
            this.ckReverse.AutoSize = true;
            this.ckReverse.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckReverse.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ckReverse.Location = new System.Drawing.Point(333, 0);
            this.ckReverse.Name = "ckReverse";
            this.ckReverse.Size = new System.Drawing.Size(75, 21);
            this.ckReverse.TabIndex = 3;
            this.ckReverse.Text = "反向选择";
            this.ckReverse.UseVisualStyleBackColor = true;
            this.ckReverse.CheckedChanged += new System.EventHandler(this.ckReverse_CheckedChanged);
            // 
            // lbSelectedSampleId
            // 
            this.lbSelectedSampleId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSelectedSampleId.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSelectedSampleId.FormattingEnabled = true;
            this.lbSelectedSampleId.ItemHeight = 21;
            this.lbSelectedSampleId.Location = new System.Drawing.Point(3, 19);
            this.lbSelectedSampleId.Name = "lbSelectedSampleId";
            this.lbSelectedSampleId.Size = new System.Drawing.Size(312, 340);
            this.lbSelectedSampleId.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.datePicker1);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.groupBox2.Location = new System.Drawing.Point(12, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(527, 67);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置实验时间";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lbSelectedSampleId);
            this.groupBox3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.groupBox3.Location = new System.Drawing.Point(432, 86);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(318, 362);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "已选样本编号";
            // 
            // btnSetting
            // 
            this.btnSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnSetting.Image")));
            this.btnSetting.Location = new System.Drawing.Point(704, 28);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(46, 49);
            this.btnSetting.TabIndex = 10;
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // datePicker1
            // 
            this.datePicker1.InvalidForeColor = System.Drawing.SystemColors.ControlText;
            this.datePicker1.Location = new System.Drawing.Point(6, 26);
            this.datePicker1.Name = "datePicker1";
            this.datePicker1.PickerDayFont = new System.Drawing.Font("宋体", 14F);
            this.datePicker1.PickerDayHeaderFont = new System.Drawing.Font("Segoe UI", 12F);
            this.datePicker1.PickerFooterFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.datePicker1.PickerHeaderFont = new System.Drawing.Font("Segoe UI", 12F);
            this.datePicker1.ShowPickerWeekHeader = false;
            this.datePicker1.Size = new System.Drawing.Size(515, 35);
            this.datePicker1.TabIndex = 1;
            this.datePicker1.ValueChanged += new System.EventHandler<CustomControls.CheckDateEventArgs>(this.datePicker1_ValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 487);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(612, 523);
            this.Name = "MainForm";
            this.Text = "Spife 4000数据导出软件";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckedListBox cklbSampleId;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckBox ckAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ckReverse;
        private System.Windows.Forms.ListBox lbSelectedSampleId;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSetting;
        private CustomControls.DatePicker datePicker1;
    }
}

