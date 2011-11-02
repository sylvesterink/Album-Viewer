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
        private float _drawX = 0;
        private float _drawY = 0;
        private bool _isModified = false;
        private Image _imageTemp;
        private Image _imageViewer;
        private string _imagePath;
        private string _photoName;

        //Constructor function, saves passed values and calls main
        //Cavan
        public Form_Viewer(string path, string imageName)
        {
            InitializeComponent();
            //Sets painting variables
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            _photoName = imageName;
            viewImage(path);
        }

        //Main function: sets all needed values for painting image
        //Passed file path
        //Cavan
        private void viewImage(string path)
        {
            _imageViewer = Image.FromFile(path);
            this.Text = _photoName;
            _imagePath = path;
            _picWidth = _imageViewer.Size.Width;
            _picHeight = _imageViewer.Size.Height;
            //Sets window starting size
            this.Resize += new System.EventHandler(this.Form_Viewer_Resize); //Enables resize event handler
            timer_Resize.Start(); //resizes the window right after loading
        }

        //Form resize event function
        //Invalidates the form so current image is removed and paint event is called
        //Cavan
        private void Form_Viewer_Resize(object sender, EventArgs e)
        {
            panel1.Left = this.Width / 2 - panel1.Width / 2;
            panel1.Top = this.Height - 100;// panel1.Height;
            Invalidate();
        }

        //Form paint event
        //Main drawing function. Draws image to window size or max image size
        //Cavan
        protected override void OnPaint(PaintEventArgs e)
        {
            // Call the OnPaint method of the base class.
            base.OnPaint(e);

            // Call methods of the System.Drawing.Graphics object.
            if (this.Width - 50 < _picWidth || this.Height - 50 < _picHeight)
            {
                //Form is smaller than image size, draw to form
                _imageTemp = Utilities.ScalImage(_imageViewer, new Size(this.Width - 50, this.Height - 50));
                e.Graphics.DrawImage(_imageTemp, _drawX, _drawY);
            }
            else
            {
                //Form is larger than image size, draw image at its normal size
                _imageTemp = Utilities.ScalImage(_imageViewer, new Size(_picWidth, _picHeight));
                e.Graphics.DrawImage(_imageTemp, _drawX, _drawY);
            }
            _imageTemp.Dispose();
        } 



        //Timer tick event. 
        //Draws picture name soon after form load. Stops after 1 tick
        //Cavan
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            using (Graphics gWrite = this.CreateGraphics())
            {
                gWrite.DrawString(_photoName, new Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Black), new Point(0, this.Height - 52));
            }
            timer_Paint.Stop();
        }

        //Form end resize event.
        //Draws picture name when done resizing window
        //Cavan
        private void Form_Viewer_ResizeEnd(object sender, EventArgs e)
        {
            using (Graphics gWrite = this.CreateGraphics()) 
            {
                gWrite.DrawString(_photoName, new Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Black), new Point(0, this.Height - 52));
            }
        }

        //Form closing event
        //Disposes created images and this Form thread
        //Cavan
        private void Form_Viewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            _imageTemp.Dispose(); //release resources on temp image

            if (_isModified && MessageBox.Show("Would you like to save the changes you have made?","Confirm Save",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _imageViewer.Save(_imagePath); //save the rotated image
            }

            _imageViewer.Dispose(); //release resources on loaded image
            this.Dispose();
        }

        //Timer Tick event
        //Resizes the window to a more manageable size, starts the paint timer
        //Cavan
        private void timer_Resize_Tick(object sender, EventArgs e)
        {
            timer_Resize.Stop();
            this.Show();
            Size screenSize = new Size((int)(Screen.PrimaryScreen.Bounds.Width / 1.5), (int)(Screen.PrimaryScreen.Bounds.Height / 1.5));
            if (_picWidth > screenSize.Width || _picHeight > screenSize.Height)
            {
                this.Size = screenSize;
            }
            else
            {
                this.Width = _picWidth + 50;
                this.Height = _picHeight + 50;
            } 
            timer_Paint.Start(); //Used to call initial paint values that cannot be called on load events
        }

        private void button_rotate_cc_Click(object sender, EventArgs e)
        {
            _isModified = true;
            _imageViewer.RotateFlip(RotateFlipType.Rotate90FlipNone);
            Invalidate();
        }

        private void button_rotate_ccw_Click(object sender, EventArgs e)
        {
            _isModified = true;
            _imageViewer.RotateFlip(RotateFlipType.Rotate270FlipNone);
            Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Utilities.printImage(_imagePath);
        }
    }
}
