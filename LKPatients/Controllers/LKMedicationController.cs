using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LKPatients.Models;

namespace LKPatients.Controllers
{
    public class LKMedicationController : Controller
    {
            private readonly PatientsContext _context;

            public LKMedicationController(PatientsContext context)
            {
                _context = context;
            }

        // GET: LKMedications
        public async Task<IActionResult> Index(string medicationTypeId)
        {
            if (medicationTypeId != null)
            {
                Response.Cookies.Append("MedicationTypeId", medicationTypeId);
                HttpContext.Session.SetString("MedicationTypeId", medicationTypeId);
            }
            else if (Request.Query["MedicationTypeId"].Any())
            {
                medicationTypeId = Request.Query["MedicationTypeId"].ToString();
                Response.Cookies.Append("MedicationTypeId", medicationTypeId);
                HttpContext.Session.SetString("MedicationTypeId", medicationTypeId);
            }
            else if (Request.Cookies["MedicationTypeId"] != null)
            {
                medicationTypeId = Request.Cookies["MedicationTypeId"].ToString();
            }
            else if (HttpContext.Session.GetString("MedicationTypeId") != null)
            {
                medicationTypeId = HttpContext.Session.GetString("MedicationTypeId");
            }
            else
            {
                TempData["message"] = "Please select medication type";
                return RedirectToAction("Index", "LKMedicationTypes");
            }

            ViewData["MedicationTypeName"] = HttpContext.Session.GetString("MedicationTypeName");
            ViewData["MedicationTypeId"] = HttpContext.Session.GetString("MedicationTypeId");

            var patientsContext = _context.Medication.Include(m => m.ConcentrationCodeNavigation).Include(m => m.DispensingCodeNavigation).Include(m => m.MedicationType)
            .Where(m => m.MedicationTypeId == Convert.ToInt32(medicationTypeId))
              .OrderBy(m => m.Name).ThenBy(m => m.Concentration);
            return View(await patientsContext.ToListAsync());
        }

        // GET: LKMedication/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["MedicationTypeName"] = HttpContext.Session.GetString("MedicationTypeName");
            var medication = await _context.Medication
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // GET: LKMedication/Create
        // Medication create view
        public IActionResult Create()
        {
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit.OrderBy(x => x.ConcentrationCode), "ConcentrationCode", "ConcentrationCode");
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit.OrderBy(x => x.DispensingCode), "DispensingCode", "DispensingCode");
            ViewData["MedicationTypeId"] = HttpContext.Session.GetString("MedicationTypeId");
            ViewData["MedicationTypeName"] = HttpContext.Session.GetString("MedicationTypeName");
            return View();
        }

        // POST: LKMedication/Create
        // post method -- create data by passing object details
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            if (ModelState.IsValid)
            {
                var ExistMedicationContext = _context.Medication
               .Where(m => m.Name == medication.Name)
               .Where(m => m.Concentration == medication.Concentration)
               .Where(m => m.ConcentrationCode == medication.ConcentrationCode).FirstOrDefault();
                if (ExistMedicationContext != null)
                {
                    ViewData["Error-message"] = "Medication name,Concentration and ConcentrationCode already exists.";
                }
                else
                {
                    _context.Add(medication);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit.OrderBy(x => x.ConcentrationCode), "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit.OrderBy(x => x.DispensingCode), "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = HttpContext.Session.GetString("MedicationTypeId");
            return View(medication);
        }

        // GET: LKMedication/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Medication == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }

            ViewData["MedicationTypeName"] = HttpContext.Session.GetString("MedicationTypeName");
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit.OrderBy(d => d.ConcentrationCode), "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit.OrderBy(d => d.DispensingCode), "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.Set<MedicationType>(), "MedicationTypeId", "MedicationTypeId", medication.MedicationTypeId);
            return View(medication);
        }

        // POST: LKMedication/Edit/5
        // Edit Medication types data  by passing object
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            if (id != medication.Din)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationExists(medication.Din))
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
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit.OrderBy(d => d.ConcentrationCode), "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit.OrderBy(d => d.DispensingCode), "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.Set<MedicationType>(), "MedicationTypeId", "MedicationTypeId", medication.MedicationTypeId);
            return View(medication);
        }

        // GET: LKMedication/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["MedicationTypeName"] = HttpContext.Session.GetString("MedicationTypeName");

            var medication = await _context.Medication
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // POST: LKMedication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Medication == null)
            {
                return Problem("Entity set 'LKPatientContext.Medication'  is null.");
            }
            var medication = await _context.Medication.FindAsync(id);
            if (medication != null)
            {
                _context.Medication.Remove(medication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationExists(string id)
        {
            return (_context.Medication?.Any(e => e.Din == id)).GetValueOrDefault();
        }
    }
}
