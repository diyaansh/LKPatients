using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LKPatients.Models;

namespace LKPatients.Controllers
{
    public class LKMedicationTypeController : Controller
    {
        private readonly PatientsContext _context;

        public LKMedicationTypeController(PatientsContext context)
        {
            _context = context;
        }

        // GET: LKMedicationType
        public async Task<IActionResult> Index()
        {
            return _context.MedicationType != null ?
                        View(await _context.MedicationType.OrderBy(m => m.Name).ToListAsync()) :
                        Problem("Entity set 'PatientsContext.MedicationType'  is null.");
        }

        // GET: LKMedicationType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MedicationType == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationType
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }

        // GET: LKMedicationType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LKMedicationTypes/Create
        //post method -- create data by passing object details
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicationType);
        }

        // GET: LKMedicationTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MedicationType == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationType.FindAsync(id);
            if (medicationType == null)
            {
                return NotFound();
            }
            return View(medicationType);
        }

        // POST: LKMedicationTypes/Edit/5
        //edit Medication type data  by passing object
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (id != medicationType.MedicationTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationTypeExists(medicationType.MedicationTypeId))
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
            return View(medicationType);
        }

        // GET: LKMedicationTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MedicationType == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationType
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }

        // POST: LKMedicationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MedicationType == null)
            {
                return Problem("Entity set 'PatientsContext.MedicationType'  is null.");
            }
            var medicationType = await _context.MedicationType.FindAsync(id);
            if (medicationType != null)
            {
                _context.MedicationType.Remove(medicationType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationTypeExists(int id)
        {
            return (_context.MedicationType?.Any(e => e.MedicationTypeId == id)).GetValueOrDefault();
        }
    }
}
