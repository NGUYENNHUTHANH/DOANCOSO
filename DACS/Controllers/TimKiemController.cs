using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACS.Models;
using PagedList.Mvc;
using PagedList;

namespace DACS.Controllers
{
    public class TimKiemController : Controller
    {
        dbQLDULICHDataContext db = new dbQLDULICHDataContext();
        [HttpPost]
        // GET: TimKiem
        public ActionResult KetQuaTimKiem(FormCollection f, int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.TuKhoa = sTuKhoa;
            List<SANPHAM> lstKQTK = db.SANPHAMs.Where(n => n.TenSP.Contains(sTuKhoa)).ToList();
            //phân trang
           
            int pageSize = 6;
            int pageNum = (page ?? 1);
            if (lstKQTK.Count ==0)
            {
                ViewBag.ThongBao = "Không tìm thấy kết quả nào ";
                return View(db.SANPHAMs.OrderBy(n => n.TenSP).ToPagedList(pageNum, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " Kết quả";
            return View(lstKQTK.OrderBy(n => n.TenSP).ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        // GET: TimKiem
        public ActionResult KetQuaTimKiem(string sTuKhoa, int? page)
        {
            ViewBag.TuKhoa = sTuKhoa;
            List<SANPHAM> lstKQTK = db.SANPHAMs.Where(n => n.TenSP.Contains(sTuKhoa)).ToList();
            //phân trang

            int pageSize = 6;
            int pageNum = (page ?? 1);
            if (lstKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy kết quả nào ";
                return View(db.SANPHAMs.OrderBy(n => n.TenSP).ToPagedList(pageNum, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + "Kết quả";
            return View(lstKQTK.OrderBy(n => n.TenSP).ToPagedList(pageNum, pageSize));
        }

    }
}