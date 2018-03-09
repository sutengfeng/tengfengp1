using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;

namespace DAL
{
    /// <summary>
    /// 从Excel中导入数据
    /// </summary>
     public  class ImportDataFromExcel
    {
        /// <summary>
        /// 将选择的Excel数据表查询后封装为对象集合
        /// </summary>
        /// <param name="path">Excel文件的路径</param>
        /// <returns></returns>
        public List<Student> GetStudentByExcel(string path)
        {
            List<Student> list = new List<Student>();
            string sql = "select * from [Students$]";
            DataSet ds = OleDbHelper.GetDataSet(sql,path);
            DataTable dt = ds.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Student()
                {
                    StudentName = row["姓名"].ToString(),
                    Gender = row["性别"].ToString(),
                    PhoneNumber = row["电话号码"].ToString(),
                    Birthday = Convert.ToDateTime(row["出生日期"].ToString()),
                    StudentIdNo = row["身份证号"].ToString(),
                    Age = DateTime.Now.Year - Convert.ToDateTime(row["出生日期"].ToString()).Year,
                    CardNo = row["考勤卡号"].ToString(),
                    StudentAddress = row["家庭住址"].ToString(),
                    ClassId =Convert.ToInt32(row["班级编号"])
                });
            }
            return list;
        }
    }
}
