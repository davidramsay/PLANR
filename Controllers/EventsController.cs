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


namespace PLANR.Controllers
{
    public class EventsController : Controller
    {
        private readonly TaskTrackerContext _context;

        public EventsController(TaskTrackerContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var TaskTrackerContext = _context.Events.Include(e => e.Category);
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
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "CategoryAbbreviation");
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
