using System.ComponentModel.DataAnnotations;

namespace StudentDemo.DominStd.Entities
{
    /// <summary>
    /// 学生分数数据模型
    /// </summary>
    public class ScoreModel
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 学生id
        /// </summary>
        public int StudentId { get; set; }
        /// <summary>
        /// 语文
        /// </summary>
        public int Chinese { get; set; }
        /// <summary>
        /// 数学
        /// </summary>
        public int Maths { get; set; }
        /// <summary>
        /// 英语
        /// </summary>
        public int English { get; set; }
    }
}
