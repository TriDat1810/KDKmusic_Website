using KDKmusicWebsite.Areas.Admin.Extensions;
using KDKmusicWebsite.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace KDKmusicWebsite.Areas.Admin.Controllers
{
    public class User_ManagerController : Controller
    {
        // GET: Admin/User_Manager
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
        #endregion

        #region SHOWDISPLAY
        public ActionResult ShowDisplay(int? page)
        {
            if (Session["Admin_User_name"] != null)
            {
                string userName = Session["Admin_User_name"].ToString();
                var check = data.Admins.FirstOrDefault(s => s.User_name == userName);
                if (check != null)
                {
                    //Tạo biến quy định số sản phẩm trên mới trang
                    int pageSize = 5;
                    //Tạo biến số trang;
                    int pageNumber = (page ?? 1);

                    //Lấy top
                    var showlist = data.Users.OrderBy(c => c.User_name);
                    return View(showlist.ToPagedList(pageNumber, pageSize));
                }
            }
            return RedirectToAction("Login", "AdminLogin");
        }
        #endregion

        #region EDIT
        public ActionResult Edit(int id)
        {
            var user = data.Users.SingleOrDefault(c => c.User_Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult Edit(User editUser, FormCollection collection)
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
                    User user = data.Users.SingleOrDefault(c => c.User_Id == editUser.User_Id);
                    if (user != null)
                    {
                        user.Password = EncryptPassword.mahoamd5(editUser.Password);
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
            var user = data.Users.FirstOrDefault(u => u.User_Id == id);

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
            var user = data.Users.FirstOrDefault(u => u.User_Id == id); ;

            if (user == null)
            {
                // Nếu không tìm thấy người dùng, trả về trang lỗi 404
                return HttpNotFound();
            }

            // Xóa người dùng khỏi cơ sở dữ liệu
            data.Users.DeleteOnSubmit(user);
            data.SubmitChanges();

            return RedirectToAction("ShowDisplay");
        }

        #endregion

        #region SEARCHING
        public ActionResult Search(string searchString, int? page)
        {
            var searchName = from s in data.Users
                             select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                searchName = searchName.Where(s => s.User_name.Contains(searchString));
            }

            //Tạo biến quy định số sản phẩm trên mới trang
            int pageSize = 5;
            //Tạo biến số trang;
            int pageNumber = (page ?? 1);
            return View(searchName.ToPagedList(pageNumber, pageSize));
        }
        #endregion
    }
}