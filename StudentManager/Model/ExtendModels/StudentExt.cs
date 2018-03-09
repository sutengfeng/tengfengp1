using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 学员实体【扩展】
    /// </summary>
    public class StudentExt : Student
    {
        //public Student StudentObj { get; set; }
        public StudentClass ClassObj { get; set; }
        public ScoreList ScoreObj { get; set; }
        public string ClassName { get; set; }
        public string CSharp { get; set; }
        public string SQLServerDB { get; set; }
        public DateTime DTime { get; set; }//签到时间
    }
}
