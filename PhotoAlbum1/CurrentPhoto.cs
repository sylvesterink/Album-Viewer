using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoAlbumViewOfTheGods
{
    //Simple class representing the current photo selected by the user
    //Cavan
    class Photo
    {
        pictureData photoData; //data array representing current photo

        public string dateModified
        {
            get { return photoData.dateModified; }
            set { photoData.dateModified = value; }
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
