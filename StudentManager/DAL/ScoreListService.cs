using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Models;

namespace DAL
{
    /// <summary>
    /// 成绩表数据访问类
    /// </summary>
    public class ScoreListService
    {
        /// <summary>
        /// 根据班级查询考试成绩（或全校成绩列表）
        /// </summary>
        /// <param name="className">班级名称</param>
        /// <returns>返回成绩查询结果</returns>
        //public List<Student> QueryScoreList(string className)
        //{
        //    string sql = "select Students.StudentId,StudentName,ClassName,Gender,CSharp,SQLServerDB ";
        //    sql += "from Students ";
        //    sql += "inner join StudentClass on StudentClass.ClassId=Students.ClassId ";
        //    sql += "inner join ScoreList on ScoreList.StudentId=Students.StudentId ";
        //    if (className != null && className.Length != 0)
        //    {
        //        sql += "where ClassName='"+className+"'";
        //    }
        //    SqlDataReader objReader = SQLHelper.GetReader(sql);
        //    List<Student> stuList = new List<Student>();
        //    while (objReader.Read())
        //    {
        //        stuList.Add(new Student()
        //        {
        //            StudentId = Convert.ToInt32(objReader["StudentId"]),
        //            StudentName = objReader["StudentName"].ToString(),
        //            ClassName = objReader["ClassName"].ToString(),
        //            Gender = objReader["Gender"].ToString(),
        //            CSharp = Convert.ToInt32(objReader["CSharp"]),
        //            SQLServerDB = Convert.ToInt32(objReader["SQLServerDB"])
        //        });
        //    }
        //    objReader.Close();
        //    return stuList;
        //}
        public List<StudentExt> QueryScoreList(string className)
        {
            string sql = "select Students.StudentId,StudentName,ClassName,Gender,CSharp,SQLServerDB ";
            sql += "from Students ";
            sql += "inner join StudentClass on StudentClass.ClassId=Students.ClassId ";
            sql += "inner join ScoreList on ScoreList.StudentId=Students.StudentId ";
            if (className != null && className.Length != 0)
            {
                sql += "where ClassName='" + className + "'";
            }
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<StudentExt> stuList = new List<StudentExt>();
            while (objReader.Read())
            {
                stuList.Add(new StudentExt()
                {
                    StudentObj=new Student()
                    {
                        StudentId = Convert.ToInt32(objReader["StudentId"]),
                        StudentName = objReader["StudentName"].ToString(),  
                        Gender = objReader["Gender"].ToString(),          
                     },
                    ClassObj=new StudentClass()
                    {
                        ClassName = objReader["ClassName"].ToString(),
                    },
                     ScoreObj=new ScoreList()
                     {
                         CSharp = Convert.ToInt32(objReader["CSharp"]),
                         SQLServerDB = Convert.ToInt32(objReader["SQLServerDB"])
                     }            
                });
            }
            objReader.Close();
            return stuList;
        }
        #region 调用存储过程统计考试信息
        public List<StudentExt> GetScoreInfo(string className, out Dictionary<string, string> dicParam, out List<string> absentList)
        {
            //定义参数
            SqlParameter inputClassName = new SqlParameter("@className", className);
            inputClassName.Direction = ParameterDirection.Input;//设置参数为输入类型

            SqlParameter outStuCount = new SqlParameter("@stuCount", SqlDbType.Int);
            outStuCount.Direction = ParameterDirection.Output;//设置参数为输出类型
            SqlParameter outAbsentCount = new SqlParameter("@absentCount", SqlDbType.Int);
            outAbsentCount.Direction = ParameterDirection.Output;//设置参数为输出类型
            SqlParameter outAvgDB = new SqlParameter("@avgDB", SqlDbType.Int);
            outAvgDB.Direction = ParameterDirection.Output;//设置参数为输出类型
            SqlParameter outAvgCSharp = new SqlParameter("@avgCSharp", SqlDbType.Int);
            outAvgCSharp.Direction = ParameterDirection.Output;//设置参数为输出类型

            //执行查询
            SqlParameter[] param = new SqlParameter[] { inputClassName, outStuCount, outAbsentCount, outAvgDB, outAvgCSharp };
            SqlDataReader objReader = SQLHelper.Getreader("usp_ScoreQuery", param);
            //读取考试成绩列表
            List<StudentExt> ScoreList = new List<StudentExt>();
            while (objReader.Read())
            {
                ScoreList.Add(new StudentExt()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    ClassName = objReader["ClassName"].ToString(),
                    CSharp=objReader["CSharp"].ToString(),
                    SQLServerDB = objReader["SQLServerDB"].ToString()
                });
            }
            //读取缺考人员列表
            List<string> absentName = new List<string>();
            if (objReader.NextResult())
            {
                while (objReader.Read())
                {
                    absentName.Add(objReader["StudentName"].ToString());
                }
            };
            //关闭读取器
            objReader.Close();
            //获取输出参数
            Dictionary<string, string> outDicParam = new Dictionary<string, string>();
            outDicParam["stuCount"] = outStuCount.Value.ToString();
            outDicParam["absentCount"] = outAbsentCount.Value.ToString();
            outDicParam["avgDB"] = outAvgDB.Value.ToString();
            outDicParam["avgCSharp"] = outAvgCSharp.Value.ToString();
            //返回输出参数和结果
            dicParam = outDicParam;
            absentList = absentName;
            return ScoreList;
        }
        #endregion 
        #region 成绩查询统计
        /// <summary>
        /// 根据班级统计考试成绩相关信息（或全校考试成绩统计）
        /// </summary>
        /// <param name="classId">班级编号</param>
        /// <returns>返回包含统计结果的泛型集合</returns>
        public Dictionary<string, string> QueryScoreInfo(string classId)
        {
            //查询考试总人数、CSharp和DB平均成绩
            string sql = "select stuCount=Count(*),avgCSharp=avg(CSharp),avgDB=avg(SQLServerDB) from ScoreList ";
            sql += "inner join Students on Students.StudentId=ScoreList.StudentId ";
            if (classId != null && classId.Length != 0)
            {
                sql += string.Format("where ClassId={0} ",classId);
            }
            //查询缺考总人数
            sql += ";select absentCount=Count(*) from Students where StudentId not in ";
            sql+="(select StudentId from ScoreList )";
            if (classId != null && classId.Length>0)
            {
                sql += string.Format("and ClassId={0}",classId);
            }
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            Dictionary<string, string> scoreInfo = null;
            if (objReader.Read())//读取考试统计结果
            {
                scoreInfo = new Dictionary<string, string>();
                scoreInfo.Add("stuCount",objReader["stuCount"].ToString());
                scoreInfo.Add("avgCSharp", objReader["avgCSharp"].ToString());
                scoreInfo.Add("avgDB", objReader["avgDB"].ToString());
            }
            //读取缺考人员列表
            if(objReader.NextResult())
            {
                if (objReader.Read())
                {
                    scoreInfo.Add("absentCount", objReader["absentCount"].ToString());
                }
            }
            objReader.Close();
            return scoreInfo;
        }
        /// <summary>
        /// 根据班级查询缺考人员列表（或全校缺考人员列表）
        /// </summary>
        /// <param name="classId">班级编号</param>
        /// <returns>返回缺考人员姓名</returns>
        public List<string> QueryAbsentList(string classId)
        {
            string sql = "select StudentName from Students where StudentId not in(select StudentId from ScoreList)";
            if (classId != null && classId.Length>0)
            {
                sql += string.Format("and ClassId={0}", classId);
            }
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<string> nameList = new List<string>();
            while (objReader.Read())
            {
                nameList.Add(objReader["StudentName"].ToString());
            }
            objReader.Read();
            return nameList;
        }
        #endregion
        #region 使用DataSet保存数据
        /// <summary>
        /// 获取全部考试成绩
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllScoreList()
        {           
            string sql = "select Students.StudentId,StudentName,ClassName,Gender,PhoneNumber,CSharp,SQLServerDB ";
            sql += "from Students ";
            sql += "inner join StudentClass on StudentClass.ClassId=Students.ClassId ";
            sql += "inner join ScoreList on ScoreList.StudentId=Students.StudentId";
            return SQLHelper.GetDataSet(sql);
        }
        #endregion
    }
}
