using KDKmusicWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KDKmusicWebsite.Areas.Admin.Controllers
{
    public class Admin_HomeController : Controller
    {
        // GET: Admin/Admin_Home
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();
        public ActionResult AdminIndex()
        {
            if (Session["Admin_User_name"] != null)
            {
                string userName = Session["Admin_User_name"].ToString();
                var check = data.Admins.FirstOrDefault(s => s.User_name == userName);
                if (check != null)
                {
                    return View(check);
                }
            }
            return RedirectToAction("Login", "AdminLogin");
        }
    }
}