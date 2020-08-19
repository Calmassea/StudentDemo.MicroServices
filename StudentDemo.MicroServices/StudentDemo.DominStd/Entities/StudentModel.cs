using System.ComponentModel.DataAnnotations;
namespace StudentDemo.DominStd.Entities
{
    /// <summary>
    /// 学生信息数据模型
    /// </summary>
    public class StudentModel
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Profession { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public string SClass { get; set; }
    }
}
