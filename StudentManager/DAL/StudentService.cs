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
    /// 学员管理数据访问类
    /// </summary>
    public class StudentService : IStudentService
    {
        /// <summary>
        /// 判断身份证号是否存在
        /// </summary>
        /// <param name="StudentIdNo"></param>
        /// <returns></returns>
        public bool IsIdNoExisted(string StudentIdNo)
        {
            string sql = "select count(*) from Students where StudentIdNo="+StudentIdNo;
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (result == 1) return true;
            else return false;
        }
        /// <summary>
        /// 考勤卡号
        /// </summary>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        public bool IsCardNoExisted(string CardNo)
        {
            string sql = string.Format("select count(*) from Students where CardNo='{0}'", CardNo);
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (result == 1) return true;
            else return false;
        }
        /// <summary>
        /// 将学员对象保存到数据库
        /// </summary>
        /// <param name="objStudent"></param>
        /// <returns>返回当前新学员的学号</returns>
        public int AddStudent(Student objStudent)
        {
            //【1】编写sql语句
            StringBuilder sqlBuilder = new StringBuilder("insert into Students ");
            sqlBuilder.Append("(StudentName,Gender,Birthday,Age,StudentIdNo,CardNo,PhoneNumber,StudentAddress,ClassId,StuImage)");
            sqlBuilder.Append("values(@StudentName,@Gender,@Birthday,@Age,@StudentIdNo,@CardNo,@PhoneNumber,@StudentAddress,@ClassId,@StuImage)");
            //定义参数数组
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@StudentName",objStudent.StudentName),
                new SqlParameter("@Gender",objStudent.Gender),
                new SqlParameter("@Birthday",objStudent.Birthday),
                new SqlParameter("@Age",objStudent.Age),
                new SqlParameter("@StudentIdNo",objStudent.StudentIdNo),
                new SqlParameter("@CardNo",objStudent.CardNo),
                new SqlParameter("@PhoneNumber",objStudent.PhoneNumber),
                new SqlParameter("@StudentAddress",objStudent.StudentAddress),
                new SqlParameter("@ClassId",objStudent.ClassId),
                new SqlParameter("@StuImage",objStudent.StuImage)
             };
            //sqlBuilder.Append(" values('{0}','{1}','{2}',{3},{4},'{5}','{6}','{7}',{8},'{9}');select @@identity");
            //【2】解析对象
            //string sql = string.Format(sqlBuilder.ToString(),objStudent.StudentName,objStudent.Gender,
            //objStudent.Birthday.ToString("yyyy-MM-dd"),objStudent.Age,objStudent.StudentIdNo, 
            //objStudent.CardNo, objStudent.PhoneNumber,objStudent.StudentAddress,objStudent.ClassId,objStudent.StuImage);
            //【3】提交sql语句
            try
            {
                //return Convert.ToInt32(SQLHelper.GetSingleResult(sql,param));//执行sql语句，返回学号
                return Convert.ToInt32(SQLHelper.Update(sqlBuilder.ToString(), param));
            }
            catch (Exception ex)
            {

                throw new Exception("添加学员时，数据库访问异常："+ex.Message );
            }
        }

        /// <summary>
        /// 根据班级查询学员信息
        /// </summary>
        /// <param name="ClassName">班级名称</param>
        /// <returns>返回学员信息列表</returns>
        public List<Student> GetStudentByClass(string ClassName)
        {
            string sql = "select StudentId,StudentName,Gender,Birthday,StudentIdNo,PhoneNumber,ClassName from Students ";
            sql += "inner join StudentClass on Students.ClassId=StudentClass.ClassId ";
            sql += "where ClassName='{0}'";
            sql = string.Format(sql,ClassName);
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<Student> stuList = new List<Student>();
            while (objReader.Read())
            {
                stuList.Add(new Student()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    Birthday=Convert.ToDateTime(objReader["Birthday"].ToString()),
                    StudentIdNo=objReader["StudentIdNo"].ToString(),
                    PhoneNumber=objReader["PhoneNumber"].ToString(),
                    ClassName=objReader["ClassName"].ToString()
                });               
            }
            objReader.Close();
            return stuList;

        }
        /// <summary>
        /// 根据学号查询学员对象
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public Student GetStudentById(string studentId)
        {
            //string sql = "select StudentId,StudentName,Gender,Birthday,StudentIdNo,PhoneNumber,ClassName,StudentAddress,CardNo,StuImage from Students ";
            //sql += "inner join StudentClass on Students.ClassId=StudentClass.ClassId ";
            //sql += "where StudentId="+studentId;            
            //SqlDataReader objReader = SQLHelper.GetReader(sql);
            //Student objStudent = null;
            //if (objReader.Read())
            //{
            //    objStudent = new Student()
            //    {
            //        StudentId = Convert.ToInt32(objReader["StudentId"]),
            //        StudentName = objReader["StudentName"].ToString(),
            //        Gender = objReader["Gender"].ToString(),
            //        Birthday = Convert.ToDateTime(objReader["Birthday"].ToString()),
            //        StudentIdNo = objReader["StudentIdNo"].ToString(),
            //        PhoneNumber = objReader["PhoneNumber"].ToString(),
            //        ClassName = objReader["ClassName"].ToString(),
            //        StudentAddress=objReader["StudentAddress"].ToString(),
            //        CardNo=objReader["CardNo"].ToString(),
            //        StuImage=objReader["StuImage"]==null?"":objReader["StuImage"].ToString()
            //    };
            //}
            //objReader.Close();
            //return objStudent;
            string whereSql = string.Format(" where StudentId='{0}'", studentId);
            return this.GetStudentByWhereSql(whereSql);
        }
        /// <summary>
        /// 修改学员时判断身份证号和其他学员是否重复
        /// </summary>
        /// <param name="StudentIdNo">新的身份证号</param>
        /// <param name="StudentId">当前学员的学号</param>
        /// <returns></returns>
        public bool IsIdNoExisted(string newStudIdNo,string StudentId)
        {
            string sql =string .Format( "select count(*) from Students where StudentIdNo={0} and StudentId<>studentId" , newStudIdNo);
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (result == 1) return true;
            else return false;
        }
        private Student GetStudentByWhereSql(string WhereSql)
        {
            string sql = "select StudentId,StudentName,Gender,Birthday,StudentIdNo,PhoneNumber,ClassName,StudentAddress,CardNo,StuImage from Students ";
            sql += "inner join StudentClass on Students.ClassId=StudentClass.ClassId ";
            sql += WhereSql;
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            Student objStudent = null;
            if (objReader.Read())
            {
                objStudent = new Student()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    Birthday = Convert.ToDateTime(objReader["Birthday"].ToString()),
                    StudentIdNo = objReader["StudentIdNo"].ToString(),
                    PhoneNumber = objReader["PhoneNumber"].ToString(),
                    ClassName = objReader["ClassName"].ToString(),
                    StudentAddress = objReader["StudentAddress"].ToString(),
                    CardNo = objReader["CardNo"].ToString(),
                    StuImage = objReader["StuImage"] == null ? "" : objReader["StuImage"].ToString()
                };
            }
            objReader.Close();
            return objStudent;

        }
        //修改学员时考号的判断
        public bool IsCardNoExisted(string newCardNo, string StudentId)
        {
            string sql = string.Format("select count(*) from Students where CardNo={0} and StudentId<>studentId", newCardNo);
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (result == 1) return true;
            else return false;
        }
        /// <summary>
        /// 根据卡号查询学员信息
        /// </summary>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        public Student GetStudentByCardNo(string CardNo)
        { 
            string whereSql = string.Format(" where CardNo='{0}'", CardNo);
            return this.GetStudentByWhereSql(whereSql);
        }

        /// <summary>
        /// 修改学员信息
        /// </summary>
        /// <param name="objStudent"></param>
        /// <returns></returns>
        public int ModifyStudent(Student objStudent)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("update Students set StudentName='{0}',Gender='{1}',Birthday='{2}',");
            sqlBuilder.Append("StudentIdNo={3},Age={4},PhoneNumber='{5}',StudentAddress='{6}',");
            sqlBuilder.Append("CardNo='{7}',ClassId={8},StuImage='{9}' ");
            sqlBuilder.Append("where StudentId={10}");
            string sql = string.Format(sqlBuilder.ToString(), objStudent.StudentName, objStudent.Gender,
            objStudent.Birthday.ToString("yyyy-MM-dd"), objStudent.StudentIdNo, objStudent.Age,
            objStudent.PhoneNumber, objStudent.StudentAddress, objStudent.CardNo,
            objStudent.ClassId, objStudent.StuImage, objStudent.StudentId);
            try
            {
                return SQLHelper.UpDate(sql);
            }
            catch (Exception ex)
            {

                throw new Exception("修改学员信息时数据访问发生异常：" + ex.Message);
            }
        }
        #region 删除学员
        /// <summary>
        /// 根据学号删除学员对象
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public int DeleteStudent(string studentId)
        {
            string sql = "delete from Students where StudentId=" + studentId;
            try
            {
                return SQLHelper.UpDate(sql);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new Exception("该学号被其他实体引用，不能直接删除该学员对象！");
                }
                else
                {
                    throw new Exception("删除学员对象发生数据操作异常：\r\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public int DeleteStudent(Student objStudent)
        //{
        //    string sql = "delete from Students where StudentId=" + objStudent.StudentId;
        //    try
        //    {
        //        return SQLHelper.UpDate(sql);
        //    }
        //    catch (SqlException ex)
        //    {
        //        if (ex.Number == 547)
        //        {
        //            throw new Exception("该学号被其他实体引用，不能直接删除该学员对象！");
        //        }
        //        else
        //        {
        //            throw new Exception("删除学员对象发生数据操作异常：\r\n" + ex.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion
    }
}
