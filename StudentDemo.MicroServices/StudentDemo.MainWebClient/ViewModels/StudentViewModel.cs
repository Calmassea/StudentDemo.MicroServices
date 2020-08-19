using StudentDemo.DataDomin.DataUnitTest;
using StudentDemo.DominStd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.MainWebClient.ViewModels
{
    public class StudentViewModel
    {
        /// <summary>
        /// 学生列表
        /// </summary>
        public IEnumerable<StudentModel> Students { get; set; }
        /// <summary>
        /// 一个学生
        /// </summary>
        public StudentModel Student { get; set; } = new StudentModel();
    }
}
