using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing; //needed for Image compares
using System.Drawing.Drawing2D;
using System.Security.Cryptography; //needed for MD5 values

namespace PhotoAlbumViewOfTheGods
{
    //Public static class providing common functions for PhotoAlbum namespace
    //Cavan
    public static class Utilities
    {
        //Utility Function. Passed file path
        //Returns the filename without path or extension
        public static string getNameFromPath(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
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

        //Utility Funtion. Passed string file path
        //Method: Removes extension, adds (#) if file exists. Reappends extension to new filename
        //Returns A unique name if a file with passed path already exists using (#) format at the end.
        //Cavan & Zach
        public static string getAppendName(string filePath)
        {
            int append = 2;
            string extension = Path.GetExtension(filePath);
            filePath = filePath.Substring(0, (filePath.Length - extension.Length)); //path without extensions
            while(true)
            {
                if(!File.Exists(filePath + "(" + append.ToString() + ")" + extension))
                    break;
                append++;
            }
            
            return filePath + "(" + append.ToString() + ")" + extension;
        }

        //Utility Function. Passed two file paths
        //Method: Loda path into images. Gets byte arrays of pictures. Get MD5 value of byte arrays. Compares MD5 values
        //Returns bool represeting if the 2 files are Images and MD5 values are equal
        //assistance from http://www.dreamincode.net/code/snippet2859.htm
        //Cavan
        public static bool areImagesEqual(string path1, string path2)
        {
            ImageConverter imageConverter = new ImageConverter();
            Image photo1;
            Image photo2;
            try
            {
                photo1 = Image.FromFile(path1);
                photo2 = Image.FromFile(path2);
            }
            catch { return false; }//returns false if both files are not images

            byte[] photoBytes1 = new byte[1];
            byte[] photoBytes2 = new byte[1];
            //Convert images into byte arrays
            photoBytes1 = (byte[])imageConverter.ConvertTo(photo1, photoBytes1.GetType());
            photoBytes2 = (byte[])imageConverter.ConvertTo(photo2, photoBytes2.GetType());

            MD5 m = MD5.Create();
            byte[] h1 = m.ComputeHash(photoBytes1);
            byte[] h2 = m.ComputeHash(photoBytes2);

            //Check for equal MD5s
            for (int i = 0; i < h1.Length && i < h2.Length; i++)
            {
                if (!(h1[i] == h2[i]))
                {
                    //mismatch found
                    photo1.Dispose();
                    photo2.Dispose();
                    return false;
                }
            }

            photo1.Dispose();
            photo2.Dispose();
            return true;
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
