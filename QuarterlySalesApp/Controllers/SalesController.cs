using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var sales = from s in _context.Sales
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                sales = sales.Where(s => s.Employee.FirstName.Contains(searchString)
                                       || s.Employee.LastName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    sales = sales.OrderByDescending(s => s.Employee.FirstName);
                    break;
                case "Date":
                    sales = sales.OrderBy(s => s.Year);
                    break;
                case "date_desc":
                    sales = sales.OrderByDescending(s => s.Year);
                    break;
                default:
                    sales = sales.OrderBy(s => s.Employee.FirstName);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Sale>.CreateAsync(sales.AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        // GET: Sales/Create
        // GET: Sales/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FullName");
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
