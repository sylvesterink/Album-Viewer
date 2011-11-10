using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing; //needed for Image compares
using System.Drawing.Drawing2D;
using System.Security.Cryptography; //needed for MD5 values
using System.Xml.Linq;
using System.Windows.Forms;

namespace PhotoAlbumViewOfTheGods
{
    /// <summary>
    /// Public static class providing common functions for PhotoAlbum namespace
    /// </summary>
    public static class Utilities
    {
        private static string printImagePath;
        public struct AllImagesInfo
        {
            public string MD5;
            public string path;
        }

        /// <summary>
        /// Returns the filename without path or extension
        /// </summary>
        /// <param name="path">Path to retrieve name from</param>
        /// <returns>Name of file</returns>
        public static string getNameFromPath(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Gets the current timestamp
        /// David
        /// </summary>
        /// <returns>Current time in milliseconds</returns>
        public static string getTimeStamp()
        {
            return ((long)((DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks) / 10000000)).ToString();
        }

        /// <summary>
        /// Print an image
        /// David
        /// TODO: Make it work properly with a print dialog
        /// </summary>
        /// <param name="filePath">path to image to print</param>
        public static void printImage(string filePath)
        {
            printImagePath = filePath;
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintImage);
            pd.Print();
        }

        /// <summary>
        /// Make sure the lengtho of a string is not longer than specified size
        /// </summary>
        /// <param name="text">string to check</param>
        /// <param name="length">Maximum size of string</param>
        /// <returns>True if string is within size limit</returns>
        public static bool checkStringLength(string text, int length)
        {
            return (text.Length > length) ? true : false;
        }

        /// <summary>
        /// Print the image
        /// http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/eb80fbbe-6b89-4c3d-9ede-88a2b105c714/
        /// David
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private static void PrintImage(object o, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap img = new Bitmap(printImagePath);
            Point p = new Point(10, 10);
            e.Graphics.DrawImage(img, p);
            img.Dispose(); //dispose of the image!!!!
        }

        /// <summary>
        /// Remove all photos not referenced by any album
        /// David
        /// </summary>
        /// <returns>Number of photos removed</returns>
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

        /// <summary>
        /// Gets a list of all the users
        /// David and Brandon
        /// </summary>
        /// <param name="usersDirectory">The Users directory to get the list from.</param>
        /// <returns>List of all users</returns>
        public static List<string> listOfUsers(string usersDirectory)
        {
            List<string> users = Directory.GetDirectories(usersDirectory).ToList();
            for (int i = 0; i < users.Count; i++)
            {
                users[i] = users[i].Replace(usersDirectory+"\\", "");
            }
            users.Sort();
            return users;
        }

        /// <summary>
        /// Get information of all the images
        /// </summary>
        /// <returns>Information for each image</returns>
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

        /// <summary>
        /// Calculate the MD5 sum value for an image
        /// </summary>
        /// <param name="path">Path to the image to calculate MD5 sum of</param>
        /// <returns>MD5 sum</returns>
        public static string CalculateMD5(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
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

        /// <summary>
        /// Utility Function. Passed integer ID
        /// Returns string value of passed ID
        /// Cavan
        /// </summary>
        /// <param name="id">Integer to convert to string</param>
        /// <returns>Integer in string form</returns>
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

        /// <summary>
        /// Utlity Function. Passed any string
        /// Returns true if string doesn't contain any invalid file or path characters. False otherwise
        /// Cavan
        /// </summary>
        /// <param name="text">String to check</param>
        /// <returns>True/False depending on whether invalid characters found</returns>
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

        /// <summary>
        /// Utility Function. Passed a string file path
        /// Returns bool value representing if file can be interpreted as an Image
        /// Cavan
        /// </summary>
        /// <param name="photoPath">Path to image file</param>
        /// <returns>True/false depending on whether image is acceptable</returns>
        public static bool isImageValid(string photoPath)
        {
            try
            {
                using (Image temp = Image.FromFile(photoPath))
                {
                    temp.Dispose();
                    return true;
                };
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if string contains valid characters
        /// David and Brandon
        /// </summary>
        /// <param name="text">String to check</param>
        /// <returns>True/false depending on whether image is acceptable</returns>
        public static bool isValidString(string text)
        {
            return (System.Text.RegularExpressions.Regex.IsMatch(text, "^[a-zA-Z0-9_ -]+$")) ? true : false;
        }

        /// <summary>
        /// Utility Funtion. Passed string file path
        /// Removes extension, adds (#) if file exists. Reappends extension to new filename
        /// Returns A unique name if a file with passed path already exists using (#) format at the end.
        /// Cavan & Zach
        /// </summary>
        /// <param name="filePath">Filename to append</param>
        /// <returns>Appended name</returns>
        public static string getAppendName(string filePath)
        {
            int append = 2;
            string extension = Path.GetExtension(filePath);
            string newPath;
            filePath = filePath.Substring(0, (filePath.Length - extension.Length)); //path without extensions
            while (true)
            {
                newPath = filePath + "(" + append.ToString() + ")" + extension;
                if (!File.Exists(newPath))
                {
                    break;
                }
                append++;
            }
            return newPath;
        }

        /// <summary>
        /// Scale image to desired size. Passed image and desired size.
        /// Method: calculates new width and height keeping aspect ratio.  Creates a new image of passed size without modifying passed image
        /// Returns: new resized image.
        /// Assistance and function from: http://stackoverflow.com/questions/7029601/scale-image-for-image-list-c
        /// Cavan
        /// </summary>
        /// <param name="imgToResize">Image path</param>
        /// <param name="size">New size of image</param>
        /// <returns>Resized image</returns>
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
