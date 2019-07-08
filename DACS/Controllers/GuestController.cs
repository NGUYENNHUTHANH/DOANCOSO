using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACS.Models;
using PagedList;
using PagedList.Mvc;
using Newtonsoft.Json;

namespace DACS.Controllers
{
    public class GuestController : Controller
    {
        dbQLDULICHDataContext data = new dbQLDULICHDataContext();
        // GET: Guest
        private List<SANPHAM> layspmoi(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);

            var spmoi = layspmoi(15);
            return View(spmoi.ToPagedList(pageNum, pageSize));
        }

        public ActionResult LienHe()
        {
            return View();
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult Phanloai()
        {
            var phanloai = from cd in data.PHANLOAIs select cd;
            return PartialView(phanloai);
        }
        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult LUHANH()
        {
            var thuonghieu = from th in data.DONVILUHANHs select th;
            return PartialView(thuonghieu);
        }
        public ActionResult SPTheophanloai(int id)
        {
            var chude = from cd in data.CHUDEs where cd.MaPL == id select cd;
            return View(chude);
        }
        public ActionResult SPTheochude(int id)
        {
            var sanpham = from sp in data.SANPHAMs where sp.MaCD == id select sp;
            return View(sanpham);
        }
        public ActionResult SPluhanh(int id)
        {
            var sanpham = from sp in data.SANPHAMs where sp.MaTH == id select sp;
            return View(sanpham);
        }
        public ActionResult Details(int id)
        {
            var sanpham = from sp in data.SANPHAMs
                          where sp.MaSP == id
                          select sp;
            return View(sanpham.Single());
        }
        public JsonResult SearchSanPham(string term)
        {
            var a = data.SANPHAMs.Where(n => n.TenSP.Contains(term)).Select(n => new { n.TenSP, n.Anhbia, n.MaSP }).ToList();
            return Json(new
            {
                data = a,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }

}