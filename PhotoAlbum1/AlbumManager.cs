using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace PhotoAlbumViewOfTheGods
{
    class AlbumManager
    {
        private Dictionary<string, Album> albumList = new Dictionary<string, Album>();
        private string currentAlbum;

        //directory info
        private string directory;
        private string albumFolder;
        private string photoFolder;
        private string albumExtension;

        //Constructor that initializes variables used in this class
        public AlbumManager(string folderDir, string albumDirectory, string photoDirectory, string ext)
        {
            directory = folderDir;
            albumFolder = albumDirectory;
            photoFolder = photoDirectory;
            albumExtension = ext;
            currentAlbum = "";
        }

        public void loadAlbums()
        {
            string[] albumNameList = getAlbumList();

            foreach (string albumName in albumNameList)
            {
                Album newAlbum = new Album();
                newAlbum.load(albumName);
                albumList.Add(albumName, newAlbum);
            }

            if (albumList.Count > 0)
                currentAlbum = albumNameList[0];
        }

        //Creates a new album with the user entered name returns true if successful
        //Zach
        public bool createAlbum(string albumName)
        {
            //create file using directory, albumName
            try
            {
                FileStream f = File.Create(directory + albumFolder + "\\" + albumName + albumExtension);
                f.Close();

                return true;
            }
            catch
            {
                //if can't create file
                return false;
            }
        }

        //Deletes selected album with the given name, returns true if successful
        //Cavan & Zach
        public bool deleteAlbum(string albumName)
        {
            //delete file using directory, albumName
            try
            {
                File.Delete(albumName);
                return true;
            }
            catch
            {
                //if can't access file to delte
                return false;
            }
        }

        //Returns an array of the albums
        public string[] getAlbumList()
        {
            return Directory.GetFiles(directory + albumFolder, "*" + albumExtension);
        }
    }
}
