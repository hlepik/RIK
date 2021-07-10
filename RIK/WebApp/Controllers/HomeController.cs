using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App;
using DAL.App.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppUnitOfWork _uow;


        public HomeController(ILogger<HomeController> logger, IAppUnitOfWork uow)
        {
            _logger = logger;
            _uow = uow;
        }

        public async Task<IActionResult> Index()
        {
            var events = new AllEventsViewModel();
            events.ComingEvents = await _uow.Event.GetAllComingEvents();
            events.PreviousEvents = await _uow.Event.GetAllPreviousEvents();
            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid eventId)
        {

            var person = await _uow.Person.FirstOrDefaultAsync(id);
            var company = await _uow.Company.FirstOrDefaultAsync(id);

            if (person != null)
            {
                await _uow.Person.RemoveAsync(id);
            }
            else if (company != null)
            {

                await _uow.Company.RemoveAsync(id);
            }
            else
            {
                _uow.Person.RemoveAllPersonsAsync(id);
                _uow.Company.RemoveAllCompaniesAsync(id);
                await _uow.Event.RemoveAsync(id);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction("Create", "Home", new {id = eventId});

        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vm = new ParticipantsViewModel();
            var participants = _uow.Event.GetAllParticipants(id.Value);
            vm.Event = await _uow.Event.FirstOrDefaultAsync(id.Value);
            vm.Participants = participants;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParticipantsViewModel vm)
        {

            if (!ModelState.IsValid)
            {

                var participants = _uow.Event.GetAllParticipants(vm.Event!.Id);
                vm.Event = await _uow.Event.FirstOrDefaultAsync(vm.Event!.Id);
                vm.Participants = participants;

                return View(vm);
            }

            if (vm.Person != null)
            {
                vm.Person!.EventId = vm.Event!.Id;
                _uow.Person.Add(vm.Person);
            }
            else
            {
                vm.Company!.EventId = vm.Event!.Id;
                _uow.Company.Add(vm.Company);
            }

            await _uow.SaveChangesAsync();
            ModelState.Clear();
            var participant =  _uow.Event.GetAllParticipants(vm.Event!.Id);
            vm.Event = await _uow.Event.FirstOrDefaultAsync(vm.Event!.Id);
            vm.Participants = participant;
            vm.Person = null;
            vm.Company = null;
            return View(vm);
        }

        public async Task<IActionResult> Participants(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _uow.Person.FirstOrDefaultAsync(id.Value);

            if (person != null)
            {
                return RedirectToAction("Edit", "Persons",new {id=id});
            }
            var company = await _uow.Company.FirstOrDefaultAsync(id.Value);
            if(company != null)
            {
                return RedirectToAction("Edit", "Companies", new {id});
            }

            return NotFound();
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vm = new ParticipantsViewModel();
            var participants =  _uow.Event.GetAllParticipants(id.Value);
            vm.Event = await _uow.Event.FirstOrDefaultAsync(id.Value);
            vm.Participants = participants;

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}