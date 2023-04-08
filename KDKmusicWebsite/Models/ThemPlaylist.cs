using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KDKmusicWebsite.Models;

namespace KDKmusicWebsite.Models
{

    public class ThemPlayList
    {
        DBkdkMusicModelDataContext data = new DBkdkMusicModelDataContext();
        public int iPlaylist_id { get; set; }
        public int iSong_Id { get; set; }
        public string sSong_Name { get; set; }
        public int iArtis_Id { get; set; }
        public string sArtis_Mame { get; set; }

        public string sSong_Path { get; set; }
        public string sSong_Image { get; set; }


        public ThemPlayList(int Song_id)
        {

            iSong_Id = Song_id;
            Song song = data.Songs.FirstOrDefault(n => n.Song_Id == iSong_Id);
            //Artist artist = data.Artists.FirstOrDefault(n => n.Artist_Id == iArtis_Id);
            sArtis_Mame = song.Artist.Artist_Name;
            sSong_Name = song.Song_Name;
            sSong_Path = song.Song_Path;
            sSong_Image = song.Song_Image;
        }
    }
}