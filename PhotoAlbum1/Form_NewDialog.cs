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
    //New album name form
    //Prompts user to input a name for a new album
    //Value is accessed by albumNameValue after form is closed
    //Must be disposed explicitly
    //Passed a list of current album paths and file extension
    //Cavan
    public partial class Form_NewFileDialog : Form
    {
        string[] albumsList;
        //Return Value
        public string albumNameValue
        {
            get
            {
                return textBox_AlbumName.Text;
            }
        }

        //Constructor function.
        //Passed a list of current album paths and file extension
        //Method: gets file names from file paths
        //Cavan
        public Form_NewFileDialog(string[] albums, string ext)
        {
            InitializeComponent();
            albumsList = albums;
            for (int i = albumsList.Count() - 1; i >= 0; i--)
            {
                albumsList[i] = Utilities.getNameFromPath(albumsList[i]).ToUpper();
            }
            label_Extension.Text = ext;
        }

        //Cancel button event
        //Closes the form setting the name value to null
        //Cavan
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            textBox_AlbumName.Text = "";
            this.Close();
        }

        //Create button event
        //Checks for invalid names. If valid, closes form
        //Cavan
        private void button_Create_Click(object sender, EventArgs e)
        {
            //Check if name is null or contains invalid characters
            if (textBox_AlbumName.Text == "" || !Utilities.isStringValid(textBox_AlbumName.Text))
            {
                label_Error.Text = "Invalid File Name";
                label_Error.Visible = true;
            }
            //Checks for albums with the same name
            else if(albumsList.Contains(textBox_AlbumName.Text.ToUpper()))
            {
                label_Error.Text = "Album name already exists";
                label_Error.Visible = true;
            }
            else
            {
                //name is valid
                this.Close();
            }
        }

    }
}
