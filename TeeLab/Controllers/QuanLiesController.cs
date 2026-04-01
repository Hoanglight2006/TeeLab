using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Teelab.Models;
using Teelab.DataBase;

namespace Teelab.Controllers
{
    public class QuanLiesController : Controller
    {
        private readonly AppDbContext _context;

        public QuanLiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: QuanLies
        public async Task<IActionResult> Index()
        {
            return View(await _context.QuanLys.ToListAsync());
        }

        // GET: QuanLies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quanLy = await _context.QuanLys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quanLy == null)
            {
                return NotFound();
            }

            return View(quanLy);
        }

        // GET: QuanLies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuanLies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Hoten,Diachi,Ngaysinh,Sdt")] QuanLy quanLy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quanLy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quanLy);
        }

        // GET: QuanLies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quanLy = await _context.QuanLys.FindAsync(id);
            if (quanLy == null)
            {
                return NotFound();
            }
            return View(quanLy);
        }

        // POST: QuanLies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Hoten,Diachi,Ngaysinh,Sdt")] QuanLy quanLy)
        {
            if (id != quanLy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quanLy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuanLyExists(quanLy.Id))
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
            return View(quanLy);
        }

        // GET: QuanLies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quanLy = await _context.QuanLys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quanLy == null)
            {
                return NotFound();
            }

            return View(quanLy);
        }

        // POST: QuanLies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quanLy = await _context.QuanLys.FindAsync(id);
            if (quanLy != null)
            {
                _context.QuanLys.Remove(quanLy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuanLyExists(int id)
        {
            return _context.QuanLys.Any(e => e.Id == id);
        }
    }
}
