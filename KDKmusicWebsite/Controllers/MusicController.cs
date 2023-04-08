using KDKmusicWebsite.Models;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;

namespace KDKmusicWebsite.Controllers
{
    public class MusicController : Controller
    {
        DBkdkMusicContextDataContext data = new DBkdkMusicContextDataContext();

        // Hiển thị các bài hát có id theo thể loại nhạc hoặc hiển thị tất cả nếu id của thể loại nhạc là null
        public ActionResult ShowDisplay(int? Genre_id)
        {
            if(Genre_id == null)
            {
                var showlist = data.Songs.ToList();
                return View(showlist);
            }
            else
            {
                var showlist = data.Songs.Where(s => s.Genre_Id == Genre_id).ToList();
                return View(showlist);
            }
        }

        //Cũng giống như sự kiện trên nhưng nó dùng cho id nghệ sĩ
        public ActionResult ShowDisplayByArtist(int? Artist_id)
        {
            if (Artist_id == null)
            {
                var showlist = data.Songs.ToList();
                return View(showlist);
            }
            else if (Artist_id != null)
            {
                var showlist = data.Songs.Where(s => s.Artist_Id == Artist_id).ToList();
                return View(showlist);
            }
            return null;
        }

  
    }
}