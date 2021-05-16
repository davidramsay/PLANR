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
    public class EventsController : Controller
    {
        private readonly TaskTrackerContext _context;

        public EventsController(TaskTrackerContext context)
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
        // GET: Events
        public async Task<IActionResult> Index()
        {
            var user = GetUser();
            int userId = user.UserId;
            var TaskTrackerContext = (from e in _context.Events
                                      join c in _context.Categories
                                      on e.Categoryid equals c.Categoryid
                                      join u in _context.Users
                                      on c.UserId equals u.UserId
                                      where u.UserId == userId
                                      select e);
            return View(await TaskTrackerContext.ToListAsync());
        }
        public async Task<IActionResult> TodaysEvents()
        {
            var user = GetUser();
            int userId = user.UserId;
            var TaskTrackerContext = (from e in _context.Events
                                      join c in _context.Categories
                                      on e.Categoryid equals c.Categoryid
                                      join u in _context.Users
                                      on c.UserId equals u.UserId
                                      where u.UserId == userId
                                      select e).Where(t => t.EventStart.Day == DateTime.Today.Day).OrderBy(d => d.EventStart);
            return View(await TaskTrackerContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eevent = await _context.Events
                .Include(e => e.Category)
                .FirstOrDefaultAsync(m => m.Eventid == id);
            if (eevent == null)
            {
                return NotFound();
            }

            return View(eevent);
        }

        // GET: Events/Create
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

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Eventid,Categoryid,EventName,EventDesc,EventDate,EventStart,EventEnd")] Event eevent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eevent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "CategoryAbbreviation", eevent.Categoryid);
            return View(eevent);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eevent = await _context.Events.FindAsync(id);
            if (eevent == null)
            {
                return NotFound();
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "CategoryAbbreviation", eevent.Categoryid);
            return View(eevent);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Eventid,Categoryid,EventName,EventDesc,EventDate,EventStart,EventEnd")] Event eevent)
        {
            if (id != eevent.Eventid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eevent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eevent.Eventid))
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
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "CategoryAbbreviation", eevent.Categoryid);
            return View(eevent);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eevent = await _context.Events
                .Include(e => e.Category)
                .FirstOrDefaultAsync(m => m.Eventid == id);
            if (eevent == null)
            {
                return NotFound();
            }

            return View(eevent);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eevent = await _context.Events.FindAsync(id);
            _context.Events.Remove(eevent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Eventid == id);
        }
    }
}
