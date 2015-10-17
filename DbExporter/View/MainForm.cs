using DbExporter.Common;
using System;
//using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DbExporter.View
{
    public partial class MainForm : Form
    {
        private IDbProvider DbProvider { get; set; }
        private IExporter DbExporter { get; set; }
        public MainForm()
        {
            InitializeComponent();
            this.datePicker1.MaxDate = DateTime.Now;
            ResetDbProvider();
        }

        private void SetBoldDates()
        {
            try
            {
                this.datePicker1.PickerBoldedDates = this.DbProvider.GetAllTestDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取数据失败！请检查配置是否正确。");
            }
        }

        private void SetUiText()
        {
            this.Text = string.Format("数据导出软件v1.0({0})", GlobalConfigVars.DbType);
            this.groupBox1.Text = ShowBase.GetDescription();
        }

        public void ResetDbProvider()
        {
            SetUiText();
            this.DbProvider = GlobalConfigVars.GetDbProvider();
            this.DbExporter = GlobalConfigVars.GetExporter();
            this.lbSelectedSampleId.Items.Clear();
            SetBoldDates();
            RefreshSampleIdList(datePicker1.Value);
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            SettingForm settingForm = new SettingForm();
            if (DialogResult.OK == settingForm.ShowDialog())
            {
                ResetDbProvider();
            }
        }

        private void datePicker1_ValueChanged(object sender, CustomControls.CheckDateEventArgs e)
        {
            lbSelectedSampleId.Items.Clear();
            RefreshSampleIdList(datePicker1.Value);
        }

        private void RefreshSampleIdList(DateTime filterDate)
        {
            try
            {
                this.cklbSampleId.DataSource = this.DbProvider.GetResultByFilterDate(filterDate);//.ToList();
                this.cklbSampleId.DisplayMember = "Label";
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取数据失败！请检查配置是否正确。");
            }
        }

        private void RefreshSelectedSampleIdList()
        {
            lbSelectedSampleId.Items.Clear();
            for (int i = 0; i < cklbSampleId.CheckedItems.Count; i++)
            {
                lbSelectedSampleId.Items.Add(this.cklbSampleId.CheckedItems[i]);
            }
            lbSelectedSampleId.DisplayMember = "Label";
            btnExport.Enabled = lbSelectedSampleId.Items.Count > 0;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (lbSelectedSampleId.Items.Count <= 0) return;
            List<ShowBase> ds = new List<ShowBase>();
            foreach (var item in lbSelectedSampleId.Items)
            {
                ds.Add((ShowBase)item);
            }
            this.DbExporter.Export(ds);//lbSelectedSampleId.Items.Cast<string>().ToList());
            MessageBox.Show( "导出完成！", "导出提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAll.Checked)
            {
                for (int j = 0; j < cklbSampleId.Items.Count; j++)
                    cklbSampleId.SetItemChecked(j, true);
            }
            else
            {
                for (int j = 0; j < cklbSampleId.Items.Count; j++)
                    cklbSampleId.SetItemChecked(j, false);
            }
            RefreshSelectedSampleIdList();
        }

        private void ckReverse_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < cklbSampleId.Items.Count; i++)
            {
                if (cklbSampleId.GetItemChecked(i))
                {
                    cklbSampleId.SetItemChecked(i, false);
                }
                else
                {
                    cklbSampleId.SetItemChecked(i, true);
                }
            }
            RefreshSelectedSampleIdList();
        }

        private void cklbSampleId_SelectedValueChanged(object sender, EventArgs e)
        {
            RefreshSelectedSampleIdList();
        }

    }
}
