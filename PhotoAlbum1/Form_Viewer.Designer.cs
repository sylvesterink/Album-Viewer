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
            this.imageNameLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonNextImage = new System.Windows.Forms.Button();
            this.buttonPrevImage = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.button_rotate_cc.Location = new System.Drawing.Point(84, 25);
            this.button_rotate_cc.Name = "button_rotate_cc";
            this.button_rotate_cc.Size = new System.Drawing.Size(75, 23);
            this.button_rotate_cc.TabIndex = 0;
            this.button_rotate_cc.Text = "Rotate Right";
            this.button_rotate_cc.UseVisualStyleBackColor = true;
            this.button_rotate_cc.Click += new System.EventHandler(this.button_rotate_cc_Click);
            // 
            // button_rotate_ccw
            // 
            this.button_rotate_ccw.Location = new System.Drawing.Point(165, 25);
            this.button_rotate_ccw.Name = "button_rotate_ccw";
            this.button_rotate_ccw.Size = new System.Drawing.Size(75, 23);
            this.button_rotate_ccw.TabIndex = 1;
            this.button_rotate_ccw.Text = "Rotate Left";
            this.button_rotate_ccw.UseVisualStyleBackColor = true;
            this.button_rotate_ccw.Click += new System.EventHandler(this.button_rotate_ccw_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(327, 25);
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
            this.panel1.Controls.Add(this.buttonPrevImage);
            this.panel1.Controls.Add(this.buttonNextImage);
            this.panel1.Controls.Add(this.imageNameLabel);
            this.panel1.Controls.Add(this.button_rotate_cc);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button_rotate_ccw);
            this.panel1.Location = new System.Drawing.Point(0, 413);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 53);
            this.panel1.TabIndex = 3;
            // 
            // imageNameLabel
            // 
            this.imageNameLabel.AutoSize = true;
            this.imageNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imageNameLabel.Location = new System.Drawing.Point(3, 0);
            this.imageNameLabel.Name = "imageNameLabel";
            this.imageNameLabel.Size = new System.Drawing.Size(0, 24);
            this.imageNameLabel.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(483, 410);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // buttonNextImage
            // 
            this.buttonNextImage.Location = new System.Drawing.Point(246, 25);
            this.buttonNextImage.Name = "buttonNextImage";
            this.buttonNextImage.Size = new System.Drawing.Size(75, 23);
            this.buttonNextImage.TabIndex = 4;
            this.buttonNextImage.Text = ">";
            this.buttonNextImage.UseVisualStyleBackColor = true;
            this.buttonNextImage.Click += new System.EventHandler(this.buttonNextImage_Click);
            // 
            // buttonPrevImage
            // 
            this.buttonPrevImage.Location = new System.Drawing.Point(4, 25);
            this.buttonPrevImage.Name = "buttonPrevImage";
            this.buttonPrevImage.Size = new System.Drawing.Size(75, 23);
            this.buttonPrevImage.TabIndex = 5;
            this.buttonPrevImage.Text = "<";
            this.buttonPrevImage.UseVisualStyleBackColor = true;
            this.buttonPrevImage.Click += new System.EventHandler(this.buttonPrevImage_Click);
            // 
            // Form_Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(484, 466);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
    }
}