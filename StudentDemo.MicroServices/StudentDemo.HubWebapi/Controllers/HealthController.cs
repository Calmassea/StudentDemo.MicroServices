using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.HubWebapi.Controllers
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
            Console.WriteLine("This api is health!");
            return Ok();
        }
    }
}
