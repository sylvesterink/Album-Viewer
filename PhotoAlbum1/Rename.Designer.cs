namespace PhotoAlbumViewOfTheGods
{
    partial class Rename
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
            this.Rename_box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Error_label = new System.Windows.Forms.Label();
            this.OKbutton = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Rename_box
            // 
            this.Rename_box.Location = new System.Drawing.Point(65, 66);
            this.Rename_box.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Rename_box.MaxLength = 80;
            this.Rename_box.Name = "Rename_box";
            this.Rename_box.Size = new System.Drawing.Size(223, 22);
            this.Rename_box.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter a new name:";
            // 
            // Error_label
            // 
            this.Error_label.AutoSize = true;
            this.Error_label.ForeColor = System.Drawing.Color.Red;
            this.Error_label.Location = new System.Drawing.Point(143, 95);
            this.Error_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Error_label.Name = "Error_label";
            this.Error_label.Size = new System.Drawing.Size(145, 17);
            this.Error_label.TabIndex = 2;
            this.Error_label.Text = "Duplicate name exists";
            this.Error_label.Visible = false;
            // 
            // OKbutton
            // 
            this.OKbutton.Location = new System.Drawing.Point(297, 66);
            this.OKbutton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(47, 25);
            this.OKbutton.TabIndex = 3;
            this.OKbutton.Text = "OK";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click_1);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(352, 66);
            this.cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(97, 27);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // Rename
            // 
            this.AcceptButton = this.OKbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 135);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.Error_label);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Rename_box);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Rename";
            this.Text = "Rename album";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Rename_box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Error_label;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Button cancel;
    }
}