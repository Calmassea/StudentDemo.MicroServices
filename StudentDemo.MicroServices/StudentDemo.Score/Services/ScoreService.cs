using StudentDemo.ScoreWebapi.Models;
using StudentDemo.Tools.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.ScoreWebapi.Services
{
    /// <summary>
    /// 分数服务类
    /// </summary>
    public class ScoreService : CommonService<ScoreDbContext>
    {
        public ScoreService(ScoreDbContext dbcontext) : base(dbcontext)
        {
        }
    }
}
