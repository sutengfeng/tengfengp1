using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DAL;
using Models;


namespace StudentManager
{
    public partial class FrmScoreManage : Form
    {
        private ScoreListService objScoreService = new ScoreListService();
        public FrmScoreManage()
        {
            InitializeComponent();
            //绑定班级下拉框
            this.cboClass.DataSource = new StudentClassService().GetAllClass();
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.SelectedIndex = -1;
            //将下拉框的事件关联
            this.cboClass.SelectedIndexChanged += new System.EventHandler(this.cboClass_SelectedIndexChanged);
            this.dgvScoreList.AutoGenerateColumns = false;
        }     
     //根据班级查询
        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("请首先选择班级！", "提示信息");
                return;
            }
            else
            {
                Query(this.cboClass.Text.Trim());
            }
            //【1】显示成绩查询结果
            //this.dgvScoreList.DataSource = objScoreService.QueryScoreList(this.cboClass.Text.Trim());
            //new Common.DataGridViewStyle().DgvStyle1(this.dgvScoreList);
            //QueryScore(this.cboClass.SelectedValue.ToString());
        }
        private void Query(string className)
        {
            //[1]定义方法的输出参数（相对于后台调用）
            Dictionary<string, string> dicParam = null;
            List<string> absentList = null;
            //[2]执行查询并接收返回的集合与参数
            List<StudentExt> scoreList = objScoreService.GetScoreInfo(className, out dicParam, out absentList);
            //[3]显示结果查询列表
            this.dgvScoreList.AutoGenerateColumns = false;
            this.dgvScoreList.DataSource = scoreList;
            //[4]显示统计信息（方法输出参数）
            this.lblAttendCount.Text = dicParam["stuCount"];
            this.lblCount.Text = dicParam["absentCount"];
            this.lblDBAvg.Text= dicParam["avgDB"];
            this.lblCSharpAvg.Text= dicParam["avgCSharp"];
            //[5]显示缺考人员列表
            this.lblList.Items.Clear();
            if (absentList.Count != 0)
            {
                this.lblList.Items.AddRange(absentList.ToArray());
            }
            else
            {
                this.lblList.Items.Add("没有缺考");
            }
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void QueryScore(string classId)
        {
            //【2】显示查询统计结果
            Dictionary<string, string> infoList = objScoreService.QueryScoreInfo(classId);
            this.lblAttendCount.Text = infoList["stuCount"];
            this.lblCount.Text = infoList["absentCount"];
            this.lblCSharpAvg.Text = infoList["avgCSharp"];
            this.lblDBAvg.Text = infoList["avgDB"];
            //【3】显示缺考人员列表
            List<string> absentList = objScoreService.QueryAbsentList(classId);
            this.lblList.Items.Clear();
            if (absentList.Count == 0)
            {
                this.lblList.Items.Add("没有缺考");
            }
            else
            {
                this.lblList.Items.AddRange(absentList.ToArray());
            }
        }
        //全校查询
        private void btnStat_Click(object sender, EventArgs e)
        {
            //this.dgvScoreList.DataSource = objScoreService.QueryScoreList("");
            QueryScore("");
        }
        //添加行号
        private void dgvScoreList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvScoreList, e);
        }

    
     

        private void dgvScoreList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
         
        }
      
        private void dgvScoreList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        //解析组合属性
        private void dgvScoreList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.Value is Student)
            {
                e.Value = (e.Value as Student).StudentId;
            }
            if (e.ColumnIndex == 1 && e.Value is Student)
            {
                e.Value = (e.Value as Student).StudentName;
            }
            if (e.ColumnIndex == 2 && e.Value is Student)
            {
                e.Value = (e.Value as Student).Gender;
            }
            if (e.ColumnIndex == 3 && e.Value is StudentClass)
            {
                e.Value = (e.Value as StudentClass).ClassName;
            }
            if (e.ColumnIndex == 4 && e.Value is ScoreList)
            {
                e.Value = (e.Value as ScoreList).CSharp;
            }
            if (e.ColumnIndex == 5 && e.Value is ScoreList)
            {
                e.Value = (e.Value as ScoreList).SQLServerDB;
            }

        }
    }
}