using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PhotoAlbumViewOfTheGods
{
    /// <summary>
    /// Photo data structure
    /// </summary>
    public struct pictureData
    {
        public string id;
        public string path;
        public string name;
        public string description;
        public Size size;
        public string MD5;
        public string dateAdded;
        public string dateModified;
        public string albumPath;
    }

    /// <summary>
    /// Simple class representing the current photo selected by the user
    /// Cavan
    /// </summary>
    class Photo
    {
        pictureData photoData; //data array representing current photo

        public string dateModified
        {
            get { return photoData.dateModified; }
            set { photoData.dateModified = value; }
        }

        public string albumPath
        {
            get { return photoData.albumPath; }
            set { photoData.albumPath = value; }
        }

        public string MD5
        {
            get { return photoData.MD5; }
            set { photoData.MD5 = value; }
        }

        //Sets and returns current ID as string
        public string id
        {
            get { return photoData.id; }
            set { photoData.id = value; }
        }

        //Sets and returns current path value
        public string path
        {
            get { return photoData.path; }
            set { photoData.path = value; }
        }

        //Sets and returns current description
        public string description
        {
            get { return photoData.description; }
            set { photoData.description = value; }
        }

        //Sets and returns current photo name
        public string name
        {
            get { return photoData.name; }
            set { photoData.name = value; }
        }

        //Sets current photo. Passed pictureData structure
        public void setData(pictureData data)
        {
            photoData = data;
        }

        //Returns current photo data struct
        public pictureData getData()
        {
            return photoData;
        }

    }
}
