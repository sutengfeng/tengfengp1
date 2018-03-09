using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Models;

namespace DAL
{
    public class MenuListService
    {
        /// <summary>
        /// 获取所有的菜单子项
        /// </summary>
        /// <returns></returns>
        public List<MenuList> GetAllMenu()
        {
            string sql = "select MenuId,MenuName,MenuCode,ParentId from MenuList";
            List<MenuList> menuList = new List<MenuList>();
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            while (objReader.Read())
            {
                menuList.Add(new MenuList()
                {
                    MenuId = Convert.ToInt32(objReader["MenuId"]),
                    MenuName=objReader["MenuName"].ToString(),
                    MenuCode = objReader["MenuCode"].ToString(),
                    ParentId=objReader["ParentId"].ToString()
                });
            }
            objReader.Close();
            return menuList;
        }
    }
}
