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
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!GlobalConfigVars.IsWellSetting(this.DbType, tbDbFolder.Text, tbExportPath.Text))
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            // 保存
            GlobalConfigVars.SaveSetting(this.DbType.ToString(), tbDbFolder.Text, tbExportPath.Text);
        }
    }
}
