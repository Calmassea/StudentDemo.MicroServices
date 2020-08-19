using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentDemo.DominStd.Entities;
using StudentDemo.DominStd.Response;
using StudentDemo.ScoreWebapi.Models;
using StudentDemo.Tools.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentDemo.ScoreWebapi.Controllers
{
    [Route("api/scoreservice/score")]
    [ApiController]
    public class StudentScoreController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICommonService _scoretService;
        private readonly ScoreDbContext _dbcontext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public StudentScoreController(IConfiguration configuration, ICommonService commonService, ScoreDbContext dbcontext)
        {
            _configuration = configuration;
            _scoretService = commonService;
            _dbcontext = dbcontext;
        }
        /// <summary>
        /// 获取所有成绩
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        //[Authorize]
        public ServiceResultOfT<IEnumerable<ScoreModel>> Get()
        {
            var result = new ServiceResultOfT<IEnumerable<ScoreModel>>();
            result.Entities = new List<ScoreModel>();

            if ((result.Entities = _scoretService.GetAllNoTracking<ScoreModel>().ToList()) != null)
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
        /// 查找一个成绩
        /// </summary>
        /// <param name="ScoreId">分数id</param>
        /// <returns></returns>
        [HttpGet("{ScoreId}")]
        //[Authorize]
        public ServiceResultOfT<ScoreModel> Get(int ScoreId)
        {
            var result = new ServiceResultOfT<ScoreModel>();

            if ((result.Entities = _scoretService.Find<ScoreModel>(ScoreId)) != null)
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
        /// 查找一个学生的成绩
        /// </summary>
        /// <param name="StudentId">学生id</param>
        /// <returns></returns>
        [HttpGet("Stu/{StudentId}")]
        //[Authorize]
        public ServiceResultOfT<ScoreModel> StuGet(int StudentId)
        {
            var result = new ServiceResultOfT<ScoreModel>();

            if ((result.Entities = _scoretService.QueryOnly<ScoreModel>(s=>s.StudentId==StudentId,false)) != null)
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
        /// 新增一条成绩
        /// </summary>
        /// <param name="entity">成绩实体</param>
        /// <returns></returns>
        [HttpPost]
        public ScoreModel Post(ScoreModel entity)
        {
            if (_scoretService.Add(entity) == ReturnResultEnum.EnumResult.success)
            {
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 修改成绩
        /// </summary>
        /// <param name="entity">成绩实体</param>
        /// <returns></returns>
        [HttpPut]
        public ScoreModel Put(ScoreModel entity)
        {
            if (_scoretService.Edit(entity) == ReturnResultEnum.EnumResult.success)
            {
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 删除成绩
        /// </summary>
        /// <param name="ScoreId"></param>
        /// <returns></returns>
        [HttpDelete("{ScoreId}")]
        public IEnumerable<ScoreModel> Delete(int ScoreId)
        {
            var entity = _scoretService.Find<ScoreModel>(ScoreId);
            if (_scoretService.Romve(entity) == ReturnResultEnum.EnumResult.success)
            {
                return _scoretService.GetAllNoTracking<ScoreModel>();
            }
            return null;
        }
        /// <summary>
        /// 删除成绩
        /// </summary>
        /// <param name="StudentId">学生id</param>
        /// <returns></returns>
        [HttpDelete("Stu/{StudentId}")]
        public IEnumerable<ScoreModel> StuDelete(int StudentId)
        {
            var entity = _scoretService.QueryOnly<ScoreModel>(s=>s.StudentId== StudentId,true);
            if (_scoretService.Romve(entity) == ReturnResultEnum.EnumResult.success)
            {
                return _scoretService.GetAllNoTracking<ScoreModel>();
            }
            return null;
        }
    }
}
