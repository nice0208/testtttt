using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Detecting_System
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createNew;
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, "ThisApp", out createNew);
            if (!createNew)
            {
                MessageBox.Show("程序已打开", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
                return;
            }
        #if DEBUG
            /* This is a special debug setting needed only for GigE cameras.
                            See 'Building Applications with pylon' in the Programmer's Guide. */
            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "1000" /*ms*/);
        #else
            
            /* This is a special debug setting needed only for GigE cameras.
                            See 'Building Applications with pylon' in the Programmer's Guide. */
            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "1000" /*ms*/);
        #endif
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmParent());
                //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                ////处理非UI线程异常
                //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            }
            catch (Exception es)
            {
                //MessageBox.Show(es.Message);
                //throw;
            }
        }
    }
}
