using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace KDKmusicWebsite.Areas.Admin.Extensions
{
    public static class MP3Extesions
    {
        public static bool IsMp3(this HttpPostedFileBase file)
        {
            var acceptedExtensions = new[] { ".mp3" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!acceptedExtensions.Contains(fileExtension))
            {
                return false;
            }

            try
            {
                using (var mp3 = new Mp3FileReader(file.InputStream))
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