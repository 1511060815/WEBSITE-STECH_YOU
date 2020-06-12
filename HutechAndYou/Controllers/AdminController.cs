using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HutechAndYou.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.UI.WebControls;

namespace HutechAndYou.Controllers
{
    public class AdminController : Controller
    {
        dbQLBLaptopDataContext data = new dbQLBLaptopDataContext();
        public ActionResult Index()
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            return View();
        }

        //List Lien he
        public ActionResult LienHe(int? page)
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var Lienhe = from s in data.Lienhes                       
                          select s;
            return View(Lienhe.ToPagedList(pageNum, pageSize));

        }

        //List sản phẩm
        public ActionResult SanPham(int? page)
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var SanPham = from s in data.SanPhams
                         select s;
            return View(SanPham.ToPagedList(pageNum, pageSize));

        }

        //List đơn đặt hàng
        [HttpGet]
        public ActionResult DonDatHang(int? page)
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var donhang = from s in data.DonDatHangs
                          select s;
            return View(donhang.ToPagedList(pageNum, pageSize));
        }
        [HttpGet, ActionName("xacnhangiaohang")]
        public ActionResult DonDatHang(int id)
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            DonDatHang ddh = data.DonDatHangs.SingleOrDefault(n => n.MaDH == id);
            ddh.TinhTrangGiaoHang = 3;
            ddh.NgayGiao = DateTime.Now;
            UpdateModel(ddh);
            data.SubmitChanges();
            return RedirectToAction("DonDatHang", "Admin");
        }

        //Chi tiết đơn đặt hàng
        public ActionResult ChiTietDDH(int ID ,int ? page)
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var CTDDH = from s in data.ChiTietDDHs
                          where s.MaDH == ID
                          select s;
            return View(CTDDH.ToPagedList(pageNum, pageSize));
        }

        //Đăng Nhập
        [HttpGet]
        public ActionResult AdminLogin() 
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(FormCollection collection)
        {
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            if (string.IsNullOrEmpty(tendangnhap))
            {
                ViewData["loi"] = "bạn hãy nhập tài khoản";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["loi"] = "bạn hãy nhập mật khẩu";
            }
            else
            {
                QuanTri qt = data.QuanTris.SingleOrDefault(n => n.tentk == tendangnhap && n.matkhautk == matkhau);
                if (qt != null)
                {
                    ViewData["loi"] = "đăng nhập thành công";
                    Session["TaikhoanAdmin"] = qt;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewData["loi"] = "mật khẩu hoặc tài khoản của bạn đã sai";
                }
            }
            return View();
        }

        //Them san pham
        [HttpGet]
        public ActionResult ThemSanPham()
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            ViewBag.MaHang = new SelectList(data.Hangs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSanPham(FormCollection collection, SanPham sp, HttpPostedFileBase fileUpload, HttpPostedFileBase fileUpload1, HttpPostedFileBase fileUpload2, HttpPostedFileBase fileUpload3)
        {
            var tensp = collection["tensp"];
            var tenanh = collection["tenanh"];
            var ngaynhap = String.Format("{0:MM/dd/yyyy}", collection["ngaynhap"]);
            var soluong = collection["soluong"];
            var giaban = collection["giaban"];
            var cpu = collection["cpu"];
            var loaicpu = collection["loaicpu"];
            var ram = collection["ram"];
            var card = collection["card"];
            var manhinh = collection["manhinh"];
            var ocung = collection["ocung"];
            var noidung = collection["noidung"];
            var result = data.SanPhams.Where(x => x.TenSP.Equals(tensp)).ToList();
            if (string.IsNullOrEmpty(tensp))
            {
                ViewData["loi"] = "vui lòng nhập tên sản phẩm";
            }
            else if (string.IsNullOrEmpty(ngaynhap))
            {
                ViewData["loi"] = "hãy nhập đầy đủ thông tin";
            }
            else if (string.IsNullOrEmpty(giaban))
            {
                ViewData["loi"] = "vui lòng nhập giá bán";
            }
            else if (string.IsNullOrEmpty(cpu))
            {
                ViewData["loi"] = "cpu của laptop là gì?";
            }
            else if (string.IsNullOrEmpty(loaicpu))
            {
                ViewData["loi"] = "loại cpu của laptop là gì?";
            }
            else if (string.IsNullOrEmpty(ram))
            {
                ViewData["loi"] = "dung lượng ram là bao nhiêu?";
            }
            else if (string.IsNullOrEmpty(card))
            {
                ViewData["loi"] = "crad màn hình là loại gì?";
            }
            else if (string.IsNullOrEmpty(manhinh))
            {
                ViewData["loi"] = "kích thước màn hình của laptop";
            }
            else if (string.IsNullOrEmpty(ocung))
            {
                ViewData["loi"] = "dung lượng ổ cứng là bao nhiêu?";
            }
            else if (result.Count == 0)
            {
                if (ModelState.IsValid && fileUpload != null && fileUpload1 != null && fileUpload2 != null && fileUpload3 != null)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var fileName1 = Path.GetFileName(fileUpload1.FileName);
                    var fileName2 = Path.GetFileName(fileUpload2.FileName);
                    var fileName3 = Path.GetFileName(fileUpload3.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinh Anh/SanPham"), fileName);
                    var path1 = Path.Combine(Server.MapPath("~/Hinh Anh/SanPham"), fileName1);
                    var path2 = Path.Combine(Server.MapPath("~/Hinh Anh/SanPham"), fileName2);
                    var path3 = Path.Combine(Server.MapPath("~/Hinh Anh/SanPham"), fileName3);
                    if (System.IO.File.Exists(path))
                    {
                        ViewData["loi"] = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                        fileUpload1.SaveAs(path1);
                        fileUpload2.SaveAs(path2);
                        fileUpload3.SaveAs(path3);
                        sp.TenSP = tensp;
                        sp.LoaiCpu = loaicpu;
                        sp.CPU = cpu;
                        sp.NgayCapNhat = DateTime.Parse(ngaynhap);
                        sp.Ram = ram;
                        sp.NoiDung = noidung;
                        sp.ManHinh = manhinh;
                        sp.OCung = ocung;
                        sp.Crad = card;
                        sp.GiaBan = int.Parse(giaban);
                        sp.Soluongton = int.Parse(soluong);
                        sp.AnhMH = fileName;
                        sp.AnhMH1 = fileName1;
                        sp.AnhMH2 = fileName2;
                        sp.AnhMH3 = fileName3;
                        data.SanPhams.InsertOnSubmit(sp);
                        data.SubmitChanges();
                        return RedirectToAction("SanPham","Admin");
                    }
                }
                else
                {
                    ViewData["loi"] = "Chưa có ảnh minh họa";
                }
            }
            else
            {
                ViewData["loi"] = "tên laptop đã được dùng";
            }
            ViewBag.MaHang = new SelectList(data.Hangs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            return this.ThemSanPham();
        }

        //Sua San Pham
        [HttpGet]
        public ActionResult SuaSanPham(int id)
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaHang = new SelectList(data.Hangs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            return View(sp);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSanPham(FormCollection collection, HttpPostedFileBase fileUpload, HttpPostedFileBase fileUpload1, HttpPostedFileBase fileUpload2, HttpPostedFileBase fileUpload3, int id)
        {
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaSP == id);
            var tensp = collection["tensp"];
            var tenanh = collection["tenanh"];
            var soluong = collection["soluong"];
            var giaban = collection["giaban"];
            var cpu = collection["cpu"];
            var loaicpu = collection["loaicpu"];
            var ram = collection["ram"];
            var card = collection["card"];
            var manhinh = collection["manhinh"];
            var ocung = collection["ocung"];
            var noidung = collection["noidung"];
            ViewBag.MaHang = new SelectList(data.Hangs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            if (fileUpload != null)
            {
                var fileName = Path.GetFileName(fileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Hinh Anh/SanPham"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewData["loi"] = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload.SaveAs(path);
                    sp.AnhMH = fileName;
                }
            }
            if (fileUpload1 != null)
            {
                var fileName1 = Path.GetFileName(fileUpload1.FileName);
                var path1 = Path.Combine(Server.MapPath("~/Hinh Anh/SanPham"), fileName1);
                if (System.IO.File.Exists(path1))
                {
                    ViewData["loi"] = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload1.SaveAs(path1);
                    sp.AnhMH1 = fileName1;
                }
            }
            if (fileUpload2 != null)
            {
                var fileName2 = Path.GetFileName(fileUpload2.FileName);
                var path2 = Path.Combine(Server.MapPath("~/Hinh Anh/SanPham"), fileName2);
                if (System.IO.File.Exists(path2))
                {
                    ViewData["loi"] = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload2.SaveAs(path2);
                    sp.AnhMH2 = fileName2;
                }
            }
            if (fileUpload3 != null)
            {
                var fileName3 = Path.GetFileName(fileUpload3.FileName);
                var path3 = Path.Combine(Server.MapPath("~/Hinh Anh/SanPham"), fileName3);
                if (System.IO.File.Exists(path3))
                {
                    ViewData["loi"] = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload3.SaveAs(path3);
                    sp.AnhMH3 = fileName3;
                }
              }
                sp.TenSP = tensp;
                sp.LoaiCpu = loaicpu;
                sp.CPU = cpu;
                sp.Ram = ram;
                sp.NoiDung = noidung;
                sp.ManHinh = manhinh;
                sp.OCung = ocung;
                sp.Crad = card;
                sp.GiaBan = int.Parse(giaban);
                sp.Soluongton = int.Parse(soluong);
                UpdateModel(sp);
                data.SubmitChanges();
            return RedirectToAction("SanPham", "Admin"); ;
        }

            //Chi tiet san pham
            public ActionResult ChiTiet(int ID)
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaSP == ID);
            ViewBag.MaSP = sp.MaSP;
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);

        }


        // Xóa sản phẩm
        public ActionResult XoaSanPham(int id)
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost, ActionName("xoasanpham")]
        public ActionResult XNXoaSanPham(int id)
        {
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SanPhams.DeleteOnSubmit(sp);
            data.SubmitChanges();
            return RedirectToAction("SanPham","Admin");
        }

        //List tin tức
        public ActionResult Tintuc(int? page)
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var tintuc = from s in data.Tintucs
                          select s;
            return View(tintuc.ToPagedList(pageNum, pageSize));

        }

        // Xóa tin tức
        public ActionResult XoaTinTuc(int id)
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            Tintuc sp = data.Tintucs.SingleOrDefault(n => n.MaTinTuc == id);
            ViewBag.MaTinTuc = sp.MaTinTuc;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost, ActionName("xoatintuc")]
        public ActionResult XNXoaTinTuc(int id)
        {
            Tintuc sp = data.Tintucs.SingleOrDefault(n => n.MaTinTuc == id);
            ViewBag.MaSP = sp.MaTinTuc;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.Tintucs.DeleteOnSubmit(sp);
            data.SubmitChanges();
            return RedirectToAction("Tintuc","Admin");
        }

        //Them tin tuc
        [HttpGet]
        public ActionResult ThemTinTuc()
        {
            if (Session["TaikhoanAdmin"] == null || Session["TaikhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemTinTuc(FormCollection collection, Tintuc  sp, HttpPostedFileBase fileUpload)
        {
            var tensp = collection["tensp"];
            var noidung = collection["noidung"];
            var noidung1 = collection["noidung1"];
            var noidung2 = collection["noidung2"];
            var link = collection["link"];
            var result = data.Tintucs.Where(x => x.TieuDe.Equals(tensp)).ToList();
            if (string.IsNullOrEmpty(tensp))
            {
                ViewData["loi"] = "bạn hãy nhập tiêu đề";
            }
            else if (string.IsNullOrEmpty(noidung))
            {
                ViewData["loi"] = "bạn hãy nhập nội dung thứ nhất";
            }
            else if (string.IsNullOrEmpty(noidung1))
            {
                ViewData["loi"] = "bạn hãy nhập nội dung thứ 2";
            }
            else if (string.IsNullOrEmpty(noidung2))
            {
                ViewData["loi"] = "bạn hãy nội dung thứ 3";
            }
            else if (string.IsNullOrEmpty(link))
            {
                ViewData["loi"] = "bạn hãy nhập link liên kết";
            }
            else if (result.Count == 0)
            {
                if (ModelState.IsValid && fileUpload != null)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinh Anh/Gioi Thieu"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewData["loi"] = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                        sp.TieuDe = tensp;
                        sp.NoiDung = noidung;
                        sp.Noidung2 = noidung1;
                        sp.Noidung3 = noidung2;
                        sp.Link = link;
                        sp.AnhTinTuc = fileName;
                        data.Tintucs.InsertOnSubmit(sp);
                        data.SubmitChanges();
                        return RedirectToAction("Tintuc", "Admin");
                    }
                }
                else
                {
                    ViewData["loi"] = "Chưa có ảnh minh họa";
                }
            }
            else
            {
                ViewData["loi"] = "tiêu đề tin tức đã được sử dụng";
            }
            return this.ThemTinTuc();
        }

    }
}
