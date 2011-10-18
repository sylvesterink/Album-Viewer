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
            this.button_Create = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.textBox_AlbumName = new System.Windows.Forms.TextBox();
            this.label_AlbumName = new System.Windows.Forms.Label();
            this.label_Error = new System.Windows.Forms.Label();
            this.label_Extension = new System.Windows.Forms.Label();
            this.timer_FocusBox = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // button_Create
            // 
            this.button_Create.Location = new System.Drawing.Point(52, 101);
            this.button_Create.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_Create.Name = "button_Create";
            this.button_Create.Size = new System.Drawing.Size(117, 41);
            this.button_Create.TabIndex = 2;
            this.button_Create.Text = "Create";
            this.button_Create.UseVisualStyleBackColor = true;
            this.button_Create.Click += new System.EventHandler(this.button_Create_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(217, 101);
            this.button_Cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(121, 41);
            this.button_Cancel.TabIndex = 3;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // textBox_AlbumName
            // 
            this.textBox_AlbumName.Location = new System.Drawing.Point(121, 32);
            this.textBox_AlbumName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_AlbumName.MaxLength = 80;
            this.textBox_AlbumName.Name = "textBox_AlbumName";
            this.textBox_AlbumName.Size = new System.Drawing.Size(216, 22);
            this.textBox_AlbumName.TabIndex = 1;
            // 
            // label_AlbumName
            // 
            this.label_AlbumName.AutoSize = true;
            this.label_AlbumName.Location = new System.Drawing.Point(28, 36);
            this.label_AlbumName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_AlbumName.Name = "label_AlbumName";
            this.label_AlbumName.Size = new System.Drawing.Size(88, 17);
            this.label_AlbumName.TabIndex = 3;
            this.label_AlbumName.Text = "Album Name";
            // 
            // label_Error
            // 
            this.label_Error.AutoSize = true;
            this.label_Error.ForeColor = System.Drawing.Color.DarkRed;
            this.label_Error.Location = new System.Drawing.Point(173, 60);
            this.label_Error.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Error.Name = "label_Error";
            this.label_Error.Size = new System.Drawing.Size(159, 17);
            this.label_Error.TabIndex = 4;
            this.label_Error.Text = "File name already exists";
            this.label_Error.Visible = false;
            // 
            // label_Extension
            // 
            this.label_Extension.AutoSize = true;
            this.label_Extension.Location = new System.Drawing.Point(339, 36);
            this.label_Extension.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Extension.Name = "label_Extension";
            this.label_Extension.Size = new System.Drawing.Size(0, 17);
            this.label_Extension.TabIndex = 5;
            // 
            // Form_NewFileDialog
            // 
            this.AcceptButton = this.button_Create;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(404, 177);
            this.ControlBox = false;
            this.Controls.Add(this.label_Extension);
            this.Controls.Add(this.label_Error);
            this.Controls.Add(this.label_AlbumName);
            this.Controls.Add(this.textBox_AlbumName);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Create);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form_NewFileDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Album";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Create;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.TextBox textBox_AlbumName;
        private System.Windows.Forms.Label label_AlbumName;
        private System.Windows.Forms.Label label_Error;
        private System.Windows.Forms.Label label_Extension;
        private System.Windows.Forms.Timer timer_FocusBox;
    }
}