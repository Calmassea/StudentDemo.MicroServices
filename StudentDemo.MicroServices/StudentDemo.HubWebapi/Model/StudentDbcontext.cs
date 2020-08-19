using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using StudentDemo.DataDomin.DataUnitTest;
using StudentDemo.Domin.Shared;
using StudentDemo.DominStd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.HubWebapi.Model
{
    /// <summary>
    /// 上下文
    /// </summary>
    public class StudentDbcontext:DbContext
    {
        public StudentDbcontext(DbContextOptions<StudentDbcontext> options):base(options)
        {
           
        }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<StudentModel> students { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
