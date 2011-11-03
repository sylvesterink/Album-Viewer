using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing; //needed for Image compares
using System.Drawing.Drawing2D;
using System.Security.Cryptography; //needed for MD5 values
using System.Xml.Linq;

namespace PhotoAlbumViewOfTheGods
{
    

    //Public static class providing common functions for PhotoAlbum namespace
    //Cavan
    public static class Utilities
    {
        private static string printImagePath;
        public struct AllImagesInfo
        {
            public string MD5;
            public string path;
        }
        //Utility Function. Passed file path
        //Returns the filename without path or extension
        public static string getNameFromPath(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public static string getTimeStamp()
        {
            return ((long)((DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks) / 10000000)).ToString();
        }

        public static void printImage(string filePath)
        {
            printImagePath = filePath;
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintImage);
            pd.Print();
        }

        //http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/eb80fbbe-6b89-4c3d-9ede-88a2b105c714/
        private static void PrintImage(object o, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap img = new Bitmap(printImagePath);
            Point p = new Point(10, 10);
            e.Graphics.DrawImage(img, p);
        }

        public static int cleanUpPhotos()
        {
            int totalRemoved = 0;
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
            List<AllImagesInfo> _allImageInfo = getAllImageInfo();
            foreach(string photoPath in Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Photos","*.*",SearchOption.AllDirectories).Where(s=>s.EndsWith(".jpg",true,ci) || s.EndsWith(".png",true,ci) || s.EndsWith(".jpeg",true,ci) || s.EndsWith(".gif",true,ci) || s.EndsWith(".bmp",true,ci)))
            {
                if (!_allImageInfo.Exists(x => x.path == photoPath))
                {
                    try
                    {
                        if (File.Exists(photoPath))
                        {
                            File.Delete(photoPath);
                            totalRemoved++;
                        }
                        else
                        {
                            throw new Exception("File path does not exist!!");
                        }
                    }catch(Exception e){
                        System.Windows.Forms.MessageBox.Show("An error has occurred trying to remove photos. " + e.Message,"Error",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
            }
            return totalRemoved;
        }

        public static List<AllImagesInfo> getAllImageInfo()
        {
            AllImagesInfo imageInfo;
            List<AllImagesInfo> allImageInfo = new List<AllImagesInfo>();
            string[] albums = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Users", "*.album", SearchOption.AllDirectories);
            foreach (string album in albums)
            {
                XDocument xdoc = new XDocument();
                try
                {
                    xdoc = XDocument.Load(album);
                    var Albums = from AlbumInfo in xdoc.Descendants("AlbumInfo")
                    select new
                    {
                        Header = AlbumInfo.Attribute("name").Value,
                        Children = AlbumInfo.Descendants("PictureInfo")
                    };

                    foreach (var albumInfo in Albums)
                    {
                        foreach (var PictureInfo in albumInfo.Children)
                        {
                            imageInfo.MD5 = PictureInfo.Attribute("md5").Value;
                            imageInfo.path = PictureInfo.Attribute("path").Value;
                            allImageInfo.Add(imageInfo);
                        }
                    }
                }
                catch { }
            }
            return allImageInfo;
        }

        public static string CalculateMD5(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }

        //Utility Function. Passed integer ID
        //Returns string value of passed ID
        //Cavan
        public static string getIdFromInt(int id)
        {
            string value = Convert.ToString(id);
            //Add prefix 0s to maintain 3 digit ID.
            while (value.Count() < 3)
            {
                value = "0" + value;
            }
            return value;
        }

        //Utlity Function. Passed any string
        //Returns true if string doesn't contain any invalid file or path characters. False otherwise
        //Cavan
        public static bool isStringValid(string text)
        {
            //Load all invalid characters
            char[] invalid1 = Path.GetInvalidFileNameChars();
            char[] invalid2 = Path.GetInvalidPathChars();

            foreach (char value in text)
            {
                if (invalid1.Contains(value) || invalid2.Contains(value))
                    return false;
            }
            //No invalid matches found
            return true;
        }

        //Utility Function. Passed a string file path
        //Returns bool value representing if file can be interpreted as an Image
        //Cavan
        public static bool isImageValid(string photoPath)
        {
            try
            {
                using (Image temp = Image.FromFile(photoPath)) { };
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool isValidString(string text)
        {
            return (System.Text.RegularExpressions.Regex.IsMatch(text, "^[a-zA-Z0-9_ -]+$")) ? true : false;
        }

        //Utility Funtion. Passed string file path
        //Method: Removes extension, adds (#) if file exists. Reappends extension to new filename
        //Returns A unique name if a file with passed path already exists using (#) format at the end.
        //Cavan & Zach
        public static string getAppendName(string filePath)
        {
            int append = 2;
            string extension = Path.GetExtension(filePath);
            filePath = filePath.Substring(0, (filePath.Length - extension.Length)); //path without extensions
            while (true)
            {
                if (!File.Exists(filePath + "(" + append.ToString() + ")" + extension))
                    break;
                append++;
            }

            return filePath + "(" + append.ToString() + ")" + extension;
        }

        //Scale image to desired size. Passed image and desired size.
        //Method: calculates new width and height keeping aspect ratio.  Creates a new image of passed size without modifying passed image
        //Returns: new resized image.
        //Assistance and function from: http://stackoverflow.com/questions/7029601/scale-image-for-image-list-c
        //Cavan
        public static Image ScalImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            //Get new dimensions
            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            //Keep aspect ratio
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            //Get new size
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            //Create new image
            Bitmap b = new Bitmap(destWidth, destHeight);

            using (Graphics g = Graphics.FromImage((Image)b))
            {
                g.InterpolationMode = InterpolationMode.Low;
                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            }
            return (Image)b;
        }
         
    }
}
