using KDKmusicWebsite.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

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
            imageData = data.Artists.FirstOrDefault(m => m.Artist_Id == imageId)?.Artist_Image.ToArray(); ;

            // Kiểm tra xem hình ảnh có tồn tại không
            if (imageData != null)
            {
                // Chuyển đổi kiểu dữ liệu từ Varbinary(Max) sang Image
                Image image = null;
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    image = Image.FromStream(ms);
                }

                // Trả về hình ảnh dưới dạng FileResult
                return File(imageData, "image/jpeg"); //hoặc "image/png" nếu là định dạng png
            }

            // Trả về 404 Not Found nếu không tìm thấy hình ảnh
            return HttpNotFound();
        }

    }
}