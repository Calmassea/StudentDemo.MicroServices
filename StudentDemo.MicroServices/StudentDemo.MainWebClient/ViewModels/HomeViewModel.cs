using StudentDemo.DataDomin.DataUnitTest;
using StudentDemo.DominStd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.MainWebClient.ViewModels
{
    public class HomeViewModel
    {
      public  IEnumerable<StudentModel> Students { get; set; } 
    }
}
