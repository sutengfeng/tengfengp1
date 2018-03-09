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
    public partial class FrmModifyPwd : Form
    {
        public FrmModifyPwd()
        {
            InitializeComponent();
        }
        //修改密码
        private void btnModify_Click(object sender, EventArgs e)
        {
            # region 密码验证
            //验证
            if (this.txtOldPwd.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入原密码!","提示信息");
                this.txtOldPwd.Focus();
                return;
            }
            //判断用户输入的密码和登录保存的密码是否一致
            if (this.txtOldPwd.Text.Trim() != Program.currentAdmin.LoginPwd)
            {
                MessageBox.Show("请输入正确的原密码!", "提示信息");
                this.txtOldPwd.Focus();
                this.txtOldPwd.SelectAll();
                return;
            }
            //判断新密码的长度
            if (this.txtNewPwd.Text.Trim().Length < 6)
            {
                MessageBox.Show("新密码长度不能小于6为、位","提示信息");
                this.txtNewPwd.Focus();
                return;
            }
            if (this.txtNewPwdConfirm.Text.Trim().Length == 0)
            {
                MessageBox.Show("请再次输入新密码!", "提示信息");
                this.txtNewPwdConfirm.Focus();
                return;
            }
            if (this.txtNewPwdConfirm.Text.Trim() != this.txtNewPwd.Text.Trim())
            {
                MessageBox.Show("两次输入密码不一致!", "提示信息");
                return;
            }
            #endregion

            try
            {
                Admin objAdmin = new Admin()
                {
                    LoginId = Program.currentAdmin.LoginId,
                    LoginPwd = this.txtNewPwd.Text.Trim()
                };
                if (new AdminService().ModifyPwd(objAdmin) == 1)
                {
                    MessageBox.Show("密码修改成功，请妥善保管！", "提示信息");
                    //同时修改当前保存的用户密码
                    Program.currentAdmin.LoginPwd = this.txtNewPwd.Text.Trim();
                    this.Close();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
