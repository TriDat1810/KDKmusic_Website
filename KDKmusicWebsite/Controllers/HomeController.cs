using KDKmusicWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KDKmusicWebsite.Controllers
{
    public class HomeController : Controller
    {
        DBkdkMusicContextDataContext data = new DBkdkMusicContextDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search(string keyword)
        {
            var songs = data.Songs.Join(data.Artists, s => s.Artist_Id, a => a.Artist_Id, (s, a) => new { Song = s, ArtistName = a.Artist_Name }).Where(s => s.Song.Song_Name.Contains(keyword) || s.ArtistName.Contains(keyword)).Select(s => s.Song);

            return View(songs);
        }
    }
}