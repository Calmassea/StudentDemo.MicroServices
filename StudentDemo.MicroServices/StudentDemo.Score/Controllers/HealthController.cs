using Microsoft.AspNetCore.Mvc;
using System;

namespace StudentDemo.ScoreWebapi.Controllers
{
    /// <summary>
    /// 健康检查
    /// </summary>
    [Route("api/[Controller]")]
    public class HealthController:ControllerBase
    {
        /// <summary>
        /// 健康检查接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Console.WriteLine("This Score api is health!");
            return Ok();
        }
    }
}
