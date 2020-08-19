using StudentDemo.DataDomin.DataUnitTest;
using StudentDemo.DominStd.Entities;
using StudentDemo.DominStd.Response;
using StudentDemo.MainWebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.MainWebClient.Services
{
    /// <summary>
    /// 学生服务接口
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// 获取所有学生
        /// </summary>
        /// <returns>IEnumerable<Student></returns>
        public Task<ServiceResultOfT<IEnumerable<StudentModel>>> GetAllAsync();
        /// <summary>
        /// 获取一个学生
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ServiceResultOfT<StudentModel>> GetStudentAsync(int id);
        /// <summary>
        /// 添加一名学生
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public Task<ServiceResultOfT<StudentModel>> AddStudentAsync(StudentModel student);
        /// <summary>
        /// 删除一名学生
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ServiceResultOfT<IEnumerable<StudentModel>>> DeleteStudentAsync(int id);
        /// <summary>
        /// 修改一名学生
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public Task<ServiceResultOfT<StudentModel>> EditStudentAsync(StudentModel student);
    }
}
