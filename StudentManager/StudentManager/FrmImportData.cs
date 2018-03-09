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
    public partial class FrmImportData : Form
    {
        private List<Student> stuList = null;//用来保存导入的学员对象
        private ImportDataFromExcel objImport = new ImportDataFromExcel();
        public FrmImportData()
        {
            InitializeComponent();
            this.dgvStudentList.AutoGenerateColumns = false;
        }
        private void btnChoseExcel_Click(object sender, EventArgs e)
        {
            //打开文件
            OpenFileDialog openFile = new OpenFileDialog();
            DialogResult result = openFile.ShowDialog();
            if(result==DialogResult.OK)
            {
                string path = openFile.FileName;//获取Excel文件的路径
                this.stuList = objImport.GetStudentByExcel(path);
                //显示数据
                //this.dgvStudentList.DataSource = null;
                this.dgvStudentList.DataSource = this.stuList; 
            }
        }
        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }
        //保存到数据库
        private void btnSaveToDB_Click(object sender, EventArgs e)
        {

        }
    }
}

