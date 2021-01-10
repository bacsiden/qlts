using DK.Application.Models;
using DK.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DK.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ITypeRepository _typeRepository;
        private readonly ITaiSanRepository _taiSanRepository;
        public CategoryController(ITypeRepository typeRepository, ITaiSanRepository taiSanRepository)
        {
            _typeRepository = typeRepository;
            _taiSanRepository = taiSanRepository;
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

        [HttpPost]
        public async Task<ActionResult> Merge(string categoryName, string newName, List<Guid> oldIds)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                return RedirectToAction("Index", new { id = categoryName });
            }

            try
            {
                var oldTypes = _typeRepository.Find(m => m.Name == categoryName).AsEnumerable().Where(m => oldIds.Any(id => id == m.Id)).ToList();
                var oldNames = oldTypes.Select(m => m.Title);

                var taisans = new List<TaiSan>();

                switch (categoryName)
                {
                    case TypeConstant.ChatLuong:
                        taisans = _taiSanRepository.Find(m => true).AsEnumerable().Where(m => oldNames.Any(name => name == m.ChatLuong)).ToList();
                        taisans.ForEach(item =>
                        {
                            item.ChatLuong = newName;
                        });

                        break;
                    case TypeConstant.ChungLoai:
                        taisans = _taiSanRepository.Find(m => true).AsEnumerable().Where(m => oldNames.Any(name => name == m.ChungLoai)).ToList();
                        taisans.ForEach(item =>
                        {
                            item.ChungLoai = newName;
                        });
                        break;
                    case TypeConstant.DanhMuc:
                        taisans = _taiSanRepository.Find(m => true).AsEnumerable().Where(m => oldNames.Any(name => name == m.DanhMuc)).ToList();
                        taisans.ForEach(item =>
                        {
                            item.DanhMuc = newName;
                        });
                        break;
                    case TypeConstant.LoaiXe:
                        taisans = _taiSanRepository.Find(m => true).AsEnumerable().Where(m => oldNames.Any(name => name == m.LoaiXe)).ToList();
                        taisans.ForEach(item =>
                        {
                            item.LoaiXe = newName;
                        });
                        break;
                    case TypeConstant.NguonKinhPhi:
                        taisans = _taiSanRepository.Find(m => true).AsEnumerable().Where(m => oldNames.Any(name => name == m.NguonKinhPhi)).ToList();
                        taisans.ForEach(item =>
                        {
                            item.NguonKinhPhi = newName;
                        });
                        break;
                    case TypeConstant.PhongBan:
                        taisans = _taiSanRepository.Find(m => true).AsEnumerable().Where(m => oldNames.Any(name => name == m.PhongQuanLy)).ToList();
                        taisans.ForEach(item =>
                        {
                            item.PhongQuanLy = newName;
                        });
                        break;
                    case TypeConstant.Tags:
                        taisans = _taiSanRepository.Find(m => true).AsEnumerable().Where(m => oldNames.Any(name => m.Tags.Any(tag => tag == name))).ToList();
                        taisans.ForEach(item =>
                        {
                            var count = item.Tags.Count;
                            item.Tags = item.Tags.Where(tag => oldNames.Any(name => name == tag)).ToList();
                            if (item.Tags.Count < count)
                                item.Tags.Add(newName);
                        });
                        break;
                }

                foreach (var item in taisans)
                {
                    await _taiSanRepository.UpdateAsync(item);
                }

                foreach (var id in oldIds)
                {
                    await _typeRepository.DeleteAsync(id);
                }

                await _typeRepository.AddAsync(new Application.Models.Type
                {
                    Name = categoryName,
                    Title = newName.Trim()
                });
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Index", new { id = categoryName });
        }

        public ActionResult CountAffectedAssets(string categoryName, List<string> categories)
        {
            if (categories == null || categories.Count == 0)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            var count = 0;
            switch (categoryName)
            {
                case TypeConstant.ChatLuong:
                    count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => categories.Any(name => name == m.ChatLuong));

                    break;
                case TypeConstant.ChungLoai:
                    count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => categories.Any(name => name == m.ChungLoai));
                    break;
                case TypeConstant.DanhMuc:
                    count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => categories.Any(name => name == m.DanhMuc));
                    break;
                case TypeConstant.LoaiXe:
                    count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => categories.Any(name => name == m.LoaiXe));
                    break;
                case TypeConstant.NguonKinhPhi:
                    count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => categories.Any(name => name == m.NguonKinhPhi));
                    break;
                case TypeConstant.PhongBan:
                    count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => categories.Any(name => name == m.PhongQuanLy));
                    break;
                case TypeConstant.Tags:
                    count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => categories.Any(name => m.Tags.Any(tag => tag == name)));
                    break;
            }

            return Json(count, JsonRequestBehavior.AllowGet);
        }
    }
}
