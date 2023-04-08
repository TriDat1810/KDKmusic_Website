using KDKmusicWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KDKmusicWebsite.Controllers
{
    public class ArtistController : Controller
    {
        DBkdkMusicContextDataContext data = new DBkdkMusicContextDataContext();
        // GET: Artist
        public ActionResult ShowDisplay()
        {
            var showlist = data.Artists.ToList();
            return View(showlist);
        }

        public ActionResult Details(int id)
        {
            var details = data.Artists.FirstOrDefault(a => a.Artist_Id == id);
            if(details == null)
            {
                return HttpNotFound();
            }
            return View(details);
        }
    }
}