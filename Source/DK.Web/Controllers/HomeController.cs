using DK.Application.Models;
using DK.Application.Repositories;
using DK.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DK.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ITaiSanRepository _taiSanRepository;
        private readonly ITypeRepository _typeRepository;
        public HomeController(ITaiSanRepository taiSanRepository, ITypeRepository typeRepository)
        {
            _taiSanRepository = taiSanRepository;
            _typeRepository = typeRepository;
        }
        // GET: Home
        public ActionResult Index(TaiSanSearchModel search)
        {
            var types = _typeRepository.Find(m => true).ToList();

            ViewBag.ChungLoai = types.Where(m => m.Name == TypeConstant.ChungLoai).Select(m => m.Title);
            ViewBag.DanhMuc = types.Where(m => m.Name == TypeConstant.DanhMuc).Select(m => m.Title);
            ViewBag.NguonKinhPhi = types.Where(m => m.Name == TypeConstant.NguonKinhPhi).Select(m => m.Title);
            ViewBag.ChatLuong = types.Where(m => m.Name == TypeConstant.ChatLuong).Select(m => m.Title);
            ViewBag.LoaiXe = types.Where(m => m.Name == TypeConstant.LoaiXe).Select(m => m.Title);
            ViewBag.Tags = types.Where(m => m.Name == TypeConstant.Tags).Select(m => m.Title);

            ViewBag.SearchModel = search;
            var list = _taiSanRepository.Find(m => true).AsEnumerable().ToPagedList(search.Page, _pageSize);
            var pager = new PagerModel
            {
                list = list
            };

            return View(pager);
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
    }
}
