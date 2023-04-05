using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace KDKmusicWebsite.Areas.Admin.Extensions
{
    public static class ImageExtensions
    {
        public static bool IsImage(this HttpPostedFileBase file)
        {
            var acceptedExtensions = new[] { ".jpeg", ".jpg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!acceptedExtensions.Contains(fileExtension))
            {
                return false;
            }

            try
            {
                using (var image = Image.FromStream(file.InputStream))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}