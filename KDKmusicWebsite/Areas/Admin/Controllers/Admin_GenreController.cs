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
        DBkdkMusicContextDataContext data = new DBkdkMusicContextDataContext();
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Music_Genre newGenre)
        {
            if (ModelState.IsValid)
            {
                Music_Genre genre = new Music_Genre();
                genre.Genre_Name = newGenre.Genre_Name;
                data.Music_Genres.InsertOnSubmit(genre);
                data.SubmitChanges();
                return RedirectToAction("ShowDisplay");
            }
            else
            {
                return View(newGenre);
            }
        }

        public ActionResult Delete(int id)
        {
            var genre = data.Music_Genres.FirstOrDefault(c => c.Genre_Id== id);

            if (genre == null)
            {
                return HttpNotFound();
            }

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int genre_id)
        {
            var genre = data.Music_Genres.FirstOrDefault(c => c.Genre_Id == genre_id);

            if (genre == null)
            {
                return HttpNotFound();
            }

            data.Music_Genres.DeleteOnSubmit(genre);
            data.SubmitChanges();
            return RedirectToAction("ShowDisplay");
        }

        public ActionResult Edit(int id)
        {
            Music_Genre genre = data.Music_Genres.SingleOrDefault(c => c.Genre_Id == id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(genre);
            }
        }

        [HttpPost]
        public ActionResult Edit(Music_Genre editedGenre)
        {
            if (ModelState.IsValid)
            {
                Music_Genre genre = data.Music_Genres.SingleOrDefault(c => c.Genre_Id== editedGenre.Genre_Id);
                if (genre != null)
                {
                    genre.Genre_Name = editedGenre.Genre_Name;
                    data.SubmitChanges();
                    return RedirectToAction("ShowDisplay");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return View(editedGenre);
            }
        }
    }
}