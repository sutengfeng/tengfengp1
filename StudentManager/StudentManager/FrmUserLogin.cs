using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Models;
using DAL;

namespace StudentManager
{
    public partial class FrmUserLogin : Form
    {
        private AdminService objAdminservice = new AdminService();
        public FrmUserLogin()
        {
            InitializeComponent();
        }


        //登录
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //[1]数据验证
            if (this.txtLoginId.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入登录账号！","提示信息");
                this.txtLoginId.Focus();
                return;
            }
            if (!Common.DataValidate.IsInteger(this.txtLoginId.Text.Trim()))
            {
                MessageBox.Show("登录账号必须为正整数！", "提示信息");
                this.txtLoginId.Focus();
                return;
            }
            if (this.txtLoginPwd.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入账号密码！", "提示信息");
                this.txtLoginPwd.Focus();
                return;
            }
            //登录账号和密码不能包含危险字符

            //[2]封装对象（包含要提交的数据）
            Admin objAdmin = new Admin()
            {
                LoginId = Convert.ToInt32(this.txtLoginId.Text.Trim()),
                LoginPwd=this.txtLoginPwd.Text.Trim()
            };
            try
            {
                new AdminService().AdminLogin(objAdmin);
                //[3]和后台交互
                Program.currentAdmin = new AdminService().AdminLogin(objAdmin);
                //[4]处理交互结果(需要保存数据、需要返回值)
                if (Program.currentAdmin != null)//登录成功
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                 }
                else
                {
                    MessageBox.Show("用户名或密码错误","提示信息");
                }
            }
            catch (Exception ex)
            {

                   MessageBox.Show(ex.Message, "数据访问操作异常");
            }
        }
        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        //改进用户体验
        private void txtLoginId_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtLoginId.Text.Trim().Length != 0 && e.KeyValue == 13)
            {
                this.txtLoginPwd.Focus();
                this.txtLoginPwd.SelectAll();
            }
        }

        private void txtLoginPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtLoginPwd.Text.Trim().Length != 0 && e.KeyValue == 13)
            {
                btnLogin_Click(null, null);
            }

        }
    }
}
