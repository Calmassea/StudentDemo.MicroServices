using StudentDemo.DominStd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.ScoreWebClient.ViewModels
{
    /// <summary>
    /// 学生成绩视图模型
    /// </summary>
    public class StudentScoreViewModel
    {
        /// <summary>
        /// 学生成绩列表
        /// </summary>
        public IEnumerable<ScoreModel> studentScores { get; set; }
        public ScoreModel studentScore { get; set; } = default;
    }
}
