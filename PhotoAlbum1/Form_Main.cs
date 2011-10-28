using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
//using System.Resources;
//using System.Text.RegularExpressions;
//using System.Threading;

namespace PhotoAlbumViewOfTheGods
{
    //Photo data structure
    struct pictureData
    {
        public string id;
        public string path;
        public string name;
        public string description;
        public Size size;
    }

    //Main UI form
    public partial class Form_Main : Form
    {
        //Directory Locations
        private const string LAST_USER_FILE = "last_user.ofthegods";
        public const string FILETYPE = ".album"; // Must match XMLInterface FILETYPE const
        public const string FOLDER_USERS = "\\Users"; //Folder to store album files
        public const string FOLDER_PHOTOS = "\\Photos"; //Folder to store all images
        string APP_DIRECTORY = Directory.GetCurrentDirectory();
        const string APPNAME = "Photo Album Viewer of the Gods";
        string lastUsedFolder; //Stores last dir for importing images
        public string currentUser;

        //Thumbnail Panel attributes
        Size frameSize;
        //public string picViewPath = "";
        Panel panel_CurrentPanel = new Panel();
        //int currentId = 0;
        Color color_backColor = Color.White; 
        Color color_borderColor = Color.Black;
        const int framesPerRow = 6;
        int frameWidth = 0; //defined in form_load function
        int frameSpacingY = 20;
        int frameSpacingX = 0; // defined in form_load function
        float borderFactor = 8; //Border size will be framewidth / borderFactor

        //Data Structures
        pictureData pictureDataStored;
        List<pictureData> pictureList = new List<pictureData>(); //used to store XMLInterface class list
        XMLInterface albumData; //class to interact with album files
        string[] nameList;
        TreeNode treeNode = new TreeNode();
        Photo currentPhoto = new Photo();

        //Constructor function
        //Initializes form
        public Form_Main()
        {
            InitializeComponent();
        }

        //View button event. Currently not used
        //private void button_View_Click(object sender, EventArgs e)
        //{
        //    openViewer();
        //}

        //Panel double click event
        //If panel was double left clicked calls openviewer
        //Cavan
        private void panel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                openViewer();
        }

        //Treehide button enter event
        //Colors button to show it
        //Cavan
        private void treeMin_MouseEnter(object sender, EventArgs e)
        {
            buttonHideTree.BackColor = Color.Gray;
        }

        //Treehide button leave event
        //Colors button to hide it
        //Cavan
        private void treeMin_MouseLeave(object sender, EventArgs e)
        {
            buttonHideTree.BackColor = Color.Transparent;
        }

        //Main panel click event
        //Focuses panel so it can be scrolled if needed
        //Zach & Cavan
        private void panel1_Click(object sender, EventArgs e)
        {
            panel1.Focus();
        }

        //Context Menu view event
        //Calls openViewer to view current picture
        //Cavan
        private void viewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openViewer();
        }

        //Context Menu remove event
        //Calls deletePhoto
        //Zach & Cavan
        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            deletePhoto();
        }

        //Panel delete key event (currently implemented with protected override)
        //private void panel_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)Keys.Delete)
        //        deletePhoto();
        //}

        //Textbox picture name leave event
        //Clears rename text label when textbox loses focus
        //Cavan
        private void textBox_Name_Leave_1(object sender, EventArgs e)
        {
            label_NameError.Visible = false;
        }

        //MenuItem Exit click event
        //Close Program
        //Zach & Cavan
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Menu close
        //Calls albumClose to close current album
        //Zach & Cavan
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            albumClose();
        }

        //Remove the currently selected picture from the thumbnail view
        //private void button_Remove_Click(object sender, EventArgs e)
        //{
        //    deletePhoto();
        //}

        //Album tree double click event
        //Double clicking on the album name will open it
        //Zach & Cavan
        private void treeView_Albums_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            openAlbum(treeNode.Name);
        }

        //Menu rename album event
        //Calls renameAlbum to rename the current album
        //Zach & Cavan
        private void toolStripMenuItem_Rename_Click(object sender, EventArgs e)
        {
            renameAlbum();
        }

        //Album tree context menu open event
        //calls openAlbum to open the specified album
        //Zach & Cavan
        private void openAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openAlbum(treeNode.Name);
        }

        //Album tree context menu delete event
        //Calls deleteALbum to delete the specified album
        //Zach & Cavan
        private void deleteAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteAlbum(treeNode.Name);
        }

        //Album tree context menu rename event
        //Calls renameAlbum to rename the specified album
        //Zach & Cavan
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renameAlbum();
        }

        //Menu delete album event
        //calls deleteAlbum to delete the current album
        //Cavan
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteAlbum(albumData.filePath);
        }

        //Update button click event
        //Updates the current photo with the name and description
        //Updates the data array with the new info as well
        //Updates the list view with the new data
        //Zach & Cavan
        private void RenamePic_Click(object sender, EventArgs e)
        {
            renamePicture();
            currentPhoto.description = richTextBox_Description.Text;
            albumData.setData(currentPhoto.getData(), Convert.ToInt32(currentPhoto.id));
            populateList();
            button_RenamePic.Enabled = false;
            panel1.Focus();
        }

        //Context menu remove event
        //Calls deletePhoto to remove the slected photo
        //Cavan
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deletePhoto();
        }

        //Form Main load event
        //Initializes current directory and creates folders if they don't exist
        //Checks for any album files and updates menus to allow open if any are found
        //Sets thumbnail frame spacing and updates the album list tree
        //Cavan
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = APPNAME;

            if (File.Exists(APP_DIRECTORY +"\\"+ LAST_USER_FILE))
            {
                TextReader s = new StreamReader(APP_DIRECTORY +"\\"+ LAST_USER_FILE);
                currentUser = s.ReadLine();
                s.Close();
                if (!Directory.Exists(APP_DIRECTORY + FOLDER_USERS + "\\" + currentUser))
                {
                    MessageBox.Show("An error has occurred - Error: 0x44GGD7", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                File.Create(APP_DIRECTORY + LAST_USER_FILE);
            }

            if (!Directory.Exists(APP_DIRECTORY + FOLDER_USERS) || Directory.GetDirectories(APP_DIRECTORY + FOLDER_USERS).Count() == 0)
            {
                MessageBox.Show("Welcome to Photo Album Viewer of the Gods. Before you can continue you must create a new user account.", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Directory.CreateDirectory(APP_DIRECTORY + FOLDER_USERS);
                Form_NewUser namePrompt = new Form_NewUser(APP_DIRECTORY + FOLDER_USERS, true);
                namePrompt.ShowDialog();
                //Retrieve name entered by user
                string userName = namePrompt.userName;
                namePrompt.Dispose();
                //Create File if user pressed create

                Directory.CreateDirectory(APP_DIRECTORY + FOLDER_USERS + "\\" + userName);
                TextWriter tw = new StreamWriter(APP_DIRECTORY + "\\" + LAST_USER_FILE);
                tw.Write(userName);
                tw.Close();
                currentUser = userName;
            }


            //Initilize data structure
            albumData = new XMLInterface(APP_DIRECTORY, FOLDER_USERS+"\\"+currentUser, FOLDER_PHOTOS, FILETYPE);

            //Check Folders and create if needed
            if (Directory.Exists(APP_DIRECTORY + FOLDER_USERS) && Directory.GetDirectories(APP_DIRECTORY+FOLDER_USERS).Count() > 0)
            {
                string[] files = albumData.getAlbumList();
                if (files.Count() != 0)
                {
                    openToolStripMenuItem.Enabled = true;
                }
            }
            
            if (!Directory.Exists(APP_DIRECTORY + FOLDER_PHOTOS))
                Directory.CreateDirectory(APP_DIRECTORY + FOLDER_PHOTOS);

            albumData.filePath = ""; //stat with no album open
            //Set up Frames
            panel1.BackColor = color_backColor;
            frameWidth = (panel1.Width / (framesPerRow + 1)) - 4;
            frameSpacingX = frameWidth / framesPerRow;
            frameSize = new Size(frameWidth, frameWidth);
            //Update Border size
            panel_Border.Size = new Size((int)(frameWidth + (frameWidth / borderFactor)), (int)(frameWidth + (frameWidth / borderFactor)));

            populateTree(); //Display any album files
            panel1.Focus(); //Start focus on main panel so any tree nodes are not slected
        }

        //Populates the album list with any found album files
        //Cavan
        private void populateTree()
        {
            TreeNode node;
            string[] albumList = Directory.GetFiles(APP_DIRECTORY + FOLDER_USERS + "\\" + currentUser, "*" + FILETYPE);
            treeView_Albums.Nodes.Clear(); //Clears nodes so they can be added again

            //Loop through each found file and add it to tree. Set its path and show the file name
            for (int i = 0; i < albumList.Length; i++)
            {
                node = new TreeNode();
                node.Name = albumList[i];
                node.Text = Utilities.getNameFromPath(albumList[i]);
                node.ImageIndex = 0;
                treeView_Albums.Nodes.Add(node);
                //Highlight if album node is open
                if (node.Name == albumData.filePath)
                    node.BackColor = Color.LightGray;
            }
        }

        //Populates picture list view
        //Method: retrieve album data and clear tree
        //        loop through each picture and add its name, description, and path to the photo tree.
        //Cavan
        private void populateList()
        {
            TreeNode newNode;
            pictureList = albumData.getDataList();
            treeView_Pictures.Nodes.Clear();
            string[] nodes = albumData.getPictureList();
            string tempText = "";
            for (int i = 0; i < pictureList.Count; i++)
            {
                //Photo list
                newNode = new TreeNode();
                newNode.Name = pictureList[i].path;
                //Append description if there is any
                tempText = pictureList[i].description;
                if (tempText.Length > 50)
                    newNode.Text = pictureList[i].name + "   |   " + tempText.Substring(0, 50) + "...";
                else if (tempText.Length > 0)
                    newNode.Text = pictureList[i].name + "   |   " + pictureList[i].description.Substring(0, pictureList[i].description.Length);
                else
                    newNode.Text = pictureList[i].name;
                treeView_Pictures.Nodes.Add(newNode);
            }
        }

        //Thumbnail click event
        //Shows border panel around clicked panel. Sets current photo. enables and populates picture info panel
        //Focusses thumbnail panel for delete button catching
        //Calls selectNode method
        //If right clicked also shows the context menu
        //Cavan & Zach
        private void panel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                panel_CurrentPanel.BackColor = color_backColor;
                Panel p = sender as Panel;
                string id = p.Name.Substring(p.Name.Length - 3, 3); //get id
                //currentId = Convert.ToInt32(id);
                //Set border panel
                panel_Border.Location = new Point((p.Location.X - (frameWidth / 20 + 2)), (p.Location.Y - (frameWidth / 20)) - 3);
                panel_Border.Show();
                p.BringToFront();
                pictureDataStored = albumData.getData(Convert.ToInt32(id)); //get album data
                currentPhoto.setData(pictureDataStored); //Sets current photo properties

                //Set picture info panel
                textBox_Name.Text = pictureDataStored.name;
                //picViewPath = pictureDataStored.path;
                richTextBox_Description.Text = pictureDataStored.description;
                label_picSize.Text = (pictureDataStored.size.Width + ", " + pictureDataStored.size.Height);

                p.BackColor = panel_Border.BackColor; //reset last thubmnail backcolor

                panel_CurrentPanel = p;
                p.Focus(); //so delete key works
                panel_PictureData.Enabled = true;
                removeToolStripMenuItem.Enabled = true;
                selectTreeNode(Convert.ToInt32(id)); //update list view
                label_NameError.Visible = false;

            }
            //Show context menu
            if (e.Button == MouseButtons.Right)
            {
                //fileNameToolStripMenuItem.Text = currentPhoto.name;
                fileNameToolStripMenuItem.Visible = false;
                toolStripSeparator_Picture.Visible = false;
                contextMenuStrip_Picture.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        //Picture list tree node click
        //Updates picture data panel with picture info. Sets current panel and find corresponding panel
        //Sets border and thubmnails for thumbnail view
        //calls select node method
        //Cavan & Zach
        private void treeView_Pictures_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                //Get data
                int id = e.Node.Index;
                pictureDataStored = albumData.getData(Convert.ToInt32(id));
                currentPhoto.setData(pictureDataStored);
                //Set data panel
                textBox_Name.Text = pictureDataStored.name;
                richTextBox_Description.Text = pictureDataStored.description;
                label_picSize.Text = pictureDataStored.size.Width + ", " + pictureDataStored.size.Height;
                panel_PictureData.Enabled = true;
                removeToolStripMenuItem.Enabled = true;
                label_NameError.Visible = false;
                panel_CurrentPanel.BackColor = panel1.BackColor;
                //Finds panel based on id and sets border and current panel so both views match
                foreach (Panel value in panel1.Controls)
                {
                    if (value.Name == ("panel" + Utilities.getIdFromInt(id)))
                    {
                        panel_CurrentPanel = value;
                        panel_CurrentPanel.Name = "panel" + Utilities.getIdFromInt(id);
                        panel_Border.Location = new Point((value.Location.X - (frameWidth / 20 + 2)), (value.Location.Y - (frameWidth / 20)) - 3);
                        panel_Border.SendToBack();
                        panel_Border.Show();
                        panel_CurrentPanel.BackColor = panel_Border.BackColor;
                        //break;
                    }
                }
                selectTreeNode(Convert.ToInt32(id));

            }
            //Show context menu
            if (e.Button == MouseButtons.Right)
            {
                fileNameToolStripMenuItem.Text = currentPhoto.name;
                fileNameToolStripMenuItem.Visible = true;
                toolStripSeparator_Picture.Visible = true;
                treeView_Pictures.SelectedNode = e.Node;
                contextMenuStrip_Picture.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        //Gets node from passed index ID. Sets that node as the selected node
        //Cavan & Zach
        private void selectTreeNode(int id)
        {
            //Get the node with corresponding id and select it so that both views match
            TreeNode node = new TreeNode();
            int intId = Convert.ToInt32(id);
            foreach (TreeNode value in treeView_Pictures.Nodes)
            {
                node = value;
                if (node.Index == intId)
                    break;
            }
            treeView_Pictures.SelectedNode = node;
        }


        //Album tree node click event
        //Saves the clicked node. If right clicked shows context menu
        //Cavan & Zach
        private void treeView_Albums_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Left)
            {
                treeNode.Name = e.Node.Name;
            }
            if (e.Button == MouseButtons.Right)
            {
                treeView_Albums.SelectedNode = e.Node;
                toolStripMenuItem_AlbumName.Text = Utilities.getNameFromPath(treeNode.Name) + ":";
                contextMenuStrip_Tree.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        //Treehide button click event
        //Toggles the album tree
        //Method: Sets text and posiions of the button, line panel, and album label
        //Cavan
        private void buttonHideTree_Click(object sender, EventArgs e)
        {
            //Hides the album view
            if (buttonHideTree.Text == "<")
            {
                buttonHideTree.Text = ">";
                buttonHideTree.Location = new Point(5, buttonHideTree.Location.Y);
                panel_Line.Location = new Point(0, panel_Line.Location.Y);
                treeView_Albums.Hide();
                label_TreeHelp.Visible = false;
                this.Width = this.Size.Width - treeView_Albums.Width - 11;
            }
            else
            {
                //Shows the album view
                buttonHideTree.Text = "<";
                treeView_Albums.Show();
                this.Width = this.Size.Width + treeView_Albums.Width + 11;
                panel_Line.Location = new Point(tabControl_List.Location.X - 10, panel_Line.Location.Y);
                buttonHideTree.Location = new Point(panel_Line.Location.X - buttonHideTree.Width, buttonHideTree.Location.Y);
                label_TreeHelp.Visible = true;
            }
            buttonHideTree.ForeColor = Color.White;
        }

        //Menu open album event
        //Sets file dialog filter, title, and multisilect. Shows the dialog. Calls openAlbum if a file was selected
        //Cavan
        /*private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog_Load.Filter = "Album files|*.abm"; //Allow only album files
            openFileDialog_Load.Title = "Open Album";
            openFileDialog_Load.Multiselect = false;
            openFileDialog_Load.InitialDirectory = DIRECTORY + FOLDER;
            if (openFileDialog_Load.ShowDialog() == DialogResult.OK)
            {
                openAlbum(openFileDialog_Load.FileName);
            }
        }*/

        //Open Album method. Passed the file path to open
        //Close current album if one is open.
        //Failure to access file shows errors
        //Cavan
        private void openAlbum(string albumPath)
        {
            //If there is a file open, close it then open, else just open
            if (!(albumData.filePath == ""))
            {
                //If album can't close show error message and stop
                if (albumClose())
                {
                    //Open
                    openAlbumFromFile(albumPath);
                }
                else
                {
                    handleError("Unable to access file to save");
                }
            }
            else
            {
                //Open
                openAlbumFromFile(albumPath);
            }
            panel1.Focus();
        }

        //Open album from file
        //Sets album to open. calls loadAlbum. Populates the picture views
        //Failure to load file calls errors
        //Cavan
        private void openAlbumFromFile(string path)
        {
            //Open
            //albumData.filePath = path;
            if (!albumData.loadAlbum(path))
            {
                handleError("Unable to load album.");
            }
            else
            {
                //Show picture views and set title and label text with file name
                populateScreen();
                populateList();
                populateTree();
                toolStripStatusLabel_ALbumName.Text = Utilities.getNameFromPath(path) + "  |";
                albumData.filePath = path;
                enableItems();
                this.Text = APPNAME + " : " + Utilities.getNameFromPath(path);
            }
        }

        //Display thumbnails method
        //Method: Retrieves current data list. For each picture, creates a thumbnail panel. Shows and positions each thumbnail
        //        Sets each panel's properties and event handlers.
        //Cavan
        private void populateScreen()
        {
            panel1.Hide();
            Panel tempPanel;
            pictureData picData = new pictureData();
            int scrollPosition = panel1.VerticalScroll.Value; //save user scroll position
            
            pictureList = albumData.getDataList();
            
            panel1.VerticalScroll.Value = 0; //set scroll to top for correct reference when setting panel location
            int loopCount = pictureList.Count();
            for (int i = 0; i < loopCount; i++)
            {          
                //If panel is already added, skip it (mainly for when inporting pictures)
                if (panel1.Controls.Find("panel" + Utilities.getIdFromInt(i), false).Count() > 0)
                    continue;

                picData = pictureList[i];
                tempPanel = getNewThumbnail(picData, i); //Create new panel
                //tempPanel.BackColor = color_backColor; Used if main panel color has changed.
                panel1.Controls.Add(tempPanel); //Add panel to main panel

                tempPanel.Location = getFrameLocation(i); //Sets panel position from frame variables               
            }

            
            foreach (Panel value in panel1.Controls)
            {
                value.Show();
            }
            panel_Border.Visible = false;
            
            panel1.Show();
            //Update Tooltip size
            toolStripStatusLabel_Total.Text = pictureList.Count.ToString();
            //Update nameList
            nameList = albumData.getPictureList();
            panel1.Focus();
            panel1.VerticalScroll.Value = scrollPosition; //return user scroll posiiton
        }

        //Ceate New Panel thumbnail
        //Parameters: accepts picturedata struct
        //Creates a new panel, sets its picture data, and sets background image to scaled down image from file
        //Returns thumbnial panel
        private Panel getNewThumbnail(pictureData imageData, int id)
        {
            Panel newPanel = new Panel();
            newPanel.Visible = false;
            newPanel.Name = "panel" + imageData.id;
            newPanel.Size = frameSize;
            newPanel.BackgroundImageLayout = ImageLayout.Zoom;
            //Image.GetThumbnailImageAbort imageCallback = new Image.GetThumbnailImageAbort(thumbailCallback);
            //Attempts to load background Image. If fail, loads a default image and updates description
            try
            {
                Image tempImage = Image.FromFile(imageData.path);
                imageData.size = tempImage.Size;
                albumData.setData(imageData, id);
                newPanel.BackgroundImage = Utilities.ScalImage(tempImage, new Size(frameSize.Width, frameSize.Height));
                //newPanel.BackgroundImage = Image.FromFile(imageData.path).GetThumbnailImage(frameSize.Width, frameSize.Height, imageCallback, IntPtr.Zero);
                imageData.size = newPanel.BackgroundImage.Size;
            }
            catch
            {
                newPanel.BackgroundImage = Resources.Resource1.warning;
                imageData.size = new Size(0, 0);
                if (!imageData.description.Contains("File not found"))
                {
                    imageData.description = "File not found\n\n" + imageData.description;
                }
                albumData.setData(imageData, id);
            }
            //Panel event handlers
            newPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_MouseClick);
            newPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.panel_MouseDoubleClick);
           
            return newPanel;
        }

        //Frame Position function
        //Returns frame position based on int id
        //Cavan
        private Point getFrameLocation(int id)
        {    
            Point p = new Point(((id % (framesPerRow)) * (frameWidth + frameSpacingX) + (frameSpacingX / 2)), 
                ((id / framesPerRow) * (frameWidth + frameSpacingY)) + 6); //+6 to offset from top
            return p;
        }

        //Thumbnail Callback funtion
        //Used to retrieve a thumbnail
        //Cavan
        private bool thumbailCallback()
        {
            return false;
        }

        /*Background and border panel color change
        private void changeBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog_Background.ShowDialog() == DialogResult.OK)
            {
                color_backColor = colorDialog_Background.Color;
            }
            //Update colors
            panel1.Enabled = false;
            foreach (Panel value in panel1.Controls)
            {
                value.BackColor = color_backColor;
            }
            panel1.BackColor = color_backColor;
            panel1.Enabled = true;
            panel_Border.BackColor = Color.FromArgb((byte)~color_backColor.R, (byte)~color_backColor.G, (byte)~color_backColor.B);
        }
         * */

        //Name textbox key press
        //Method: If enter was pressed, update info and reload list view
        //        If any other key, show update button
        private void textBox_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                renamePicture();
                currentPhoto.description = richTextBox_Description.Text;
                albumData.setData(currentPhoto.getData(), Convert.ToInt32(currentPhoto.id));
                populateList();
                button_RenamePic.Enabled = false;
                e.Handled = true;
            }
            else
            {
                label_NameError.Visible = false;
                button_RenamePic.Enabled = true;
            }
        }

        //Rename Picture method
        //Checks for invalid names and updates picture data. Notifyes user of success or failure
        //Cavan
        private void renamePicture()
        {
            //Check for invalid characters or blank name. If invalid, show error message
            if (textBox_Name.Text.Trim() != "" && Utilities.isStringValid(textBox_Name.Text))
            {
                //Valid name, update picture data
                currentPhoto.name = textBox_Name.Text;
                string id = panel_CurrentPanel.Name.Substring(panel_CurrentPanel.Name.Length - 3, 3); //get id
                pictureDataStored = albumData.getData(Convert.ToInt32(id));
                textBox_Name.Text = textBox_Name.Text.Trim(); //trim spaces
                pictureDataStored.name = textBox_Name.Text;
                albumData.setData(pictureDataStored, Convert.ToInt32(id));
                nameList = albumData.getPictureList(); //Update nameList
                //Notify user of success
                label_NameError.ForeColor = Color.Green;
                label_NameError.Text = "Update successful";
                label_NameError.Visible = true;
                populateList();
            }
            else
            {
                label_NameError.Text = "Invalid Name";
                label_NameError.ForeColor = Color.DarkRed;
                label_NameError.Visible = true;
            }

        }

        //Description keypress event
        //Shows update button
        //Cavan & Zach
        private void richTextBox_Description_KeyPress(object sender, KeyPressEventArgs e)
        {
            button_RenamePic.Enabled = true;
        }

        /*Checkbox save event. Not used
        private void checkBox_Description_Click(object sender, EventArgs e)
        {
            //pictureDataStored = albumData.getData(Convert.ToInt32(currentPhoto.id));
            //pictureDataStored.description = richTextBox_Description.Text;
            currentPhoto.description = richTextBox_Description.Text;
            //albumData.setData(pictureDataStored, Convert.ToInt32(currentPhoto.id));
            albumData.setData(currentPhoto.getData(), Convert.ToInt32(currentPhoto.id));
            //checkBox_Description.Checked = true;
            populateList();
        }
         * */

        //MenuItem New click event
        //Calls createAlbum and and enables menu items if successfull. Updates album tree list. Displays errors if can't access files
        //Cavan
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_NewFileDialog namePrompt = new Form_NewFileDialog(albumData.getAlbumList(), FILETYPE);
            namePrompt.ShowDialog();
            //Retrieve name entered by user
            string albumName = namePrompt.albumNameValue;
            namePrompt.Dispose();
            //Create File if user pressed create
            if (albumName != "")
            {
                //Attmpe to close file
                if (albumClose())
                {
                    //Create new album
                    if (albumData.createAlbum(albumName))
                    {
                        enableItems(); //enables menu items

                        albumData.filePath = APP_DIRECTORY + FOLDER_USERS + "\\" + albumName + FILETYPE; //sets current file path
                        populateTree();
                        this.Text = APPNAME + " : " + Utilities.getNameFromPath(albumData.filePath);
                    }
                    else
                    {
                        handleError("Failed to create album");
                    }
                }
                else
                {
                    handleError("Unable to access file to save");
                }

            }
        }

        //Enable album method
        //Enables menu items and cals error handle to clear errors
        private void enableItems()
        {
            //closeToolStripMenuItem.Enabled = true;
            importToolStripMenuItem.Enabled = true;
            toolStripStatusLabel_TotalLabel.Text = "Pictures: ";
            deleteToolStripMenuItem.Enabled = true;
            renameToolStripMenuItem.Enabled = true;
        }

        //Disable items method
        //Disables menu items, label texts and form title
        //Cavan & Zach
        private void disableItems()
        {
            //Reset text and lables
            toolStripStatusLabel_ALbumName.Text = "";
            toolStripStatusLabel_TotalLabel.Text = "No open album";
            toolStripStatusLabel_Total.Text = "";
            label_NameError.Visible = false;
            //Reset menu options
            //closeToolStripMenuItem.Enabled = false;
            importToolStripMenuItem.Enabled = false;
            removeToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
            renameToolStripMenuItem.Enabled = false;
            //Set form values
            this.Text = APPNAME;
        }

        //Close album method
        //Attempts to save album. calls error if failed. If succeded clears displays and trees, disables menu and clear texts. Clears errors
        //Cavan
        private bool albumClose()
        {
            if (!albumData.saveAlbum())
            {
                handleError("Unable to save file");
                return false;
            }
            else
            {
                clearDisplay();
                //Clear Trees
                treeView_Pictures.Nodes.Clear();
                albumData.clearAlbum();

                disableItems();

                populateTree(); //remove highlight


                return true;
            }
        }

        //Clear thumbnails method
        //Disposes all panels and images created for thumbnails. Disables picture data panel and clears panel data.
        //Cavan
        private void clearDisplay()
        {
            int controlCount = panel1.Controls.Count - 1;
            int controlCounter = 0;
            Control[] controlList = new Control[controlCount];
            //get all panels, excluding border panel
            foreach (Panel value in panel1.Controls)
            {
                if (value != panel_Border)
                {
                    controlList[controlCounter] = value;
                    controlCounter++;
                }
            }
            //dispoose all panels
            for (int j = controlList.Count() - 1; j >= 0; j--)
            {
                controlList[j].Dispose();
            }

            //Clear and disable data panel
            panel_Border.Visible = false;
            textBox_Name.Text = "";
            richTextBox_Description.Text = "";
            panel_PictureData.Enabled = false;
            label_picSize.Text = "";
            removeToolStripMenuItem.Enabled = false;
        }

        //MenuItem import picture click event
        //Clears errors and sets up load file dialog
        //Prompts user to find files and adds them to the album. Calls addPhoto passing path. Updates both views
        //Checks if image is valid, calls errors if it is not.
        //Cavan
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog_Load.Filter = "Image Files|*.jpg;*.jpeg;*.bmp;*.gif;*.png";
            openFileDialog_Load.Title = "Import Photo";
            openFileDialog_Load.FileName = "";
            openFileDialog_Load.InitialDirectory = lastUsedFolder;
            openFileDialog_Load.Multiselect = true; //alow loading of multiple files
            if (openFileDialog_Load.ShowDialog() == DialogResult.OK)
            {
                lastUsedFolder = Path.GetDirectoryName(openFileDialog_Load.FileNames[0]);
                foreach (string value in openFileDialog_Load.FileNames)
                {
                    //check if file is valid
                    if (Utilities.isImageValid(value))
                    {
                        albumData.addPhoto(value);
                        populateScreen();
                        populateList();
                    }
                    else
                    {
                        handleError("Invalid Photo Selected: " + value);
                    }
                }
            }
        }

        //Form Closing event
        //Saves current album, removes event handler and closes
        //Cavan
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            albumClose();
            this.FormClosing -= Form_Main_FormClosing;
            this.Close();
        }

        //Album tree double click event
        //Calls openViewer to view picture
        //Cavan
        private void treeView_Pictures_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
                openViewer();
        }

        //Rename album method
        //Closes current album. creates name prompt. Retrieves user entered value. Calls loadAlbum passing album name.
        //Calls deleteAlbum, to delete the album file. Calls saveAlbum, to write file with the new name.
        //Calls error handler if any files cannot be accessed
        //Zach
        private void renameAlbum()
        {
            if (albumClose())
            {
                Rename namePrompt = new Rename(nameList);
                namePrompt.ShowDialog();
                //Retrieve name entered by user
                string Replacement = namePrompt.Newname;
                namePrompt.Dispose();
                if (Replacement != "")
                {
                    albumData.loadAlbum(treeNode.Name);
                    if (albumData.deleteAlbum(treeNode.Name))
                    {
                        albumData.filePath = APP_DIRECTORY + FOLDER_USERS + "\\" + Replacement + FILETYPE;
                        albumData.saveAlbum();
                        albumData.clearAlbum();
                        populateTree();
                    }
                    else
                    {
                        handleError( "Unable to access file to delete");
                    }
                }
            }
            else
            {
                handleError("Unable to access file to save");

            }
        }

        //Delete album method
        //Passed file to delete
        //Closes album if its open. Deletes file. Calls error handler if any files can't be accessed. Updates album tree
        //Zach
        private void deleteAlbum(string filePathToDelete)
        {
            if (albumData.filePath == filePathToDelete)
            {
                if (albumClose())
                {
                    if (albumData.deleteAlbum(filePathToDelete))
                    {
                        populateTree();   
                    }
                    else
                    {
                        handleError("Unable to access file to delete");
                    }
                }
            }
            if (albumData.deleteAlbum(filePathToDelete))
                populateTree();
            else
            {
                handleError("Unable to access file to delete");
            }
        }

        //View image method
        //Creates a new viewer form passing the photo path and name. Shows new form
        //Cavan
        private void openViewer()
        {
            if (File.Exists(currentPhoto.path)) //Shows nothing if file wasn't found (warning images)
            {
                Form_Viewer picView = new Form_Viewer(currentPhoto.path, currentPhoto.name);
                //picView.Show();
            }
        }

        //Delete Key event
        //Overrides current function to intercept delete key. Used in order to accept delete on panels
        //Calls delete photo if delete was pressed while on thumbnail panel.
        //If delete was pressed on picture tree, finds picture id, and deletes photo only if a node is selected
        //Resends message if conditions weren't matched
        //Cavan
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            TreeNode node = new TreeNode();
            node.Name = null;
            if (keyData == Keys.Delete && ActiveControl.GetType() == typeof(Panel) && ActiveControl != panel1 && ActiveControl != panel_Border)
            {
                deletePhoto();
            }
            else if (keyData == Keys.Delete && ActiveControl.GetType() == typeof(TreeView) && ActiveControl == treeView_Pictures)
            {
                foreach (TreeNode value in treeView_Pictures.Nodes)
                {
                    if (value.IsSelected == true)
                    {
                        deletePhoto();
                        break;
                    }
                }

            }
            //Also can intercept enter keys and anything else. Currently disabled.
            //else if (keyData == Keys.Enter && ActiveControl.GetType() == typeof(Panel) && ActiveControl != panel1 && ActiveControl != panel_Border)
            //    openViewer();
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        //Remove photo method
        //Removes picture data from data set. Clears display, then reloads both views
        //Cavan
        private void deletePhoto()
        {
            albumData.RemovePic(currentPhoto.id);

            clearDisplay();
            populateScreen();
            populateList();
        }

        //Update info button event
        //Hides name text box label and disables update button
        //Zach
        private void button_RenamePic_Leave(object sender, EventArgs e)
        {
            label_NameError.Visible = false;
            button_RenamePic.Enabled = false;
        }

        //List mode switch click event
        //If a picture is selected, focuses picture list to show selected node. Used when switching from thumbnail view to list view
        //Cavan & Zach
        private void tabControl_List_Click(object sender, EventArgs e)
        {
            if (panel_PictureData.Enabled)
            {
                treeView_Pictures.Focus();
                panel1.ScrollControlIntoView(panel_CurrentPanel);
            }
        }

        //Event Handler
        //Clears errors
        //Cavan
        private void handleError(string message)
        {
            //toolStripStatusLabel_Error.Text = "";
            //toolStripStatusLabel_Clear.Visible = false;
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void newUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_NewUser namePrompt = new Form_NewUser(APP_DIRECTORY+FOLDER_USERS,false);
            namePrompt.ShowDialog();
            //Retrieve name entered by user
            string userName = namePrompt.userName;
            namePrompt.Dispose();
            //Create File if user pressed create
            if (userName != "")
            {
                Directory.CreateDirectory(APP_DIRECTORY+FOLDER_USERS + "\\" + userName);
            }
        }
    }


}
