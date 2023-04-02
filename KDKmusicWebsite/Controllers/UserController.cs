using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using KDKmusicWebsite.Models;
using System.Configuration;
using Facebook;

namespace KDKmusicWebsite.Controllers
{
    public class UserController : Controller
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();
        // GET: User
        private string mahoamd5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var dulieu = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();

                for (int i = 0; i < dulieu.Length; i++)
                {
                    builder.Append(dulieu[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static bool checkkitu(string input)
        {
            char[] specialChar = { ' ', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '{', '}', '[', ']', '|', '\\', ':', ';', '\"', '\'', '<', '>', ',', '.', '?', '/' };
            foreach (char item in input)
            {
                if (specialChar.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool checkkhoangtrang(string input)
        {
            return input.Contains(" ");
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
            else if (checktk(taikhoan))
            {
                ViewData["Loi1"] = "Tài khoản đã tồn tại";
            }
            else if (checkkhoangtrang(taikhoan))
            {
                ViewData["Loi1"] = "Tài khoản không được có khoảng trắng";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được để trống";
            }
            else if (checkkitu(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không có kí tự đặc biệt";
            }
            else if (checkkhoangtrang(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được có khoảng trắng";
            }
            else if (String.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi3"] = "Phải nhập lại mật khẩu";
            }
            else if (nhaplaimatkhau != matkhau)
            {
                ViewData["Loi3"] = "Không trùng với mật khẩu";
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
                //lưu mật khẩu dưới dạng md5
                user.Password = mahoamd5(matkhau);
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




        //Login bằng facebook
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public long InsertForFacebook(User entity)
        {
            var user = data.Users.SingleOrDefault(n => n.User_name == entity.User_name);
            if (user == null)
            {
                data.Users.InsertOnSubmit(entity);
                data.SubmitChanges();
                return entity.User_Id;
            }
            return user.User_Id;
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                //lấy thông tin của người dùng
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string username = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.User_name = email;
                user.E_mail = email;
                user.Password = email;
                var insertResurt = InsertForFacebook(user);
                if (insertResurt > 0)
                {
                    Session["User"] = user;
                    Session["User_name"] = user.User_name;

                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}