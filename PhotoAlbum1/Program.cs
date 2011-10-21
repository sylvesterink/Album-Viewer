using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System;
namespace PhotoAlbumViewOfTheGods
{
    static class Program
    {
        
        public static Process PriorProcess()
        // URL: http://www.ai.uga.edu/mc/SingleInstance.html
        // Returns a System.Diagnostics.Process pointing to
        // a pre-existing process with the same name as the
        // current one, if any; or null if the current process
        // is unique.
        {
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process p in procs)
            {
                if ((p.Id != curr.Id) &&
                    (p.MainModule.FileName == curr.MainModule.FileName))
                    return p;
            }
            return null;
        }
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
            if (PriorProcess() != null)
            {
                MessageBox.Show("You can only run one instance of PhotoAlbumViewOfTheGods at a time.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (Environment.Version.Major < 4)
            {
                MessageBox.Show(".NET version 4.0 or greater is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form_Main());
        }
    }
}
