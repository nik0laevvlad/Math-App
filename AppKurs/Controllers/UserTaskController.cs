using AppKurs.CloudStorage;
using AppKurs.Data;
using AppKurs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] UserTask model)
        {
            UserTask uTask = new UserTask
            {
                TaskTitle = model.TaskTitle,
                TaskText = model.TaskText,
                TaskAnswer = model.TaskAnswer,
                TaskUser = User.Identity.Name,
                TaskTopic = model.TaskTopic,
                ImageFile = model.ImageFile
            };
            
            if(uTask.ImageFile != null)
            {
                await UploadFile(uTask);
            }

            _db.UserTasks.Add(uTask);
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

        [HttpPost, Route("UserTask/DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userTask = await _db.UserTasks.FindAsync(id);

            if(userTask.ImageStorageName != null)
            {
                await _cloudStorage.DeleteFileAsync(userTask.ImageStorageName);
            }

            _db.UserTasks.Remove(userTask);
            await _db.SaveChangesAsync();
            return Redirect("/Home/Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var userTask = await _db.UserTasks.FindAsync(id);
            if(userTask == null)
            {
                return NotFound();
            }
            return View(userTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, [Bind("Id,TaskTitle,TaskTopic,TaskText,ImageUrl,ImageFile,ImageStorageName")] UserTask userTask)
        {
            if(id != userTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(userTask.ImageFile != null)
                    {
                        if(userTask.ImageStorageName != null)
                        {
                            await _cloudStorage.DeleteFileAsync(userTask.ImageStorageName);
                        }

                        await UploadFile(userTask);
                    }

                    userTask.TaskUser = userTask.TaskUser;
                    _db.Update(userTask);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTaskExists(userTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userTask);
        }

        private bool UserTaskExists(int id)
        {
            return _db.UserTasks.Any(e => e.Id == id);
        }
    }
}
