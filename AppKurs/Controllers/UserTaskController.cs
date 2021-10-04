using AppKurs.Data;
using AppKurs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppKurs.Controllers
{
    public class UserTaskController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserTaskController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index() => View(_db.UserTasks.ToList());

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] UserTask model)
        {
            UserTask uTask = new UserTask
            {
                TaskTitle = model.TaskTitle,
                TaskText = model.TaskText,
                TaskAnswer = model.TaskAnswer,
                TaskUser = User.Identity.Name,
                TaskTopic = model.TaskTopic
            };

            _db.UserTasks.Add(uTask);
            _db.SaveChanges();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var currentUser = await _db.ApplicationUsers
                .FirstOrDefaultAsync(m => m.UserName == User.Identity.Name);
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

            var usertask = await _db.UserTasks
                .FirstOrDefaultAsync(m => m.Id == id);

            var solved = await _db.SolvedTasks
                .FirstOrDefaultAsync(t =>
                    t.UserId == currentUser.Id &&
                    t.TaskId == id);

            if (solved != null)
            {
                return View(new SolvedViewModel { UserTasks = usertask, SolvedTasks = sTask });
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
        public async Task<IActionResult> DetailsAsync(int? id, [FromForm] SolvedTask model, UserTask userTask)
        {
            var currentUser = await _db.ApplicationUsers
                .FirstOrDefaultAsync(m => m.UserName == User.Identity.Name);
            var thisTask = await _db.SolvedTasks
                .FirstOrDefaultAsync(t =>
                    t.UserId == currentUser.Id &&
                    t.TaskId == id);

            thisTask.UserAnswer = model.UserAnswer;
            _db.SolvedTasks.Update(thisTask);
            _db.SaveChanges();

            return View(new SolvedViewModel { SolvedTasks = model, UserTasks = userTask });
        }
    }
}
