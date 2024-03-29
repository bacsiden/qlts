﻿using DK.Application;
using DK.Application.Models;
using DK.Application.Repositories;
using DK.Web.Core;
using DK.Web.Models;
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
        private readonly string HtmlFolder = System.Web.HttpContext.Current.Server.MapPath("~/Html\\");
        private readonly ITaiSanRepository _taiSanRepository;
        private readonly ITypeRepository _typeRepository;
        private readonly ITaiSanService _taiSanService;
        private readonly IKiemKeRepository _kiemKeRepository;
        private readonly IViewFieldRepository _viewFieldRepository;
        public HomeController(ITaiSanRepository taiSanRepository, ITypeRepository typeRepository, ITaiSanService taiSanService, IKiemKeRepository kiemKeRepository,
            IViewFieldRepository viewFieldRepository)
        {
            _taiSanRepository = taiSanRepository;
            _typeRepository = typeRepository;
            _taiSanService = taiSanService;
            _kiemKeRepository = kiemKeRepository;
            _viewFieldRepository = viewFieldRepository;
        }
        // GET: Tài sản đã phê duyệt
        public ActionResult Index(TaiSanSearchModel search)
        {
            search.IsApproved = true;

            CreateDropDownViewBag();
            ViewBag.SearchModel = search;
            ViewBag.ViewFields = _viewFieldRepository.ListForTaiSan(User.Identity.Name);

            var result = SearchTaiSan(search);
            return View(result);
        }

        public ActionResult SetView(List<ViewField> allViews, string returnUrl)
        {
            var lst = _viewFieldRepository.ListForTaiSan(User.Identity.Name);
            foreach (var item in allViews)
            {
                var old = lst.FirstOrDefault(m => m.Id == item.Id);
                if (old?.Display != item.Display)
                    _viewFieldRepository.Set(item.Id, nameof(ViewField.Display), item.Display);
            }
            return CustomRedirect(returnUrl);
        }

        public ActionResult Preview(TaiSanSearchModel search)
        {
            var list = _taiSanRepository.Find(search);
            _taiSanService.ExportDataAsync(list.ToList(), search).GetAwaiter().GetResult();
            var html = System.IO.File.ReadAllText($"{HtmlFolder}{search.pattern}.html");
            return View(model: html);
        }

        // GET: Tài sản chưa phê duyệt
        public ActionResult TaisanUnApproved(TaiSanSearchModel search)
        {
            search.IsApproved = false;

            CreateDropDownViewBag();
            ViewBag.SearchModel = search;
            ViewBag.ViewFields = _viewFieldRepository.ListForTaiSan(User.Identity.Name);

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
                _taiSanService.ImportNewTaiSan(User.Identity.Name, taisan.InputStream);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
            return RedirectToAction(nameof(TaisanUnApproved));
        }
        public ActionResult ImportUpdate(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportUpdate(HttpPostedFileBase taisan, string returnUrl)
        {
            try
            {
                _taiSanService.ImportUpdateTaiSan(taisan.InputStream);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
            return CustomRedirect(returnUrl);
        }
        public ActionResult Dashboard()
        {
            var dashboard = new DashboardModel();
            var taisanList = _taiSanRepository.Find(m => true).AsEnumerable();
            dashboard.TongSoTaiSan = taisanList.Sum(m => m.SoLuong ?? 1);
            dashboard.TongSoDotKiemKe = _typeRepository.Find(m => m.Name == TypeConstant.KiemKe).Count();
            dashboard.TongSoXe = taisanList.Where(m => !string.IsNullOrEmpty(m.LoaiXe)).Sum(m => (m.SoLuong ?? 1));
            dashboard.TongNguonKinhPhi = taisanList.Sum(m => m.NguyenGiaKeToan * (m.SoLuong ?? 1)) ?? 0;
            dashboard.RecentKiemKes = _typeRepository.Find(m => m.Name == TypeConstant.KiemKe).OrderByDescending(m => m.Id).Take(6).ToList();

            var total = taisanList.Count();
            foreach (var categoryName in Application.Models.Type.MenuCategories)
            {
                var count = 0;
                switch (categoryName)
                {
                    case TypeConstant.Group:
                        count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => !string.IsNullOrEmpty(m.GroupName));
                        break;
                    case TypeConstant.ChatLuong:
                        count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => !string.IsNullOrEmpty(m.ChatLuong));
                        break;
                    case TypeConstant.ChungLoai:
                        count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => !string.IsNullOrEmpty(m.ChungLoai));
                        break;
                    case TypeConstant.LoaiXe:
                        count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => !string.IsNullOrEmpty(m.LoaiXe));
                        break;
                    case TypeConstant.NguonKinhPhi:
                        count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => !string.IsNullOrEmpty(m.NguonKinhPhi));
                        break;
                    case TypeConstant.NguonKinhPhiKhac:
                        count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => !string.IsNullOrEmpty(m.NganSachKhac));
                        break;
                    case TypeConstant.PhongBan:
                        count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => !string.IsNullOrEmpty(m.PhongQuanLy));
                        break;
                    case TypeConstant.Tags:
                        count = _taiSanRepository.Find(m => true).AsEnumerable().Count(m => m.Tags.Count > 0);
                        break;
                }

                var percent = (int)(Math.Floor((float)count * 100 / total) / 5) * 5;
                dashboard.DanhMuc.Add(categoryName, count);
                dashboard.DanhMucPercent.Add(categoryName, percent);
            }

            ViewBag.Total = total;
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
            taiSan.JoinedTags = string.Join(",", taiSan.Tags);
            return View(taiSan);
        }

        [HttpPost]
        public ActionResult NewOrEditAsset(TaiSan taiSan, string returnUrl)
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
                return View(taiSan);
            }

            var canManage = RoleList.GetAll().Any(User.IsInRole);
            if (taiSan?.IsApproved == true && !canManage)
            {
                return CustomRedirect(returnUrl);
            }

            var types = _typeRepository.Find(m => true).ToList();
            var number = types.First(m => m.Id == TypeConstant.TaiSanSequenceId);
            var currentAsset = _taiSanRepository.Get(taiSan.Id);
            if (currentAsset != null)
            {
                taiSan.Children = currentAsset.Children;
            }
            else
            {
                var existingCodes = _taiSanService.GetExistingCodes();
                taiSan.GenerateCode(existingCodes);
                taiSan.CreatedBy = User.Identity.Name;
                taiSan.Number = ++number.Number;
            }

            taiSan.Tags = string.IsNullOrWhiteSpace(taiSan.JoinedTags) ? new List<string>() : taiSan.JoinedTags.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(m => m.Trim()).ToList();

            _taiSanRepository.Upsert(taiSan);
            _typeRepository.Update(number);

            var newTypes = new List<Application.Models.Type>();
            
            _taiSanService.AddNewType(types, newTypes, TypeConstant.Tags, taiSan.Tags);
            if (newTypes.Any())
                _typeRepository.AddRange(newTypes);

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
                _taiSanService.ExportDataAsync(list.ToList(), search);
            search.pattern = null;

            var baseUrl = search.IsApproved ? nameof(Index) : nameof(TaisanUnApproved);
            return new PagerViewModel
            {
                BaseUrl = Url.Action(baseUrl, search.ToPagingModel()),
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
            ViewBag.GroupName = types.Where(m => m.Name == TypeConstant.Group).Select(m => m.Title).Distinct();
            ViewBag.ChungLoai = types.Where(m => m.Name == TypeConstant.ChungLoai).Select(m => m.Title).Distinct();
            ViewBag.DanhMuc = types.Where(m => m.Name == TypeConstant.Group).Select(m => m.Title).Distinct();
            ViewBag.NguonKinhPhi = types.Where(m => m.Name == TypeConstant.NguonKinhPhi).Select(m => m.Title).Distinct();
            ViewBag.NganSachKhac = types.Where(m => m.Name == TypeConstant.NguonKinhPhiKhac).Select(m => m.Title).Distinct();
            ViewBag.ChatLuong = types.Where(m => m.Name == TypeConstant.ChatLuong).Select(m => m.Title).Distinct();
            ViewBag.LoaiXe = types.Where(m => m.Name == TypeConstant.LoaiXe).Select(m => m.Title).Distinct();
            ViewBag.PhongBan = types.Where(m => m.Name == TypeConstant.PhongBan).Select(m => m.Title).Distinct();
            ViewBag.Tags = types.Where(m => m.Name == TypeConstant.Tags).Select(m => m.Title).Distinct();
            ViewBag.Members = UserManager.Users.Select(m => m.UserName).ToList();
        }
    }
}
