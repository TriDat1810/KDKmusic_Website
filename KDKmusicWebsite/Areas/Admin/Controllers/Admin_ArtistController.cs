using KDKmusicWebsite.Areas.Admin.Extensions;
using KDKmusicWebsite.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDKmusicWebsite.Areas.Admin.Controllers
{
    public class Admin_ArtistController : Controller
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();
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

        public ActionResult DisplayImage(int imageId)
        {
            byte[] imageData = null;

            // Kết nối tới database và lấy hình ảnh từ cột Artist_Image theo id của nghệ sĩ

            // Kiểm tra xem hình ảnh có tồn tại không
            if (imageData != null)
            {
                // Chuyển đổi kiểu dữ liệu từ Varbinary(Max) sang Image
                System.Drawing.Image image = null;
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    image = System.Drawing.Image.FromStream(ms);
                }

                // Trả về hình ảnh dưới dạng FileResult
                return File(imageData, "image/jpeg"); //hoặc "image/png" nếu là định dạng png
            }

            // Trả về 404 Not Found nếu không tìm thấy hình ảnh
            return HttpNotFound();
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



    }
}
