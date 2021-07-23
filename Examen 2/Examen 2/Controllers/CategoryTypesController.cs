using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Examen_2.Data;
using Examen_2.Models;
using Examen_2.Common;

namespace Examen_2.Controllers
{
    public class CategoryTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly int recordsPerPage = 10;

        private Pag<CategoryType> paginationCategories;

        public CategoryTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CategoryTypes
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            int totalRecords = 0;

            if (search == null)
            {
                search = "";
            }

            totalRecords = await _context.CategoryTypes.CountAsync(
                c => c.CategoryName.Contains(search));

            var categorias = await _context.CategoryTypes
                .Where(c => c.CategoryName.Contains(search))
                .ToListAsync();

            var categoriasResult = categorias.OrderBy(o => o.CategoryName)
                                    .Skip((page - 1) * recordsPerPage)
                                    .Take(recordsPerPage);

            var totalPage = (int)Math.Ceiling((Double)totalRecords / recordsPerPage);

            paginationCategories = new Pag<CategoryType>()
            {
                RecordsPerPage = recordsPerPage,
                TotalRecords = totalRecords,
                TotalPage = totalPage,
                CurrentPage = page,
                Search = search,
                Result = categoriasResult   
            };

            var applicationDbContext = _context.CategoryTypes.Where(h => h.CategoryName.Contains(search)).Include(h => h.Homeworks);
            return View(paginationCategories);
        }

        // GET: CategoryTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryType = await _context.CategoryTypes
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (categoryType == null)
            {
                return NotFound();
            }

            return View(categoryType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] CategoryType categoryType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryType = await _context.CategoryTypes.FindAsync(id);
            if (categoryType == null)
            {
                return NotFound();
            }
            return View(categoryType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] CategoryType categoryType)
        {
            if (id != categoryType.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryTypeExists(categoryType.CategoryId))
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
            return View(categoryType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryType = await _context.CategoryTypes
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (categoryType == null)
            {
                return NotFound();
            }

            return View(categoryType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryType = await _context.CategoryTypes.FindAsync(id);
            _context.CategoryTypes.Remove(categoryType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryTypeExists(int id)
        {
            return _context.CategoryTypes.Any(e => e.CategoryId == id);
        }
    }
}
