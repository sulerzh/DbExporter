using System;
using System.Diagnostics;
using System.Windows.Forms;
using DbExporter.Helper;
using DbExporter.View;

namespace DbExporter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //检测应用程序是否已经运行
            //如果已经运行，则只允许一个实例
            Process instance = OnlyOneInstance.RunningInstance();
            if (instance != null)
            {
                OnlyOneInstance.HandleRunningInstance(instance);
                return;
            }

            //配置文件不存在或配置已失效，则创建
            if (!GlobalConfigVars.IsWellSetting())
            {
                SettingForm settingForm = new SettingForm();
                // 取消配置，退出应用程序
                if (settingForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            Application.Run(new MainForm());
        }
    }
}
