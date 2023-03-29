using KDKmusicWebsite.Models;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace KDKmusicWebsite.Controllers
{
    public class MusicController : Controller
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();

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

        //Phát nhạc theo id bài hát
        public ActionResult Play(int id)
        {
            var mp3Data = data.Songs.FirstOrDefault(m => m.Song_Id == id)?.Song_Data.ToArray();
            if (mp3Data == null)
            {
                return HttpNotFound();
            }

            //Chuyển đổi dạng nhị phân sang dạng mp3 (sử dụng thư viện NAudio).
            using (var ms = new MemoryStream(mp3Data))
            {
                using (var reader = new Mp3FileReader(ms))
                {
                    using (var writer = new LameMP3FileWriter(Response.OutputStream, reader.WaveFormat, LAMEPreset.STANDARD))
                    {
                        reader.CopyTo(writer);
                    }
                }
            }

            //Cho trình duyệt hiển thị bản nhạc trực tiếp thay vì tải xuống
            Response.ContentType = "audio/mpeg";
            Response.AddHeader("Content-Disposition", "inline;filename=output.mp3");
            Response.Flush();
            Response.End();
            return null;
        }

        //Lớp ghi đè để giải phóng tài nguyên sử dụng.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                data.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}