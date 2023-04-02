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
    public class Admin_GenreController : Controller
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();
        // GET: Admin/Admin_Genre
        public ActionResult ShowDisplay(int? page)
        {
            //Tạo biến quy định số sản phẩm trên mới trang
            int pageSize = 5;
            //Tạo biến số trang;
            int pageNumber = (page ?? 1);

            //Lấy top 1 Country
            var showlist = data.Music_Genres.OrderBy(c => c.Genre_Name);
            return View(showlist.ToPagedList(pageNumber, pageSize));
        }
    }
}