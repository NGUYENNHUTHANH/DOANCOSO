using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DACS.Models
{
    public class GioHang
    {
        dbQLDULICHDataContext data = new dbQLDULICHDataContext();
        public int iMaSP { set; get; }
        public string sTenSP { set; get; }
        public string sAnhbia { get; set; }
        public Double dDongia { get; set; }
        public int iSoluong { get; set; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        public GioHang(int MasP)
        {
            iMaSP = MasP;
            SANPHAM sanpham = data.SANPHAMs.Single(n => n.MaSP == iMaSP);
            sTenSP = sanpham.TenSP;
            sAnhbia = sanpham.Anhbia;
            dDongia = double.Parse(sanpham.Gia.ToString());
            iSoluong = 1;
        }
    }
}