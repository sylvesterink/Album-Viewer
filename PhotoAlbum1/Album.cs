using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace PhotoAlbumViewOfTheGods
{
    class Album
    {
        private List<Photo> photoList = new List<Photo>();
        private string _filePath = "";

        //Loads the album information from a .abm(XML format) file and adds information to a query
        //then, the datalist vector will get each pictures information
        //Zach
        public bool load(string albumName)
        {
            Photo PicData = new Photo();

            //if fails to get or read file
            //return false;
            XDocument xdoc = new XDocument();
            try
            {
                xdoc = XDocument.Load(albumName);
            }
            catch
            {
                return false;
            }
            //Run query, only header item is the album name. children contains all picture information
            var Albums = from AlbumInfo in xdoc.Descendants("AlbumInfo")
                         select new
                         {
                             Header = AlbumInfo.Attribute("name").Value,
                             Children = AlbumInfo.Descendants("PictureInfo")
                         };
            //Loop through results and add the info to the datalist for each picture
            foreach (var albumInfo in Albums)
            {
                foreach (var PictureInfo in albumInfo.Children)
                {
                    PicData.id = PictureInfo.Attribute("id").Value;
                    PicData.path = PictureInfo.Attribute("path").Value;
                    PicData.name = PictureInfo.Attribute("name").Value;
                    PicData.description = PictureInfo.Attribute("description").Value;
                    photoList.Add(PicData);
                }
            }
            //read and load xml data into dataList
            _filePath = albumName;
            return true;
        }

        //Zach: Save function that queries the datalist and writes it in XML format
        public bool save()
        {
            //loop and load each element into pictureData struct, then 
            //write values in xml
            if (_filePath != "")
            {
                try
                {
                    XElement xmlSave = new XElement("Album",
                        new XElement("AlbumInfo",
                            new XAttribute("name", _filePath),
                            from picInfo in photoList
                            select new XElement("PictureInfo",
                                       new XAttribute("id", picInfo.id),
                                       new XAttribute("path", picInfo.path),
                                       new XAttribute("name", picInfo.name),
                                       new XAttribute("description", picInfo.description)
                            )
                           )
                        );
                    //_filepath is the albums name
                    xmlSave.Save(_filePath);
                    _filePath = "";
                    return true;
                }
                catch
                { return false; }
            }
            //Still return true if there was nothing to save
            return true;
        }

        //Function is called when importing a picture, where path is the picture's path
        //Copies the pic to a new photos folder
        //Compares pictures if a duplicate name exists
        //adds the image to the datalist
        //sets a default description to nothing
        //Zach & Cavan
        public void addPhoto(string path, string directory, string photoFolder)
        {
            Photo image = new Photo();
            image.description = "";
            string newPath;
            string extension = Path.GetExtension(path);
            string[] imageList = Directory.GetFiles(directory + photoFolder);//, "*" + extension);
            bool flagPath = false;

            int idCount = photoList.Count;
            newPath = directory + photoFolder + "\\" + Utilities.getNameFromPath(path) + extension;
            //Checks to see if a pic with the same name exists, checks if it exists then compare the pics
            try
            {
                if (!File.Exists(newPath))
                {
                    File.Copy(path, newPath);
                }
                else
                {
                    foreach (string value in imageList)
                    {
                        if (Utilities.areImagesEqual(path, value))
                        {
                            flagPath = true;
                            newPath = value;
                            break;
                        }
                    }

                    if (!flagPath)
                    {
                        newPath = Utilities.getAppendName(newPath);
                        File.Copy(path, newPath);
                    }

                }

                image.path = newPath;
                image.name = Utilities.getNameFromPath(path);
                image.id = Utilities.getIdFromInt(idCount);
                photoList.Add(image);
            }
            catch { }
        }

        //Zach: Parameter is a string value of what thumbnail was clicked on
        //Removes picture from panel; gets the datalist count and removes the 
        //Currently selected picture, the vector should update itself when an
        //element is removed
        //TODO: id should not be a string
        public void removePhoto(string id)
        {
            try
            {
                int numofpics = photoList.Count - 1;
                int IDvalue;
                int CurrentID = Convert.ToInt32(id);
                Photo Removing;
                photoList.RemoveAt(CurrentID);
                while (CurrentID != numofpics)
                {
                    Removing = photoList[CurrentID];
                    IDvalue = Convert.ToInt32(Removing.id) - 1;
                    Removing.id = Utilities.getIdFromInt(IDvalue);
                    photoList[CurrentID] = Removing;
                    CurrentID++;
                }
            }
            catch { }
        }

        public Photo getPhoto(int id)
        {
            Photo tempData = new Photo();
            tempData.name = "";
            tempData.id = "0";
            tempData.path = "";
            tempData.description = "";
            //if invalid id is received
            if (id >= photoList.Count)
                return tempData;
            else
            {
                tempData = photoList[id];
                return tempData;
            }
        }

        //sets the data from the dataStruct into the dataList vector, returns false if id is too big
        public bool setPhoto(Photo dataStruct, int id)
        {
            if (id >= photoList.Count)
                return false;
            else
            {
                photoList[id] = dataStruct;
                return true;
            }
        }

        //Gets the list of pictures which are used for the list view
        //Names are retrieved from the datalist
        public string[] getPhotoList()
        {
            string[] picList = new string[photoList.Count];

            for (int i = 0; i < photoList.Count; i++)
            {
                picList[i] = photoList[i].name;
            }

            return picList;
        }

        //Returns true if no data is loaded into the list
        public bool isEmpty()
        {
            if (photoList.Count == 0)
                return true;
            else
                return false;
        }

        //GET DATALIST? !!!
    }
}
