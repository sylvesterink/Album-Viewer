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
    /// <summary>
    /// Form used for searching for a photo by name
    /// </summary>
    public partial class Form_Search : Form
    {
        public string searchTerm
        {
            get
            {
                return textBoxSearchTerm.Text;
            }
        }

        /// <summary>
        /// Initializd form
        /// </summary>
        public Form_Search()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Accept search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Clear searched item and cancel search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            textBoxSearchTerm.Text = "";
            this.Close();
        }
    }
}
