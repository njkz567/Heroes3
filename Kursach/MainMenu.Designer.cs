namespace Kursach
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.close = new System.Windows.Forms.Button();
            this.play = new System.Windows.Forms.Button();
            this.loadGameMenu = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(350, 258);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(85, 25);
            this.close.TabIndex = 0;
            this.close.Text = "close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // play
            // 
            this.play.Location = new System.Drawing.Point(350, 155);
            this.play.Name = "play";
            this.play.Size = new System.Drawing.Size(85, 25);
            this.play.TabIndex = 1;
            this.play.Text = "Играть";
            this.play.UseVisualStyleBackColor = true;
            this.play.Click += new System.EventHandler(this.play_Click);
            // 
            // loadGameMenu
            // 
            this.loadGameMenu.Location = new System.Drawing.Point(350, 208);
            this.loadGameMenu.Name = "loadGameMenu";
            this.loadGameMenu.Size = new System.Drawing.Size(85, 23);
            this.loadGameMenu.TabIndex = 2;
            this.loadGameMenu.Text = "Загрузка";
            this.loadGameMenu.UseVisualStyleBackColor = true;
            this.loadGameMenu.Click += new System.EventHandler(this.loadGameMenu_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.loadGameMenu);
            this.Controls.Add(this.play);
            this.Controls.Add(this.close);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainMenu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Button play;
        private System.Windows.Forms.Button loadGameMenu;
    }
}