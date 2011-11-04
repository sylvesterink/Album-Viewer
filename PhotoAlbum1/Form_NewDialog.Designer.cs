namespace PhotoAlbumViewOfTheGods
{
    partial class Form_NewFileDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_NewFileDialog));
            this.button_Create = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.textBox_AlbumName = new System.Windows.Forms.TextBox();
            this.label_AlbumName = new System.Windows.Forms.Label();
            this.timer_FocusBox = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // button_Create
            // 
            this.button_Create.Location = new System.Drawing.Point(91, 52);
            this.button_Create.Name = "button_Create";
            this.button_Create.Size = new System.Drawing.Size(75, 22);
            this.button_Create.TabIndex = 2;
            this.button_Create.Text = "Create";
            this.button_Create.UseVisualStyleBackColor = true;
            this.button_Create.Click += new System.EventHandler(this.button_Create_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(185, 52);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(69, 22);
            this.button_Cancel.TabIndex = 3;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // textBox_AlbumName
            // 
            this.textBox_AlbumName.Location = new System.Drawing.Point(91, 26);
            this.textBox_AlbumName.MaxLength = 100;
            this.textBox_AlbumName.Name = "textBox_AlbumName";
            this.textBox_AlbumName.Size = new System.Drawing.Size(163, 20);
            this.textBox_AlbumName.TabIndex = 1;
            // 
            // label_AlbumName
            // 
            this.label_AlbumName.AutoSize = true;
            this.label_AlbumName.Location = new System.Drawing.Point(21, 29);
            this.label_AlbumName.Name = "label_AlbumName";
            this.label_AlbumName.Size = new System.Drawing.Size(67, 13);
            this.label_AlbumName.TabIndex = 3;
            this.label_AlbumName.Text = "Album Name";
            // 
            // Form_NewFileDialog
            // 
            this.AcceptButton = this.button_Create;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(287, 96);
            this.Controls.Add(this.label_AlbumName);
            this.Controls.Add(this.textBox_AlbumName);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Create);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_NewFileDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Album";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Create;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.TextBox textBox_AlbumName;
        private System.Windows.Forms.Label label_AlbumName;
        private System.Windows.Forms.Timer timer_FocusBox;
    }
}