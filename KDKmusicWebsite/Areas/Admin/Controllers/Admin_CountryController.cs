using KDKmusicWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Net;

namespace KDKmusicWebsite.Areas.Admin.Controllers
{
    public class Admin_CountryController : Controller
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();

        // GET: Admin/Admin_Country
        public ActionResult ShowDisplay(int? page)
        {
            //Tạo biến quy định số sản phẩm trên mới trang
            int pageSize = 5;
            //Tạo biến số trang;
            int pageNumber = (page ?? 1);

            //Lấy top
            var showlist = data.Countries.OrderBy(c => c.Country_Name);
            return View(showlist.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Country newCountry)
        {
            if (ModelState.IsValid)
            {
                Country country = new Country();
                country.Country_Name = newCountry.Country_Name;
                data.Countries.InsertOnSubmit(country);
                data.SubmitChanges();
                return RedirectToAction("ShowDisplay");
            }
            else
            {
                return View(newCountry);
            }
        }

        public ActionResult Delete(int id)
        {
            // Lấy thông tin quốc gia cần xóa từ Database theo mã quốc gia
            var country = data.Countries.FirstOrDefault(c => c.Country_Id == id);

            // Nếu không tìm thấy quốc gia thì hiển thị trang lỗi
            if (country == null)
            {
                return HttpNotFound();
            }

            // Trả về View ConfirmDelete với dữ liệu của quốc gia cần xóa
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int country_id)
        {
            // Code xóa quốc gia từ database
            var country = data.Countries.FirstOrDefault(c => c.Country_Id == country_id);

            // Nếu không tìm thấy quốc gia thì hiển thị trang lỗi
            if (country == null)
            {
                return HttpNotFound();
            }

            data.Countries.DeleteOnSubmit(country);
            data.SubmitChanges();
            return RedirectToAction("ShowDisplay");
        }

        public ActionResult Edit(int id)
        {
            Country country = data.Countries.SingleOrDefault(c => c.Country_Id == id);
            if (country == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(country);
            }
        }

        [HttpPost]
        public ActionResult Edit(Country editedCountry)
        {
            if (ModelState.IsValid)
            {
                Country country = data.Countries.SingleOrDefault(c => c.Country_Id == editedCountry.Country_Id);
                if (country != null)
                {
                    country.Country_Name = editedCountry.Country_Name;
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
                return View(editedCountry);
            }
        }
    }
}