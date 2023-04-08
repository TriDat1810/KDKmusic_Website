using KDKmusicWebsite.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace KDKmusicWebsite.Areas.Admin.Controllers
{
    public class Interaction_ManagerController : Controller
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();

        // GET: Admin/Interaction_Manager
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

                    //Lấy top 5
                    var showlist = data.Interactions.OrderBy(c => c.Interaction_Id);
                    return View(showlist.ToPagedList(pageNumber, pageSize));
                }
            }
            return RedirectToAction("Login", "AdminLogin");
        }
    }
}