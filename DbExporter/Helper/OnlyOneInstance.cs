using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DbExporter.Helper
{
    public class OnlyOneInstance
    {
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(System.IntPtr hWnd); 
        private const int WS_SHOWNORMAL = 1;

        //不允许有两个程序同时启动
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            //遍历正在有相同名字运行的进程 
            foreach (Process process in processes)
            {
                //忽略现有的进程 
                if (process.Id != current.Id)
                {
                    //确保进程从EXE文件运行 
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //返回另一个进程实例 
                        return process;
                    }
                }
            }
            //没有其它的进程,返回Null 
            return null;
        }

        public static void HandleRunningInstance(Process instance)
        {
            MessageBox.Show(null, "该应用系统已经运行！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL);

            //设置真实进程为foreground window 
            SetForegroundWindow(instance.MainWindowHandle);
        }
    }
}
