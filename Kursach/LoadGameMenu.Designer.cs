namespace Kursach
{
    partial class LoadGameMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadGameMenu));
            this.close = new System.Windows.Forms.Button();
            this.load = new System.Windows.Forms.Button();
            this.saves = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(358, 415);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(87, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Назад";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // load
            // 
            this.load.Location = new System.Drawing.Point(358, 373);
            this.load.Name = "load";
            this.load.Size = new System.Drawing.Size(87, 23);
            this.load.TabIndex = 1;
            this.load.Text = "Загрузить";
            this.load.UseVisualStyleBackColor = true;
            this.load.Click += new System.EventHandler(this.load_Click);
            // 
            // saves
            // 
            this.saves.FormattingEnabled = true;
            this.saves.Location = new System.Drawing.Point(72, 109);
            this.saves.Name = "saves";
            this.saves.Size = new System.Drawing.Size(121, 24);
            this.saves.TabIndex = 2;
            // 
            // LoadGameMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.saves);
            this.Controls.Add(this.load);
            this.Controls.Add(this.close);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoadGameMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoadGameMenu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Button load;
        private System.Windows.Forms.ComboBox saves;
    }
}