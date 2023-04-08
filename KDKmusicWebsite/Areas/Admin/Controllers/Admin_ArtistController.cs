using KDKmusicWebsite.Areas.Admin.Extensions;
using KDKmusicWebsite.Models;
using PagedList;
using System;
using System.Drawing.Printing;
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
            if (Session["Admin_User_name"] != null)
            {
                string userName = Session["Admin_User_name"].ToString();
                var check = data.Admins.FirstOrDefault(s => s.User_name == userName);
                if (check != null)
                {
                    //Tạo biến quy định số sản phẩm trên mới trang
                    int pageSize = 5;
                    //Tạo biến số trang;
                    int pageNumber = (page ?? 1);

                    //Lấy top
                    var showlist = data.Artists.OrderBy(c => c.Artist_Name);
                    return View(showlist.ToPagedList(pageNumber, pageSize));
                }
            }
            return RedirectToAction("Login", "AdminLogin");
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
                    var artist = new Artist
                    {
                        Artist_Name = model.Artist_Name,
                        Artist_Info = model.Artist_Info,
                        Country_Id = model.Country_Id
                    };

                    var fileName = Path.GetFileName(fileUpLoad.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assets/Mine/images/"), fileName);

                    var fileCount = 1;
                    //Kiểm tra sự tồn tại của file
                    while (System.IO.File.Exists(path))
                    {
                        fileName = Path.GetFileNameWithoutExtension(fileUpLoad.FileName) + "-" + fileCount.ToString() + Path.GetExtension(fileUpLoad.FileName);
                        path = Path.Combine(Server.MapPath("~/Assets/Mine/images/"), fileName);
                        fileCount++;
                    }

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

            ViewBag.CountryList = new SelectList(data.Countries.OrderBy(c => c.Country_Name), "Country_Id", "Country_Name", model.Country_Id);

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
            if (ModelState.IsValid)
            {
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
                    var fileName = Path.GetFileName(fileUpLoad.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assets/Mine/images/"), fileName);

                    //Xóa file cũ trước đó
                    if (!string.IsNullOrEmpty(artist.Artist_Image))
                    {
                        var oldImagePath = Server.MapPath(artist.Artist_Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var fileCount = 1;
                    //Kiểm tra sự tồn tại của file
                    while (System.IO.File.Exists(path))
                    {
                        fileName = Path.GetFileNameWithoutExtension(fileUpLoad.FileName) + "-" + fileCount.ToString() + Path.GetExtension(fileUpLoad.FileName);
                        path = Path.Combine(Server.MapPath("~/Assets/Mine/images/"), fileName);
                        fileCount++;
                    }

                    fileUpLoad.SaveAs(path);
                    artist.Artist_Image = "~/Assets/Mine/images/" + fileName;

                    data.SubmitChanges();
                    return RedirectToAction("ShowDisplay");
                }
                else
                {
                    ModelState.AddModelError("fileUpLoad", "Please upload an image file.");
                    ViewBag.CountryList = new SelectList(data.Countries.OrderBy(c => c.Country_Name), "Country_Id", "Country_Name", artist.Country_Id);
                    return View(artist);
                }
            }
            ViewBag.CountryList = new SelectList(data.Countries.OrderBy(c => c.Country_Name), "Country_Id", "Country_Name", model.Country_Id);
            return View(model);
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

            //Xóa file tồn tại trong project
            if (!string.IsNullOrEmpty(artist.Artist_Image))
            {
                var songFilePath = Server.MapPath(artist.Artist_Image);
                if (System.IO.File.Exists(songFilePath))
                {
                    System.IO.File.Delete(songFilePath);
                }
            }

            data.Artists.DeleteOnSubmit(artist);
            data.SubmitChanges();
            return RedirectToAction("ShowDisplay");
        }
        #endregion

        #region SEARCHING
        public ActionResult Search(string searchString, int? page)
        {
            if (Session["Admin_User_name"] != null)
            {
                string userName = Session["Admin_User_name"].ToString();
                var check = data.Admins.FirstOrDefault(s => s.User_name == userName);
                if (check != null)
                {
                    var searchName = from s in data.Artists
                                     select s;

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        searchName = searchName.Where(s => s.Artist_Name.Contains(searchString));
                    }

                    //Tạo biến quy định số sản phẩm trên mới trang
                    int pageSize = 5;
                    //Tạo biến số trang;
                    int pageNumber = (page ?? 1);
                    return View(searchName.ToPagedList(pageNumber, pageSize));
                }
            }
            return RedirectToAction("Login", "AdminLogin");
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
