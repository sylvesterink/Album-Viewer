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
    public partial class Form_Search : Form
    {
        public string searchTerm
        {
            get
            {
                return textBoxSearchTerm.Text;
            }
        }

        public Form_Search()
        {
            InitializeComponent();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            textBoxSearchTerm.Text = "";
            this.Close();
        }
    }
}
