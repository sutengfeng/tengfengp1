using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace DAL
{
    /// <summary>
    /// 访问Access数据库的通用类
    /// </summary>
    class OleDbHelper
    {
        ////适合于excel2003以后的版本
        //private static string connString = "Provider=Microsoft.Jet.OLEDB.12.0;Data Source={0};Extended properties=Excel 8.0";
        //创建链接字符串(适合于excel2007以后的版本)

        private static string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties = 'Excel 8.0;HDR=NO;IMEX=1'";
        #region
        public static int Update(string sql)
        {
            OleDbConnection conn = new OleDbConnection(connString);
            OleDbCommand cmd = new OleDbCommand(sql,conn);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //写入日志
                throw ex;
            }
            finally
            {
                conn.Close();              
            }
        }
        public static object GetSingleResult(string sql)
        {
            OleDbConnection conn = new OleDbConnection(connString);
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //写入日志
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public static OleDbDataReader GetReader(string sql)
        {
            OleDbConnection conn = new OleDbConnection(connString);
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                //写入日志
                throw ex;
            }          
        }
        /// <summary>
        /// 执行返回数据集的查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql)
        {
            OleDbConnection conn = new OleDbConnection(connString);
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);//创建数据适配器对象
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                //写入日志
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
# endregion
        /// <summary>
        /// 将指定路径的Excel导入到数据集中
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql,string path)
        {
            OleDbConnection conn = new OleDbConnection(string.Format(connString,path));
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);//创建数据适配器对象
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                //写入日志
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
