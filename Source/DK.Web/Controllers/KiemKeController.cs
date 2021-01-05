using DK.Application;
using DK.Application.Models;
using DK.Application.Repositories;
using DK.Web.Core;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DK.Web.Controllers
{
    [Authorize]
    public class KiemKeController : BaseController
    {
        private readonly ITaiSanRepository _taiSanRepository;
        private readonly ITypeRepository _typeRepository;
        private readonly IKiemKeRepository _kiemKeRepository;
        private readonly ITaiSanService _taiSanService;
        public KiemKeController(ITaiSanRepository taiSanRepository, ITypeRepository typeRepository, IKiemKeRepository kiemKeRepository, ITaiSanService taiSanService)
        {
            _taiSanRepository = taiSanRepository;
            _typeRepository = typeRepository;
            _kiemKeRepository = kiemKeRepository;
            _taiSanService = taiSanService;
        }

        // GET: Category
        public ActionResult Index(PagerInfo pagerInfo)
        {
            var list = _typeRepository.Find(m => m.Name == TypeConstant.KiemKe);
            var pager = new Pager(list.Count(), pagerInfo.PageIndex, pagerInfo.PageSize);

            var result = new PagerViewModel
            {
                BaseUrl = Url.Action("Index", new { pagerInfo.PageIndex, pagerInfo.PageSize }),
                Items = list.OrderByDescending(m => m.Id).Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };

            return View(result);
        }

        public async Task<ActionResult> Detail(Guid id, PagerInfo pagerInfo)
        {
            var kiemke = await _typeRepository.GetAsync(id);
            ViewBag.KiemKe = kiemke;

            var list = _kiemKeRepository.Find(m => m.KiemKeId == kiemke.Id).ToEnumerable();
            var pager = new Pager(list.Count(), pagerInfo.PageIndex, pagerInfo.PageSize);

            var result = new PagerViewModel
            {
                BaseUrl = Url.Action("Detail", new { id, pagerInfo.PageIndex, pagerInfo.PageSize }),
                Items = list.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };

            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult> Detail(Application.Models.Type model)
        {
            var kiemke = await _typeRepository.GetAsync(model.Id);
            kiemke.Title = model.Title;
            _typeRepository.Update(kiemke);

            return RedirectToAction("Detail", new { kiemke.Id });
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            var kiemke = await _typeRepository.GetAsync(id);
            if (kiemke == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                await _kiemKeRepository.DeleteManyAsync(nameof(KiemKe.KiemKeId), id);
                await _typeRepository.DeleteAsync(id);
            }
            catch (Exception _)
            {

            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Create(TaiSanSearchModel search)
        {
            try
            {
                var list = _taiSanRepository.Find(search);
                if (list.Count() == 0)
                {
                    return RedirectToAction("Index", "Home", search.ToPagingModel());
                }

                var type = new Application.Models.Type
                {
                    Name = TypeConstant.KiemKe,
                    Title = $"Kiểm kê ngày {DateTime.Now:dd-MM-yyyy}",
                };

                await _typeRepository.AddAsync(type);
                var listKiemKes = GetListKiemKes(type.Id, list);
                await _kiemKeRepository.AddRangeAsync(listKiemKes);

                return RedirectToAction("Detail", new { type.Id });
            }
            catch (Exception _)
            {

            }

            return RedirectToAction("Index", "Home", search.ToPagingModel());
        }

        public ActionResult Import(Guid id)
        {
            return View(id);
        }

        [HttpPost]
        public ActionResult Import(Guid id, HttpPostedFileBase taisan)
        {
            _taiSanService.ImportKiemKe(taisan.InputStream, id);
            return RedirectToAction(nameof(Detail), new { id = id });
        }

        public async Task<ActionResult> Export(Guid id, string pattern)
        {
            var kiemKes = _kiemKeRepository.Find(m => m.KiemKeId == id).ToList();
            await _taiSanService.ExportKiemKeAsync(kiemKes, pattern);
            return RedirectToAction(nameof(Detail), new { id = id });
        }

        private IEnumerable<KiemKe> GetListKiemKes(Guid kiemKeId, IEnumerable<TaiSan> list)
        {
            return list.AsEnumerable().Select(m => new KiemKe
            {
                KiemKeId = kiemKeId,
                No = 0,
                Code = m.Code,
                Name = m.Name,
                NamSuDung = m.NamSuDung,
                SoLuongKeToan = m.SoLuong,
                NguyenGiaKeToan = m.NguyenGiaKeToan,
                GiaTriConLaiKeToan = m.GiaTriConLai
            });
        }
    }
}
