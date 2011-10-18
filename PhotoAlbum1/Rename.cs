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
    //Rename Class Written by Zach
    //This form is used for the prompt displayed when the user wishes to renmae their selected album
    //Entered value is retrieved by acessing Newname variable
    public partial class Rename : Form
    {
        public Rename(string[] albums)
        {
            InitializeComponent();
        }
        //Zach: called to get the name from the Rename file dialog's text
        public string Newname
        {
            get
            {
                return Rename_box.Text;
            }
        }

        //Zach: When the Ok button is clicked, function will check if the string entered is not empty
        // or checks to see if it does not already exist
        private void OKbutton_Click_1(object sender, EventArgs e)
        {
            if (Rename_box.Text.Trim() == "" || !Utilities.isStringValid(Rename_box.Text))
            {
                Error_label.Text = "Invalid name";
                Error_label.Visible = true;
            }
            else if (File.Exists(Directory.GetCurrentDirectory() + Form_Main.FOLDER + "\\" + Rename_box.Text + Form_Main.FILETYPE))
            {
                Error_label.Text = "Duplicate album exists";
                Error_label.Visible = true;
            }
            else
            {
                this.Close();
            }
        }

        //Zach: clicking cancel sets the text to empty
        private void cancel_Click(object sender, EventArgs e)
        {
            Rename_box.Text = "";
            this.Close();
        }


    }
}
