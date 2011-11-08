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
    public partial class Form_About : Form
    {
        public Form_About()
        {
            InitializeComponent();

            Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            label2.Text = "Version: " + v.Major + "." + v.Minor;
            label5.Text = "(Build: " + v.Build + ", Revision: " + v.Revision + ")";
        }
    }
}
