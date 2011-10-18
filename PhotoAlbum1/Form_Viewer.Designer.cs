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
            // Form_Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(599, 536);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(300, 299);
            this.Name = "Form_Viewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Image Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Viewer_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.Form_Viewer_ResizeEnd);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timer_Paint;
        private System.Windows.Forms.Timer timer_Resize;
    }
}