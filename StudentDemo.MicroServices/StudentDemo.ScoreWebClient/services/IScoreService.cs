using StudentDemo.DominStd.Entities;
using StudentDemo.DominStd.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.ScoreWebClient.services
{
    /// <summary>
    /// 分数服务接口
    /// </summary>
    public interface IScoreService
    {
        /// <summary>
        /// 获取所有学生
        /// </summary>
        /// <returns>IEnumerable<Student></returns>
        public Task<ServiceResultOfT<IEnumerable<ScoreModel>>> GetAllAsync();
        /// <summary>
        /// 获取一个学生
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ServiceResultOfT<ScoreModel>> GetStudentAsync(int id);
        /// <summary>
        /// 添加一名学生
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public Task<ServiceResultOfT<ScoreModel>> AddStudentAsync(ScoreModel student);
        /// <summary>
        /// 删除一名学生
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ServiceResultOfT<IEnumerable<ScoreModel>>> DeleteStudentAsync(int id);
        /// <summary>
        /// 修改一名学生
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public Task<ServiceResultOfT<ScoreModel>> EditStudentAsync(ScoreModel student);
    }
}
