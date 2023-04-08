using KDKmusicWebsite.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KDKmusicWebsite.Controllers
{
    public class AddPlaylistController : Controller
    {
        DBkdkMusicContextDataContext data = new DBkdkMusicContextDataContext();
        // GET: AddPlaylist

        #region Thêm playlist
      
        public List<ThemPlayList> Layplaylist()
        {
            List<ThemPlayList> lstPlaylist = Session["PlayList"] as List<ThemPlayList>;
            if (lstPlaylist == null)
            {
                lstPlaylist = new List<ThemPlayList>();
                Session["PlayList"] = lstPlaylist;
            }
            return lstPlaylist;
        }

        public ActionResult AddMusic(int iSong_Id, string strUrl)
        {
            List<ThemPlayList> lstPlaylist = Layplaylist();
            ThemPlayList song = lstPlaylist.Find(n => n.iSong_Id == iSong_Id);
            if (song == null)
            {
                song = new ThemPlayList(iSong_Id);
                lstPlaylist.Add(song);
                TempData["SuccessMessage"] = "Đã thêm!";
                return Redirect(strUrl);
            }
            else
            {
                return Redirect(strUrl);
            }
        }
        public ActionResult AddMusictoPlaylist()
        {
            List<ThemPlayList> lstPlaylist = Layplaylist();
            if (lstPlaylist.Count == 0)
            {
                TempData["SuccessMessage"] = "Danh sách tạm thời của bạn không có gì!";
                return RedirectToAction("ShowDisplay", "Music");
            }
            return View(lstPlaylist);
        }

        #endregion
        #region Xóa nhạc đã lưu tạm thời
        public ActionResult delMusic(int i1Song_id)
        {

            List<ThemPlayList> lstPlaylist = Layplaylist();
            ThemPlayList music = lstPlaylist.SingleOrDefault(n => n.iSong_Id == i1Song_id);
            if (music != null)
            {
                lstPlaylist.RemoveAll(n => n.iSong_Id == i1Song_id);
                TempData["SuccessMessage"] = "Đã xóa bài!";
                return RedirectToAction("AddMusictoPlaylist");
            }
            if (music == null)
            {
                return RedirectToAction("ShowDisplay", "Music");
            }
            return RedirectToAction("AddMusictoPlaylist"); 
        }
        public ActionResult delallMusic()
        {
            List<ThemPlayList> lstPlaylist = Layplaylist();
            lstPlaylist.Clear();
            TempData["SuccessMessage"] = "Đã xóa tất cả!";
            return RedirectToAction("ShowDisplay", "Music");
        }
        #endregion
        #region Tạo tên playlist và lưu 
        private bool chechtenplaylist(string name)
        {
            return data.Playlists.Count(x => x.Playlist_Name == name) > 0;
        }
       
        [HttpGet]
        public ActionResult CreatePlaylist()
        {
            return View();
        }
    
        [HttpPost]
        public ActionResult CreatePlaylist( FormCollection collection, Playlist playlist)
        {
            var tenplaylist = collection["playlistName"];

            if (String.IsNullOrEmpty(tenplaylist))
            {
                ViewData["Loi"] = "Tên không được để trống";
            }
            else if (chechtenplaylist(tenplaylist))
            {
                ViewData["Loi"] = "Tên danh sách đã tồn tại đã tồn tại";
            }else
            {
                    playlist.Playlist_Name = tenplaylist;
                    playlist.Create_At = DateTime.Now;
                    User user = data.Users.FirstOrDefault(n => n.User_name == (string)Session["User_name"]);
                    playlist.User_Id = user.User_Id; 
                    data.Playlists.InsertOnSubmit(playlist);
                    data.SubmitChanges();

                //Lấy nhạc trong Session đã thêm
                List<ThemPlayList> lstPlaylist = Layplaylist();
                foreach (var item in lstPlaylist)
                {
                    // Tạo một đối tượng mới để lưu thông tin của bản ghi
                    Playlist_Song songPlaylist = new Playlist_Song();

                    // Thiết lập các thuộc tính của đối tượng

                    
                    songPlaylist.Playlist_Id = playlist.Playlist_Id; 
                    songPlaylist.Song_Id = item.iSong_Id;

                    // Lưu đối tượng vào cơ sở dữ liệu
                    data.Playlist_Songs.InsertOnSubmit(songPlaylist);
                    data.SubmitChanges();
                }
                return RedirectToAction("ShowPlaylists", "AddPlaylist");// Chuyển hướng đến trang chính của ứng dụng
            }
            return this.CreatePlaylist();
        }
      
        public ActionResult ShowPlaylists()
        {
            TempData["SuccessMessage"] = "Đã tạo danh sách thành công!";
            // Lấy danh sách các playlist từ cơ sở dữ liệu
            List<Playlist> playlists = data.Playlists.OrderBy(p => p.Playlist_Id).ToList();

            // Hiển thị danh sách các playlist
            return View(playlists);
        }
       
        public ActionResult ShowPlaylistsSong(int? Playlist_id )
        {
            if (Playlist_id == null)
            {
                var showlist = data.Playlist_Songs.ToList();
                return View(showlist);
            }
            else
            {
                var showlist = data.Playlist_Songs.Where(s => s.Playlist_Id == Playlist_id).ToList();
                return View(showlist);
            }
        }


        #endregion
        #region Xóa tên danh sách đã lưu
        public ActionResult delPlaylist(int Playlist_Id)
        {
            // Lấy danh sách các playlist từ cơ sở dữ liệu
            var playlists = data.Playlists.FirstOrDefault(c => c.Playlist_Id == Playlist_Id);
            if (playlists == null)
                {
                    return HttpNotFound();
                }

            data.Playlists.DeleteOnSubmit(playlists);
            data.SubmitChanges();
            TempData["SuccessMessage"] = "Đã xóa!";
            return RedirectToAction("ShowPlaylists", "AddPlaylist");
        }
        #endregion
        #region Xóa nhạc trong danh sách đã lưu
        public ActionResult delSonginPlaylist(int Song_Id, int playlist_id)
        {
            // Lấy danh sách các playlist từ cơ sở dữ liệu
            var playlistsSong = data.Playlist_Songs.FirstOrDefault
                (c => c.Song_Id == Song_Id && c.Playlist_Id == playlist_id);
            if (playlistsSong == null)
            {
                return HttpNotFound();
            }

            data.Playlist_Songs.DeleteOnSubmit(playlistsSong);
            data.SubmitChanges();
            TempData["SuccessMessage"] = "Đã xóa!";
            return RedirectToAction("ShowPlaylistsSong", "AddPlaylist", new { @Playlist_id = playlist_id });
        }
        #endregion
    }
}