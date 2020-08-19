using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentDemo.DominStd.Entities;
using StudentDemo.DominStd.Enums;
using StudentDemo.DominStd.Response;
using StudentDemo.ScoreWebClient.services;
using StudentDemo.ScoreWebClient.ViewModels;

namespace StudentDemo.ScoreWebClient.Controllers
{
    [Route("StudentScore")]
    public class ScoreController : Controller
    {
        private readonly IScoreService _studentService;

        public ScoreController(IScoreService studentService)
        {
            _studentService = studentService;
        }
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Index()
        {
            var result = await _studentService.GetAllAsync();
            if (result.Code == ServiceResultCode.Unauthorized)
            {
                RedirectToAction();
            }
            var studentViewModel = new StudentScoreViewModel
            {
                studentScores = result.Entities
            };
            return View(studentViewModel);
        }
        [HttpGet]
        [Route("Details/{id}")]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _studentService.GetStudentAsync(id);
            var studentViewModel = new StudentScoreViewModel
            {
                studentScore = result.Entities
            };
            return View(studentViewModel);
        }
        [HttpGet]
        [Route("AddScore")]
        [Authorize(Roles = "管理员")]
        public IActionResult AddScore()
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
        public async Task<ServiceResultOfT<IEnumerable<ScoreModel>>> AllAsync()
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
        public async Task<ServiceResultOfT<ScoreModel>> Add(StudentScoreViewModel model)
        {
            var student = model.studentScore;
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
        public async Task<ServiceResultOfT<IEnumerable<ScoreModel>>> Delete(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            return result;
        }
        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = "管理员")]
        [ValidateAntiForgeryToken]
        public async Task<ServiceResultOfT<ScoreModel>> Edit(StudentScoreViewModel model)
        {
            var student = model.studentScore;
            var result = await _studentService.EditStudentAsync(student);
            return result;
        }
    }
}
