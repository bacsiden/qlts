﻿using DK.Application;
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
using Newtonsoft.Json;

namespace DK.Web.Controllers
{
    [Authorize]
    public class KiemKeController : BaseController
    {
        private readonly string HtmlFolder = System.Web.HttpContext.Current.Server.MapPath("~/Html\\");
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
            var list = _typeRepository.Find(m => m.Name == TypeConstant.KiemKe);
            var pager = new Pager(list.Count(), pagerInfo.PageIndex, pagerInfo.PageSize);

            var result = new PagerViewModel
            {
                BaseUrl = Url.Action("Index", new { pagerInfo.PageSize }),
                Items = list.OrderByDescending(m => m.Created).Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };

            return View(result);
        }

        public async Task<ActionResult> Detail(Guid id, DefaultSearchModel pagerInfo)
        {
            var kiemke = await _typeRepository.GetAsync(id);
            ViewBag.KiemKe = kiemke;

            var list = _kiemKeRepository.Find(m => m.KiemKeId == kiemke.Id).OrderBy(m => m.Number);
            var pager = new Pager(list.Count(), pagerInfo.PageIndex, pagerInfo.PageSize);

            var result = new PagerViewModel
            {
                BaseUrl = Url.Action("Detail", new { id, pagerInfo.PageSize }),
                Items = list.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };

            return View(result);
        }

        public ActionResult Merge(List<Guid> kiemKeIds, string name, string returnUrl)
        {
            if (!kiemKeIds.Any()) return Redirect(returnUrl);
            var kiemKes = new List<KiemKe>();
            foreach (var item in kiemKeIds)
            {
                kiemKes.AddRange(_kiemKeRepository.Find(m => m.KiemKeId == item));
            }
            var kiemKeLst = new List<KiemKe>();
            var kkType = _typeRepository.Add(new Application.Models.Type { Title = name, Name = TypeConstant.KiemKe, CreatedBy = User.Identity.Name });
            foreach (var item in kiemKes)
            {
                var kk = kiemKeLst.FirstOrDefault(m => m.Code == item.Code);
                if (kk == null)
                {
                    kiemKeLst.Add(item);
                    item.KiemKeId = kkType.Id;
                }
                else if (kk.SoLuongKiemKe < item.SoLuongKiemKe)
                {
                    var index = kiemKeLst.FindIndex(m => m.Id == kk.Id);
                    kiemKeLst[index].SoLuongKiemKe = item.SoLuongKiemKe;
                }
            }
            foreach (var item in kiemKeIds)
            {
                _kiemKeRepository.DeleteMany(nameof(KiemKe.KiemKeId), item);
                _typeRepository.Delete(item);
            }
            _kiemKeRepository.AddRange(kiemKeLst);

            return Redirect(returnUrl);
        }

        public string PushData(Guid id)
        {
            try
            {
                var kiemKes = _kiemKeRepository.Find(m => m.KiemKeId == id);
                string output = JsonConvert.SerializeObject(kiemKes);
                var devices = MediaDevice.GetDevices();
                using (var device = devices.First())
                {
                    device.Connect();
                    var filename = $@"Bộ nhớ trong dùng chung\Download\kiem_ke.json";
                    using (StreamWriter w = new StreamWriter($"{HtmlFolder}KiemKe.json", false))
                    {
                        w.Write(output);
                    }
                    if (device.FileExists(filename)) device.DeleteFile(filename);
                    device.UploadFile($"{HtmlFolder}KiemKe.json", filename);
                    device.Disconnect();
                }

                return null;
            }
            catch (Exception e)
            {
                var message = e.Message;
                if (message?.Equals("Sequence contains no elements", StringComparison.OrdinalIgnoreCase) == true)
                    message = "Thiết bị kiếm kê chưa kết nối với máy chủ";
                return "Có lỗi khi xuất dữ liệu ra thiết bị. " + message;
            }
        }

        public string PullData(Guid id)
        {
            try
            {
                var type = _typeRepository.Get(id);
                var kiemKes = _kiemKeRepository.Find(m => m.KiemKeId == id).ToList();
                var devices = MediaDevice.GetDevices();
                using (var device = devices.First())
                {
                    device.Connect();
                    var filename = $@"Bộ nhớ trong dùng chung\Download\kiem_ke.json";
                    if (!device.FileExists(filename)) throw new Exception($"Không tìm thấy đợt kiểm kê '{type.Title}' từ thiết bị kiểm kê");
                    var serializer = new JsonSerializer();
                    var memorySteam = new MemoryStream();
                    device.DownloadFile(filename, $"{HtmlFolder}KiemKe.json");
                    var str = new StreamReader($"{HtmlFolder}KiemKe.json");
                    using (var jsonTextReader = new JsonTextReader(str))
                    {
                        var lstKiemKe = serializer.Deserialize<List<KiemKe>>(jsonTextReader);
                        foreach (var item in lstKiemKe)
                        {
                            var kk = kiemKes.FirstOrDefault(m => m.Code == item.Code);
                            if (kk != null && kk.SoLuongKiemKe != item.SoLuongKiemKe)
                            {
                                _kiemKeRepository.Set(item.Id, nameof(KiemKe.SoLuongKiemKe), item.SoLuongKiemKe);
                            }
                        }
                    }
                    memorySteam.Close();
                    str.Close();

                    device.Disconnect();
                }

                return null;
            }
            catch (Exception e)
            {
                var message = e.Message;
                if (message?.Equals("Sequence contains no elements", StringComparison.OrdinalIgnoreCase) == true)
                    message = "Thiết bị kiếm kê chưa kết nối với máy chủ";
                return "Có lỗi khi đọc dữ liệu từ thiết bị. " + message;
            }
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

        public async Task<ActionResult> Export(Guid id, string pattern, bool preview = false)
        {
            var kiemKes = _kiemKeRepository.Find(m => m.KiemKeId == id).OrderBy(m => m.Number).ToList();
            await _taiSanService.ExportKiemKeAsync(kiemKes, pattern, preview);
            return RedirectToAction(nameof(Detail), new { id = id });
        }
        public ActionResult Preview(Guid id, string pattern)
        {
            var kiemKes = _kiemKeRepository.Find(m => m.KiemKeId == id).ToList();
            _taiSanService.ExportKiemKeAsync(kiemKes, pattern, true).GetAwaiter().GetResult();
            var html = System.IO.File.ReadAllText($"{HtmlFolder}{pattern}.html");
            return View(model: html);
        }

        private IEnumerable<KiemKe> GetListKiemKes(Guid kiemKeId, IEnumerable<TaiSan> list)
        {
            return list.AsEnumerable().Select(m => new KiemKe
            {
                TaiSanId = m.Id,
                KiemKeId = kiemKeId,
                No = 0,
                Code = m.Code,
                Number = m.Number,
                NguoiQuanLy = m.NguoiQuanLy,
                NguoiSuDung = m.NguoiSuDung,
                DongVi = m.PhongQuanLy,
                Name = m.Name,
                GroupName = m.GroupName,
                NamSuDung = m.NamSuDung,
                SoLuongKeToan = m.SoLuong,
                NguyenGiaKeToan = m.NguyenGiaKeToan,
                GiaTriConLaiKeToan = m.GiaTriConLai
            });
        }
    }
}
