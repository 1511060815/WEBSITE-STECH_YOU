using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HutechAndYou.Controllers
{
    public class LoginController : Controller
    {
        dbQLBLaptopDataContext data = new dbQLBLaptopDataContext();
        //Đăng Nhập
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
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
                KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendangnhap && n.MatKhau == matkhau);
                if (kh != null)
                {
                    ViewData["loi"] = "đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("DangNhapTC","Home");
                }
                else
                {
                    ViewData["loi"] = "mật khẩu hoặc tài khoản của bạn đã sai";
                }
            }
            return View();
        }

        //Đăng Ký
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var tendangnhap = collection["tendangnhap"];
            var email = collection["email"];
            var hoten = collection["hoten"];
            var matkhau = collection["matkhau"];
            var sdt = collection["sdt"];
            var diachi = collection["diachi"];
            var result = data.KhachHangs.Where(x => x.TaiKhoan.Equals(tendangnhap)).ToList();
            var result2 = data.KhachHangs.Where(x => x.Email.Equals(email)).ToList();
            if (string.IsNullOrEmpty(tendangnhap))
            {
                ViewData["loi"] = "bạn hãy nhập tên tài khoản";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["loi"] = "bạn hãy nhập mật khẩu";
            }
            else if (string.IsNullOrEmpty(email))
            {
                ViewData["loi"] = "bạn hãy nhập email";
            }
            else if (string.IsNullOrEmpty(hoten))
            {
                ViewData["loi"] = "bạn hãy nhập họ tên của minh";
            }
            else if (string.IsNullOrEmpty(sdt))
            {
                ViewData["loi"] = "bạn hãy nhập số điện thoại";
            }
            else if (string.IsNullOrEmpty(diachi))
            {
                ViewData["loi"] = "bạn hãy nhập nơi ở hiện tại";
            }
            else if (result.Count == 0)
            {
                if (result2.Count == 0)
                {
                    kh.HoTen = hoten;
                    kh.TaiKhoan = tendangnhap;
                    kh.MatKhau = matkhau;
                    kh.Email = email;
                    kh.DiaChi = diachi;
                    kh.DienThoai = sdt;
                    data.KhachHangs.InsertOnSubmit(kh);
                    data.SubmitChanges();
                    return RedirectToAction("DangKyTC","Home");
                }
                else
                {
                    ViewData["loi"] = "email của bạn đã được sử dụng";
                }

            }
            else
            {
                ViewData["loi"] = "tên đăng nhập của bạn đã được sử dụng";
            }
            return this.DangKy();
        }
    }
}