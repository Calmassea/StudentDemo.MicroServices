using Microsoft.EntityFrameworkCore;
using StudentDemo.DominStd.Entities;
using System;

namespace StudentDemo.HubWebapi.Model
{
    /// <summary>
    /// 数据初始化
    /// </summary>
    public static class InitializeDbcontext
    {
        /// <summary>
        /// 加载种子数据
        /// </summary>
        public static void Seed(this ModelBuilder modelBuilder)
        {
            Random random = new Random();
            int sclass = random.Next(1, 9) + 1;
            int key = 1;
            modelBuilder.Entity<StudentModel>(o =>
            {
                o.ToTable("students");
                o.HasData(
                        new StudentModel { ID = key++, Name = "黎明", Age = 20, Profession = "软件工程", SClass = $"软件190{sclass}" },
                        new StudentModel { ID = key++, Name = "小红", Age = 21, Profession = "软件工程", SClass = $"软件180{sclass}" },
                        new StudentModel { ID = key++, Name = "黎明", Age = 19, Profession = "软件工程", SClass = $"软件190{sclass}" },
                        new StudentModel { ID = key++, Name = "小明", Age = 20, Profession = "软件工程", SClass = $"软件190{sclass}" },
                        new StudentModel { ID = key++, Name = "小黑", Age = 22, Profession = "网络工程", SClass = $"软件190{sclass}" },
                        new StudentModel { ID = key++, Name = "黎明", Age = 18, Profession = "网络工程", SClass = $"软件190{sclass}" },
                        new StudentModel { ID = key++, Name = "张三", Age = 21, Profession = "网络工程", SClass = $"软件170{sclass}" },
                        new StudentModel { ID = key++, Name = "黎明", Age = 21, Profession = "软件工程", SClass = $"软件190{sclass}" },
                        new StudentModel { ID = key++, Name = "李四", Age = 24, Profession = "软件工程", SClass = $"软件190{sclass}" },
                        new StudentModel { ID = key++, Name = "王五", Age = 15, Profession = "软件工程", SClass = $"软件190{sclass}" },
                        new StudentModel { ID = key++, Name = "孙八", Age = 20, Profession = "软件工程", SClass = $"软件190{sclass}" },
                        new StudentModel { ID = key, Name = "小六", Age = 20, Profession = "软件工程", SClass = $"软件190{sclass}" }
                );
            });
        }
    }
}
