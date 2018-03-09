using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Models;
using DAL;


namespace StudentManager
{
    public partial class FrmEditStudent : Form
    {
        private StudentService objStudentService = new StudentService();

        public FrmEditStudent()
        {
            InitializeComponent();
        }

        public FrmEditStudent(Student objStudent):this()
        {
            //初始化班级下拉框
            this.cboClassName.DataSource = new  StudentClassService().GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";//设置下拉框显示文本
            this.cboClassName.ValueMember = "ClassId";//设置下拉框显示文本对应的value
            //显示学员详细信息
            this.txtStudentName.Text = objStudent.StudentName;
            this.txtStudentIdNo.Text = objStudent.StudentIdNo;
            this.txtPhoneNumber.Text = objStudent.PhoneNumber;
            this.dtpBirthday.Text = objStudent.Birthday.ToShortDateString();
            this.txtAddress.Text = objStudent.StudentAddress;
            this.txtStudentId.Text = objStudent.StudentId.ToString();
            this.cboClassName.Text = objStudent.ClassName;
            if (objStudent.Gender == "男") this.rdoMale.Checked = true;
            else this.rdoFemale.Checked = true;      
            this.txtCardNo.Text = objStudent.CardNo;
            //显示照片
            this.pbStu.Image = objStudent.StuImage.Length != 0 ?
                (Image)new Common.SerializeObjectToString().DeserializeObject
                (objStudent.StuImage) : Image.FromFile("default.png");

        }

        //提交修改
        private void btnModify_Click(object sender, EventArgs e)
        {
            //数据验证（和添加几乎一样，考勤卡号和身份证号需要特殊判断）
            //和添加学员对象一样的验证，

            //验证身份证号
            if (objStudentService.IsIdNoExisted(this.txtStudentIdNo.Text.Trim(), this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("身份证号不能和其他现有学员身份证相同！请修改！", "修改提示");
                this.txtStudentIdNo.SelectAll();
                this.txtStudentIdNo.Focus();
                return;
            }
            //卡号验证
            if (objStudentService.IsIdNoExisted(this.txtCardNo.Text.Trim(), this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("身份证号不能和其他现有学员身份证相同！请修改！", "修改提示");
                this.txtCardNo.SelectAll();
                this.txtCardNo.Focus();
                return;
            }
            //封装对象
            Student objStudent = new Student()
            {
                StudentName = this.txtStudentName.Text.Trim(),
                Gender = this.rdoMale.Checked ? "男" : "女",
                Birthday = Convert.ToDateTime(this.dtpBirthday.Text),
                StudentIdNo = this.txtStudentIdNo.Text.Trim(),
                PhoneNumber = this.txtPhoneNumber.Text.Trim(),
                StudentAddress = this.txtStudentName.Text.Trim(),
                ClassId = Convert.ToInt32(this.cboClassName.SelectedValue),
                ClassName = this.cboClassName.Text,//为了列表展示需要
                Age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year,
                CardNo = this.txtCardNo.Text.Trim(),
                StuImage = this.pbStu.Image != null ? new Common.SerializeObjectToString().SerializeObject(this.pbStu.Image) : "",
                StudentId=Convert.ToInt32(this.txtStudentId.Text.Trim())
            };
            try
            {
                if (objStudentService.ModifyStudent(objStudent) == 1)
                {
                    MessageBox.Show("学员信息修改成功！", "提示信息");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示信息");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       //选择照片
        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog objFileDialog = new OpenFileDialog();
            DialogResult result = objFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                this.pbStu.Image = Image.FromFile(objFileDialog.FileName);
        }
    }
}