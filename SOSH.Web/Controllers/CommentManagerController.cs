using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SOSH.Web;
using SOSH.Web.Models.ViewModels;

namespace SOSH.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CommentManagerController : Controller
    {
        private readonly SOSHContext _context;

        public CommentManagerController(SOSHContext context)
        {
            _context = context;
        }

        // GET: CommentManager
        public async Task<IActionResult> Index()
        {
            var comments = await _context.Comments
                                .Join(_context.Products,
                               comnt => comnt.ProductId,
                               pro => pro.Id,
                               (comnt, pro) => new CommentViewModel
                               {
                                   CommentName = comnt.CommentName,
                                   CommentText = comnt.CommentText,
                                   Email = comnt.Email,
                                   IsPublished = comnt.IsPublished,
                                   ProductId = comnt.ProductId,
                                   ProductName = pro.Name,
                                   CommentId = comnt.CommentId
                               }).ToListAsync();
            return View(comments);
        }

        // GET: CommentManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == 0)
                return NotFound();

            var comments = await (from comm in _context.Comments
                                  join pro in _context.Products on comm.ProductId equals pro.Id
                                  where comm.CommentId == id
                                  select new CommentViewModel
                                  {
                                      CommentId = comm.CommentId,
                                      CommentName = comm.CommentName,
                                      CommentText = comm.CommentText,
                                      Email = comm.Email,
                                      IsPublished = comm.IsPublished,
                                      ProductId = comm.ProductId,
                                      ProductName = pro.Name

                                  }).FirstOrDefaultAsync();

            if (comments == null)
                return NotFound();

            return View(comments);
        }




        // POST: CommentManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptComment(int CommentId)
        {
            if (CommentId == 0)
            {
                return NotFound();
            }
            var comment = await _context.Comments.FindAsync(CommentId);
            comment.IsPublished = true;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(CommentId))
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
            return View(comment);
        }

        // GET: CommentManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: CommentManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}
