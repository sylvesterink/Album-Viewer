using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;
//Cavan & Zach: This class deals with events that use or modify data in the XML(.abm) files
namespace PhotoAlbumViewOfTheGods
{
    class XMLInterface
    {
        private List<pictureData> dataList = new List<pictureData>();//Vector containing picture info
        private string directory;
        private string albumFolder;
        private string photoFolder;
        private string albumExtension;
        private string usersDirectory;
        private string _filePath = "";
        private string _currentAlbumName;
        private string _currentUser;

        //Gets filepath
        public string filePath
        {
            get {return _filePath;}
            set {
                _currentAlbumName = System.IO.Path.GetFileName(value);
                _filePath = value; }
        }

        public string CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                albumFolder = usersDirectory + "\\" + _currentUser;
            }
        }

        public string currentAlbum
        {
            get { return _currentAlbumName; }
            set { _currentAlbumName = value; }
        }

        //Constructor that initializes variables used in this class
        public XMLInterface(string folderDir, string albumDirectory, string photoDirectory, string ext, string currentUser)
        {
            directory = folderDir;
            usersDirectory = albumDirectory;
            albumFolder = albumDirectory + "\\" + currentUser;
            photoFolder = photoDirectory;
            albumExtension = ext;
            _currentUser = currentUser;
        }

        //sets the data from the dataStruct into the dataList vector, returns false if id is too big
        public bool setData(pictureData dataStruct, int id)
        {
            if (id >= dataList.Count)
                return false;
            else
            {
                dataList[id] = dataStruct;
                return true;
            }
        }

        //Function is called when importing a picture, where path is the picture's path
        //Copies the pic to a new photos folder
        //Compares pictires if a duplicate name exists
        //adds the image to the datalist
        //sets a default description to nothing
        //Zach & Cavan
        public void addPhoto(string path)
        {
            saveAlbum();
            List<Utilities.AllImagesInfo> allImages = Utilities.getAllImageInfo();
            pictureData image = new pictureData();
            image.description = "";
            string newPath;
            bool flagPath = false;
            string imageName = Utilities.getNameFromPath(path);
            string calculateMD5 = Utilities.CalculateMD5(path);
            int totalImages = allImages.Count;
            string dateAdded = Utilities.getTimeStamp();
            
            newPath = directory + photoFolder + "\\" + imageName + Path.GetExtension(path);
            //Checks to see if a pic with the same name exists, checks if it exists then compare the pics
            try
            {
                if (!File.Exists(newPath))
                {
                    File.Copy(path, newPath);
                }
                else
                {
                    for (int i = 0; i < totalImages; i++)
                    {
                        if (allImages[i].MD5 == calculateMD5)
                        {
                            flagPath = true;
                            newPath = allImages[i].path;
                            break;
                        }
                    }

                    if (!flagPath)
                    {
                        File.Copy(path, Utilities.getAppendName(newPath));
                    }

                }

                image.dateAdded = dateAdded;
                image.dateModified = "0";
                image.MD5 = calculateMD5;
                image.path = newPath;
                image.name = imageName;
                image.id = Utilities.getIdFromInt(dataList.Count);
                dataList.Add(image);
                saveAlbum();
            }
            catch { }
            
        }

        //Function is used to get picture info from and element in the datalist
        //The id of the picture is sent and the info for that pic is returned
        public pictureData getData(int id)
        {
            pictureData tempData = new pictureData();
            tempData.name = "";
            tempData.id = "0";
            tempData.path = "";
            tempData.description = "";
            //if invalid id is received
            if (id >= dataList.Count)
                return tempData;
            else
            {
                tempData = dataList[id];
                return tempData;
            }
        }

        //Gets the list of pictures which are used for the list view
        //Names are retrieved from the datalist
        public string[] getPictureList()
        {
            string[] picList = new string[dataList.Count];
            if (isEmpty())
                return picList;
            else
            {
                for (int i = dataList.Count - 1; i >= 0; i--)
                {
                    picList[i] = dataList[i].name;
                }
                return picList;
            }
        }

        //Returns an array of the albums
        public string[] getAlbumList()
        {
            return Directory.GetFiles(directory + albumFolder, "*" + albumExtension);
        }

        //Clears datalist vector
        public void clearAlbum()
        {
            dataList.Clear();
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

        //Creates a new album with the user entered name returns true if successful
        //Zach
        public bool createAlbum(string albumName)
        {
            //create file using directory, albumName
            try
            {
                FileStream f = File.Create(directory + albumFolder + "\\" + albumName + albumExtension);
                f.Close();
                //Close Current
                dataList.Clear();
                return true;
            }
            catch
            {
                //if can't create file
                return false;
            }
        }
        //Loads the album information from a .abm(XML format) file and adds information to a query
        //then, the datalist vector will get each pictures information
        //Zach
        public bool loadAlbum(string albumName)
        {
            pictureData PicData = new pictureData();
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
            //Run query, only header item is the album name. children contains all pciture information
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
                    PicData.MD5 = PictureInfo.Attribute("md5").Value;
                    PicData.dateAdded = PictureInfo.Attribute("dateAdded").Value;
                    PicData.dateModified = PictureInfo.Attribute("dateModified").Value;
                    PicData.albumPath = albumName;
                    dataList.Add(PicData);
                }
            }
            //read and load xml data into dataList
            _filePath = albumName;
            _currentAlbumName = albumName;

            return true;


        }

        //Zach: Save function that queries the datalist and writes it in XML format
        public bool saveAlbum()
        {
            //loop and load each element into pictureData struct, then 
            //write values in xml
            string saveTo = directory + albumFolder + "\\" + _currentAlbumName;
            if (_currentAlbumName != "")
            {
                try
                {
                    XElement xmlSave = new XElement("Album",
                        new XElement("AlbumInfo",
                            new XAttribute("name", saveTo),
                            from picInfo in dataList
                            select new XElement("PictureInfo",
                                        new XAttribute("id", picInfo.id),
                                        new XAttribute("path", picInfo.path),
                                        new XAttribute("name", picInfo.name),
                                        new XAttribute("description", picInfo.description),
                                        new XAttribute("md5", picInfo.MD5),
                                        new XAttribute("dateAdded", picInfo.dateAdded),
                                        new XAttribute("dateModified",picInfo.dateModified)
                            )
                        ));
                    //_filepath is the albums name
                    xmlSave.Save(saveTo);
                }
                catch (SystemException e)
                {
                    MessageBox.Show("Error: "+e.Message);
                    return false;
                }                
            }
            //Still return true if there was nothing to save
            return true;
        }

        //Returns current xml data in a vector of struct pictureData
        public List<pictureData> getDataList()
        {
            return dataList;
        }
    
        //Returns true if no data is loaded into the list
        public bool isEmpty()
        {
            if(dataList.Count == 0)
                return true;
            else
                return false;
        }

        //Zach: Parameter is a string value of what thumbnail was clicked on
        //Removes picture from panel; gets the datalist count and removes the 
        //Curerntly selected picture, the vector should update itself when an
        //element is removed
        public void RemovePic(string id)
        {
            try
            {
                int numofpics = dataList.Count - 1;
                int IDvalue;
                int CurrentID = Convert.ToInt32(id);
                pictureData Removing;
                dataList.RemoveAt(CurrentID);
                while (CurrentID != numofpics)
                {
                    Removing = dataList[CurrentID];
                    IDvalue = Convert.ToInt32(Removing.id) - 1;
                    Removing.id = Utilities.getIdFromInt(IDvalue);
                    dataList[CurrentID] = Removing;
                    CurrentID++;
                }
            }
            catch { }
        }
    
    }
}
