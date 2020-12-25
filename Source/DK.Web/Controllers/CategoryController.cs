using DK.Application.Models;
using DK.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DK.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ITypeRepository _typeRepository;
        public CategoryController(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        // GET: Category
        public ActionResult Index(string id)
        {
            ViewBag.Name = id;
            var list = _typeRepository.Find(m => m.Name == id).ToList();
            if (list.Count == 0)
            {
                list = Enumerable.Repeat(new Application.Models.Type(), 3).ToList();
            }
            return View(list);
        }

        [HttpPost]
        public async Task<ActionResult> Index(string id, List<string> list)
        {
            try
            {
                if (list.Count == 0)
                {
                    return RedirectToAction("Index", new { id });
                }

                await _typeRepository.DeleteManyAsync(nameof(Application.Models.Type.Name), id);
                var listTypes = list.Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => new Application.Models.Type
                {
                    Name = id,
                    Title = m.Trim()
                });

                await _typeRepository.AddRangeAsync(listTypes);
                return RedirectToAction("Index", new { id });
            }
            catch
            {
                var listTypes = Enumerable.Repeat(new Application.Models.Type(), 3).ToList();
                return View(listTypes);
            }
        }
    }
}
