using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.Extensions;

namespace MiddleEducationPlan.Web.Controllers
{
    public class ProjectViewController : Controller
    {
        private readonly IProjectService projectService;

        public ProjectViewController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await this.projectService.GetProjectsAsync(new GetProjectModel());

            if (result.Count == 0)
                return NotFound();

            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        //// POST: Todos/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Description,CreatedDate")] Todo todo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        context.Add(todo);
        //        await context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(todo);
        //}

        //// GET: Todos/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var todo = await context.Todo
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (todo == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(todo);
        //}

        //// GET: Todos/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var todo = await context.Todo.FindAsync(id);
        //    if (todo == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(todo);
        //}

        //// POST: Todos/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Description,CreatedDate")] Todo todo)
        //{
        //    if (id != todo.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            context.Update(todo);
        //            await context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TodoExists(todo.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(todo);
        //}

        //// GET: Todos/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var todo = await context.Todo
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (todo == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(todo);
        //}

        //// POST: Todos/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var todo = await context.Todo.FindAsync(id);
        //    context.Todo.Remove(todo);
        //    await context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool TodoExists(int id)
        //{
        //    return context.Todo.Any(e => e.Id == id);
        //}


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
