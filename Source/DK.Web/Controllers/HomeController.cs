using DK.Application;
using DK.Application.Models;
using DK.Application.Repositories;
using DK.Web.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DK.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ITaiSanRepository _taiSanRepository;
        private readonly ITypeRepository _typeRepository;
        private readonly ITaiSanService _taiSanService;
        public HomeController(ITaiSanRepository taiSanRepository, ITypeRepository typeRepository, ITaiSanService taiSanService)
        {
            _taiSanRepository = taiSanRepository;
            _typeRepository = typeRepository;
            _taiSanService = taiSanService;
        }
        // GET: Home
        public ActionResult Index(TaiSanSearchModel search)
        {
            CreateDropDownViewBag();
            ViewBag.SearchModel = search;

            var result = SearchTaiSan(search);
            return View(result);
        }

        private PagerViewModel SearchTaiSan(TaiSanSearchModel search)
        {
            var list = _taiSanRepository.Find(search);

            var pager = new Pager(list.Count(), search.PageIndex);
            if (search.pattern != null)
                _taiSanService.ExportTaiSanAsync(list.ToList(), search.pattern);
            return new PagerViewModel
            {
                BaseUrl = Url.Action("Index", search.ToPagingModel()),
                Items = list.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };
        }

        private void CreateDropDownViewBag()
        {
            var types = _typeRepository.Find(m => true).ToList();
            ViewBag.ChungLoai = types.Where(m => m.Name == TypeConstant.ChungLoai).Select(m => m.Title);
            ViewBag.DanhMuc = types.Where(m => m.Name == TypeConstant.DanhMuc).Select(m => m.Title);
            ViewBag.NguonKinhPhi = types.Where(m => m.Name == TypeConstant.NguonKinhPhi).Select(m => m.Title);
            ViewBag.ChatLuong = types.Where(m => m.Name == TypeConstant.ChatLuong).Select(m => m.Title);
            ViewBag.LoaiXe = types.Where(m => m.Name == TypeConstant.LoaiXe).Select(m => m.Title);
            ViewBag.PhongBan = types.Where(m => m.Name == TypeConstant.PhongBan).Select(m => m.Title);
            ViewBag.Tags = types.Where(m => m.Name == TypeConstant.Tags).Select(m => m.Title);
        }
        [HttpPost]
        public Task GetReportAsync(TaiSanSearchModel search, string pattern)
        {
            var taiSans = _taiSanRepository.Find(search).ToList();
            return _taiSanService.ExportTaiSanAsync(taiSans, pattern);
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void FakeData()
        {
            _taiSanRepository.DeleteManyAsync("GroupCode", "").GetAwaiter().GetResult();

            var list = new List<TaiSan>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(new TaiSan
                {
                    Code = $"Code {i}",
                    Name = $"Name {i}",
                    ChungLoai = "Chủng loại 1",
                    Tags = new List<string> { "Tag 1" }
                });
            }

            _taiSanRepository.AddRangeAsync(list);
        }
    }
}
