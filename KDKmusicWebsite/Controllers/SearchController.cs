using KDKmusicWebsite.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KDKmusicWebsite.Controllers
{
    public class SearchController : Controller
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();

        //Tìm kiếm theo tên bài hát hoặc tên nghệ sĩ
        public ActionResult SearchForSong(string name)
        {
            var searchName = from s in data.Songs
                        select s;

            if (!String.IsNullOrEmpty(name))
            {
                searchName = searchName.Where(s => s.Song_Name.Contains(name) || s.Artist.Artist_Name.Contains(name));
            }

            return View(searchName.ToList());
        }
    }
}