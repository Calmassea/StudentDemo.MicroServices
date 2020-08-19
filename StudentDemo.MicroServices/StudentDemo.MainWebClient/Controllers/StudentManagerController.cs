using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentDemo.DataDomin.DataUnitTest;
using StudentDemo.DominStd.Entities;
using StudentDemo.DominStd.Enums;
using StudentDemo.DominStd.Response;
using StudentDemo.MainWebClient.Models;
using StudentDemo.MainWebClient.Services;
using StudentDemo.MainWebClient.ViewModels;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace StudentDemo.MainWebClient.Controllers
{
    [Route("StudentManager")]
    public class StudentManagerController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IDistributedCache _cache;

        public StudentManagerController(IStudentService studentService, IDistributedCache cache)
        {
            _studentService = studentService;
            _cache = cache;
        }
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Index()
        {
            string key = "StudentHome_";
            var context = new ServiceResultOfT<IEnumerable<StudentModel>>();
            var result = _cache.GetStringAsync(key).Result;
            if (result==null)
            {
                context = await _studentService.GetAllAsync();
                await _cache.SetStringAsync(key,JsonConvert.SerializeObject(context),new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(20)));
            }
            else
            {
                context = JsonConvert.DeserializeObject<ServiceResultOfT<IEnumerable<StudentModel>>>(result) ;
            }
            if (context.Code == ServiceResultCode.Unauthorized)
            {
                RedirectToAction();
            }
            var studentViewModel = new StudentViewModel
            {
                Students = context.Entities
            };
            return View(studentViewModel);
        }
        [HttpGet]
        [Route("Details/{id}")]
        [Authorize(Roles ="管理员")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _studentService.GetStudentAsync(id);
            var studentViewModel = new StudentViewModel
            {
                Student = result.Entities
            };
            return View(studentViewModel);
        }
        [HttpGet]
        [Route("AddStudent")]
        [Authorize(Roles = "管理员")]
        public IActionResult AddStudent()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        [Authorize(Roles = "管理员")]
        public async Task<ServiceResultOfT<IEnumerable<StudentModel>>> AllAsync()
        {
            var result = await _studentService.GetAllAsync();
            return result;
        }
        /// <summary>
        /// 添加一名学生
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "管理员")]
        [ValidateAntiForgeryToken]
        public async Task<ServiceResultOfT<StudentModel>> Add(StudentViewModel model) {
            var student = model.Student;
            var result = await _studentService.AddStudentAsync(student);
            return result;
        }
        /// <summary>
        /// 删除一名学生
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Delete/{id}")]
        [Authorize(Roles = "管理员")]
        public async Task<ServiceResultOfT<IEnumerable<StudentModel>>> Delete(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            return result;
        }
        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles ="管理员")]
        [ValidateAntiForgeryToken]
        public async Task<ServiceResultOfT<StudentModel>> Edit(StudentViewModel model)
        {
            var student =model.Student;
            var result = await _studentService.EditStudentAsync(student);
            return result;
        }
    }
}
