using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Models;
using DAL;




namespace StudentManager
{
    public partial class FrmAddStudent : Form
    {
        //创建数据访问对象
        private StudentClassService objClassService = new StudentClassService();
        private StudentService objStudentService = new StudentService();
        List<Student> stuList = new List<Student>();//用来临时保存学员对象

        public FrmAddStudent()
        {
            InitializeComponent();
            //初始化班级下拉框
            this.cboClassName.DataSource = objClassService.GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";//设置下拉框显示文本
            this.cboClassName.ValueMember = "ClassId";//设置下拉框显示文本对应的value
            this.cboClassName.SelectedIndex = -1;//默认不选中任何班级
            this.dgvStudentList.AutoGenerateColumns = false;//禁止自动生成列
        }
        //添加新学员
        private void btnAdd_Click(object sender, EventArgs e)
        {
            #region 数据验证
            if (this.txtStudentName.Text.Trim().Length == 0)
            {
                MessageBox.Show("学生姓名不能为空！", "提示信息");
                this.txtStudentName.Focus();
                return;
            }
            if (this.txtCardNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("考勤卡号不能为空！", "提示信息");
                this.txtCardNo.Focus();
                return;
            }
            //验证性别
            if (!this.rdoFemale.Checked && !this.rdoMale.Checked)
            {
                MessageBox.Show("请选择性别！", "提示信息");
                return;
            }
            //验证班级
            if (this.cboClassName.SelectedIndex == -1)
            {
                MessageBox.Show("请选择班级！", "提示信息");
                return;
            }
            //验证年龄
            int age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year;
            if (age > 35||age < 18)
            {
                MessageBox.Show("年龄必须在18-35之间！", "提示信息");
                return;
            }
            //验证考勤卡号必须是数字
            if (!Common.DataValidate.IsInteger(this.txtCardNo.Text.Trim()))
            {
                MessageBox.Show("考勤卡号必须全为数字！", "提示信息");
                this.txtStudentIdNo.SelectAll();
                this.txtStudentIdNo.Focus();
                return;
            }
            //身份证格式验证
            if (!Common.DataValidate.IsIdentityCard(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("身份证号不符合要求！", "提示信息");
                this.txtStudentIdNo.SelectAll();
                this.txtStudentIdNo.Focus();
                return;
            }
            //验证身份证中的出生日期和用户选择的出生日期是否一致
            string birthday = Convert.ToDateTime(this.dtpBirthday.Text).ToString("yyyyMMdd");
            if (!this.txtStudentIdNo.Text.Trim().Contains(birthday))
            {
                MessageBox.Show("身份证号和出生日期不匹配！", "提示信息");
                this.txtStudentIdNo.SelectAll();
                this.txtStudentIdNo.Focus();
                return;
            }
            //从数据库中判断身份证是否存在，以及卡号是否存在
            if (objStudentService.IsIdNoExisted(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("当前身份证号已经被其他学员使用！", "提示信息");
                this.txtStudentIdNo.SelectAll();
                this.txtStudentIdNo.Focus();
                return;
            }
            if (objStudentService.IsCardNoExisted(this.txtCardNo.Text.Trim()))
            {
                MessageBox.Show("当前考勤卡号已经被其他学员使用！", "提示信息");
                this.txtCardNo.SelectAll();
                this.txtCardNo.Focus();
                return;
            }
            #endregion

            //封装学员对象
            Student objStudent=new Student()
            {
                StudentName=this.txtStudentName.Text.Trim(),
                Gender=this.rdoMale.Checked?"男":"女",
                Birthday=Convert .ToDateTime(this.dtpBirthday.Text),
                StudentIdNo=this.txtStudentIdNo.Text.Trim(),
                PhoneNumber=this.txtPhoneNumber.Text.Trim(),
                StudentAddress=this.txtStudentName.Text.Trim(),
                ClassId=Convert.ToInt32(this.cboClassName.SelectedValue),
                ClassName=this.cboClassName.Text,//为了列表展示需要
                Age=DateTime.Now.Year- Convert.ToDateTime(this.dtpBirthday.Text).Year,
                CardNo=this.txtCardNo.Text.Trim(),
                StuImage=this.pbStu.Image!=null?new Common.SerializeObjectToString().SerializeObject(this.pbStu.Image):""
            };
            //调用后台数据访问方法
            try
            {
                int StudentId = objStudentService.AddStudent(objStudent);
                if (StudentId > 1)
                {
                    //同步显示添加的学员
                    objStudent.StudentId = StudentId;
                    this.stuList.Add(objStudent);
                    this.dgvStudentList.DataSource = null;
                    this.dgvStudentList.DataSource = this.stuList;
                    //询问是否继续添加
                    DialogResult result = MessageBox.Show("新学员添加成功！是否继续添加？","提示信息",
                        MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        //清空用户输入的信息
                        foreach (Control item in this.gbstuinfo.Controls)
                        {
                            if (item is TextBox)
                            {
                                item.Text = "";
                            }
                            else if (item is RadioButton)
                            {
                                ((RadioButton)item).Checked = false;
                            }
                        }
                        this.cboClassName.SelectedIndex = -1;
                        //this.rdoFemale.Checked = false;
                        //this.rdoMale.Checked = false;
                        this.pbStu.Image = null;
                        this.txtStudentName.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加学员出现数据访问异常："+ex.Message);           
            }
       
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FrmAddStudent_KeyDown(object sender, KeyEventArgs e)
        {
       

        }

        //选择照片
        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog objFileDialog = new OpenFileDialog();
            DialogResult result = objFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.pbStu.Image = Image.FromFile(objFileDialog.FileName);
            }
        }
        //启动摄像头
       
        private void btnStartVideo_Click(object sender, EventArgs e)
        {
         
        }
        //拍照
        private void btnTake_Click(object sender, EventArgs e)
        {
        
        }
        //清除照片
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.pbStu.Image = null;
        }

        private void dgvStudentList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //添加行号
        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
            //try
            //{
            //    //添加行号
            //    SolidBrush v_SolidBrush = new SolidBrush(dgvStudentList.RowHeadersDefaultCellStyle.ForeColor);
            //    int v_LineNo = 0;
            //    v_LineNo = e.RowIndex + 1;
            //    string v_Line = v_LineNo.ToString();
            //    e.Graphics.DrawString(v_Line, e.InheritedRowStyle.Font, v_SolidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("添加行号时引发错误，错误信息：" + ex.Message, "操作失败");
            //}
        }
    }
}