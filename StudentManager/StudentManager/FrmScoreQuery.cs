using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;


namespace StudentManager
{
    public partial class FrmScoreQuery : Form
    {
        private StudentClassService objClassService = new StudentClassService();
        private ScoreListService objScoreservice = new ScoreListService();
        private DataSet ds = null;//用来保存查询结果的数据集
        public FrmScoreQuery()
        {
            InitializeComponent();
            //基于datatable绑定班级下拉框
            DataTable dt = objClassService.GetAllClasses().Tables[0];
            this.cboClass.DataSource = dt;
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.DisplayMember = "ClassName";
            //显示全部考试成绩
            this.ds = objScoreservice.GetAllScoreList();//将所有成绩保存在成员变量中
            this.dgvScoreList.DataSource = ds.Tables[0];
        }     

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //根据班级名称动态筛选
        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ds == null) return;//目前不会出现
            this.ds.Tables[0].DefaultView.RowFilter = "ClassName='" + this.cboClass.Text.Trim()+"'";
        }
        //显示全部成绩
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            this.ds.Tables[0].DefaultView.RowFilter = "ClassName like '%%'";
        }
        //根据C#成绩动态筛选
        private void txtScore_TextChanged(object sender, EventArgs e)
        {
            if (this.txtScore.Text.Trim().Length == 0) return;
            if (Common.DataValidate.IsInteger(this.txtScore.Text.Trim()))
            {
                this.ds.Tables[0].DefaultView.RowFilter = "CSharp>" + this.txtScore.Text.Trim();
            }         
        }

        private void dgvScoreList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
           // Common.DataGridViewStyle.DgvRowPostPaint(this.dgvScoreList, e);
        }

        //打印当前的成绩信息
        private void btnPrint_Click(object sender, EventArgs e)
        {
          
        }
    }
}
