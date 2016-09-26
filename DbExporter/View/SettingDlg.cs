using System;
using System.Windows.Forms;

namespace DbExporter.View
{
    public partial class SettingForm : Form
    {
        private SupportedDbType DbType
        {
            get
            {
                if (rbAggRAM.Checked)
                    return SupportedDbType.AggRAM;
                if (rbPlatinum.Checked)
                    return SupportedDbType.Platinum;
                if (rbQS2000.Checked)
                    return SupportedDbType.QS2000;
                return SupportedDbType.Spife4000;
            }
            set
            {
                switch (value)
                {
                    case SupportedDbType.AggRAM:
                        rbAggRAM.Checked = true;
                        break;
                    case SupportedDbType.Platinum:
                        rbPlatinum.Checked = true;
                        break;
                    case SupportedDbType.QS2000:
                        rbQS2000.Checked = true;
                        break;
                    default:
                        rbSpife4000.Checked = true;
                        break;
                }
            }
        }
        public SettingForm()
        {
            InitializeComponent();
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbDlg = new FolderBrowserDialog();
            if (fbDlg.ShowDialog() == DialogResult.OK)
            {
                string path = fbDlg.SelectedPath;
                tbDbFolder.Text = path;
            }
        }

        private void btnSaveBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbDlg = new FolderBrowserDialog();
            if (fbDlg.ShowDialog() == DialogResult.OK)
            {
                string path = fbDlg.SelectedPath;
                tbExportPath.Text = path;
            }
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            // 初始化数据库选项
            this.DbType = GlobalConfigVars.GetDbType(Properties.Settings.Default.DbType);
            tbDbFolder.Text = Properties.Settings.Default.DbPath;
            tbExportPath.Text = Properties.Settings.Default.XmlPath;
            numBC0.Value = Properties.Settings.Default.Blp0;
            numBC1.Value = Properties.Settings.Default.Blp1;
            numBC2.Value = Properties.Settings.Default.Blp2;
            numBC3.Value = Properties.Settings.Default.Blp3;
            numBC4.Value = Properties.Settings.Default.Blp4;
            numBC5.Value = Properties.Settings.Default.Blp5;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!GlobalConfigVars.IsWellSetting(this.DbType, tbDbFolder.Text, tbExportPath.Text))
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            int[] blps = {
                (int)numBC0.Value, (int)numBC1.Value, (int)numBC2.Value,
                (int)numBC3.Value, (int)numBC4.Value, (int)numBC5.Value };
            // 保存
            GlobalConfigVars.SaveSetting(this.DbType.ToString(), tbDbFolder.Text, tbExportPath.Text, blps);
        }

        private void SettingForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btnOK_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.D || e.KeyChar == 100)
            {
                this.groupBox2.Visible = !this.groupBox2.Visible;
            }
        }
    }
}
