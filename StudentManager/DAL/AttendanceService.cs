using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    /// <summary>
    /// 考勤数据访问类
    /// </summary>
    public class AttendanceService
    {
        /// <summary>
        /// 添加考勤记录
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public string AddRecord(string cardNo)
        {
            string sql =string.Format("insert into Attendance (CardNo) values('{0}')",cardNo);
            try
            {
                SQLHelper.UpDate(sql);
                return "success";
            }
            catch (Exception ex)
            {

                return "打卡失败!系统出现异常，请联系管理员！" + ex.Message;
            }
        }
        /// <summary>
        /// 获取学生总数（应到人数）
        /// </summary>
        /// <returns></returns>
        public int GetStudentCount()
        {
            string sql = "select count(*) from Students";
            return Convert.ToInt32(SQLHelper.GetSingleResult(sql));
        }
        /// <summary>
        /// 获取当天已经签到的学员总数
        /// </summary>
        /// <returns></returns>
        public int GetSignStudents()
        {
            string sql = "select count(distinct CardNo) from Attendance where DTime between '{0}' and '{1}' ";
            DateTime dt1 = Convert.ToDateTime(SQLHelper.GetServerTime().ToShortDateString());
            sql = string.Format(sql,dt1,dt1.AddDays(1.0));
            return Convert.ToInt32(SQLHelper.GetSingleResult(sql));
        }
        /// <summary>
        /// 按照指定日期查询实到学生总数
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public int GetSignStudents(DateTime beginTime,DateTime endTime)
        {
            string sql = "select count(distinct CardNo) from Attendance where DTime between '{0}' and '{1}' ";
            DateTime dt1 = Convert.ToDateTime(SQLHelper.GetServerTime().ToShortDateString());
            sql = string.Format(sql, beginTime, endTime);
            return Convert.ToInt32(SQLHelper.GetSingleResult(sql));
        }
        /// <summary>
        /// 根据日期和姓名查询考勤记录
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="name">学员姓名</param>
        /// <returns></returns>
        public List<Student> GetStudentBySignDate(DateTime beginTime,DateTime endTime,string name)
        {
            string sql = "select DTime,StudentId,StudentName,Gender,ClassName,Attendance.CardNo from Students ";
            sql += "inner join StudentClass on Students.ClassId=StudentClass.ClassId ";
            sql += "inner join Attendance on Students.CardNo=Attendance.CardNo ";
            sql += "where DTime between'{0}' and '{1}' ";
            sql = string.Format(sql,beginTime,endTime);
            if (name != null && name.Length != 0)
            {
                sql += string.Format("and StudentName='{0}'",name);
            }
            sql += " order by DTime ASC";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<Student> list = new List<Student>();
            while(objReader.Read())
            {
                list.Add(new Student()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    CardNo = objReader["CardNo"].ToString(),
                    ClassName = objReader["ClassName"].ToString(),
                    DTime = Convert.ToDateTime(objReader["DTime"])                 
                });
            }
            objReader.Close();
            return list;
        }
    }
}
