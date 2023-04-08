using KDKmusicWebsite.Areas.Admin.Extensions;
using KDKmusicWebsite.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;

namespace KDKmusicWebsite.Areas.Admin.Controllers
{
    public class Admin_ManagerController : Controller
    {
        // GET: Admin/Admin_Manager

        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();

        #region check info
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

        private bool checkemail(string em)
        {
            return data.Admins.Count(x => x.E_mail == em) > 0;
        }

        private bool checktk(string Username)
        {
            return data.Users.Count(x => x.User_name == Username) > 0;
        }
        #endregion

        #region SHOWDISPLAY
        public ActionResult ShowDisplay(int? page)
        {


            if (Session["Admin_User_name"] != null)
            {
                string userName = Session["Admin_User_name"].ToString();
                var check = data.Admins.FirstOrDefault(s => s.User_name == userName);
                if (check != null)
                {//Tạo biến quy định số sản phẩm trên mới trang
                    int pageSize = 5;
                    //Tạo biến số trang;
                    int pageNumber = (page ?? 1);

                    //Lấy top
                    var showlist = data.Admins.OrderBy(c => c.User_name);
                    return View(showlist.ToPagedList(pageNumber, pageSize));
                }
            }
            return RedirectToAction("Login", "AdminLogin");

        }
        #endregion

        #region CREATE
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(KDKmusicWebsite.Models.Admin newAdmin, FormCollection collection)
        {
            var taikhoan = newAdmin.User_name;
            var matkhau = newAdmin.Password;
            var nhaplaimatkhau = collection["NhapLaiMatKhau"];
            var email = newAdmin.E_mail;

            if (ModelState.IsValid)
            {
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
                    KDKmusicWebsite.Models.Admin admin = new KDKmusicWebsite.Models.Admin()
                    {
                        User_name = newAdmin.User_name,
                        Password = EncryptPassword.mahoamd5(newAdmin.Password),
                        E_mail = newAdmin.E_mail,
                        FullName = newAdmin.FullName,
                        is_Admin = true
                    };


                    data.Admins.InsertOnSubmit(admin);
                    data.SubmitChanges();
                    return RedirectToAction("ShowDisplay");
                }
            }
            return View(newAdmin);
        }
        #endregion

        #region EDIT
        public ActionResult Edit(int id)
        {
            var admin = data.Admins.SingleOrDefault(c => c.User_Id == id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(admin);
            }
        }

        [HttpPost]
        public ActionResult Edit(KDKmusicWebsite.Models.Admin editUser, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var matkhau = editUser.Password;
                var nhaplaimatkhau = collection["NhapLaiMatKhau"];
                if (String.IsNullOrEmpty(matkhau))
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
                else
                {

                    KDKmusicWebsite.Models.Admin admin = data.Admins.SingleOrDefault(c => c.User_Id == editUser.User_Id);
                    if (admin != null)
                    {
                        admin.Password = EncryptPassword.mahoamd5(editUser.Password);
                        admin.FullName = editUser.FullName;
                        data.SubmitChanges();
                        return RedirectToAction("ShowDisplay");
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
            }
            return View(editUser);

        }
        #endregion

        #region DELETE
        public ActionResult Delete(int id)
        {
            // Tìm người dùng cần xóa bằng ID
            var user = data.Admins.FirstOrDefault(u => u.User_Id == id);

            if (user == null)
            {
                // Nếu không tìm thấy người dùng, trả về trang lỗi 404
                return HttpNotFound();
            }

            // Hiển thị thông báo xác nhận trước khi xóa
            ViewBag.ConfirmDelete = true;

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Tìm người dùng cần xóa bằng ID
            var user = data.Admins.FirstOrDefault(u => u.User_Id == id); ;

            if (user == null)
            {
                // Nếu không tìm thấy người dùng, trả về trang lỗi 404
                return HttpNotFound();
            }

            // Xóa người dùng khỏi cơ sở dữ liệu
            data.Admins.DeleteOnSubmit(user);
            data.SubmitChanges();

            return RedirectToAction("ShowDisplay");
        }
        #endregion
    }
}