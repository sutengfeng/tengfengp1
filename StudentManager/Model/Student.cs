using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 学员实体类
    /// </summary>
    public class Student
    {
        public int StudentId{ get; set; }
        public string StudentName { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public string StudentIdNo { get; set; }
        public int Age { get; set; }
        public string StuImage { get; set; }
        public string PhoneNumber { get; set; }
        public string CardNo { get; set; }
        public string StudentAddress { get; set; }
        public int ClassId{ get; set; }
        //扩展属性
        public string ClassName { get; set; }
        public int  CSharp { get; set; }
        public int SQLServerDB { get; set; }
        public DateTime DTime { get; set; }//签到的时间
    }
}