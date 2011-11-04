using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

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
            if (!Utilities.isValidString(userName))
            {
                MessageBox.Show("User name may only contain underscores, hyphens, and alphanumeric characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (Directory.Exists(usersFolder + userName))
            {
                MessageBox.Show("User already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (Utilities.checkStringLength(userName, 100))
            {
                MessageBox.Show("User name must be 100 characters or less.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }else{
                this.Close();
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            if (firstRun && MessageBox.Show("You must create a user before you can use this program. Canceling will exit the program. Are you sure you want to quit?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                this.Close();
            }
            else if (userName != "")
            {
                if (MessageBox.Show("Are you sure you want to discard changes?", "Discard Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    text_username.Text = "";
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }
    }
}
