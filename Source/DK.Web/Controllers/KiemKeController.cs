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
using System.IO;
using MediaDevices;

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
        public ActionResult Index(DefaultSearchModel pagerInfo)
        {
            //var devices = MediaDevice.GetDevices();
            //using (var device = devices.First())
            //{
            //    var dir = @"IPSM card\Android\data";
            //    device.Connect();
            //    var photoDir = device.GetDirectoryInfo(dir);

            //    var files = photoDir.EnumerateFiles("*.*", SearchOption.AllDirectories);
            //    var filename = @"IPSM card\Android\data\xxx.txt";
            //    device.UploadFile(@"c:\xxx.txt", filename);
            //    //foreach (var file in files)
            //    //{
            //    //    MemoryStream memoryStream = new System.IO.MemoryStream();
            //    //    device.DownloadFile(file.FullName, memoryStream);
            //    //    memoryStream.Position = 0;
            //    //}
            //    device.Disconnect();
            //}

            
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

        public async Task<ActionResult> Detail(Guid id, DefaultSearchModel pagerInfo)
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

        public ActionResult NewOrEdit(Guid parentId, Guid? id = null, string returnUrl = "")
        {
            //CreateDropDownViewBag();
            KiemKe kiemKe = null;
            if (id.HasValue)
            {
                kiemKe = _kiemKeRepository.Get(id.Value);
            }

            if (kiemKe == null)
            {
                kiemKe = new KiemKe { KiemKeId = parentId };
            }

            return View(kiemKe);
        }

        [HttpPost]
        public ActionResult NewOrEdit(KiemKe kiemke, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                    }
                }
                return View(kiemke);
            }

            var current = _kiemKeRepository.Get(kiemke.Id);
            if (current == null)
            {
                var existingCodes = _taiSanService.GetExistingCodes();
                var ts = new TaiSan() { Name = kiemke.Name };
                ts.GenerateCode(existingCodes);

                kiemke.Code = ts.Code;
                kiemke.CreatedBy = User.Identity.Name;
            }
            else
            {
                kiemke.CreatedBy = current.CreatedBy;
            }

            _kiemKeRepository.Upsert(kiemke);
            if (string.IsNullOrWhiteSpace(returnUrl)) return RedirectToAction(nameof(Detail), new { kiemke.KiemKeId }); else return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<ActionResult> Detail(Application.Models.Type model)
        {
            var kiemke = await _typeRepository.GetAsync(model.Id);
            kiemke.Title = model.Title;
            _typeRepository.Update(kiemke);

            return RedirectToAction("Detail", new { kiemke.Id });
        }

        public async Task<ActionResult> DeleteKiemKe(Guid id, string returnUrl)
        {
            var kiemke = await _kiemKeRepository.GetAsync(id);
            if (kiemke == null)
            {
                return NotFound();
            }

            returnUrl = string.IsNullOrEmpty(returnUrl) ? Url.Action(nameof(Detail), new { kiemke.KiemKeId }) : returnUrl;
            try
            {
                await _kiemKeRepository.DeleteAsync(id);
            }
            catch (Exception _)
            {

            }

            return Redirect(returnUrl);
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
                search.IsApproved = true;

                var list = _taiSanRepository.Find(search);
                if (list.Count() == 0)
                {
                    return RedirectToAction("Index", "Home", search.ToPagingModel());
                }

                var type = new Application.Models.Type
                {
                    Name = TypeConstant.KiemKe,
                    Title = $"Kiểm kê ngày {DateTime.Now:dd-MM-yyyy}",
                    CreatedBy = User.Identity.Name,
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
            try
            {
                _taiSanService.ImportKiemKe(taisan.InputStream, id);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }

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
