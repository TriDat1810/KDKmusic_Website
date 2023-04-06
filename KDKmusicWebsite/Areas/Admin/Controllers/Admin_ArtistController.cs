﻿using KDKmusicWebsite.Areas.Admin.Extensions;
using KDKmusicWebsite.Models;
using PagedList;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KDKmusicWebsite.Areas.Admin.Controllers
{
    public class Admin_ArtistController : Controller
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();

        #region ShowDisplay and Details
        // GET: Admin/Admin_Artist
        public ActionResult ShowDisplay(int? page)
        {
            //Tạo biến quy định số sản phẩm trên mới trang
            int pageSize = 5;
            //Tạo biến số trang;
            int pageNumber = (page ?? 1);

            //Lấy top
            var showlist = data.Artists.OrderBy(c => c.Artist_Name);
            return View(showlist.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int id)
        {
            var showlist = data.Artists.FirstOrDefault(data => data.Artist_Id == id);
            if (showlist == null)
            {
                return HttpNotFound();
            }
            return View(showlist);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            ViewBag.CountryList = new SelectList(data.Countries.OrderBy(c => c.Country_Name), "Country_Id", "Country_Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artist model, HttpPostedFileBase fileUpLoad)
        {
            if (ModelState.IsValid)
            {
                if (fileUpLoad != null && fileUpLoad.ContentLength > 0)
                {
                    if (ImageExtensions.IsImage(fileUpLoad))
                    {
                        var artist = new Artist
                        {
                            Artist_Name = model.Artist_Name,
                            Artist_Info = model.Artist_Info,
                            Country_Id = model.Country_Id
                        };

                        var fileName = Path.GetFileName(fileUpLoad.FileName);
                        var path = Path.Combine(Server.MapPath("~/Assets/Mine/images/"), fileName);
                        fileUpLoad.SaveAs(path);
                        artist.Artist_Image = "~/Assets/Mine/images/" + fileName;

                        data.Artists.InsertOnSubmit(artist);
                        data.SubmitChanges();

                        return RedirectToAction("ShowDisplay");
                    }
                    else
                    {
                        ModelState.AddModelError("fileUpLoad", "Please upload an image file.");
                    }
                }
                else
                {
                    ModelState.AddModelError("fileUpLoad", "Please upload an image file.");
                }
            }

            ViewBag.CountryList = new SelectList(data.Countries, "Country_Id", "Country_Name", model.Country_Id);

            return View(model);
        }

        #endregion

        #region Edit
        public ActionResult Edit(int id)
        {
            Artist artist = data.Artists.SingleOrDefault(a => a.Artist_Id == id);

            if (artist == null)
            {
                return HttpNotFound();
            }

            ViewBag.CountryList = new SelectList(data.Countries.OrderBy(c => c.Country_Name), "Country_Id", "Country_Name", artist.Country_Id);

            return View(artist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Artist model, HttpPostedFileBase fileUpLoad)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var artist = data.Artists.SingleOrDefault(c => c.Artist_Id == model.Artist_Id);
            if (artist == null)
            {
                return HttpNotFound();
            }

            artist.Artist_Name = model.Artist_Name;
            artist.Artist_Info = model.Artist_Info;
            artist.Country_Id = model.Country_Id;

            if (fileUpLoad != null && fileUpLoad.ContentLength > 0)
            {
                if (ImageExtensions.IsImage(fileUpLoad))
                {
                    var fileName = Path.GetFileName(fileUpLoad.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assets/Mine/images/"), fileName);
                    fileUpLoad.SaveAs(path);
                    artist.Artist_Image = "~/Assets/Mine/images/" + fileName;
                }
                else
                {
                    ModelState.AddModelError("fileUpLoad", "Please upload an image file.");
                    ViewBag.CountryList = new SelectList(data.Countries.OrderBy(c => c.Country_Name), "Country_Id", "Country_Name", artist.Country_Id);
                    return View(artist);
                }
            }

            data.SubmitChanges();
            return RedirectToAction("ShowDisplay");
        }
        #endregion

        #region DELETE
        public ActionResult Delete(int id)
        {
            var artist = data.Artists.FirstOrDefault(c => c.Artist_Id == id);

            if (artist == null)
            {
                return HttpNotFound();
            }

            return View(artist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int artist_id)
        {
            var artist = data.Artists.FirstOrDefault(c => c.Artist_Id == artist_id);

            if (artist == null)
            {
                return HttpNotFound();
            }

            data.Artists.DeleteOnSubmit(artist);
            data.SubmitChanges();
            return RedirectToAction("ShowDisplay");
        }
        #endregion
    }
}