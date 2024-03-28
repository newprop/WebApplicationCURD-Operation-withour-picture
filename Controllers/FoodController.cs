using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FoodController : Controller
    {
        private readonly foodContext _context;
        public FoodController(foodContext context)
        {
            _context = context;
        }
        //get:food
        public async Task<IActionResult> Index()
        {
            var data = await _context.foods.Include(i => i.Items).ToListAsync();

            ViewBag.Count = data.Count;
            ViewBag.GrandTotal = data.Sum(i => i.Items.Sum(l => l.ItemTotal));

            ViewBag.Average = data.Count > 0 ? data.Average(i => i.Items.Sum(l => l.ItemTotal)) : 0;


            return View(data);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var food = await _context.foods.Include(i => i.Items).FirstOrDefaultAsync(m => m.ID == id);
            if (food == null)
            {
                return NotFound();


            }


            return View(food);
        }  
        public IActionResult Create()
        {
            return View(new food());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(" foodName,foodDescription,foodCode, Items")] food food, string command = "")
        {
            if (command == "Add")
            {
                food.Items.Add(new());
                return View(food);
            }
            else if (command.Contains("delete"))// delete-3-sdsd-5   ["delete", "3"]
            {
                int idx = int.Parse(command.Split('-')[1]);

                food.Items.RemoveAt(idx);
                return View(food);
            }

            if (ModelState.IsValid)
            {
                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(food);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.foods.Include(i => i.Items).FirstOrDefaultAsync(i => i.ID == id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvoiceDate,CustomerName,Address,ContactNo, Items")] food food, string command = "")
        {
            if (command == "Add")
            {
                food.Items.Add(new());
                return View(food);
            }
            else if (command.Contains("delete"))// delete-3-sdsd-5   ["delete", "3"]
            {
                int idx = int.Parse(command.Split('-')[1]);

                food.Items.RemoveAt(idx);
                return View(food);
            }
            if (id != food.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!foodExists(food.ID))
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
            return View(food);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.foods.Include(i => i.Items)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var invoice = await _context.Invoices.FindAsync(id);
            //if (invoice != null)
            //{             

            //    //_context.Invoices.Remove(invoice);
            //}
           // await _context.Database.EnsureDeletedAsync($"exec spDeletefood {id}");

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool foodExists(int id)
        {
            return _context.foods.Any(e => e.ID == id);
        }
    }
}
