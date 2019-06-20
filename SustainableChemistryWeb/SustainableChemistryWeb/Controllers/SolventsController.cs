using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SustainableChemistryWeb.Controllers
{
    public class SolventsController : Controller
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly SustainableChemistryWeb.Models.SustainableChemistryContext _context;

        public SolventsController(SustainableChemistryWeb.Models.SustainableChemistryContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Solvents
        public ActionResult Index()
        {
            return View(_context.AppSolvent.ToList());
        }

        // GET: Solvents/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Solvents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Solvents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Solvents/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Solvents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Solvents/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Solvents/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}