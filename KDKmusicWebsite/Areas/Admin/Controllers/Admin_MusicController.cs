using KDKmusicWebsite.Areas.Admin.Extensions;
using KDKmusicWebsite.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KDKmusicWebsite.Areas.Admin.Controllers
{
    public class Admin_MusicController : Controller
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();

        #region ShowDisplay and Details
        // GET: Admin/AdminMusic
        public ActionResult ShowDisplay(int? page)
        {
            if (Session["Admin_User_name"] != null)
            {
                string userName = Session["Admin_User_name"].ToString();
                var check = data.Admins.FirstOrDefault(s => s.User_name == userName);
                if (check != null)
                {
                    int pageSize = 5;
                    int pageNumber = (page ?? 1);

                    var showlist = data.Songs.OrderBy(c => c.Song_Name);
                    return View(showlist.ToPagedList(pageNumber, pageSize));
                }
            }
            return RedirectToAction("Login", "AdminLogin");
        }

        public ActionResult Details(int id)
        {
            var showlist = data.Songs.FirstOrDefault(data => data.Song_Id == id);
            if (showlist == null)
            {
                return HttpNotFound();
            }
            return View(showlist);
        }
        #endregion

        #region CREATE
        public ActionResult Create()
        {
            ViewBag.GenreList = new SelectList(data.Music_Genres.OrderBy(c => c.Genre_Name), "Genre_Id", "Genre_Name");
            ViewBag.ArtistList = new SelectList(data.Artists.OrderBy(c => c.Artist_Name), "Artist_Id", "Artist_Name");
            ViewBag.AlbumList = new SelectList(data.Albums.OrderBy(c => c.Album_Name), "Album_Id", "Album_Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Song song, HttpPostedFileBase songImageFile, HttpPostedFileBase songFile)
        {
            if (ModelState.IsValid)
            {
                Song songFromDB = new Song()
                {
                    Song_Name = song.Song_Name,
                    Artist_Id = song.Artist_Id,
                    Genre_Id = song.Genre_Id,
                    Create_at = DateTime.Now,
                    Lyrics = song.Lyrics,
                };

                // Lưu file hình ảnh đại diện cho bài hát
                if (songImageFile != null && songImageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(songImageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assets/Mine/imagesForSong/"), fileName);

                    var fileCount = 1;
                    //Kiểm tra sự tồn tại của file
                    while (System.IO.File.Exists(path))
                    {
                        fileName = Path.GetFileNameWithoutExtension(songImageFile.FileName) + "-" + fileCount.ToString() + Path.GetExtension(songImageFile.FileName);
                        path = Path.Combine(Server.MapPath("~/Assets/Mine/imagesForSong/"), fileName);
                        fileCount++;
                    }
                    songImageFile.SaveAs(path);
                    songFromDB.Song_Image = "~/Assets/Mine/imagesForSong/" + fileName;
                }

                // Lưu file nhạc đại diện cho bài hát
                if (songFile != null && songFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(songFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assets/Mine/musics/"), fileName);

                    var fileCount = 1;
                    //Kiểm tra sự tồn tại của file
                    while (System.IO.File.Exists(path))
                    {
                        fileName = Path.GetFileNameWithoutExtension(songFile.FileName) + "-" + fileCount.ToString() + Path.GetExtension(songFile.FileName);
                        path = Path.Combine(Server.MapPath("~/Assets/Mine/musics/"), fileName);
                        fileCount++;
                    }
                    songFile.SaveAs(path);
                    songFromDB.Song_Path = "~/Assets/Mine/musics/" + fileName;
                }

                
                if (song.Album_Id != null)
                {
                    songFromDB.Album_Id = song.Album_Id;
                }


                data.Songs.InsertOnSubmit(songFromDB);
                data.SubmitChanges();
                return RedirectToAction("ShowDisplay");
            }

            ViewBag.ArtistList = new SelectList(data.Artists.OrderBy(c => c.Artist_Name), "Artist_Id", "Artist_Name", song.Artist_Id);
            ViewBag.AlbumList = new SelectList(data.Albums.OrderBy(c => c.Album_Name), "Album_Id", "Album_Name", song.Album_Id);
            ViewBag.GenreList = new SelectList(data.Music_Genres.OrderBy(c => c.Genre_Name), "Genre_Id", "Genre_Name", song.Genre_Id);
            return View(song);
        }
        #endregion

        #region EDIT
        public ActionResult Edit(int id)
        {
            Song song = data.Songs.FirstOrDefault(s => s.Song_Id == id);
            if (song == null)
            {
                return HttpNotFound();
            }

            ViewBag.ArtistList = new SelectList(data.Artists.OrderBy(c => c.Artist_Name), "Artist_Id", "Artist_Name", song.Artist_Id);
            ViewBag.GenreList = new SelectList(data.Music_Genres.OrderBy(c => c.Genre_Name), "Genre_Id", "Genre_Name", song.Genre_Id);
            ViewBag.AlbumList = new SelectList(data.Albums.OrderBy(c => c.Album_Name), "Album_Id", "Album_Name", song.Album_Id);

            return View(song);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Song song, HttpPostedFileBase songImageFile, HttpPostedFileBase songFile)
        {
            if (ModelState.IsValid)
            {
                Song songFromDB = data.Songs.FirstOrDefault(s => s.Song_Id == song.Song_Id);
                if (songFromDB == null)
                {
                    return HttpNotFound();
                }

                songFromDB.Song_Name = song.Song_Name;
                songFromDB.Artist_Id = song.Artist_Id;
                songFromDB.Genre_Id = song.Genre_Id;
                songFromDB.Lyrics = song.Lyrics;

                if (song.Album_Id != null)
                {
                    songFromDB.Album_Id = song.Album_Id;
                }

                if (songImageFile != null && songImageFile.ContentLength > 0 && ImageExtensions.IsImage(songImageFile))
                {
                    var fileName = Path.GetFileName(songImageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assets/Mine/imagesForSong/"), fileName);

                    //Xóa file cũ trước đó
                    if (!string.IsNullOrEmpty(songFromDB.Song_Image))
                    {
                        var oldImagePath = Server.MapPath(songFromDB.Song_Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var fileCount = 1;
                    //Kiểm tra sự tồn tại của file
                    while (System.IO.File.Exists(path))
                    {
                        fileName = Path.GetFileNameWithoutExtension(songImageFile.FileName) + "-" + fileCount.ToString() + Path.GetExtension(songImageFile.FileName);
                        path = Path.Combine(Server.MapPath("~/Assets/Mine/imagesForSong/"), fileName);
                        fileCount++;
                    }
                    songImageFile.SaveAs(path);
                    songFromDB.Song_Image = "~/Assets/Mine/imagesForSong/" + fileName;
                }

                if (songFile != null && songFile.ContentLength > 0 && MP3Extesions.IsMp3(songFile))
                {
                    var fileName = Path.GetFileName(songFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assets/Mine/musics/"), fileName);

                    //Xóa file cũ trước đó
                    if (!string.IsNullOrEmpty(songFromDB.Song_Path))
                    {
                        var oldSongPath = Server.MapPath(songFromDB.Song_Path);
                        if (System.IO.File.Exists(oldSongPath))
                        {
                            System.IO.File.Delete(oldSongPath);
                        }
                    }

                    var fileCount = 1;
                    //Kiểm tra sự tồn tại của file
                    while (System.IO.File.Exists(path))
                    {
                        fileName = Path.GetFileNameWithoutExtension(songFile.FileName) + "-" + fileCount.ToString() + Path.GetExtension(songFile.FileName);
                        path = Path.Combine(Server.MapPath("~/Assets/Mine/musics/"), fileName);
                        fileCount++;
                    }
                    songFile.SaveAs(path);
                    songFromDB.Song_Path = "~/Assets/Mine/musics/" + fileName;
                }

                data.SubmitChanges();
                return RedirectToAction("ShowDisplay");
            }

            ViewBag.ArtistList = new SelectList(data.Artists.OrderBy(c => c.Artist_Name), "Artist_Id", "Artist_Name", song.Artist_Id);
            ViewBag.GenreList = new SelectList(data.Music_Genres.OrderBy(c => c.Genre_Name), "Genre_Id", "Genre_Name", song.Genre_Id);
            ViewBag.AlbumList = new SelectList(data.Albums.OrderBy(c => c.Album_Name), "Album_Id", "Album_Name", song.Album_Id);

            return View(song);
        }

        #endregion

        #region DELETE
        public ActionResult Delete(int id)
        {
            var song = data.Songs.FirstOrDefault(c => c.Song_Id == id);

            if (song == null)
            {
                return HttpNotFound();
            }

            return View(song);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int song_id)
        {
            Song songToDelete = data.Songs.FirstOrDefault(s => s.Song_Id == song_id);

            if (songToDelete != null)
            {
                // Xóa file nhạc và file hình ảnh đại diện của bài hát khỏi thư mục
                if (!string.IsNullOrEmpty(songToDelete.Song_Path))
                {
                    var songFilePath = Server.MapPath(songToDelete.Song_Path);
                    if (System.IO.File.Exists(songFilePath))
                    {
                        System.IO.File.Delete(songFilePath);
                    }
                }

                if (!string.IsNullOrEmpty(songToDelete.Song_Image))
                {
                    var songImageFilePath = Server.MapPath(songToDelete.Song_Image);
                    if (System.IO.File.Exists(songImageFilePath))
                    {
                        System.IO.File.Delete(songImageFilePath);
                    }
                }

                // Xóa tất cả từ cơ sở dữ liệu
                data.Songs.DeleteOnSubmit(songToDelete);
                data.SubmitChanges();
                return RedirectToAction("ShowDisplay");
            }

            return HttpNotFound();
        }
        #endregion

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