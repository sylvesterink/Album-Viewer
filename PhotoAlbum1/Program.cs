using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PhotoAlbumViewOfTheGods
{
    static class Program
    {
        /// <summary>
        /// Photo Album Application that allows the user to create albums, fill them with pictures,
        /// and modify their attributes such as: name and description.
        /// 
        /// Programmed by Cavan Crawford and Zach Gardner, Team 3
        /// Album Viewer: cycle 1
        /// First draft: September 21, 2011
        /// Last Modified: October 11, 2011
        /// No assistance from other students, only various websites for coding method references
        /// Permission to publish
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form_Main());
        }
    }
}
