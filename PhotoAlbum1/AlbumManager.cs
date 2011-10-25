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
                currentAlbum = albumNameList.First();
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

        //Saves the currently selected album
        //Brandon
        public bool saveCurrentAlbum()
        {
            return albumList[currentAlbum].save();
        }

        //Adds photo to currently selected album
        //Brandon
        public void addPhotoToCurrent(string path)
        {
            albumList[currentAlbum].addPhoto(path, directory, photoFolder);
        }

        //Removes photo from currently selected album
        //Brandon
        public void removePhotoFromCurrent(string id)
        {
            albumList[currentAlbum].removePhoto(id);
        }

        //Gets photo from currently selected album
        //Brandon
        public Photo getPhotoFromCurrent(int id)
        {
            return albumList[currentAlbum].getPhoto(id);
        }

        //Sets photo info in currently selected album
        //Brandon
        public bool setPhotoInCurrent(Photo data, int id)
        {
            return albumList[currentAlbum].setPhoto(data, id);
        }

        //Gets photo list in currently selected album
        //Brandon
        public string[] getPhotoListInCurrent()
        {
            return albumList[currentAlbum].getPhotoList();
        }

        //Changes the currently selected album to the sent album if it exists
        //Brandon
        public bool selectAlbum(string albumName)
        {
            if (albumList.ContainsKey(albumName))
            {
                currentAlbum = albumName;
                return true;
            }

            return false;
        }

        //Returns an array of the albums
        public string[] getAlbumList()
        {
            return Directory.GetFiles(directory + albumFolder, "*" + albumExtension);
        }
    }
}
