using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//Needed for drawing images
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace PhotoAlbumViewOfTheGods
{
    //Public form class for viewing photo in a new window
    //Passed file path and photo name
    //Returns nothing, all values are private
    //Cavan
    public partial class Form_Viewer : Form
    {
        private int _picWidth = 0;
        private int _picHeight = 0;
        private bool _isModified = false;
        private bool _isModifiedCurrent = false;
        private Image _imageViewer;

        private List<pictureData> _pictureList;
        private int _currentImage;

        public bool isModified
        {
            get { return _isModified; }
        }

        //Constructor function, saves passed values and calls main
        //Cavan
        public Form_Viewer(List<pictureData> pictureList, int currentImage)
        {
            InitializeComponent();
            _pictureList = pictureList;
            _currentImage = currentImage;

            //Sets painting variables
            //MAY NOT BE NECESSARY FOR PICTURE BOX
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            
            viewImage(_pictureList[_currentImage].path);
        }

        //Main function: sets all needed values for painting image
        //Passed file path
        //Cavan
        private void viewImage(string path)
        {
            _imageViewer = Image.FromFile(path);
            this.Text = _pictureList[_currentImage].name;
            imageNameLabel.Text = _pictureList[_currentImage].name;
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

            panel1.Top = clientSize.Height - panel1.Height;// panel1.Height;
            panel1.Width = clientSize.Width;

            pictureBox1.Width = clientSize.Width;
            pictureBox1.Height = clientSize.Height - panel1.Height - 1;
        }

        //Form closing event
        //Disposes created images and this Form thread
        //Cavan
        private void Form_Viewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveModifiedImage();
            this.Dispose();
        }

        private void saveModifiedImage()
        {
            if (_isModifiedCurrent && MessageBox.Show("Would you like to save the changes you have made?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _imageViewer.Save(_pictureList[_currentImage].path); //save the rotated image
                _isModifiedCurrent = false;
                _isModified = true;
            }
            else
            {
                _isModifiedCurrent = false;
            }

            _imageViewer.Dispose(); //release resources on loaded image
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
            _isModifiedCurrent = true;
            _imageViewer.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Image = _imageViewer;
            Invalidate();
        }

        private void button_rotate_ccw_Click(object sender, EventArgs e)
        {
            _isModifiedCurrent = true;
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
                _currentImage--;
            viewImage(_pictureList[_currentImage].path);
        }

        private void buttonNextImage_Click(object sender, EventArgs e)
        {
            saveModifiedImage();
            if (_currentImage < _pictureList.Count-1)
                _currentImage++;
            viewImage(_pictureList[_currentImage].path);
        }
    }
}
