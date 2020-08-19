using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentDemo.DataDomin.DataUnitTest;
using StudentDemo.DominStd.Entities;
using StudentDemo.DominStd.Response;
using StudentDemo.HubWebapi.Model;
using StudentDemo.Tools.Services;

namespace StudentDemo.HubWebapi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/studentservice/student")]
    [ApiController]
    //[Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICommonService _studentService;
        private readonly StudentDbcontext _dbcontext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public StudentController(IConfiguration configuration,ICommonService commonService,StudentDbcontext dbcontext)
        {
            _configuration = configuration;
            _studentService = commonService;
            _dbcontext = dbcontext;
        }
        /// <summary>
        /// 获取所有学生
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        //[Authorize]
        public  ServiceResultOfT<IEnumerable<StudentModel>> Get()
        {
            var result = new ServiceResultOfT<IEnumerable<StudentModel>>();
            result.Entities = new List<StudentModel>();

            if ((result.Entities =  _studentService.GetAllNoTracking<StudentModel>().ToList())!=null)
            {
                result.IsSuccess(_configuration["ip"], _configuration["port"],result.Entities);
                Console.WriteLine("api is access be link");
                return result;
            }
            result.IsFailed(_configuration["ip"], _configuration["port"]);
            Console.WriteLine("api is access be link");
            return result;
        }
        /// <summary>
        /// 查找一个学生
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        [HttpGet("{StudentId}")]
        //[Authorize]
        public ServiceResultOfT<StudentModel> Get(int StudentId)
        {
            var result = new ServiceResultOfT<StudentModel>();

            if ((result.Entities = _studentService.Find<StudentModel>(StudentId)) != null)
            {
                result.IsSuccess(_configuration["ip"], _configuration["port"], result.Entities);
                Console.WriteLine("api is access be link");
                return result;
            }
            result.IsFailed(_configuration["ip"], _configuration["port"]);
            Console.WriteLine("api is access be link");
            return result;
        }
        /// <summary>
        /// 新增学生
        /// </summary>
        /// <param name="entity">学生实体</param>
        /// <returns></returns>
        [HttpPost]
        public StudentModel Post(StudentModel entity)
        {
            if ( _studentService.Add(entity)==ReturnResultEnum.EnumResult.success)
            {
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 修改学生信息
        /// </summary>
        /// <param name="entity">学生实体</param>
        /// <returns></returns>
        [HttpPut]
        public StudentModel Put(StudentModel entity)
        {
            if (_studentService.Edit(entity)==ReturnResultEnum.EnumResult.success)
            {
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 删除学生信息
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        [HttpDelete("{StudentId}")]
        public IEnumerable<StudentModel> Delete(int StudentId)
        {
            var entity = _studentService.Find<StudentModel>(StudentId);
            if (_studentService.Romve(entity)==ReturnResultEnum.EnumResult.success)
            {
                return _studentService.GetAllNoTracking<StudentModel>();
            }
            return null;
        }
    }
}