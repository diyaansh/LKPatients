using LKPatients.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LKPatients.Controllers
{
    public class LKConcentrationUnitsController : Controller
    {
        private readonly PatientsContext _context;

        public LKConcentrationUnitsController(PatientsContext context)
        {
            _context = context;
        }

        // GET: LKConcentrationUnits
        // it is the main method which redirect to the view of the respective controller when its clicked on header tab
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConcentrationUnit.ToListAsync());
        }

        // GET: LKConcentrationUnits/Details/5
        //its a detail method which get the details of all concentration unit model and show it to list
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }

        // GET: LKConcentrationUnits/Create
        // it create a new concentration unit
        public IActionResult Create()
        {
            return View();
        }

        // POST: LKConcentrationUnits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // this is a save method it saves data when user click on submit 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concentrationUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concentrationUnit);
        }

        // GET: LKConcentrationUnits/Edit/5
        //it is a edit method it edits data from that id when user click on edit
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit.FindAsync(id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }
            return View(concentrationUnit);
        }

        // POST: LKConcentrationUnits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // it post data when user click on submit button and save the edits
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (id != concentrationUnit.ConcentrationCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concentrationUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcentrationUnitExists(concentrationUnit.ConcentrationCode))
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
            return View(concentrationUnit);
        }

        // GET: LKConcentrationUnits/Delete/5
        // its a delete method it will get infromation from id
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }

        // POST: LKConcentrationUnits/Delete/5
        // it delete data when user click on submit the data will be delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var concentrationUnit = await _context.ConcentrationUnit.FindAsync(id);
            _context.ConcentrationUnit.Remove(concentrationUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcentrationUnitExists(string id)
        {
            return _context.ConcentrationUnit.Any(e => e.ConcentrationCode == id);
        }
    }
}
