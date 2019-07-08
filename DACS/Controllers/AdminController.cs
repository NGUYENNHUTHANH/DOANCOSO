using DACS.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACS.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbQLDULICHDataContext data = new dbQLDULICHDataContext();
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        //************LOGIN*****************
        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                ADMIN ad = data.ADMINs.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    Session["TenAdmin"] = collection["username"];
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        public ActionResult DangXuatAdmin()
        {
            Session["TaikhoanAdmin"] = null;
            Session["TenAdmin"] = null;
            return RedirectToAction("Login", "Admin");
        }
        #endregion
        //************************************San pham***************************************************
        #region Sản phẩm
        public ActionResult Sanpham(int? page)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(data.SANPHAMs.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Themmoisanpham()
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
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

        public ActionResult Chitietsanpham(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            SANPHAM sanpham = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }

        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            SANPHAM sanpham = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }

        [HttpPost, ActionName("Xoasanpham")]
        public ActionResult Xacnhanxoa(int id)
        {
            SANPHAM sanpham = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SANPHAMs.DeleteOnSubmit(sanpham);
            data.SubmitChanges();
            return RedirectToAction("Sanpham");
        }

        [HttpGet]
        public ActionResult Suasanpham(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            SANPHAM sanpham = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaTH = new SelectList(data.DONVILUHANHs.ToList().OrderBy(n => n.TenDV), "MaTH", "TenDV");
            return View(sanpham);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasanpham(SANPHAM sanpham, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaTH = new SelectList(data.DONVILUHANHs.ToList().OrderBy(n => n.TenDV), "MaTH", "TenDV");
            if (ModelState.IsValid)
            {

                SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.MaSP == sanpham.MaSP);
                sp.TenSP = sanpham.TenSP;
                sp.Gia = sanpham.Gia;
                sp.Mota = sanpham.Mota;
                if (fileUpload != null)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Img/Sanpham"), fileName);
                    fileUpload.SaveAs(path);
                    sp.Anhbia = fileName;
                }
                sp.Ngaycapnhat = sanpham.Ngaycapnhat;
                sp.Soluong = sanpham.Soluong;
                sp.MaCD = sanpham.MaCD;
                sp.MaTH = sanpham.MaTH;
                UpdateModel(sanpham);
                data.SubmitChanges();
            }
            return RedirectToAction("Sanpham");
        }
        #endregion
        //************************************Thuong hieu************************************************
        #region Thương hiệu
        public ActionResult Thuonghieu(int? page)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 11;
            return View(data.DONVILUHANHs.ToList().OrderBy(n => n.MaTH).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult ThemmoiTH()
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiTH(DONVILUHANH thuonghieu, HttpPostedFileBase fileupload)
        {
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
                    var path = Path.Combine(Server.MapPath("~/Img/ThuongHieu"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    thuonghieu.AnhDV = fileName;
                    data.DONVILUHANHs.InsertOnSubmit(thuonghieu);
                    data.SubmitChanges();
                }
            }
            return View();
        }

        public ActionResult ChitietTH(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            DONVILUHANH thuonghieu = data.DONVILUHANHs.SingleOrDefault(n => n.MaTH == id);
            ViewBag.MaTH = thuonghieu.MaTH;
            if (thuonghieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(thuonghieu);
        }

        [HttpGet]
        public ActionResult XoaTH(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            DONVILUHANH thuonghieu = data.DONVILUHANHs.SingleOrDefault(n => n.MaTH == id);
            ViewBag.MaTH = thuonghieu.MaTH;
            if (thuonghieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(thuonghieu);
        }

        [HttpPost, ActionName("XoaTH")]
        public ActionResult XacnhanxoaTH(int id)
        {
            var MaSP = (from sp in data.SANPHAMs
                        where sp.MaTH == id
                        select sp);
            foreach (var sp in MaSP)
            {
                data.SANPHAMs.DeleteOnSubmit(sp);
            }
            DONVILUHANH thuonghieu = data.DONVILUHANHs.SingleOrDefault(n => n.MaTH == id);
            ViewBag.MaTH = thuonghieu.MaTH;
            if (thuonghieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.DONVILUHANHs.DeleteOnSubmit(thuonghieu);
            data.SubmitChanges();
            return RedirectToAction("Thuonghieu");
        }

        [HttpGet]
        public ActionResult SuaTH(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            DONVILUHANH thuonghieu = data.DONVILUHANHs.SingleOrDefault(n => n.MaTH == id);
            ViewBag.MaTH = thuonghieu.MaTH;
            if (thuonghieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(thuonghieu);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaTH(DONVILUHANH thuonghieu, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                DONVILUHANH th = data.DONVILUHANHs.SingleOrDefault(n => n.MaTH == thuonghieu.MaTH);
                th.TenDV = thuonghieu.TenDV;
                if (fileUpload != null)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Img/ThuongHieu"), fileName);
                    fileUpload.SaveAs(path);
                    th.AnhDV = fileName;
                }
                th.DienThoai = thuonghieu.DienThoai;
                UpdateModel(thuonghieu);
                data.SubmitChanges();
            }
            return RedirectToAction("ThuongHieu");
        }
        #endregion
        //**************************************Chu de***************************************************
        #region Chủ đề
        public ActionResult Chude(int? page)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(data.CHUDEs.ToList().OrderBy(n => n.MaCD).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult ThemmoiCD()
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.MaPL = new SelectList(data.PHANLOAIs.ToList().OrderBy(n => n.TenPL), "MaPL", "TenPL");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiCD(CHUDE chude, HttpPostedFileBase fileupload)
        {
            ViewBag.MaPL = new SelectList(data.PHANLOAIs.ToList().OrderBy(n => n.TenPL), "MaPL", "TenPL");
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
                    var path = Path.Combine(Server.MapPath("~/Img/ChuDe"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    chude.AnhCD = fileName;
                    data.CHUDEs.InsertOnSubmit(chude);
                    data.SubmitChanges();
                }
            }
            return View();
        }

        public ActionResult ChitietCD(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            CHUDE chude = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.MaCD = chude.MaCD;
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chude);
        }

        [HttpGet]
        public ActionResult XoaCD(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            CHUDE chude = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.MaCD = chude.MaCD;
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chude);
        }

        [HttpPost, ActionName("XoaCD")]
        public ActionResult XacnhanxoaCD(int id)
        {
            var MaSP = (from sp in data.SANPHAMs
                        where sp.MaCD == id
                        select sp);
            foreach (var sp in MaSP)
            {
                data.SANPHAMs.DeleteOnSubmit(sp);
            }
            CHUDE chude = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.MaCD = chude.MaCD;
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.CHUDEs.DeleteOnSubmit(chude);
            data.SubmitChanges();
            return RedirectToAction("Chude");
        }

        [HttpGet]
        public ActionResult SuaCD(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.MaPL = new SelectList(data.PHANLOAIs.ToList().OrderBy(n => n.TenPL), "MaPL", "TenPL");
            CHUDE chude = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.MaCD = chude.MaCD;
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chude);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaCD(CHUDE chude, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaPL = new SelectList(data.PHANLOAIs.ToList().OrderBy(n => n.TenPL), "MaPL", "TenPL");
            if (ModelState.IsValid)
            {
                CHUDE cd = data.CHUDEs.SingleOrDefault(n => n.MaCD == chude.MaCD);
                cd.TenChuDe = chude.TenChuDe;
                if (fileUpload != null)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Img/Chude"), fileName);
                    fileUpload.SaveAs(path);
                    cd.AnhCD = fileName;
                }
                cd.MaPL = chude.MaPL;
                UpdateModel(chude);
                data.SubmitChanges();
            }
            return RedirectToAction("Chude");
        }
        #endregion
        //*************************************Phan loai*************************************************
        #region Phân loại
        public ActionResult Phanloai(int? page)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 11;
            return View(data.PHANLOAIs.ToList().OrderBy(n => n.MaPL).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult ThemmoiPL()
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiPL(PHANLOAI phanloai)
        {
            if (ModelState.IsValid)
            {
                data.PHANLOAIs.InsertOnSubmit(phanloai);
                data.SubmitChanges();
            }
            return View();
        }

        public ActionResult ChitietPL(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            PHANLOAI phanloai = data.PHANLOAIs.SingleOrDefault(n => n.MaPL == id);
            ViewBag.MaPL = phanloai.MaPL;
            if (phanloai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phanloai);
        }

        [HttpGet]
        public ActionResult XoaPL(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            PHANLOAI phanloai = data.PHANLOAIs.SingleOrDefault(n => n.MaPL == id);
            ViewBag.MaPL = phanloai.MaPL;
            if (phanloai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phanloai);
        }

        [HttpPost, ActionName("XoaPL")]
        public ActionResult XacnhanxoaPL(int id)
        {
            var MaSP = (from sp in data.SANPHAMs
                        join cd in data.CHUDEs on sp.MaCD equals cd.MaCD
                        join pl in data.PHANLOAIs on cd.MaPL equals pl.MaPL
                        where pl.MaPL == id
                        select sp);
            foreach (var sp in MaSP)
            {
                data.SANPHAMs.DeleteOnSubmit(sp);
            }
            var MaCD = (from cd in data.CHUDEs
                        where cd.MaPL == id
                        select cd);
            foreach (var cd in MaCD)
            {
                data.CHUDEs.DeleteOnSubmit(cd);
            }
            PHANLOAI phanloai = data.PHANLOAIs.SingleOrDefault(n => n.MaPL == id);
            ViewBag.MaPL = phanloai.MaPL;
            if (phanloai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.PHANLOAIs.DeleteOnSubmit(phanloai);
            data.SubmitChanges();
            return RedirectToAction("Phanloai");
        }

        [HttpGet]
        public ActionResult SuaPL(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            PHANLOAI phanloai = data.PHANLOAIs.SingleOrDefault(n => n.MaPL == id);
            ViewBag.MaPL = phanloai.MaPL;
            if (phanloai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phanloai);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaPL(PHANLOAI phanloai)
        {
            if (ModelState.IsValid)
            {
                PHANLOAI pl = data.PHANLOAIs.SingleOrDefault(n => n.MaPL == phanloai.MaPL);
                pl.TenPL = phanloai.TenPL;
                UpdateModel(phanloai);
                data.SubmitChanges();
            }
            return RedirectToAction("Chude");
        }
        #endregion
        //*************************************Khach hang************************************************
        #region Khách hàng
        public ActionResult Khachhang(int? page)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 11;
            return View(data.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult ThemmoiKH()
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiKH(KHACHHANG khachhang)
        {
            if (ModelState.IsValid)
            {
                data.KHACHHANGs.InsertOnSubmit(khachhang);
                data.SubmitChanges();
            }
            return View();
        }

        public ActionResult ChitietKH(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            KHACHHANG khachhang = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = khachhang.MaKH;
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }

        [HttpGet]
        public ActionResult XoaKH(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            KHACHHANG khachhang = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = khachhang.MaKH;
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }

        [HttpPost, ActionName("XoaKH")]
        public ActionResult XacnhanxoaKH(int id)
        {
            var ChiTietDonDatHang = (from ctdh in data.CHITIETDONHANGs
                                     join ddh in data.DONDATHANGs on ctdh.MaDonHang equals ddh.MaDonHang
                                     where ddh.MaKH == id
                                     select ctdh);
            foreach (var ctddh in ChiTietDonDatHang)
            {
                data.CHITIETDONHANGs.DeleteOnSubmit(ctddh);
            }
            var DonDatHang = (from ddh in data.DONDATHANGs
                              where ddh.MaKH == id
                              select ddh);
            foreach (var ddh in DonDatHang)
            {
                data.DONDATHANGs.DeleteOnSubmit(ddh);
            }
            KHACHHANG khachhang = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = khachhang.MaKH;
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.KHACHHANGs.DeleteOnSubmit(khachhang);
            data.SubmitChanges();
            return RedirectToAction("Khachhang");
        }

        [HttpGet]
        public ActionResult SuaKH(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            KHACHHANG khachhang = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = khachhang.MaKH;
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaKH(KHACHHANG khachhang)
        {
            if (ModelState.IsValid)
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == khachhang.MaKH);
                kh.HoTen = khachhang.HoTen;
                kh.Taikhoan = khachhang.Taikhoan;
                kh.Matkhau = khachhang.Matkhau;
                kh.Email = khachhang.Email;
                kh.DiachiKH = khachhang.DiachiKH;
                kh.DienthoaiKH = khachhang.DienthoaiKH;
                kh.Ngaysinh = khachhang.Ngaysinh;
                UpdateModel(khachhang);
                data.SubmitChanges();
            }
            return RedirectToAction("Khachhang");
        }
        #endregion
        //****************************************Admin**************************************************
        #region Admin
        public ActionResult ListAdmin(int? page)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 11;
            return View(data.ADMINs.ToList().OrderBy(n => n.MaAdmin).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult ThemmoiAdmin()
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiAdmin(ADMIN admin)
        {
            if (ModelState.IsValid)
            {
                data.ADMINs.InsertOnSubmit(admin);
                data.SubmitChanges();
            }
            return View();
        }

        public ActionResult ChitietAdmin(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ADMIN admin = data.ADMINs.SingleOrDefault(n => n.MaAdmin == id);
            ViewBag.MaAdmin = admin.MaAdmin;
            if (admin == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(admin);
        }

        [HttpGet]
        public ActionResult XoaAdmin(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ADMIN admin = data.ADMINs.SingleOrDefault(n => n.MaAdmin == id);
            ViewBag.MaAdmin = admin.MaAdmin;
            if (admin == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(admin);
        }

        [HttpPost, ActionName("XoaAdmin")]
        public ActionResult XacnhanxoaAdmin(int id)
        {
            ADMIN admin = data.ADMINs.SingleOrDefault(n => n.MaAdmin == id);
            ViewBag.MaAdmin = admin.MaAdmin;
            if (admin == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.ADMINs.DeleteOnSubmit(admin);
            data.SubmitChanges();
            return RedirectToAction("ListAdmin");
        }

        [HttpGet]
        public ActionResult SuaAdmin(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ADMIN admin = data.ADMINs.SingleOrDefault(n => n.MaAdmin == id);
            ViewBag.MaAdmin = admin.MaAdmin;
            if (admin == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(admin);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaAdmin(ADMIN admin)
        {
            if (ModelState.IsValid)
            {
                ADMIN ad = data.ADMINs.SingleOrDefault(n => n.MaAdmin == admin.MaAdmin);
                ad.UserAdmin = admin.UserAdmin;
                ad.PassAdmin = admin.PassAdmin;
                ad.Hoten = admin.Hoten;
                UpdateModel(admin);
                data.SubmitChanges();
            }
            return RedirectToAction("ListAdmin");
        }
        #endregion
        //************************************Don dat hang***********************************************
        #region Đơn đặt hàng
        public ActionResult Dondathang(int? page)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 11;
            return View(data.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ChitietDH(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            DONDATHANG dondathang = data.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            ViewBag.MaDonHang = dondathang.MaDonHang;
            if (dondathang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dondathang);
        }

        public ActionResult Chitietdonhang(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var lstChitietDH = data.CHITIETDONHANGs.Where(n => n.MaDonHang == id).ToList();
            return View(lstChitietDH);
        }

        [HttpGet]
        public ActionResult XoaDH(int id)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            DONDATHANG dondathang = data.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            ViewBag.MaDonHang = dondathang.MaDonHang;
            if (dondathang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dondathang);
        }

        [HttpPost, ActionName("XoaDH")]
        public ActionResult XacnhanxoaDH(int id)
        {
            var MaDonHang = (from ctdh in data.CHITIETDONHANGs
                             where ctdh.MaDonHang == id
                             select ctdh);
            foreach (var ddh in MaDonHang)
            {
                data.CHITIETDONHANGs.DeleteOnSubmit(ddh);
            }
            DONDATHANG dondathang = data.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            ViewBag.MaDonHang = dondathang.MaDonHang;
            if (dondathang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.DONDATHANGs.DeleteOnSubmit(dondathang);
            data.SubmitChanges();
            return RedirectToAction("Dondathang");
        }
        #endregion
    }
}