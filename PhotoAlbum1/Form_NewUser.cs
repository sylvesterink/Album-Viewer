using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhotoAlbumViewOfTheGods
{
    public partial class Form_NewUser : Form
    {
        public string userName
        {
            get
            {
                return text_username.Text;
            }
        }

        private bool firstRun;
        private string usersFolder;

        public Form_NewUser(string directory, bool isFirstRun)
        {
            InitializeComponent();
            usersFolder = directory;
            firstRun = isFirstRun;
            if (firstRun)
            {
                this.ControlBox = false;
            }
        }

        private void button_adduser_Click(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(text_username.Text, "^[a-zA-Z0-9_-]+$"))
            {
                MessageBox.Show("User name may only contain underscores, hyphens, and alphanumeric characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (System.IO.Directory.Exists(usersFolder + text_username.Text))
            {
                MessageBox.Show("User already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else{
                this.Close();
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            if (firstRun)
            {
                MessageBox.Show("You must create a user before you can continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }else if (text_username.Text != "") {
                if (MessageBox.Show("Are you sure you want to discard changes?", "Discard Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    text_username.Text = "";
                    this.Close();
                }
            }
        }
    }
}
