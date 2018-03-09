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
    /// 管理员数据访问类
    /// </summary>
    public class AdminService
    {
        //修饰符public→返回值Admin→参数（Admin）
        /// <summary>
        /// 根据账号和密码登录
        /// </summary>
        /// <param name="objAdmin">包含账号和密码的管理员对象</param>
        /// <returns></returns>
        public Admin AdminLogin(Admin objAdmin)
        {
            //SQL语句编写
            //string sql = "select AdminName from Admins where LoginId={0} and LoginPwd='{1}'";
            string sql = "select AdminName from Admins where LoginId=@LoginId and LoginPwd=@LoginPwd";
            //调用通用数据访问类
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@LoginId",objAdmin.LoginId),
                new SqlParameter("@LoginPwd",objAdmin.LoginPwd)
            };
            //sql = string.Format(sql, objAdmin.LoginId, objAdmin.LoginPwd);
            //返回结果
            try
            {
                SqlDataReader objReader = SQLHelper.GetReader(sql,param);
                if (objReader.Read())
                {
                    objAdmin.AdminName = objReader["AdminName"].ToString();
                }
                else
                {
                    objAdmin = null;
                }
                objReader.Close();
            }
            //catch (SqlException ex)
            //{

            //    throw new Exception("应用程序和数据库链接出现问题："+ex.Message);
            //}
            catch (Exception ex)
            {
                throw new Exception("用户登录出现数据访问异常："+ex.Message);
            }
            return objAdmin;
        }
        /// <summary>
        /// 修改管理员登录密码
        /// </summary>
        /// <param name="objAdmin"></param>
        /// <returns></returns>
        public int ModifyPwd(Admin objAdmin)
        {
            string sql = "update Admins set LoginPwd='{0}' where LoginId={1}";
            sql = string.Format(sql, objAdmin.LoginPwd, objAdmin.LoginId);
            try
            {
                return SQLHelper.UpDate(sql);
            }
            catch (Exception ex)
            {

                throw new Exception("修改密码出现数据访问异常："+ex.Message);
            }
           
        }
    }
}
 