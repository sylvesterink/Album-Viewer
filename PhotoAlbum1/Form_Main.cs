using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PhotoAlbumViewOfTheGods
{
    //Main UI form
    public partial class Form_Main : Form
    {
        public const string FOLDER_USERS = "\\Users"; //Folder to store album files
        public const string FOLDER_PHOTOS = "\\Photos"; //Folder to store all images

        //Directory Locations
        private string _currentUser;
        private string _directoryCurrent;
        private string _directoryPhotos;
        private string _directoryUsers;
        private string _directoryCurrentUser;

        //Constant Names
        private const string _constantUsers = "\\Users";
        private const string _constantPhotos = "\\Photos";
        private const string _constantFileType = ".album";
        private const string _constantLastUser = "\\last_user.ofthegods";
        private const string _constantAppName = "Photo Album Viewer of the Gods";
        
        //File/Folder Locations
        private string _lastUserFile;
        private string _lastImportedFrom;

        //Thumbnail Panel attributes
        private Size frameSize;
        private Panel _panel_CurrentPanel;
        private Color color_backColor = Color.White;
        private Color color_borderColor = Color.Black;//Color.LightCyan;
        private const int _framesPerRow = 6;
        private int _frameWidth = 0;
        private const int _frameSpacingY = 20;
        private int _frameSpacingX = 0;
        private const float _borderFactor = 8;

        //Memember Variables
        private int _periodCounter = 3;

        //Data Structures    
        private TreeNode _treeNode;
        private Photo _currentPhoto;
        private XMLInterface _albumData;
        private List<pictureData> _pictureList;
        private pictureData _pictureDataStored;
        private List<string> _allUsers;
        private System.Timers.Timer _timer = new System.Timers.Timer();

        /// <summary>
        /// Consturctor function for the program. Initalizes member variables and components
        /// </summary>
        public Form_Main()
        {
            _treeNode = new TreeNode();
            _currentPhoto = new Photo();
            _panel_CurrentPanel = new Panel();
            _pictureList = new List<pictureData>();

            _directoryCurrent = Directory.GetCurrentDirectory();
            _directoryPhotos = _directoryCurrent + _constantPhotos;
            _directoryUsers = _directoryCurrent + _constantUsers;
            _lastUserFile = _directoryCurrent + _constantLastUser;
            InitializeComponent();
        }

        //Form Main load event
        //Initializes current directory and creates folders if they don't exist
        //Checks for any album files and updates menus to allow open if any are found
        //Sets thumbnail frame spacing and updates the album list tree
        private void Form1_Load(object sender, EventArgs e)
        {
            bool isFirstTime = true;

            if (!Directory.Exists(_directoryUsers)) //if the users directory does not exist
            {
                Directory.CreateDirectory(_directoryUsers); //create the users directory
            }

            if (!Directory.Exists(_directoryPhotos)) //if the photos directory does not exist
            {
                Directory.CreateDirectory(_directoryPhotos); //create the photos directory
            }

            _allUsers = Utilities.listOfUsers(_directoryUsers); //gets all the users - keep after we make sure we have already created the users directory
            if (File.Exists(_lastUserFile)) //get the last user to use the program
            {
                TextReader tr = new StreamReader(_lastUserFile); //open the last user file
                _currentUser = tr.ReadLine(); //read in the user
                tr.Close(); //close the text reader
                tr.Dispose(); //release all resources of the text reader

                if (_currentUser.Replace(" ", "") != "") //if the user is blank then it's their first time
                {
                    _directoryCurrentUser = _directoryUsers + "\\" + _currentUser;
                    isFirstTime = false; //someone is messing with the config file!!
                }
                else
                {
                    _directoryCurrentUser = _directoryUsers + "\\" + _currentUser; //set the users current directory
                }
            }
            
            if (isFirstTime || !File.Exists(_lastUserFile))
            {
                MessageBox.Show("Welcome to Photo Album Viewer of the Gods. Before you can continue you must create a new user profile.", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
                newUser();
                populateUsers();
                if (_allUsers.Count == 0) //happens if the user did not create an account
                {
                    this.Close();
                }
                
                _currentUser = _allUsers.Last();
                _directoryCurrentUser = _directoryUsers + "\\" + _currentUser;
                updateLastUserFile();
                _allUsers.Sort();
            }

            if (!Directory.Exists(_directoryCurrentUser)) //if the current user directory does not exist
            {
                Directory.CreateDirectory(_directoryCurrentUser); //creates the user's directory
            }


            //Initilize data structure
            _albumData = new XMLInterface(_directoryCurrent, FOLDER_USERS, FOLDER_PHOTOS, _constantFileType, _currentUser);

            _albumData.filePath = ""; //stat with no album open
            //Set up Frames
            panel1.BackColor = color_backColor;
            _frameWidth = (panel1.Width / (_framesPerRow + 1)) - 4;
            _frameSpacingX = _frameWidth / _framesPerRow;
            frameSize = new Size(_frameWidth, _frameWidth);
            //Update Border size
            panel_Border.Size = new Size((int)(_frameWidth + (_frameWidth / _borderFactor)), (int)(_frameWidth + (_frameWidth / _borderFactor)));

            populateTree(); //Display any album files
            panel1.Focus(); //Start focus on main panel so any tree nodes are not slected
            updateStatusBar("Current User: " + _currentUser);
            populateUsers();
            cleanupPhotosToolStripMenuItem.Enabled = true; //enable the "Cleanup Photos" button under options
            newAlbumToolStripMenuItem.Enabled = true; //enable the "New Album" button under file
            switchUserToolStripMenuItem.Enabled = (_allUsers.Count > 1) ? true : false; //enable the "Switch User" button under file
            this.Text = _constantAppName;
        }


        //Panel double click event
        //If panel was double left clicked calls openviewer
        private void panel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                openViewer();
            }
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

        //Album tree double click event
        //Double clicking on the album name will open it
        //Zach & Cavan
        private void treeView_Albums_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            openAlbum(_treeNode.Name);
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
            openAlbum(_treeNode.Name);
        }

        //Album tree context menu delete event
        //Calls deleteALbum to delete the specified album
        //Zach & Cavan
        private void deleteAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteAlbum(_treeNode.Name);
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
            deleteAlbum(_albumData.filePath);
        }

        //Update button click event
        //Updates the current photo with the name and description
        //Updates the data array with the new info as well
        //Updates the list view with the new data
        //Zach & Cavan
        private void RenamePic_Click(object sender, EventArgs e)
        {
            renamePicture();
            _currentPhoto.description = richTextBox_Description.Text;
            _currentPhoto.dateModified = Utilities.getTimeStamp();
            _albumData.setData(_currentPhoto.getData(), Convert.ToInt32(_currentPhoto.id));
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

        
        /// <summary>
        /// Updates the last user file
        /// </summary>
        private void updateLastUserFile()
        {
            TextWriter tw = new StreamWriter(_lastUserFile);
            tw.Write(_currentUser);
            tw.Close();
        }

        private void populateUsers()
        {
            switchUserToolStripMenuItem.DropDownItems.Clear();
            foreach (string userName in _allUsers)
            {
                if (_currentUser != userName)
                {
                    ToolStripItem toolStripItem = new ToolStripMenuItem(userName, null, new EventHandler(switchUser));
                    switchUserToolStripMenuItem.DropDownItems.Add(toolStripItem);
                }                
            }
        }

        private void switchUser(object sender, EventArgs e)
        {
            albumClose();
            _currentUser = sender.ToString();
            _directoryCurrentUser = _directoryUsers + "\\" + _currentUser;
            _albumData.CurrentUser = _currentUser;
            _albumData.currentAlbum = "";
            _albumData.filePath = "";
            clearDisplay();
            populateList();
            populateTree();
            populateUsers();
            updateLastUserFile();
            updateStatusBar("Current user: " + _currentUser);
        }

        private string[] totalUsers()
        {
            return Directory.GetDirectories(_directoryUsers);
        }

        //Populates the album list with any found album files
        //Cavan
        private void populateTree()
        {
            int totalAlbums = 0;
            TreeNode node;
            string[] albumList = Directory.GetFiles(_directoryCurrentUser, "*" + _constantFileType);
            treeView_Albums.Nodes.Clear(); //Clears nodes so they can be added again
            totalAlbums = albumList.Count();

            //Loop through each found file and add it to tree. Set its path and show the file name
            for (int i = 0; i < totalAlbums; i++)
            {
                node = new TreeNode();
                node.Name = albumList[i];
                node.Text = Utilities.getNameFromPath(albumList[i]);
                node.ImageIndex = 0;
                treeView_Albums.Nodes.Add(node);

                if (node.Name == _albumData.filePath) //Highlight if album node is open
                {
                    node.BackColor = Color.LightGray;
                }
            }
        }

        private string getAlbumID(string albumName)
        {
            XDocument xdoc = new XDocument();
            xdoc = XDocument.Load(albumName);
            return Utilities.getIdFromInt(xdoc.Descendants("PictureInfo").Count()).ToString();
        }

        private void copyImageToAlbum(object sender, EventArgs e)
        {
            string timestamp = Utilities.getTimeStamp();
            string albumPath = _directoryCurrentUser + "\\" + sender.ToString() + _constantFileType;
            try
            {
                XDocument xdoc = new XDocument();
                xdoc = XDocument.Load(albumPath);
                XElement newNode = new XElement("PictureInfo",
                    new XAttribute("id", getAlbumID(albumPath)),
                    new XAttribute("path", _currentPhoto.path),
                    new XAttribute("name", _currentPhoto.name),
                    new XAttribute("md5", _currentPhoto.MD5),
                    new XAttribute("description",_currentPhoto.description),
                    new XAttribute("dateAdded", timestamp),
                    new XAttribute("dateModified", timestamp)
                );
                xdoc.Descendants("AlbumInfo").Single().Add(newNode);
                xdoc.Save(albumPath);
            }
            catch
            {
                MessageBox.Show("you gone done sumthin wrong");
            }
        }

        //Populates picture list view
        //Method: retrieve album data and clear tree
        //        loop through each picture and add its name, description, and path to the photo tree.
        //Cavan
        private void populateList()
        {
            TreeNode newNode;
            _pictureList = _albumData.getDataList();
            treeView_Pictures.Nodes.Clear();
            string[] nodes = _albumData.getPictureList();
            string tempText = "";
            ToolStripItem toolStripItem;
            string[] albums = _albumData.getAlbumList();
            copyImageToAnotherAlbumToolStripMenuItem.Enabled = false;

            if (albums.Count() > 1)
            {
                copyImageToAnotherAlbumToolStripMenuItem.DropDownItems.Clear();
                copyImageToAnotherAlbumToolStripMenuItem.Enabled = true;
                foreach (string albumName in albums)
                {
                    if (albumName != _albumData.currentAlbum)
                    {
                        toolStripItem = new ToolStripMenuItem(Utilities.getNameFromPath(albumName), null, new EventHandler(copyImageToAlbum));
                        copyImageToAnotherAlbumToolStripMenuItem.DropDownItems.Add(toolStripItem);
                    }
                }
            }

            
            for (int i = 0; i < _pictureList.Count; i++)
            {
                //Photo list
                newNode = new TreeNode();
                newNode.Name = _pictureList[i].path;
                //Append description if there is any
                tempText = _pictureList[i].description;
                if (tempText.Length > 50)
                    newNode.Text = _pictureList[i].name + "   |   " + tempText.Substring(0, 50) + "...";
                else if (tempText.Length > 0)
                    newNode.Text = _pictureList[i].name + "   |   " + _pictureList[i].description.Substring(0, _pictureList[i].description.Length);
                else
                    newNode.Text = _pictureList[i].name;
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
                _panel_CurrentPanel.BackColor = color_backColor;
                Panel p = sender as Panel;
                string id = p.Name.Substring(p.Name.Length - 3, 3); //get id
                //currentId = Convert.ToInt32(id);
                //Set border panel
                panel_Border.Location = new Point((p.Location.X - (_frameWidth / 20 + 2)), (p.Location.Y - (_frameWidth / 20)) - 3);
                panel_Border.Show();
                p.BringToFront();
                _pictureDataStored = _albumData.getData(Convert.ToInt32(id)); //get album data
                _currentPhoto.setData(_pictureDataStored); //Sets current photo properties

                //Set picture info panel
                textBox_Name.Text = _pictureDataStored.name;
                //picViewPath = pictureDataStored.path;
                richTextBox_Description.Text = _pictureDataStored.description;
                label_picSize.Text = _pictureDataStored.size.Width + " x " + _pictureDataStored.size.Height + " pixels";

                p.BackColor = panel_Border.BackColor; //reset last thubmnail backcolor

                _panel_CurrentPanel = p;
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
                _pictureDataStored = _albumData.getData(Convert.ToInt32(id));
                _currentPhoto.setData(_pictureDataStored);
                //Set data panel
                textBox_Name.Text = _pictureDataStored.name;
                richTextBox_Description.Text = _pictureDataStored.description;
                label_picSize.Text = _pictureDataStored.size.Width + ", " + _pictureDataStored.size.Height;
                panel_PictureData.Enabled = true;
                removeToolStripMenuItem.Enabled = true;
                label_NameError.Visible = false;
                _panel_CurrentPanel.BackColor = panel1.BackColor;
                //Finds panel based on id and sets border and current panel so both views match
                foreach (Panel value in panel1.Controls)
                {
                    if (value.Name == ("panel" + Utilities.getIdFromInt(id)))
                    {
                        _panel_CurrentPanel = value;
                        _panel_CurrentPanel.Name = "panel" + Utilities.getIdFromInt(id);
                        panel_Border.Location = new Point((value.Location.X - (_frameWidth / 20 + 2)), (value.Location.Y - (_frameWidth / 20)) - 3);
                        panel_Border.SendToBack();
                        panel_Border.Show();
                        _panel_CurrentPanel.BackColor = panel_Border.BackColor;
                        //break;
                    }
                }
                selectTreeNode(Convert.ToInt32(id));

            }
            //Show context menu
            if (e.Button == MouseButtons.Right)
            {
                fileNameToolStripMenuItem.Text = _currentPhoto.name;
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
                _treeNode.Name = e.Node.Name;
            }
            if (e.Button == MouseButtons.Right)
            {
                treeView_Albums.SelectedNode = e.Node;
                toolStripMenuItem_AlbumName.Text = Utilities.getNameFromPath(_treeNode.Name) + ":";
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
            if (_albumData.filePath == "") //if no album is open
            {
                openAlbumFromFile(albumPath);
            }else if(_albumData.filePath != albumPath){ ///make sure we're not opening the same album
                if (albumClose()) //close the open album
                {
                    openAlbumFromFile(albumPath);
                }
                else
                {
                    handleError("Unable to access file to save");
                }
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
            if (!_albumData.loadAlbum(path))
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
                _albumData.filePath = path;
                enableItems();
                this.Text = _constantAppName + " : " + Utilities.getNameFromPath(path);
                if (_pictureList.Count > 0)
                {
                    searchToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void disablePanel()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    panel2.Show();
                    panel2.BringToFront();
                    tabControl_List.Enabled = false;
                }));
            }
            catch { }
        }
        
        private void enablePanel()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    panel2.Hide();
                    tabControl_List.Enabled = true;
                }));
            }
            catch { }
        }

        private void showThumbnail(List<pictureData> picList)
        {
            pictureData picData = new pictureData();
            int loopCount = picList.Count;
            Panel tempPanel;
            try{
                disablePanel();
                for (int i = 0; i < loopCount; i++)
                {
                    //If panel is already added, skip it (mainly for when inporting pictures)
                    if (panel1.Controls.Find("panel" + Utilities.getIdFromInt(i), false).Count() > 0)
                    {
                        continue;
                    }
                    picData = picList[i];
                    tempPanel = getNewThumbnail(ref picData, i); //Create new panel
                    picList[i] = picData; //reset the picList[i] with the picData changed in the thumbnail - we update the photo dimensions
                    tempPanel.Location = getFrameLocation(i); //Sets panel position from frame variables   
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        tempPanel.Show();  //Still shares reference with panel1.Controls element, so this change affects that one
                        panel1.Controls.Add(tempPanel); //Add panel to main panel                           
                    }));                    
                }
                enablePanel();
            }
            catch
            {

            }
        }

        //Display thumbnails method
        //Method: Retrieves current data list. For each picture, creates a thumbnail panel. Shows and positions each thumbnail
        //        Sets each panel's properties and event handlers.
        //Cavan
        private void populateScreen(string searchTerm="")
        {
            int scrollPosition = panel1.VerticalScroll.Value; //save user scroll position
            
            _pictureList = _albumData.getDataList();
            
            panel1.VerticalScroll.Value = 0; //set scroll to top for correct reference when setting panel location
            panel1.Show();
            if (searchTerm == "")
            {
                new Thread(() => this.showThumbnail(_pictureList)).Start();
            }
            else
            {
                List<pictureData> newList = new List<pictureData>();
                foreach (pictureData picture in _pictureList.FindAll(s => s.name.Contains(searchTerm)))
                {
                    newList.Add(picture);
                }
                new Thread(() => this.showThumbnail(newList)).Start();
            }

            panel_Border.Visible = false;

            //Update Tooltip size
            toolStripStatusLabel_Total.Text = _pictureList.Count.ToString();
            panel1.Focus();
            panel1.VerticalScroll.Value = scrollPosition; //return user scroll posiiton
        }

        //Ceate New Panel thumbnail
        //Parameters: accepts picturedata struct
        //Creates a new panel, sets its picture data, and sets background image to scaled down image from file
        //Returns thumbnial panel
        private Panel getNewThumbnail(ref pictureData imageData, int id)
        {
            Panel newPanel = new Panel();
            newPanel.Visible = false;
            newPanel.Name = "panel" + imageData.id;
            newPanel.Size = frameSize;
            newPanel.BackgroundImageLayout = ImageLayout.Zoom;

            //Attempts to load background Image. If fail, loads a default image and updates description
            try
            {
                Image tempImage = Image.FromFile(imageData.path);
                //imageData.size = tempImage.Size;
                //_albumData.setData(imageData, id);
                imageData.size.Height = tempImage.Height;
                imageData.size.Width = tempImage.Width;
                newPanel.BackgroundImage = Utilities.ScalImage(tempImage, new Size(frameSize.Width, frameSize.Height));
                //imageData.size = newPanel.BackgroundImage.Size;
                tempImage.Dispose();
            }
            catch
            {
                newPanel.BackgroundImage = Resources.Resource1.warning;
                //imageData.size = new Size(0, 0);
                if (!imageData.description.Contains("File not found"))
                {
                    imageData.description = "File not found\n\n" + imageData.description;
                }
                //_albumData.setData(imageData, id);
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
            Point p = new Point(((id % (_framesPerRow)) * (_frameWidth + _frameSpacingX) + (_frameSpacingX / 2)), 
                ((id / _framesPerRow) * (_frameWidth + _frameSpacingY)) + 6); //+6 to offset from top
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
                _currentPhoto.description = richTextBox_Description.Text;
                _albumData.setData(_currentPhoto.getData(), Convert.ToInt32(_currentPhoto.id));
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
                _currentPhoto.name = textBox_Name.Text;
                string id = _panel_CurrentPanel.Name.Substring(_panel_CurrentPanel.Name.Length - 3, 3); //get id
                _pictureDataStored = _albumData.getData(Convert.ToInt32(id));
                textBox_Name.Text = textBox_Name.Text.Trim(); //trim spaces
                _pictureDataStored.name = textBox_Name.Text;
                _albumData.setData(_pictureDataStored, Convert.ToInt32(id));
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

        //MenuItem New click event
        //Calls createAlbum and and enables menu items if successfull. Updates album tree list. Displays errors if can't access files
        //Cavan
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string albumName;
            Form_NewFileDialog namePrompt = new Form_NewFileDialog(_albumData.getAlbumList(), _constantFileType);
            namePrompt.ShowDialog();
            albumName = namePrompt.albumNameValue; //Retrieve name entered by user
            namePrompt.Dispose();

            if (albumName != "") //Create File if user pressed create
            {
                //Attmpe to close file
                if (albumClose())
                {
                    //Create new album
                    if (_albumData.createAlbum(albumName))
                    {
                        enableItems(); //enables menu items

                        _albumData.filePath = _directoryCurrentUser + "\\" + albumName + _constantFileType; //sets current file path
                        populateTree();
                        updateStatusBar("Current User: "+ _currentUser+ " || "+ _constantAppName + " : " + Utilities.getNameFromPath(_albumData.filePath));
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
            this.Text = _constantAppName;
        }

        //Close album method
        //
        //Cavan
        /// <summary>
        /// Attempts to close the currently open album
        /// </summary>
        /// <returns>Returns true on successful save, false otherwise</returns>
        private bool albumClose()
        {
            if (_albumData.currentAlbum == "")
            {
                return true;
            }

            if (!saveAlbum())
            {
                handleError("Unable to save album");
                return false;
            }
            else
            {
                searchToolStripMenuItem.Enabled = false;
                clearDisplay();
                //Clear Trees
                treeView_Pictures.Nodes.Clear();
                _albumData.clearAlbum();
                disableItems();
                populateTree(); //remove highlight
                return true;
            }
        }

        /// <summary>
        /// Clear thumbnails method
        /// Disposes all panels and images created for thumbnails. Disables picture data panel and clears panel data.
        /// </summary>
        private void clearDisplay()
        {
            int controlCounter = 0;
            int controlCount = panel1.Controls.Count - 1;
            Control[] controlList = new Control[controlCount];
            panel1.Hide();
            foreach (Panel value in panel1.Controls) //get all panels, excluding border panel
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
                if (controlList[j].Name != panel2.Name) //dispose of panels that is not the processing image panel
                {
                    controlList[j].Dispose();
                }
                
            }
            panel1.Show();
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
            List<Utilities.AllImagesInfo> allImages = Utilities.getAllImageInfo();

            openFileDialog_Load.Filter = "Image Files|*.jpg;*.jpeg;*.bmp;*.png;*.gif";
            openFileDialog_Load.Title = "Import Photo";
            openFileDialog_Load.FileName = "";
            openFileDialog_Load.InitialDirectory = _lastImportedFrom;
            openFileDialog_Load.Multiselect = true; //alow loading of multiple files

            if (openFileDialog_Load.ShowDialog() == DialogResult.OK)
            {
                _lastImportedFrom = Path.GetDirectoryName(openFileDialog_Load.FileNames[0]);
                clearDisplay();
                disablePanel();
                foreach (string value in openFileDialog_Load.FileNames)
                {
                    if (Utilities.isImageValid(value)) //check if file is valid
                    {
                        _albumData.addPhoto(value, ref allImages);
                        saveAlbum();
                    }
                    else
                    {
                        handleError("Invalid Photo Selected: " + value);
                    }
                }
                enablePanel();
                populateList();
                populateScreen();
                searchToolStripMenuItem.Enabled = true;
            }
        }

        //Form Closing event
        //Saves current album, removes event handler and closes
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_albumData.currentAlbum != "")
            {
                saveAlbum();
            }
            this.FormClosing -= Form_Main_FormClosing;
            this.Close();
        }

        /// <summary>
        /// Attempts to save the currently open album
        /// </summary>
        /// <returns>Returns true on successful save, false otherwise</returns>
        private bool saveAlbum()
        {
            return (_albumData.saveAlbum()) ? true : false;
        }

        //Album tree double click event
        //Calls openViewer to view picture
        //Cavan
        private void treeView_Pictures_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                openViewer();
            }
        }

        /// <summary>
        /// Closes current album. creates name prompt. Retrieves user entered value. Calls loadAlbum passing album name.
        /// Calls deleteAlbum, to delete the album file. Calls saveAlbum, to write file with the new name.
        /// Calls error handler if any files cannot be accessed
        /// </summary>
        private void renameAlbum()
        {
            string newAlbumName;
            Form_NewFileDialog namePrompt = new Form_NewFileDialog(_albumData.getAlbumList(), _constantFileType);
            
            namePrompt.Text = "Rename Album";
            namePrompt.setValueOfCreate = "Rename";
            namePrompt.ShowDialog();
            newAlbumName = namePrompt.albumNameValue;
            namePrompt.Dispose();
            if (newAlbumName != "")
            {
                if (albumClose())
                {
                    _albumData.loadAlbum(_treeNode.Name);
                    if (_albumData.deleteAlbum(_treeNode.Name))
                    {
                        _albumData.filePath = _directoryCurrentUser + "\\" + newAlbumName + _constantFileType;
                        saveAlbum();
                        populateTree();
                        openAlbum(_albumData.filePath);
                    }
                    else
                    {
                        handleError( "Unable to access file to delete");
                    }
                }
                else
                {
                    handleError("Unable to access file to save");

                }
            }           
        }

        /// <summary>
        /// Attempts to delete the passed album path
        /// </summary>
        /// <param name="filePathToDelete">The path to the album</param>
        private void deleteAlbum(string filePathToDelete)
        {
            if (MessageBox.Show("Are you sure you want to delete this album and all the photos within?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                if (_albumData.deleteAlbum(filePathToDelete))
                {
                    populateTree();
                    _albumData.filePath = (_albumData.filePath == filePathToDelete) ? "" : filePathToDelete; //we deleted the open album so clear the filePath or else on reopen it will save the album again
                    _pictureList.Clear(); //make sure to clear images of deleted album
                    clearDisplay();
                    _albumData.currentAlbum = "";
                }
                else
                {
                    handleError("An error occurred trying to delete the album.");
                }
            }
        }

        /// <summary>
        /// Opens the picture viewer and starts the display on the current photo selected
        /// </summary>
        private void openViewer()
        {
            Image tempImage;
            List<Form_Viewer.modifiedImageInfo> modifiedImages;
            if (File.Exists(_currentPhoto.path)) //Shows nothing if file wasn't found (warning images)
            {
                Form_Viewer picView = new Form_Viewer(_pictureList, Convert.ToInt32(_currentPhoto.id), _constantAppName);
                picView.StartPosition = FormStartPosition.CenterParent;
                picView.ShowDialog();
                _pictureList = picView.pictureList;
                if (picView.isModified) //if any pictures were modified
                {
                    modifiedImages = picView.modifiedImages; //get list of modified images
                    try
                    {
                        for (int i = 0; i < modifiedImages.Count; i++) //loop through all modified images
                        {
                            tempImage = Image.FromFile(modifiedImages[i].path); //grab the image
                            panel1.Controls[modifiedImages[i].id].BackgroundImage = Utilities.ScalImage(tempImage, new Size(frameSize.Width, frameSize.Height)); //change the background image
                            tempImage.Dispose(); //dispose of the temp image
                        }
                    }
                    catch { }
                }
                picView.Dispose();
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
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Deletes a photo from an album and then saves the album
        /// </summary>
        private void deletePhoto()
        {
            if (MessageBox.Show("Are you sure you want to delete this photo?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                _albumData.RemovePic(_currentPhoto.id);
                saveAlbum();
                clearDisplay();
                populateScreen();
                populateList();

                if (_albumData.isEmpty())
                {
                    searchToolStripMenuItem.Enabled = false;
                }
            }
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
                panel1.ScrollControlIntoView(_panel_CurrentPanel);
            }
        }

        /// <summary>
        /// This function displays a message box
        /// </summary>
        /// <param name="message">A string that will be displayed in an error messagebox.</param>
        private void handleError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void newUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newUser();
            populateUsers();
        }

        private void newUser()
        {
            Form_NewUser namePrompt = new Form_NewUser(ref _allUsers, _directoryUsers, false);
            namePrompt.StartPosition = FormStartPosition.CenterParent;
            namePrompt.ShowDialog();
            namePrompt.Dispose();
            switchUserToolStripMenuItem.Enabled = (_allUsers.Count > 1) ? true : false; //enable the "Switch User" button under file
        }

        private void cleanupPhotosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int totalRemoved;
            string wording;
            if (MessageBox.Show("You are about to remove all photos that are not in use by any of the users of this program. This action cannot be undone. Are you sure you want to continue?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                totalRemoved = Utilities.cleanUpPhotos();
                wording = totalRemoved + ((totalRemoved == 1) ? " photo was removed" : " photos were removed");
                MessageBox.Show(wording, "Photos Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void printImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utilities.printImage(_currentPhoto.path);
        }

        private void printAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int total;
            string wording;
            openAlbum(_treeNode.Name);
            total = _pictureList.Count;
            wording = (total == 1) ? "photo" : "photos";

            if (total == 0)
            {
                MessageBox.Show("There are no photos in this album!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }else if(MessageBox.Show("Are you sure you want to print " + total + " " + wording + "?", "Confirm Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                for (int i = 0; i < total; i++)
                {
                    Utilities.printImage(_pictureList[i].path);
                }
            }            
        }

        private void updateStatusBar(string statusBarText)
        {
            toolStripStatusLabel_TotalLabel.Text = statusBarText;
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string searchTerm;
            Form_Search searchForm = new Form_Search();
            searchForm.StartPosition = FormStartPosition.CenterParent;
            searchForm.ShowDialog();
            searchTerm = searchForm.searchTerm;
            searchForm.Dispose();

            if (searchTerm != "")
            {
                clearDisplay();
                populateScreen(searchTerm);
            }
           
        }
    }
}
