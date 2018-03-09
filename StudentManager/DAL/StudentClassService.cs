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
    /// 班级数据访问类
    /// </summary>
    public class StudentClassService
    {
        /// <summary>
        /// 查询所有的班级对象
        /// </summary>
        /// <returns></returns>
        public List<StudentClass> GetAllClass()
        {
            string sql = "select ClassName,ClassId from StudentClass";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<StudentClass> list = new List<StudentClass>();
            while (objReader.Read())
            {
                list.Add(new StudentClass()
                {
                     ClassId=Convert.ToInt32(objReader["ClassId"]),
                     ClassName=objReader["ClassName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }

        /// <summary>
        /// 获取所有的班级（存放到数据集里面）
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllClasses()
        {
            string sql = "select ClassName,ClassId from StudentClass";
            return SQLHelper.GetDataSet(sql);
        }
        
    }
}
