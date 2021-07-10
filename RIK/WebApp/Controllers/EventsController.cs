using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App;
using DAL.App.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;

namespace WebApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public EventsController(IAppUnitOfWork uow)
        {
            _uow = uow;

        }

        // GET: Events
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");

        }


        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            if (!ModelState.IsValid) return View(@event);

            if (@event.EventDate < DateTime.Today)
            {
                return View(@event);
            }
            _uow.Event.Add(@event);
            await _uow.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _uow.Event.FirstOrDefaultAsync(id.Value);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(@event);

            _uow.Event.Update(@event);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.Event.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
