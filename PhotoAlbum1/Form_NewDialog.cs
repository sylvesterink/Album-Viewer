using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PhotoAlbumViewOfTheGods
{
    /// <summary>
    /// New album name form
    /// Prompts user to input a name for a new album
    /// Value is accessed by albumNameValue after form is closed
    /// Must be disposed explicitly
    /// Passed a list of current album paths and file extension
    /// Cavan
    /// </summary>
    public partial class Form_NewFileDialog : Form
    {
        private string[] _albumsList;
        private bool _userClose = false;
        public string albumNameValue
        {
            get
            {
                return textBox_AlbumName.Text;
            }
        }

        /// <summary>
        /// Set value of text box
        /// </summary>
        public string setValueOfCreate
        {
            set { button_Create.Text = value; }
        }

        /// <summary>
        /// Constructor function.
        /// gets file names from file paths
        /// Cavan
        /// </summary>
        /// <param name="albums">List of current album paths</param>
        /// <param name="ext">file extension</param>
        public Form_NewFileDialog(string[] albums, string ext)
        {
            InitializeComponent();
            _albumsList = albums;
            int totalAlbums = _albumsList.Length;
            for (int i = 0; i < totalAlbums; i++)
            {
                _albumsList[i] = Utilities.getNameFromPath(_albumsList[i]);
            }
        }

        /// <summary>
        /// Cancel button event
        /// Closes the form setting the name value to null
        /// Cavan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            textBox_AlbumName.Text = "";
            _userClose = true;
            this.Close();
        }

        /// <summary>
        /// Create button event
        /// Checks for invalid names. If valid, closes form
        /// Cavan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Create_Click(object sender, EventArgs e)
        {
            //Check if name is null or contains invalid characters
            //if (textBox_AlbumName.Text == "" || !Utilities.isStringValid(textBox_AlbumName.Text))
            string albumName = textBox_AlbumName.Text.Trim();
            if (albumName == "")
            {
                MessageBox.Show("Your album name cannot be blank.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox_AlbumName.Focus();
            }else if (!Utilities.isValidString(albumName))
            {
                MessageBox.Show("Your album name may only contain underscores, hyphens, spaces, and alphanumeric characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox_AlbumName.Focus();
            }
            //Checks for albums with the same name
            else if (_albumsList.Contains(albumName, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("That album name has alrady been created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox_AlbumName.Focus();
            }
            else if (albumName == "")
            {
                MessageBox.Show("You cannot have a blank album name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_AlbumName.Focus();
            }
            else if (Utilities.checkStringLength(albumName, 100))
            {
                MessageBox.Show("Album names must be 100 characters or less.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_AlbumName.Focus();
            }
            else
            {
                _userClose = true;
                this.Close();
            }
        }

        /// <summary>
        /// Cancels creation of new item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_NewFileDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!_userClose)
                {
                    textBox_AlbumName.Text = "";
                }
            }
        }
    }
}
