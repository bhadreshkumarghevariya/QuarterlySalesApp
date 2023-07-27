using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuarterlySalesApp.Data;
using QuarterlySalesApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuarterlySalesApp.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sales
        public async Task<IActionResult> Index(string filterByEmployee, string filterByYear, string filterByQuarter)
        {
            var sales = from s in _context.Sales.Include(s => s.Employee)
                        select s;

            if (!string.IsNullOrEmpty(filterByEmployee))
            {
                sales = sales.Where(s => s.Employee.FirstName.Contains(filterByEmployee));
            }

            if (!string.IsNullOrEmpty(filterByYear))
            {
                int year = int.Parse(filterByYear);
                sales = sales.Where(s => s.Year == year);
            }

            if (!string.IsNullOrEmpty(filterByQuarter))
            {
                int quarter = int.Parse(filterByQuarter);
                sales = sales.Where(s => s.Quarter == quarter);
            }

            return View(await sales.ToListAsync());
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleId,Quarter,Year,Amount,EmployeeId")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sale);
        }
    }
}
