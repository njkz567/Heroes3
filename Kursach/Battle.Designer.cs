namespace Kursach
{
    partial class Battle
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Battle));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.hexGrid = new System.Windows.Forms.PictureBox();
            this.info = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.turnSkip = new System.Windows.Forms.Button();
            this.attackTimer = new System.Windows.Forms.Timer(this.components);
            this.underFireTimer = new System.Windows.Forms.Timer(this.components);
            this.deathTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.hexGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // hexGrid
            // 
            this.hexGrid.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.hexGrid.BackColor = System.Drawing.Color.Coral;
            this.hexGrid.Location = new System.Drawing.Point(200, 50);
            this.hexGrid.Name = "hexGrid";
            this.hexGrid.Size = new System.Drawing.Size(1200, 600);
            this.hexGrid.TabIndex = 5;
            this.hexGrid.TabStop = false;
            this.hexGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.hexGrig_Paint);
            this.hexGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hexGrid_MouseDown);
            // 
            // info
            // 
            this.info.AutoSize = true;
            this.info.Location = new System.Drawing.Point(0, 0);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(0, 16);
            this.info.TabIndex = 8;
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.BackColor = System.Drawing.Color.Coral;
            this.infoLabel.Location = new System.Drawing.Point(561, 985);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(0, 16);
            this.infoLabel.TabIndex = 10;
            // 
            // turnSkip
            // 
            this.turnSkip.Location = new System.Drawing.Point(256, 894);
            this.turnSkip.Name = "turnSkip";
            this.turnSkip.Size = new System.Drawing.Size(131, 33);
            this.turnSkip.TabIndex = 11;
            this.turnSkip.Text = "Пропуск хода";
            this.turnSkip.UseVisualStyleBackColor = true;
            this.turnSkip.Click += new System.EventHandler(this.skipTurn_Click);
            // 
            // attackTimer
            // 
            this.attackTimer.Tick += new System.EventHandler(this.attackTimer_Tick);
            // 
            // underFireTimer
            // 
            this.underFireTimer.Tick += new System.EventHandler(this.underFireTimer_Tick);
            // 
            // deathTimer
            // 
            this.deathTimer.Tick += new System.EventHandler(this.deathTimer_Tick);
            // 
            // Battle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Kursach.Properties.Resources.battleBackgroundGrass;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.ControlBox = false;
            this.Controls.Add(this.turnSkip);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.info);
            this.Controls.Add(this.hexGrid);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Battle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Battle";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.hexGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.PictureBox hexGrid;
        private System.Windows.Forms.Label info;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button turnSkip;
        private System.Windows.Forms.Timer attackTimer;
        private System.Windows.Forms.Timer underFireTimer;
        private System.Windows.Forms.Timer deathTimer;
    }
}

