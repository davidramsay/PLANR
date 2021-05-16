using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLANR.Data;
using PLANR.Models;

namespace PLANR.Controllers
{
    public class RecordsController : Controller
    {
        private readonly TaskTrackerContext _context;

        public RecordsController(TaskTrackerContext context)
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

        // GET: Records
        public async Task<IActionResult> Index()
        {
            var user = GetUser();
            int userId = user.UserId;
            var contextrecords = (from t in _context.Records
                                join o in _context.Objectives
                                on t.Objectiveid equals o.Objectiveid
                                join g in _context.Goals
                                on o.Goalid equals g.Goalid
                                join c in _context.Categories
                                on g.Categoryid equals c.Categoryid
                                join u in _context.Users
                                on c.UserId equals u.UserId
                                where u.UserId == userId
                                select t).ToListAsync();
            return View(await contextrecords);
        }

        // GET: Records/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records
                .Include(r => r.Objective)
                .FirstOrDefaultAsync(m => m.Recordid == id);
            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        // GET: Records/Create
        public IActionResult Create()
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
                                      select o).ToList();
            ViewData["Objectiveid"] = new SelectList(TaskTrackerContext, "Objectiveid", "ObjectiveName");
            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Recordid,Objectiveid,RecordDate,MetricData")] Record record)
        {
            if (ModelState.IsValid)
            {
                _context.Add(record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Objectiveid"] = new SelectList(_context.Objectives, "Objectiveid", "MetricName", record.Objectiveid);
            return View(record);
        }

        // GET: Records/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @record = await _context.Records.FindAsync(id);
            if (@record == null)
            {
                return NotFound();
            }
            ViewData["Objectiveid"] = new SelectList(_context.Objectives, "Objectiveid", "MetricName", @record.Objectiveid);
            return View(@record);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Recordid,Objectiveid,RecordDate,MetricData")] Record @record)
        {
            if (id != @record.Recordid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@record);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordExists(@record.Recordid))
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
            ViewData["Objectiveid"] = new SelectList(_context.Objectives, "Objectiveid", "MetricName", @record.Objectiveid);
            return View(@record);
        }

        // GET: Records/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @record = await _context.Records
                .Include(r => r.Objective)
                .FirstOrDefaultAsync(m => m.Recordid == id);
            if (@record == null)
            {
                return NotFound();
            }

            return View(@record);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @record = await _context.Records.FindAsync(id);
            _context.Records.Remove(@record);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordExists(int id)
        {
            return _context.Records.Any(e => e.Recordid == id);
        }
    }
}
