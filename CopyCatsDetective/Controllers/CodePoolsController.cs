using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CopyCatsDetective.Models;

namespace CopyCatsDetective.Controllers
{
    public class CodePoolsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /CodePools/
        public async Task<ActionResult> Index()
        {
            return View(await db.CodePools.ToListAsync());
        }

        // GET: /CodePools/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodePool codepool = await db.CodePools.FindAsync(id);
            if (codepool == null)
            {
                return HttpNotFound();
            }
            return View(codepool);
        }

        // GET: /CodePools/Create
        public async Task<ActionResult> Create(int? categoryId)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return HttpNotFound();
            }

            var createCodePoolViewModel = new CreateCodePoolViewModel()
            {
                CategoryId = category.Id
            };
            return View(createCodePoolViewModel);
        }

        // POST: /CodePools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Language,Name,CategoryId")] CreateCodePoolViewModel codepoolViewModel)
        {
            if (ModelState.IsValid)
            {
                CodePool codePool = new CodePool()
                {
                    Name = codepoolViewModel.Name,
                    Language = codepoolViewModel.Language,
                    Category = await db.Categories.FindAsync(codepoolViewModel.CategoryId)
                };
                db.CodePools.Add(codePool);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(codepoolViewModel);
        }

        // GET: /CodePools/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodePool codepool = await db.CodePools.FindAsync(id);
            if (codepool == null)
            {
                return HttpNotFound();
            }
            return View(codepool);
        }

        // POST: /CodePools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Language,Name")] CodePool codepool)
        {
            if (ModelState.IsValid)
            {
                db.Entry(codepool).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(codepool);
        }

        // GET: /CodePools/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodePool codepool = await db.CodePools.FindAsync(id);
            if (codepool == null)
            {
                return HttpNotFound();
            }
            return View(codepool);
        }

        // POST: /CodePools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CodePool codepool = await db.CodePools.FindAsync(id);
            db.CodePools.Remove(codepool);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
