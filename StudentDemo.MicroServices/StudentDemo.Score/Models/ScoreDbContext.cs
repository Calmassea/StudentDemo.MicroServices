using Microsoft.EntityFrameworkCore;
using StudentDemo.DominStd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.ScoreWebapi.Models
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class ScoreDbContext:DbContext
    {
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="options"></param>
        public ScoreDbContext(DbContextOptions<ScoreDbContext> options):base(options)
        {

        }
        private Random random = new Random();
        /// <summary>
        /// 学生成绩表
        /// </summary>
        DbSet<ScoreModel> scores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ScoreModel>(opt=> {
                
                int sclass = random.Next(1, 9) + 1;
                int key = 1;
                int skey = 1;
                opt.HasData(
                        new ScoreModel { Id = key++, StudentId = skey++, Chinese = RandomScore(), Maths = RandomScore(), English = RandomScore() },
                        new ScoreModel { Id = key++, StudentId = skey++, Chinese = RandomScore(), Maths = RandomScore(), English = RandomScore() },
                        new ScoreModel { Id = key++, StudentId = skey++, Chinese = RandomScore(), Maths = RandomScore(), English = RandomScore() },
                        new ScoreModel { Id = key++, StudentId = skey++, Chinese = RandomScore(), Maths = RandomScore(), English = RandomScore() },
                        new ScoreModel { Id = key++, StudentId = skey++, Chinese = RandomScore(), Maths = RandomScore(), English = RandomScore() },
                        new ScoreModel { Id = key++, StudentId = skey++, Chinese = RandomScore(), Maths = RandomScore(), English = RandomScore() },
                        new ScoreModel { Id = key++, StudentId = skey++, Chinese = RandomScore(), Maths = RandomScore(), English = RandomScore() },
                        new ScoreModel { Id = key++, StudentId = skey++, Chinese = RandomScore(), Maths = RandomScore(), English = RandomScore() },
                        new ScoreModel { Id = key++, StudentId = skey++, Chinese = RandomScore(), Maths = RandomScore(), English = RandomScore() },
                        new ScoreModel { Id = key++, StudentId = skey++, Chinese = RandomScore(), Maths = RandomScore(), English = RandomScore() },
                        new ScoreModel { Id = key++, StudentId = skey++, Chinese = RandomScore(), Maths = RandomScore(), English = RandomScore() },
                        new ScoreModel { Id = key++, StudentId = skey, Chinese = RandomScore(), Maths = RandomScore(), English = RandomScore() }
                    );
            });
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptions)
        {
            base.OnConfiguring(dbContextOptions);
            dbContextOptions.EnableSensitiveDataLogging();
        }
        /// <summary>
        /// 随机函数
        /// </summary>
        /// <returns></returns>
        public int RandomScore() =>  random.Next(20,150)+1;
    }
}
