using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PhotoAlbumViewOfTheGods
{
    /// <summary>
    /// Public form class for viewing photo in a new window
    /// </summary>
    public partial class Form_Viewer : Form
    {
        private string _windowTitle;
        private int _currentImage;
        private int _picWidth = 0;
        private int _picHeight = 0;
        private int _totalFlips = 0;
        private bool _isModified = false;
        private bool _isModifiedCurrent = false;
        private Image _imageViewer;
        private List<pictureData> _pictureList;
        private Timer slideshowTimer;
        private List<modifiedImageInfo> _modifiedImages;

        public bool isModified
        {
            get { return _isModified; }
        }

        public List<pictureData> pictureList
        {
            get { return _pictureList; }
        }

        public List<modifiedImageInfo> modifiedImages
        {
            get { return _modifiedImages; }
        }

        /// <summary>
        /// Constructor function that initailizes member variables
        /// </summary>
        /// <param name="pictureList">All the pictures from the current album</param>
        /// <param name="currentImage">The image to start on</param>
        /// <param name="title">The title that will go in the top bar of the form</param>
        public Form_Viewer(List<pictureData> pictureList, int currentImage, string title)
        {
            InitializeComponent();
            _modifiedImages = new List<modifiedImageInfo>();
            _pictureList = pictureList;
            _currentImage = currentImage;
            _windowTitle = title;
            viewImage(_pictureList[_currentImage].path);
            slideshowTimer = new Timer();
            slideshowTimer.Tick += new EventHandler(TimerEventProcessor);
        }

        //Main function: sets all needed values for painting image
        //Passed file path
        //Cavan
        /// <summary>
        /// Changes the current image in the photo viewer to the passed string path
        /// </summary>
        /// <param name="path">The path to the image to display</param>
        private void viewImage(string path)
        {
            _imageViewer = Image.FromFile(path);
            this.Text = _windowTitle + " - " + _pictureList[_currentImage].name;
            imageNameLabel.Text = _pictureList[_currentImage].name;
            labelResolution.Text = _pictureList[_currentImage].size.Width + " x " + _pictureList[_currentImage].size.Height + " pixels";
            
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            time = time.AddSeconds(Convert.ToDouble(_pictureList[_currentImage].dateModified));
            labelModified.Text = "Last Modified: " + time.ToShortDateString();

            time = time.AddSeconds(-Convert.ToDouble(_pictureList[_currentImage].dateModified));
            time = time.AddSeconds(Convert.ToDouble(_pictureList[_currentImage].dateAdded));
            labelAdded.Text = "Added: " + time.ToShortDateString();

            _picWidth = _imageViewer.Size.Width;
            _picHeight = _imageViewer.Size.Height;
            //Sets window starting size
            this.Resize += new System.EventHandler(this.Form_Viewer_Resize); //Enables resize event handler
            timer_Resize.Start(); //resizes the window right after loading

            pictureBox1.Image = _imageViewer;
            resizeElements();        
        }

        //Form resize event function
        //Invalidates the form so current image is removed and paint event is called
        //Cavan
        private void Form_Viewer_Resize(object sender, EventArgs e)
        {
            resizeElements();
            Invalidate();
        }

        private void resizeElements()
        {
            Size clientSize = this.ClientSize;

            panel1.Top = clientSize.Height - panel1.Height;
            panel1.Width = clientSize.Width;

            panelButtons.Top = clientSize.Height - panel1.Height + 2;
            panelButtons.Left = clientSize.Width/2 - panelButtons.Width/2;

            pictureBox1.Width = clientSize.Width;
            pictureBox1.Height = clientSize.Height - panel1.Height - 1;
        }

        //Form closing event
        //Disposes created images and this Form thread
        //Cavan
        private void Form_Viewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (slideshowTimer.Enabled)
            {
                endSlideshow();
            }
            saveModifiedImage();
            _imageViewer.Dispose(); //release resources on loaded image
        }

        private void saveModifiedImage()
        {
            string imageID;
            string imagePath;
            string newPath;
            string newMD5;
            string newModified;
            modifiedImageInfo holder;

            List<Utilities.AllImagesInfo> allImages;

            try
            {
                if (_isModifiedCurrent && MessageBox.Show("Would you like to save the changes you have made?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {          
                    allImages = Utilities.getAllImageInfo();
                    imagePath = _pictureList[_currentImage].path;
                    imageID = _pictureList[_currentImage].id;

                    if (allImages.FindAll(s => s.path == imagePath).Count > 1) //if photo exists multiple times
                    {
                        newPath = Utilities.getAppendName(imagePath);
                    }else{
                        newPath = imagePath;
                    }

                    _imageViewer.Save(newPath);
                    try
                    {
                        XDocument xdoc = new XDocument();
                        xdoc = XDocument.Load(_pictureList[_currentImage].albumPath);
                        var Albums = from AlbumInfo in xdoc.Elements("Album").Elements("AlbumInfo").Elements("PictureInfo") select AlbumInfo;

                        foreach (XElement picture in Albums)
                        {
                            if (picture.Attribute("id").Value == imageID)
                            {
                                newMD5 = Utilities.CalculateMD5(newPath);
                                newModified = Utilities.getTimeStamp();
                                picture.Attribute("path").Value = newPath;
                                picture.Attribute("md5").Value = newMD5;
                                picture.Attribute("dateModified").Value = newModified;
                                pictureData temp = _pictureList[_currentImage];
                                temp.path = newPath;
                                temp.MD5 = newMD5;
                                temp.dateModified = newModified;
                                _pictureList[_currentImage] = temp;
                            }
                        }
                        xdoc.Save(_pictureList[_currentImage].albumPath);
                    }
                    catch
                    {
                        MessageBox.Show("you gone done sumthin wrong");
                    }
                    
                    _isModifiedCurrent = false;
                    _isModified = true;
                    _totalFlips = 0;
                    holder.path = newPath;
                    holder.id = "panel" + imageID;
                    _modifiedImages.Add(holder);
                }
                else
                {
                    _isModifiedCurrent = false;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Sorry but an error has occurred while saving your image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error: " + e.Message);
                this.Close();
            }
        }

        public struct modifiedImageInfo
        {
            public string path;
            public string id;
        }

        //Timer Tick event
        //Resizes the window to a more manageable size, starts the paint timer
        //Cavan
        private void timer_Resize_Tick(object sender, EventArgs e)
        {
            timer_Resize.Stop();
            this.Show();
        }

        private void button_rotate_cc_Click(object sender, EventArgs e)
        {
            _totalFlips = (_totalFlips >= 3) ? 0 : ++_totalFlips;
            _isModifiedCurrent = (_totalFlips == 0) ? false : true;
            _imageViewer.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Image = _imageViewer;
            Invalidate();
        }

        private void button_rotate_ccw_Click(object sender, EventArgs e)
        {
            _totalFlips = (_totalFlips <= -3) ? 0 : --_totalFlips;
            _isModifiedCurrent = (_totalFlips == 0) ? false : true;
            _imageViewer.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pictureBox1.Image = _imageViewer;
            Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Utilities.printImage(_pictureList[_currentImage].path);
        }

        private void buttonPrevImage_Click(object sender, EventArgs e)
        {
            saveModifiedImage();
            if (_currentImage > 0)
            {
                _totalFlips = 0;
                _currentImage--;
                viewImage(_pictureList[_currentImage].path);
            }
        }

        private void buttonNextImage_Click(object sender, EventArgs e)
        {
            saveModifiedImage();
            if (_currentImage < _pictureList.Count - 1)
            {
                _totalFlips = 0;
                _currentImage++;
                viewImage(_pictureList[_currentImage].path);
            }
        }

        private void buttonSlideShow_Click(object sender, EventArgs e)
        {
            if (!slideshowTimer.Enabled)
            {
                beginSlideshow();
            }
            else
            {
                endSlideshow();
            }
        }

        private void beginSlideshow()
        {
            saveModifiedImage();

            // Sets the timer interval to 5 seconds.
            slideshowTimer.Interval = 5000;
            slideshowTimer.Start();

            //disable other buttons
            buttonPrevImage.Enabled = false;
            button_rotate_cc.Enabled = false;
            button_rotate_ccw.Enabled = false;
            buttonNextImage.Enabled = false;
            button1.Enabled = false;

            buttonSlideShow.Text = "End Slideshow";
        }

        private void endSlideshow()
        {
            slideshowTimer.Stop();

            //enable other buttons
            buttonPrevImage.Enabled = true;
            button_rotate_cc.Enabled = true;
            button_rotate_ccw.Enabled = true;
            buttonNextImage.Enabled = true;
            button1.Enabled = true;

            buttonSlideShow.Text = "Begin Slideshow";
        }

        // This is the method to run when the timer is raised.
        // Source: http://msdn.microsoft.com/en-us/library/system.windows.forms.timer.tick(v=vs.71).aspx
        // Brandon
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            if (_currentImage < _pictureList.Count - 1)
                _currentImage++;
            else
                _currentImage = 0;

            viewImage(_pictureList[_currentImage].path);
        }
    }
}
