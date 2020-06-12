using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HutechAndYou.Models
{
    public class DonHang
    {
        dbQLBLaptopDataContext data = new dbQLBLaptopDataContext();
        public int iIDKH { set; get; }
        public string dNgayDat { set; get; }
        public string dNgayGiao { set; get; }
        public string sTinhTrangGH { set; get; }
        public int iMaDH { set; get; }
        public DonHang(int IDKH)
        {
            iIDKH = IDKH;
            DonDatHang DH = data.DonDatHangs.Single(n => n.IDKH == iIDKH);
            iMaDH = DH.MaDH;
            dNgayDat = String.Format("{0:MM/dd/yyyy}", DH.NgayDat);
            dNgayGiao = String.Format("{0:MM/dd/yyyy}", DH.NgayGiao);
        }
    }
}