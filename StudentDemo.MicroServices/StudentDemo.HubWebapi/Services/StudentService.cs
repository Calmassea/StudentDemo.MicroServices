using StudentDemo.HubWebapi.Model;
using StudentDemo.Tools.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.HubWebapi.Services
{
    /// <summary>
    /// 学生服务类
    /// </summary>
    public class StudentService:CommonService<StudentDbcontext>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbcontext"></param>
        public StudentService(StudentDbcontext dbcontext):base(dbcontext)
        {

        }
    }
}
