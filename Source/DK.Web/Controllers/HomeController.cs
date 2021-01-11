using DK.Application;
using DK.Application.Models;
using DK.Application.Repositories;
using DK.Web.Core;
using DK.Web.Models;
using Microsoft.AspNet.Identity;
using StructureMap.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
        // GET: Tài sản đã phê duyệt
        public ActionResult Index(TaiSanSearchModel search)
        {
            search.IsApproved = true;

            CreateDropDownViewBag();
            ViewBag.SearchModel = search;

            var result = SearchTaiSan(search);
            return View(result);
        }

        // GET: Tài sản chưa phê duyệt
        public ActionResult TaisanUnApproved(TaiSanSearchModel search)
        {
            var canManage = RoleList.GetAll().Any(User.IsInRole);
            search.IsApproved = false;

            if (!canManage)
            {
                search.CreatedBy = User.Identity.Name;
            }

            CreateDropDownViewBag();
            ViewBag.SearchModel = search;

            var result = SearchTaiSan(search);
            return View(result);
        }

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase taisan)
        {
            try
            {
                _taiSanService.ImportTaiSan(User.Identity.Name, taisan.InputStream);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
            return RedirectToAction(nameof(TaisanUnApproved));
        }

        public ActionResult Dashboard()
        {
            var dashboard = new DashboardModel();
            dashboard.TongSoTaiSan = _taiSanRepository.Find(m => true).AsEnumerable().Sum(m => m.SoLuong ?? 1);
            dashboard.TongSoDotKiemKe = _typeRepository.Find(m => m.Name == TypeConstant.KiemKe).Count();
            dashboard.TongSoXe = _taiSanRepository.Find(m => true).AsEnumerable().Where(m => !string.IsNullOrEmpty(m.LoaiXe)).Sum(m => (m.SoLuong ?? 1));
            dashboard.TongNguonKinhPhi = _taiSanRepository.Find(m => true).AsEnumerable().Sum(m => m.NguyenGiaKeToan * (m.SoLuong ?? 1)) ?? 0;
            return View(dashboard);
        }

        public ActionResult NewOrEditAsset(string returnUrl, Guid? id = null, bool isApproved = false)
        {
            CreateDropDownViewBag();
            TaiSan taiSan = null;
            if (id.HasValue)
            {
                taiSan = _taiSanRepository.Get(id.Value);
            }

            if (taiSan == null)
            {
                taiSan = new TaiSan
                {
                    IsApproved = isApproved
                };
            }
            taiSan.JoinedTags = string.Join(";", taiSan.Tags);
            return View(taiSan);
        }

        [HttpPost]
        public ActionResult NewOrEditAsset(TaiSan taisan, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                    }
                }
                CreateDropDownViewBag();
                return View(taisan);
            }

            var currentAsset = _taiSanRepository.Get(taisan.Id);
            if (currentAsset != null)
            {
                taisan.Children = currentAsset.Children;
            }
            else
            {
                var existingCodes = _taiSanService.GetExistingCodes();
                taisan.GenerateCode(existingCodes);
            }

            taisan.Tags = string.IsNullOrWhiteSpace(taisan.JoinedTags) ? new List<string>() : taisan.JoinedTags.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(m => m.Trim()).ToList();

            _taiSanRepository.Upsert(taisan);
            return CustomRedirect(returnUrl);
        }

        public ActionResult NewOrEditChildAsset(string code, Guid parentId, string returnUrl)
        {
            var taiSan = _taiSanRepository.Get(parentId);
            if (taiSan == null) return CustomRedirect(returnUrl);
            var subTS = taiSan.Children.FirstOrDefault(m => m.Code == code);
            if (subTS == null) subTS = new TaiSan();

            CreateDropDownViewBag();
            subTS.ParentId = parentId;
            return View(subTS);
        }

        [HttpPost]
        public ActionResult NewOrEditChildAsset(TaiSan ts, string returnUrl)
        {
            var taiSan = _taiSanRepository.Get(ts.ParentId);
            ModelState.AddModelError(string.Empty, "Có lỗi xảy ra, vui lòng thử lại");
            var existingCodes = _taiSanService.GetExistingCodes();
            if (ts.Code == null) ts.GenerateCode(existingCodes);
            var index = taiSan.Children.FindIndex(m => m.Code == ts.Code);
            if (index > -1) taiSan.Children.RemoveAt(index);
            taiSan.Children.Add(ts);
            _taiSanRepository.Update(taiSan);
            return CustomRedirect(returnUrl);
        }

        public ActionResult DeleteChildAsset(string code, Guid parentId, string returnUrl)
        {
            var taiSan = _taiSanRepository.Get(parentId);
            if (taiSan == null) return CustomRedirect(returnUrl);
            var index = taiSan.Children.FindIndex(m => m.Code == code);
            if (index > -1) taiSan.Children.RemoveAt(index);
            _taiSanRepository.Update(taiSan);
            return CustomRedirect(returnUrl);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTaiSan(bool isApproved, Guid[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    await _taiSanRepository.DeleteAsync(id);
                }
            }
            catch
            {
            }

            if (isApproved)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(TaisanUnApproved));
        }

        [HttpPost]
        public async Task<ActionResult> AppoveTaiSan(Guid[] ids)
        {
            try
            {
                var taisans = _taiSanRepository.Find(m => true).AsEnumerable().Where(m => ids.Contains(m.Id));
                foreach (var ts in taisans)
                {
                    ts.IsApproved = true;
                    foreach (var item in ts.Children)
                    {
                        item.IsApproved = true;
                    }

                    await _taiSanRepository.UpdateAsync(ts);
                }
            }
            catch
            {
            }

            return RedirectToAction(nameof(TaisanUnApproved));
        }

        private PagerViewModel SearchTaiSan(TaiSanSearchModel search)
        {
            var list = _taiSanRepository.Find(search);

            var pager = new Pager(list.Count(), search.PageIndex, search.PageSize);
            if (!string.IsNullOrWhiteSpace(search.pattern))
                _taiSanService.ExportDataAsync(list.ToList(), search.pattern, search.IncludeSub);
            search.pattern = null;
            return new PagerViewModel
            {
                BaseUrl = Url.Action("Index", search.ToPagingModel()),
                Items = list.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };
        }

        private ActionResult CustomRedirect(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return RedirectToAction(nameof(Index)); else return Redirect(url);
        }

        private void CreateDropDownViewBag()
        {
            var types = _typeRepository.Find(m => true).ToList();
            ViewBag.GroupName = TypeConstant.Groups;
            ViewBag.ChungLoai = types.Where(m => m.Name == TypeConstant.ChungLoai).Select(m => m.Title);
            ViewBag.DanhMuc = types.Where(m => m.Name == TypeConstant.DanhMuc).Select(m => m.Title);
            ViewBag.NguonKinhPhi = types.Where(m => m.Name == TypeConstant.NguonKinhPhi).Select(m => m.Title);
            ViewBag.ChatLuong = types.Where(m => m.Name == TypeConstant.ChatLuong).Select(m => m.Title);
            ViewBag.LoaiXe = types.Where(m => m.Name == TypeConstant.LoaiXe).Select(m => m.Title);
            ViewBag.PhongBan = types.Where(m => m.Name == TypeConstant.PhongBan).Select(m => m.Title);
            ViewBag.Tags = types.Where(m => m.Name == TypeConstant.Tags).Select(m => m.Title);
            ViewBag.Members = UserManager.Users.Select(m => m.UserName).ToList();
        }
    }
}
