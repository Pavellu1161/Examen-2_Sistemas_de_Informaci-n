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
    public class HomeworkController : Controller
    {
        private readonly int recordsPerPage = 10;

        private readonly ApplicationDbContext _context;

        private Pagination<Homework> paginationHomework;

        public HomeworkController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Homework
        public async Task<IActionResult> Index(string search, int page = 1)
        {

            int totalRecords = 0;

            if (search == null)
            {
                search = "";
            }   

            totalRecords = await _context.Homeworks.CountAsync(h=> h.Description.Contains(search));

            var homeworks = await _context.Homeworks.Where(h=> h.Description.Contains(search))
                            .Include(h=> h.CategoryType).ToListAsync();

            var homeworksResult = homeworks.OrderBy(o => o.Description).Skip((page - 1) * recordsPerPage).Take(recordsPerPage);

            var totalPages = (int)Math.Ceiling((double)totalRecords / recordsPerPage);

            paginationHomework = new Pagination<Homework>()
            {
                RecordsPerPage = recordsPerPage,
                TotalRecords = totalRecords,
                TotalPage = totalPages,
                CurrentPage = page,
                Search = search,
                Result = homeworksResult    
            };

            var applicationDbContext = _context.Homeworks.Where(h => h.Description.Contains(search)).Include(h => h.CategoryType);
            return View(paginationHomework);
        }

        // GET: Homework/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homework = await _context.Homeworks
                .Include(h => h.CategoryType)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (homework == null)
            {
                return NotFound();
            }

            return View(homework);
        }

        // GET: Homework/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.CategoryTypes, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Homework/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,Description,CreationDate,FinalDate,CategoryId")] Homework homework)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homework);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryTypes, "CategoryId", "CategoryName", homework.CategoryId);
            return View(homework);
        }

        // GET: Homework/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homework = await _context.Homeworks.FindAsync(id);
            if (homework == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryTypes, "CategoryId", "CategoryName", homework.CategoryId);
            return View(homework);
        }

        // POST: Homework/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,Description,CreationDate,FinalDate,CategoryId")] Homework homework)
        {
            if (id != homework.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homework);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeworkExists(homework.TaskId))
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
            ViewData["CategoryId"] = new SelectList(_context.CategoryTypes, "CategoryId", "CategoryName", homework.CategoryId);
            return View(homework);
        }

        // GET: Homework/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homework = await _context.Homeworks
                .Include(h => h.CategoryType)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (homework == null)
            {
                return NotFound();
            }

            return View(homework);
        }

        // POST: Homework/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homework = await _context.Homeworks.FindAsync(id);
            _context.Homeworks.Remove(homework);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeworkExists(int id)
        {
            return _context.Homeworks.Any(e => e.TaskId == id);
        }
    }
}
