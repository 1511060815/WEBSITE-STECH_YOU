using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HutechAndYou.Models
{
    public class KhachMua
    {
        dbQLBLaptopDataContext data = new dbQLBLaptopDataContext();
        public int iIDKH { set; get; }
        public Double dSoDu { set; get; }
        public KhachMua(int IDKH)
        {
            iIDKH = IDKH;
            KhachHang KH = data.KhachHangs.Single(n => n.IDKH == iIDKH);
            dSoDu = double.Parse(KH.SoTienCo.ToString());
        }
    }
}