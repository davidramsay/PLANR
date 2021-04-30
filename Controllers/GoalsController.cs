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
    public class GoalsController : Controller
    {
        private readonly TaskTrackerContext _context;

        public GoalsController(TaskTrackerContext context)
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
        // GET: Goals
        public async Task<IActionResult> Index()
        {
            var user = GetUser();
            int userId = user.UserId;
            var TaskTrackerContext = (from g in _context.Goals
                                      join c in _context.Categories
                                      on g.Categoryid equals c.Categoryid 
                                      join u in _context.Users
                                      on c.UserId equals u.UserId
                                      where u.UserId == userId
                                      select g).ToListAsync();
            return View(await TaskTrackerContext);
        }

        // GET: Goals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goal = await _context.Goals
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.Goalid == id);
            if (goal == null)
            {
                return NotFound();
            }

            return View(goal);
        }

        // GET: Goals/Create
        public IActionResult Create()
        {
            var user = GetUser();
            int userId = user.UserId;
            var TaskTrackerContext = (from c in _context.Categories
                                      join u in _context.Users
                                      on c.UserId equals u.UserId
                                      where u.UserId == userId
                                      select c);
            ViewData["Categories"] = new SelectList(TaskTrackerContext, "Categoryid", "CategoryAbbreviation");
            return View();
        }

        // POST: Goals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Goalid,Categoryid,GoalName,GoalDate")] Goal goal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(goal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "CategoryAbbreviation", goal.Categoryid);
            return View(goal);
        }

        // GET: Goals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
            {
                return NotFound();
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "CategoryAbbreviation", goal.Categoryid);
            return View(goal);
        }

        // POST: Goals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Goalid,Categoryid,GoalName,GoalDate")] Goal goal)
        {
            if (id != goal.Goalid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoalExists(goal.Goalid))
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
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "CategoryAbbreviation", goal.Categoryid);
            return View(goal);
        }

        // GET: Goals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goal = await _context.Goals
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.Goalid == id);
            if (goal == null)
            {
                return NotFound();
            }

            return View(goal);
        }

        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goal = await _context.Goals.FindAsync(id);
            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoalExists(int id)
        {
            return _context.Goals.Any(e => e.Goalid == id);
        }
    }
}
