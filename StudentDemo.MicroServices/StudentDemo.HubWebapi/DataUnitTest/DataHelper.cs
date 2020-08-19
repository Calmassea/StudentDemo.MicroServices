using System;
using System.Collections.Generic;

namespace StudentDemo.DataDomin.DataUnitTest
{
    public class DataHelper
    {
        public static List<Student> ListStudent = new List<Student>();
        public static int Studentid = 0;

        public static List<Student> GetStudent(string ip,string port)
        {
            if (ListStudent.Count!=0)
            {
                return ListStudent;
            }
            if (ListStudent.Count>=50)
            {
                ListStudent.Clear();
            }
            Random random = new Random();
            int i = 0;
            for (i=0; i < 50; i++)
            {
                Student student = new Student();
                
                student.ID = i;
                student.Name = $"张{i}";
                student.Age = 20 + i;
                student.ip = ip;
                student.port = port;
                student.Profession = "软件工程";
                student.SClass = "20180"+(random.Next(1,5)+1);
                ListStudent.Add(student);
            }
            Studentid = i - 1;
            
            return ListStudent;
        }
    }
}
