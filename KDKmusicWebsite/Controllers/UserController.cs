using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using KDKmusicWebsite.Models;

namespace KDKmusicWebsite.Controllers
{
    public class UserController : Controller
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();
        // GET: User
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
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection collection, User user)
        {
            var taikhoan = collection["TaiKhoan"];
            var matkhau = collection["MatKhau"];
            var nhaplaimatkhau = collection["NhapLaiMatKhau"];
            var email = collection["NhapEmail"];
            if (String.IsNullOrEmpty(taikhoan))
            {
                ViewData["Loi1"] = "Tài khoản không được để trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được để trống";
            }
            else if (String.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi3"] = "Phải nhập lại mật khẩu";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi4"] = "E-mail không được để trống";
            }
            else if (checkemail(email))
            {
                ViewData["Loi4"] = "E-mail đã tồn tại ";
            }
            else
            {
                user.User_name = taikhoan;
                user.Password = matkhau;
                user.E_mail = email;
                data.Users.InsertOnSubmit(user);
                data.SubmitChanges();
                return RedirectToAction("Login");
            }
            return this.Register();
        }
        private bool checktk(string Username)
        {
            return data.Users.Count(x => x.User_name == Username) > 0;
        }
        private bool checkemail(string em)
        {
            return data.Users.Count(x => x.E_mail == em) > 0;
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
                User user = data.Users.SingleOrDefault(n => n.User_name == taikhoan && n.Password == matkhau);
                if (user != null)
                {
                    Session["User"] = user;
                    Session["User_name"] = user.User_name;
                    return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LoginPartial()
        {
            return PartialView("LoginPartial");
        }
    }
}