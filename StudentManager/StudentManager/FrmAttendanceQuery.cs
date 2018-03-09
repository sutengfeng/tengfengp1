using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;
using Models;

namespace StudentManager
{
    public partial class FrmAttendanceQuery : Form
    {
        private AttendanceService objService = new AttendanceService();

        public FrmAttendanceQuery()
        {
            InitializeComponent();
            this.dgvStudentList.AutoGenerateColumns = false;
        }
        //查询考勤
        private void btnQuery_Click(object sender, EventArgs e)
        {
            //【1】根据日期和学员姓名查询考勤列表
            DateTime beginTime = Convert.ToDateTime(this.dtpTime.Text);
            DateTime endTime = beginTime.AddDays(1.0);
            this.dgvStudentList.DataSource = objService.GetStudentBySignDate(beginTime, endTime, this.txtName.Text.Trim());
            new Common.DataGridViewStyle().DgvStyle3(this.dgvStudentList);
            //【2】根据日期查询统计信息
            this.lblCount.Text = objService.GetStudentCount().ToString();
            this.lblReal.Text = objService.GetSignStudents(beginTime, endTime).ToString();
            this.lblAbsenceCount.Text = (Convert.ToInt32(this.lblCount.Text) - Convert.ToInt32(this.lblReal.Text)).ToString();
          }

        //添加行号
        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
          //  Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
