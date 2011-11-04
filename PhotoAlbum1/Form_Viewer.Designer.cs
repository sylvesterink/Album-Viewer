namespace PhotoAlbumViewOfTheGods
{
    partial class Form_Viewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Viewer));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer_Paint = new System.Windows.Forms.Timer(this.components);
            this.timer_Resize = new System.Windows.Forms.Timer(this.components);
            this.button_rotate_cc = new System.Windows.Forms.Button();
            this.button_rotate_ccw = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonSlideShow = new System.Windows.Forms.Button();
            this.buttonPrevImage = new System.Windows.Forms.Button();
            this.buttonNextImage = new System.Windows.Forms.Button();
            this.imageNameLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.labelResolution = new System.Windows.Forms.Label();
            this.labelModified = new System.Windows.Forms.Label();
            this.labelAdded = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // timer_Paint
            // 
            this.timer_Paint.Interval = 20;
            // 
            // timer_Resize
            // 
            this.timer_Resize.Interval = 20;
            this.timer_Resize.Tick += new System.EventHandler(this.timer_Resize_Tick);
            // 
            // button_rotate_cc
            // 
            this.button_rotate_cc.Location = new System.Drawing.Point(62, 54);
            this.button_rotate_cc.Name = "button_rotate_cc";
            this.button_rotate_cc.Size = new System.Drawing.Size(75, 23);
            this.button_rotate_cc.TabIndex = 0;
            this.button_rotate_cc.Text = "Rotate Right";
            this.button_rotate_cc.UseVisualStyleBackColor = true;
            this.button_rotate_cc.Click += new System.EventHandler(this.button_rotate_cc_Click);
            // 
            // button_rotate_ccw
            // 
            this.button_rotate_ccw.Location = new System.Drawing.Point(143, 54);
            this.button_rotate_ccw.Name = "button_rotate_ccw";
            this.button_rotate_ccw.Size = new System.Drawing.Size(75, 23);
            this.button_rotate_ccw.TabIndex = 1;
            this.button_rotate_ccw.Text = "Rotate Left";
            this.button_rotate_ccw.UseVisualStyleBackColor = true;
            this.button_rotate_ccw.Click += new System.EventHandler(this.button_rotate_ccw_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(285, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Print Image";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkGray;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(0, 383);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 83);
            this.panel1.TabIndex = 3;
            // 
            // buttonSlideShow
            // 
            this.buttonSlideShow.Location = new System.Drawing.Point(366, 54);
            this.buttonSlideShow.Name = "buttonSlideShow";
            this.buttonSlideShow.Size = new System.Drawing.Size(96, 23);
            this.buttonSlideShow.TabIndex = 6;
            this.buttonSlideShow.Text = "Begin Slideshow";
            this.buttonSlideShow.UseVisualStyleBackColor = true;
            this.buttonSlideShow.Click += new System.EventHandler(this.buttonSlideShow_Click);
            // 
            // buttonPrevImage
            // 
            this.buttonPrevImage.Location = new System.Drawing.Point(1, 54);
            this.buttonPrevImage.Name = "buttonPrevImage";
            this.buttonPrevImage.Size = new System.Drawing.Size(55, 23);
            this.buttonPrevImage.TabIndex = 5;
            this.buttonPrevImage.Text = "<";
            this.buttonPrevImage.UseVisualStyleBackColor = true;
            this.buttonPrevImage.Click += new System.EventHandler(this.buttonPrevImage_Click);
            // 
            // buttonNextImage
            // 
            this.buttonNextImage.Location = new System.Drawing.Point(224, 54);
            this.buttonNextImage.Name = "buttonNextImage";
            this.buttonNextImage.Size = new System.Drawing.Size(55, 23);
            this.buttonNextImage.TabIndex = 4;
            this.buttonNextImage.Text = ">";
            this.buttonNextImage.UseVisualStyleBackColor = true;
            this.buttonNextImage.Click += new System.EventHandler(this.buttonNextImage_Click);
            // 
            // imageNameLabel
            // 
            this.imageNameLabel.AutoSize = true;
            this.imageNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imageNameLabel.Location = new System.Drawing.Point(3, 5);
            this.imageNameLabel.Name = "imageNameLabel";
            this.imageNameLabel.Size = new System.Drawing.Size(68, 20);
            this.imageNameLabel.TabIndex = 3;
            this.imageNameLabel.Text = "Hide Me";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(483, 380);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.DarkGray;
            this.panelButtons.Controls.Add(this.labelAdded);
            this.panelButtons.Controls.Add(this.labelModified);
            this.panelButtons.Controls.Add(this.labelResolution);
            this.panelButtons.Controls.Add(this.imageNameLabel);
            this.panelButtons.Controls.Add(this.buttonSlideShow);
            this.panelButtons.Controls.Add(this.buttonPrevImage);
            this.panelButtons.Controls.Add(this.button_rotate_ccw);
            this.panelButtons.Controls.Add(this.buttonNextImage);
            this.panelButtons.Controls.Add(this.button1);
            this.panelButtons.Controls.Add(this.button_rotate_cc);
            this.panelButtons.Location = new System.Drawing.Point(11, 384);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(462, 80);
            this.panelButtons.TabIndex = 5;
            // 
            // labelResolution
            // 
            this.labelResolution.AutoSize = true;
            this.labelResolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResolution.Location = new System.Drawing.Point(4, 30);
            this.labelResolution.Name = "labelResolution";
            this.labelResolution.Size = new System.Drawing.Size(117, 16);
            this.labelResolution.TabIndex = 7;
            this.labelResolution.Text = "hide me resolution";
            // 
            // labelModified
            // 
            this.labelModified.AutoSize = true;
            this.labelModified.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModified.Location = new System.Drawing.Point(300, 30);
            this.labelModified.Name = "labelModified";
            this.labelModified.Size = new System.Drawing.Size(111, 16);
            this.labelModified.TabIndex = 8;
            this.labelModified.Text = "hide me modified";
            // 
            // labelAdded
            // 
            this.labelAdded.AutoSize = true;
            this.labelAdded.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdded.Location = new System.Drawing.Point(140, 30);
            this.labelAdded.Name = "labelAdded";
            this.labelAdded.Size = new System.Drawing.Size(99, 16);
            this.labelAdded.TabIndex = 9;
            this.labelAdded.Text = "hide me added";
            // 
            // Form_Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(484, 466);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "Form_Viewer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Image Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Viewer_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timer_Paint;
        private System.Windows.Forms.Timer timer_Resize;
        private System.Windows.Forms.Button button_rotate_cc;
        private System.Windows.Forms.Button button_rotate_ccw;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label imageNameLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonPrevImage;
        private System.Windows.Forms.Button buttonNextImage;
        private System.Windows.Forms.Button buttonSlideShow;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Label labelModified;
        private System.Windows.Forms.Label labelResolution;
        private System.Windows.Forms.Label labelAdded;
    }
}