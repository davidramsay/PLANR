using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLANR;
using PLANR.Data;
using PLANR.Models;
using PLANR.Models.ViewModels;

namespace PLANR.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskTrackerContext _context;

        public TasksController(TaskTrackerContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var model = new DashBoardViewModel();
            model.Tasks = await _context.Tasks.Include(t => t.Objective).Where(t => t.TaskDueDate == System.DateTime.Today).ToListAsync();
            model.Events = await _context.Events.Where(t => t.EventStart.Day == DateTime.Today.Day).Include(e => e.Category).OrderBy(d => d.EventStart).ToListAsync();
            return View(model);
        }
        // GET: AllTasks
        public async Task<IActionResult> All()
        {
            var TaskTrackerContext = _context.Tasks.Include(t => t.Objective);
            return View(await TaskTrackerContext.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Objective)
                .FirstOrDefaultAsync(m => m.Taskid == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            ViewData["Objectiveid"] = new SelectList(_context.Objectives, "Objectiveid", "MetricName");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Taskid,TaskName,TaskDescription,Objectiveid,TaskDueDate")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Objectiveid"] = new SelectList(_context.Objectives, "Objectiveid", "MetricName", task.Objectiveid);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["Objectiveid"] = new SelectList(_context.Objectives, "Objectiveid", "MetricName", task.Objectiveid);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Taskid, TaskName, TaskDescription, Objectiveid, TaskDueDate, TaskStatus")] Models.Task task)
        {
            if (id != task.Taskid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Taskid))
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
            ViewData["Objectiveid"] = new SelectList(_context.Objectives, "Objectiveid", "MetricName", task.Objectiveid);
            return View(task);
        }

        // GET: Tasks/Migrate/5

        public async Task<IActionResult> Migrate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }
        [HttpPost]
        public ActionResult Migrate1(int Taskid)
        {
            var task = _context.Tasks.Find(Taskid);
            task.TaskDueDate = task.TaskDueDate.AddDays(1);
            _context.Update(task);
            _context.SaveChanges();
            return RedirectToAction("Migrate", new { id = Taskid });
        }
        [HttpPost]

        public ActionResult Migrate7(int Taskid)
        {
            var task = _context.Tasks.Find(Taskid);
            task.TaskDueDate = task.TaskDueDate.AddDays(7);
            _context.Update(task);
            _context.SaveChanges();
            return RedirectToAction("Migrate", new { id = Taskid });
        }
        [HttpPost]

        public ActionResult Migrate30(int Taskid)
        {
            var task = _context.Tasks.Find(Taskid);
            task.TaskDueDate = task.TaskDueDate.AddMonths(1);
            _context.Update(task);
            _context.SaveChanges();
            return RedirectToAction("Migrate", new { id = Taskid });
        }

        [HttpPost]
        public ActionResult MarkComplete(int Taskid)
        {
            var task = _context.Tasks.Find(Taskid);
            task.TaskStatus = true;
            _context.Update(task);
            _context.SaveChanges();
            return RedirectToAction("Migrate", new { id = Taskid });
        }
        [HttpPost]
        public ActionResult MarkToDo(int Taskid)
        {
            var task = _context.Tasks.Find(Taskid);
            task.TaskStatus = false;
            _context.Update(task);
            _context.SaveChanges();
            return RedirectToAction("Migrate", new { id = Taskid });
        }

        // POST: Tasks/Migrate/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Migrate(int id, [Bind("TaskDueDate")] Models.Task task)
        {
            if (id != task.Taskid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Taskid))
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
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Objective)
                .FirstOrDefaultAsync(m => m.Taskid == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Taskid == id);
        }
    }
}
