using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicTopView
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //GlobalMutex();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        private static Mutex mutex = null;
        private static void GlobalMutex()
        {
            //  是否第一次创建mutex 
            bool newMutexCreated = false;
            string mutexName = " Global\\ " + " PicTopView ";//系统名称，Global为全局，表示即使通过通过虚拟桌面连接过来，也只是允许运行一次
            try
            {
                mutex = new Mutex(false, mutexName, out  newMutexCreated);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                System.Threading.Thread.Sleep(1000);
                Environment.Exit(1);
            }

            //  第一次创建mutex 
            if (newMutexCreated)
            {
                Console.WriteLine(" 程序已启动 ");
                // todo:此处为要执行的任务 
            }
            else
            {
                
                System.Threading.Thread.Sleep(1000);
                Environment.Exit(1); // 退出程序 
            }
        } 
    }
}
