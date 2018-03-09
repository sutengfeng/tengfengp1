using System.Collections.Generic;
using Models;

namespace DAL
{
    public interface IStudentService
    {
        int AddStudent(Student objStudent);
        int DeleteStudent(string studentId);
        Student GetStudentByCardNo(string CardNo);
        List<Student> GetStudentByClass(string ClassName);
        Student GetStudentById(string studentId);
        bool IsCardNoExisted(string CardNo);
        bool IsCardNoExisted(string newCardNo, string StudentId);
        bool IsIdNoExisted(string StudentIdNo);
        bool IsIdNoExisted(string newStudIdNo, string StudentId);
        int ModifyStudent(Student objStudent);
    }
}