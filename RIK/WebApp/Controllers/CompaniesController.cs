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
    public class CompaniesController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public CompaniesController(IAppUnitOfWork uow)
        {
            _uow = uow;

        }

        // GET: Companies
        public IActionResult Index(Guid id)
        {
            return RedirectToAction("Create", "Home", new {id});
        }


        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _uow.Company.FirstOrDefaultAsync(id.Value);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(company);

            _uow.Company.Update(company);
            await _uow.SaveChangesAsync();

            return RedirectToAction("Create", "Home", new {id = company.EventId});
        }


        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.Company.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
