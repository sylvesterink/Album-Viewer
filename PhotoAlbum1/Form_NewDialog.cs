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

        public string setValueOfCreate
        {
            set { button_Create.Text = value; }
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
            //if (textBox_AlbumName.Text == "" || !Utilities.isStringValid(textBox_AlbumName.Text))
            if (!Utilities.isValidString(textBox_AlbumName.Text))
            {
                MessageBox.Show("Your album name may only contain underscores, hyphens, and alphanumeric characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox_AlbumName.Focus();
            }
            //Checks for albums with the same name
            else if(albumsList.Contains(textBox_AlbumName.Text.ToUpper()))
            {
                MessageBox.Show("That album name has alrady been created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox_AlbumName.Focus();
            }
            else
            {
                //name is valid
                this.Close();
            }
        }

    }
}
