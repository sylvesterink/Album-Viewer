﻿using System;
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
        //Picture size
        int picWidth = 0;
        int picHeight = 0;
        //Draw locations
        float drawX = 0;
        float drawY = 0;

        Image image_Temp;
        Image image_Viewer;

        //Passed Values
        string imagePath;
        string photoName;

        //Constructor function, saves passed values and calls main
        //Cavan
        public Form_Viewer(string path, string pName)
        {
            InitializeComponent();
            //Sets painting variables
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            photoName = pName;
            viewImage(path);
        }

        //Main function: sets all needed values for painting image
        //Passed file path
        //Cavan
        private void viewImage(string path)
        {
            image_Viewer = Image.FromFile(path);
            this.Text = photoName;
            imagePath = path;
            picWidth = image_Viewer.Size.Width;
            picHeight = image_Viewer.Size.Height;
            //Sets window starting size
            this.Resize += new System.EventHandler(this.Form_Viewer_Resize); //Enables resize event handler
            timer_Resize.Start(); //resizes the window right after loading
        }

        //Form resize event function
        //Invalidates the form so current image is removed and paint event is called
        //Cavan
        private void Form_Viewer_Resize(object sender, EventArgs e)
        {
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
            if (this.Width - 50 < picWidth || this.Height - 50 < picHeight)
            {
                //Form is smaller than image size, draw to form
                image_Temp = Utilities.ScalImage(image_Viewer, new Size(this.Width - 50, this.Height - 50));
                e.Graphics.DrawImage(image_Temp, drawX, drawY);
            }
            else
            {
                //Form is larger than image size, draw image at its normal size
                image_Temp = Utilities.ScalImage(image_Viewer, new Size(picWidth, picHeight));
                e.Graphics.DrawImage(image_Temp, drawX, drawY);
            }
        } 



        //Timer tick event. 
        //Draws picture name soon after form load. Stops after 1 tick
        //Cavan
        private void timer1_Tick(object sender, EventArgs e)
        {
            using (Graphics gWrite = this.CreateGraphics())
            {
                gWrite.DrawString(photoName, new Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Black), new Point(0, this.Height - 52));
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
                gWrite.DrawString(photoName, new Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Black), new Point(0, this.Height - 52));

            }
        }

        //Form closing event
        //Disposes created images and this Form thread
        //Cavan
        private void Form_Viewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            image_Temp.Dispose();
            image_Viewer.Dispose();
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
            if (picWidth > screenSize.Width || picHeight > screenSize.Height)
            {
                this.Size = screenSize;
            }
            else
            {
                this.Width = picWidth + 50;
                this.Height = picHeight + 50;
            } 
            timer_Paint.Start(); //Used to call initial paint values that cannot be called on load events
            
        }

    }
}
