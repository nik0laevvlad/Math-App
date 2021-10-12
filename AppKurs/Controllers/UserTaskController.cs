using AppKurs.CloudStorage;
using AppKurs.Data;
using AppKurs.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AppKurs.Controllers
{
    public class UserTaskController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ICloudStorage _cloudStorage;

        public UserTaskController(ApplicationDbContext db, ICloudStorage cloudStorage)
        {
            _db = db;
            _cloudStorage = cloudStorage;
        }

        private async Task UploadFile(UserTask userTask)
        {
            string fileNameForStorage = FormFileName(userTask.TaskTitle, userTask.ImageFile.FileName);
            userTask.ImageUrl = await _cloudStorage.UploadFileAsync(userTask.ImageFile, fileNameForStorage);
            userTask.ImageStorageName = fileNameForStorage;
        }

        private static string FormFileName(string title, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{title}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            return fileNameForStorage;
        }

        public IActionResult Index() => View(_db.UserTasks.ToList());

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] UserTask model, UserTask userTask)
        {
            UserTask uTask = new UserTask
            {
                TaskTitle = model.TaskTitle,
                TaskText = model.TaskText,
                TaskAnswer = model.TaskAnswer,
                TaskUser = User.Identity.Name,
                TaskTopic = model.TaskTopic,
            };
            
            if(userTask.ImageFile != null)
            {
                await UploadFile(userTask);
            }

            _db.UserTasks.Add(uTask);
            _db.UserTasks.Add(userTask);
            _db.SaveChanges();

            return Redirect("/Home/Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var currentUser = await _db.ApplicationUsers
                .FirstOrDefaultAsync(m => m.UserName == User.Identity.Name);
            var usertask = await _db.UserTasks
                .FirstOrDefaultAsync(m => m.Id == id);

            if (currentUser == null)
            {
                return View(new SolvedViewModel { UserTasks = usertask });
            }

            SolvedTask sTask = new SolvedTask
            {
                TaskId = (int)id,
                UserId = currentUser.Id,
                Solved = false,
            };

            if (id == null)
            {
                return NotFound();
            }

            var currentTask = await _db.SolvedTasks
                .FirstOrDefaultAsync(t =>
                    t.UserId == currentUser.Id &&
                    t.TaskId == id);

            if (currentTask != null)
            {
                return View(new SolvedViewModel { UserTasks = usertask, SolvedTasks = currentTask });
            }
            else
            {
                _db.SolvedTasks.Add(sTask);
                _db.SaveChanges();
            }

            if (usertask == null)
            {
                return NotFound();
            }
            
            return View(new SolvedViewModel { UserTasks = usertask, SolvedTasks = sTask });
        }

        [HttpPost]
        public async Task<IActionResult> DetailsAsync(int? id, [FromForm] SolvedTask model)
        {
            var currentUser = await _db.ApplicationUsers
                .FirstOrDefaultAsync(m => m.UserName == User.Identity.Name);
            var currentTask = await _db.SolvedTasks
                .FirstOrDefaultAsync(t =>
                    t.UserId == currentUser.Id &&
                    t.TaskId == id);
            var thisAnswer = await _db.UserTasks
                .FirstOrDefaultAsync(t => t.Id == id);

            currentTask.UserAnswer = model.UserAnswer;
            _db.SolvedTasks.Update(currentTask);
            if(currentTask.UserAnswer == thisAnswer.TaskAnswer) currentTask.Solved = true;
            _db.SaveChanges();

            return RedirectToAction("Details");
        }
    }
}
