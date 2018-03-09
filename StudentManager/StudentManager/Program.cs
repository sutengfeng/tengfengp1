using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Models;

namespace StudentManager
{
    

    public static class Program
    {
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FrmMain());

            //显示登录窗体
            FrmUserLogin frmLogin = new FrmUserLogin();
            DialogResult objResult = frmLogin.ShowDialog();

            //判断登录是否成功
            if (objResult == DialogResult.OK)
            {
                Application.Run(new FrmMain());
            }
            else
            {
                Application.Exit();
            }

        }
        //声明用户的全局变量
        public static Admin currentAdmin = null;
    }
}
