using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DAL;
using Models;

namespace StudentManager
{
    public partial class FrmStudentManage : Form
    {
        private StudentClassService objClassService = new StudentClassService();
        private StudentService objStuService = new StudentService();
        private List<Student> stuList = new List<Student>();

        public FrmStudentManage()
        {
            InitializeComponent();
            //初始化班级下拉框
            this.cboClass.DataSource = objClassService.GetAllClass();
            this.cboClass.DisplayMember = "ClassName";//设置下拉框显示文本
            this.cboClass.ValueMember = "ClassId";//设置下拉框显示文本对应的value
            this.cboClass.SelectedIndex = -1;//默认不选中任何班级
            this.dgvStudentList.AutoGenerateColumns = false;//禁止自动生成列
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("请选择班级！", "查询信息");
                return;
            }
            //执行查询并绑定数据
            this.stuList= objStuService.GetStudentByClass(this.cboClass.Text.Trim());
            this.dgvStudentList.DataSource = this.stuList;
            new Common.DataGridViewStyle().DgvStyle3(this.dgvStudentList);
        }
        //根据学号查询
        private void btnQueryById_Click(object sender, EventArgs e)
        {
            //数据验证
            if (this.txtStudentId.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入查询学号！", "提示信息");
                this.txtStudentId.Focus();
                return;
            }
            if (!Common.DataValidate.IsInteger(this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("学号必须是整数！", "提示信息");
                this.txtStudentId.SelectAll();
                this.txtStudentId.Focus();
                return;
            }
            Student objStudent = objStuService.GetStudentById(this.txtStudentId.Text.Trim());
            if (objStudent == null)
            {
                MessageBox.Show("学员信息不存在！", "查询提示");
                this.txtStudentId.Focus();
            }
            else
            {
                //显示学员详细信息
                FrmStudentInfo objFrm = new FrmStudentInfo(objStudent);
                objFrm.Show();
            }

        }
        //用户输入学号好按回车键实现根据学号查询
        private void txtStudentId_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtStudentId.Text.Trim().Length != 0&&e.KeyValue==13)
            {
                btnQueryById_Click(null,null);
            }
        }
        //双击查看学员详情
        private void dgvStudentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgvStudentList.CurrentRow != null)
            {
                string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
                Student objStudent = objStuService.GetStudentById(studentId);
                FrmStudentInfo objFrm = new FrmStudentInfo(objStudent);
                objFrm.Show();
            }

        }
        //修改学员对象
        private void btnEidt_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount==0)
            {
                MessageBox.Show("没有任何要修改的信息", "提示信息");
                return;
            }
            if (this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("请选中要修改的学员的信息", "提示信息");
                return;
            }
            //获取学号
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            Student objStudent = objStuService.GetStudentById(studentId);
            //显示要修改的学员信息窗口
            FrmEditStudent objFrm = new FrmEditStudent(objStudent);
            if (objFrm.ShowDialog() == DialogResult.OK)
            {
                //同步刷新
                btnQuery_Click(null,null);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                MessageBox.Show("没有任何要删除的信息", "提示信息");
                return;
            }
            if (this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("请选中要删除的学员信息", "提示信息");
                return;
            }
            else
            {
                //删除前确认
                DialogResult result = MessageBox.Show("确认要删除吗？","删除确认",
                MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                if (result == DialogResult.Cancel) return;
                //获取学号
                string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();       
                try
                    {
                    if (objStuService.DeleteStudent(studentId) == 1)
                    {
                        btnQuery_Click(null,null);//同步刷新显示（实际开发中此方法用的较少）
                    }
                    }
                    catch (Exception ex)
                    {
                    MessageBox.Show(ex.Message,"提示信息");
                    }                            
            }

        }
        //姓名降序
        private void btnNameDESC_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0) return;
            this.stuList.Sort(new NameDESC());
            this.dgvStudentList.Refresh();
        }
        //学号降序
        private void btnStuIdDESC_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0) return;
            this.stuList.Sort(new StuIdDESC());
            this.dgvStudentList.Refresh();
        }

        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //导出Excel
        private void btnExport_Click(object sender, EventArgs e)
        {

        }
        //修改学员右键菜单
        private void tsmiModifyStu_Click(object sender, EventArgs e)
        {
            btnEidt_Click(null, null);
        }
    }
    #region 实现排序

    class NameDESC : IComparer<Student>
    {
        public int Compare(Student x,Student y)
        {
            return y.StudentName.CompareTo(x.StudentName);
        }
    }
    class StuIdDESC:IComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            return y.StudentId.CompareTo(x.StudentId);
        }
    }
    #endregion

}