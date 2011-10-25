using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PhotoAlbumViewOfTheGods
{
    class Photo
    {
        private string photoId;
        private string photoPath;
        private string photoName;
        private string photoDescription;
        private Size photoSize;

        public string id
        {
            get { return photoId; }
            set { photoId = value; }
        }

        public string path
        {
            get { return photoPath; }
            set { photoPath = value; }
        }

        public string name
        {
            get { return photoName; }
            set { photoName = value; }
        }

        public string description
        {
            get { return photoDescription; }
            set { photoDescription = value; }
        }

        public Size size
        {
            get { return photoSize; }
            set { photoSize = value; }
        }


    }
}
