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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer_Paint = new System.Windows.Forms.Timer(this.components);
            this.timer_Resize = new System.Windows.Forms.Timer(this.components);
            this.button_rotate_cc = new System.Windows.Forms.Button();
            this.button_rotate_ccw = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // timer_Paint
            // 
            this.timer_Paint.Interval = 20;
            this.timer_Paint.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer_Resize
            // 
            this.timer_Resize.Interval = 20;
            this.timer_Resize.Tick += new System.EventHandler(this.timer_Resize_Tick);
            // 
            // button_rotate_cc
            // 
            this.button_rotate_cc.Location = new System.Drawing.Point(3, 3);
            this.button_rotate_cc.Name = "button_rotate_cc";
            this.button_rotate_cc.Size = new System.Drawing.Size(103, 23);
            this.button_rotate_cc.TabIndex = 0;
            this.button_rotate_cc.Text = "Rotate Clockwise";
            this.button_rotate_cc.UseVisualStyleBackColor = true;
            this.button_rotate_cc.Click += new System.EventHandler(this.button_rotate_cc_Click);
            // 
            // button_rotate_ccw
            // 
            this.button_rotate_ccw.Location = new System.Drawing.Point(112, 3);
            this.button_rotate_ccw.Name = "button_rotate_ccw";
            this.button_rotate_ccw.Size = new System.Drawing.Size(147, 23);
            this.button_rotate_ccw.TabIndex = 1;
            this.button_rotate_ccw.Text = "Rotate Counter-Clockwise";
            this.button_rotate_ccw.UseVisualStyleBackColor = true;
            this.button_rotate_ccw.Click += new System.EventHandler(this.button_rotate_ccw_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(265, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Print Image";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_rotate_cc);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button_rotate_ccw);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(345, 37);
            this.panel1.TabIndex = 3;
            // 
            // Form_Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(366, 265);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(300, 299);
            this.Name = "Form_Viewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Image Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Viewer_FormClosing);
            this.Load += new System.EventHandler(this.Form_Viewer_Load);
            this.ResizeEnd += new System.EventHandler(this.Form_Viewer_ResizeEnd);
            this.panel1.ResumeLayout(false);
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
    }
}