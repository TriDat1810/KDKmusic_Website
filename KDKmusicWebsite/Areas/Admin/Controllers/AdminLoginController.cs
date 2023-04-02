using KDKmusicWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace KDKmusicWebsite.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();

        // GET: Admin/AdminLogin
        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        private bool checktk(string Username)
        {
            return data.Admins.Count(x => x.User_name == Username) > 0;
        }
        private bool checkemail(string em)
        {
            return data.Admins.Count(x => x.E_mail == em) > 0;
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var taikhoan = collection["TaiKhoan"];
            var matkhau = collection["Matkhau"];

            if (String.IsNullOrEmpty(taikhoan))
            {
                ViewData["Loi1"] = "Tài khoản không được để trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được để trống";
            }
            else
            {
                var admin = data.Admins.SingleOrDefault(n => n.User_name == taikhoan && n.Password == matkhau);
                if (admin != null)
                {
                    Session["User"] = admin;
                    Session["User_name"] = admin.User_name;
                    return RedirectToAction("AdminIndex", "Admin_Home");
                }
                else if (!checktk(taikhoan))
                {
                    ViewBag.Thongbao = "Tên đăng nhập không tồn tại";
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng ";
            }
            return this.Login();
        }
        public ActionResult Logoff()
        {
            Session.Clear();
            return RedirectToAction("AdminIndex", "Admin_Home");
        }

        public ActionResult LoginPartial()
        {
            return PartialView("LoginPartial");
        }
    }
}