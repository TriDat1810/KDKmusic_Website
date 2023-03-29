using Antlr.Runtime.Misc;
using KDKmusicWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace KDKmusicWebsite.Controllers
{
    public class GenreController : Controller
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();
        // GET: Genre
        public ActionResult ShowDisplay()
        {
            var showlist = data.Music_Genres.ToList();
            return View(showlist);
        }
    }
}