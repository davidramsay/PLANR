using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLANR;
using PLANR.Models;
using PLANR.Data;
using System.Security.Claims;

namespace PLANR.Controllers
{
    public class ObjectivesController : Controller
    {
        private readonly TaskTrackerContext _context;

        public ObjectivesController(TaskTrackerContext context)
        {
            _context = context;
        }
        public User GetUser()
        {
            string userToken = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var qresult = from user in _context.Users
                          where user.UserToken == userToken
                          select user;
            var result = qresult.FirstOrDefault();
            return result;
        }
        // GET: Objectives
        public async Task<IActionResult> Index()
        {
            var user = GetUser();
            int userId = user.UserId;
            var TaskTrackerContext = (from o in _context.Objectives
                                      join g in _context.Goals
                                      on o.Goalid equals g.Goalid
                                      join c in _context.Categories
                                      on g.Categoryid equals c.Categoryid
                                      join u in _context.Users
                                      on c.UserId equals u.UserId
                                      where u.UserId == userId
                                      select o).ToListAsync();
            return View(await TaskTrackerContext);
        }

        // GET: Objectives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objective = await _context.Objectives
                .Include(o => o.Goal)
                .FirstOrDefaultAsync(m => m.Objectiveid == id);
            if (objective == null)
            {
                return NotFound();
            }

            return View(objective);
        }

        // GET: Objectives/Create
        public IActionResult Create()
        {
            var user = GetUser();
            int userId = user.UserId;
            var objectivesGoalsList = (from g in _context.Goals
                                       join c in _context.Categories
                                       on g.Categoryid equals c.Categoryid
                                       join u in _context.Users
                                       on c.UserId equals u.UserId
                                       where u.UserId == userId
                                       select g).ToList();
            ViewData["Goalid"] = new SelectList(objectivesGoalsList, "Goalid", "GoalName");
            
            return View();
        }

        // POST: Objectives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ObjectiveName,Objectiveid,Goalid,MetricName,ObjectiveDueDate")] Objective objective)
        {
            if (ModelState.IsValid)
            {
                _context.Add(objective);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(objective);
        }

        // GET: Objectives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objective = await _context.Objectives.FindAsync(id);
            if (objective == null)
            {
                return NotFound();
            }
            var user = GetUser();
            int userId = user.UserId;
            var objectivesGoalsList = (from g in _context.Goals
                                       join c in _context.Categories
                                       on g.Categoryid equals c.Categoryid
                                       join u in _context.Users
                                       on c.UserId equals u.UserId
                                       where u.UserId == userId
                                       select g).ToList();
            ViewData["Goalid"] = new SelectList(objectivesGoalsList, "Goalid", "GoalName");
            return View(objective);
        }

        // POST: Objectives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ObjectiveName,Objectiveid,Goalid,MetricName,ObjectiveDueDate")] Objective objective)
        {
            if (id != objective.Objectiveid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(objective);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObjectiveExists(objective.Objectiveid))
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
            return View(objective);
        }

        // GET: Objectives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objective = await _context.Objectives
                .Include(o => o.Goal)
                .FirstOrDefaultAsync(m => m.Objectiveid == id);
            if (objective == null)
            {
                return NotFound();
            }

            return View(objective);
        }

        // POST: Objectives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var objective = await _context.Objectives.FindAsync(id);
            _context.Objectives.Remove(objective);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObjectiveExists(int id)
        {
            return _context.Objectives.Any(e => e.Objectiveid == id);
        }
    }
}
