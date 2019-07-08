using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACS.Models;

namespace DACS.Controllers
{
    public class DONVILUHANHController : Controller
    {
        // GET: DONVILUHANH
        dbQLDULICHDataContext data = new dbQLDULICHDataContext();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DangkyTH()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangkyTH(FormCollection collection, DONVILUHANH th)
        {
            var tenth = collection["TenTH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            var matkhaunl = collection["Matkhaunhaplai"];
            var diachi = collection["DiaChi"];
            var email = collection["Email"];
            var dienthoai = collection["DienThoai"];
            if (String.IsNullOrEmpty(tenth))
            {
                ViewData["Loi1"] = "Vui lòng nh?p h? và tên";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Vui lòng nh?p tên tài kho?n";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Vui lòng nh?p m?t kh?u";
            }
            else if (String.IsNullOrEmpty(matkhaunl) || matkhau != matkhaunl)
            {
                ViewData["Loi4"] = "M?t kh?u không kh?p";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Vui lòng nh?p email";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Vui lòng nh?p d?a ch?";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "Vui lòng nh?p di?n tho?i";
            }
            else
            {
                th.TenDV = tenth;
                th.Taikhoan = tendn;
                th.MatKhau = matkhau;
                th.Email = email;
                th.DiaChi = diachi;
                th.DienThoai = dienthoai;
                data.DONVILUHANHs.InsertOnSubmit(th);
                data.SubmitChanges();
                return RedirectToAction("DangnhapTH");
            }
            return this.DangkyTH();
        }
        [HttpGet]
        public ActionResult DangNhapTH()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangnhapTH(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Ph?i nh?p tên dang nh?p";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Ph?i nh?p m?t kh?u";
            }
            else
            {
                DONVILUHANH th = data.DONVILUHANHs.SingleOrDefault(n => n.Taikhoan == tendn && n.MatKhau == matkhau);
                if (th != null)
                {
                    ViewBag.Thongbao = "Ðang nh?p thành công";
                    Session["TaikhoanTH"] = th;
                    Session["TentaikhoanTH"] = collection["TenDN"].ToString();
                    return RedirectToAction("Index", "Guest");
                }
                else
                    ViewBag.Thongbao = "Tên dang nh?p ho?c m?t kh?u sai";
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["TaikhoanTH"] = null;
            Session["TentaikhoanTH"] = null;

            return RedirectToAction("Index", "Guest");
        }

        public ActionResult DangKyTHPartial()
        {
            return PartialView();
        }

        public ActionResult DangNhapTHPartial()
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult Themmoisanpham()
        {
            if (Session["TaikhoanTH"] == null || Session["TaikhoanTH"].ToString() == "")
            {
                return RedirectToAction("DangkyTH", "DONVILUHANH");
            }
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaTH = new SelectList(data.DONVILUHANHs.ToList().OrderBy(n => n.TenDV), "MaTH", "TenDV");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisanpham(SANPHAM sanpham, HttpPostedFileBase fileupload)
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaTH = new SelectList(data.DONVILUHANHs.ToList().OrderBy(n => n.TenDV), "MaTH", "TenDV");

            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Img/Sanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sanpham.Anhbia = fileName;
                    data.SANPHAMs.InsertOnSubmit(sanpham);
                    data.SubmitChanges();
                }
            }
            return View();
        }
        public ActionResult ThemmoisanphamTHPartial()
        {
            return PartialView();
        }
        public ActionResult DangXuatTH()
        {
            Session["TaikhoanTH"] = null;
            Session["TentaikhoanTH"] = null;
            Session["Themmoisanpham"] = null;
            return RedirectToAction("Index", "Guest");
        }
    }
}