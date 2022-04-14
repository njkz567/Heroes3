namespace Kursach
{
    partial class NewGameMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewGameMenu));
            this.playerLabel = new System.Windows.Forms.Label();
            this.playerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.playersPanel = new System.Windows.Forms.Panel();
            this.back = new System.Windows.Forms.Button();
            this.play = new System.Windows.Forms.Button();
            this.mapHeightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.mapHeightLabel = new System.Windows.Forms.Label();
            this.mapMakeButton = new System.Windows.Forms.Button();
            this.mapLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.playerNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapHeightNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // playerLabel
            // 
            this.playerLabel.AutoSize = true;
            this.playerLabel.Location = new System.Drawing.Point(32, 48);
            this.playerLabel.Name = "playerLabel";
            this.playerLabel.Size = new System.Drawing.Size(54, 16);
            this.playerLabel.TabIndex = 0;
            this.playerLabel.Text = "Игроки";
            // 
            // playerNumericUpDown
            // 
            this.playerNumericUpDown.Location = new System.Drawing.Point(107, 42);
            this.playerNumericUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.playerNumericUpDown.Name = "playerNumericUpDown";
            this.playerNumericUpDown.Size = new System.Drawing.Size(62, 22);
            this.playerNumericUpDown.TabIndex = 1;
            this.playerNumericUpDown.ValueChanged += new System.EventHandler(this.playerNumericUpDown_ValueChanged);
            // 
            // playersPanel
            // 
            this.playersPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.playersPanel.Location = new System.Drawing.Point(35, 70);
            this.playersPanel.Name = "playersPanel";
            this.playersPanel.Size = new System.Drawing.Size(210, 150);
            this.playersPanel.TabIndex = 6;
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(414, 402);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(75, 23);
            this.back.TabIndex = 7;
            this.back.Text = "Назад";
            this.back.UseVisualStyleBackColor = true;
            this.back.Click += new System.EventHandler(this.back_Click);
            // 
            // play
            // 
            this.play.Location = new System.Drawing.Point(299, 402);
            this.play.Name = "play";
            this.play.Size = new System.Drawing.Size(75, 23);
            this.play.TabIndex = 8;
            this.play.Text = "Играть";
            this.play.UseVisualStyleBackColor = true;
            this.play.Click += new System.EventHandler(this.play_Click);
            // 
            // mapHeightNumericUpDown
            // 
            this.mapHeightNumericUpDown.Location = new System.Drawing.Point(646, 70);
            this.mapHeightNumericUpDown.Name = "mapHeightNumericUpDown";
            this.mapHeightNumericUpDown.Size = new System.Drawing.Size(81, 22);
            this.mapHeightNumericUpDown.TabIndex = 14;
            // 
            // mapHeightLabel
            // 
            this.mapHeightLabel.AutoSize = true;
            this.mapHeightLabel.Location = new System.Drawing.Point(504, 70);
            this.mapHeightLabel.Name = "mapHeightLabel";
            this.mapHeightLabel.Size = new System.Drawing.Size(57, 16);
            this.mapHeightLabel.TabIndex = 16;
            this.mapHeightLabel.Text = "Размер";
            // 
            // mapMakeButton
            // 
            this.mapMakeButton.Location = new System.Drawing.Point(544, 98);
            this.mapMakeButton.Name = "mapMakeButton";
            this.mapMakeButton.Size = new System.Drawing.Size(128, 29);
            this.mapMakeButton.TabIndex = 17;
            this.mapMakeButton.Text = "Сгенерировать";
            this.mapMakeButton.UseVisualStyleBackColor = true;
            this.mapMakeButton.Click += new System.EventHandler(this.mapMakeButton_Click);
            // 
            // mapLabel
            // 
            this.mapLabel.AutoSize = true;
            this.mapLabel.Location = new System.Drawing.Point(581, 48);
            this.mapLabel.Name = "mapLabel";
            this.mapLabel.Size = new System.Drawing.Size(46, 16);
            this.mapLabel.TabIndex = 12;
            this.mapLabel.Text = "Карта";
            // 
            // NewGameMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mapMakeButton);
            this.Controls.Add(this.mapHeightLabel);
            this.Controls.Add(this.mapHeightNumericUpDown);
            this.Controls.Add(this.mapLabel);
            this.Controls.Add(this.play);
            this.Controls.Add(this.back);
            this.Controls.Add(this.playersPanel);
            this.Controls.Add(this.playerNumericUpDown);
            this.Controls.Add(this.playerLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewGameMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewGameMenu";
            ((System.ComponentModel.ISupportInitialize)(this.playerNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapHeightNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label playerLabel;
        private System.Windows.Forms.NumericUpDown playerNumericUpDown;
        private System.Windows.Forms.Panel playersPanel;
        private System.Windows.Forms.Button back;
        private System.Windows.Forms.Button play;
        private System.Windows.Forms.NumericUpDown mapHeightNumericUpDown;
        private System.Windows.Forms.Label mapHeightLabel;
        private System.Windows.Forms.Button mapMakeButton;
        private System.Windows.Forms.Label mapLabel;
    }
}