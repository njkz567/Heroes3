namespace Kursach
{
    partial class GlobalMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalMap));
            this.smallMenu = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.endTurn = new System.Windows.Forms.Button();
            this.nextWarlord = new System.Windows.Forms.Button();
            this.previousWarlord = new System.Windows.Forms.Button();
            this.warlordGroupBox = new System.Windows.Forms.GroupBox();
            this.hireGroupBox = new System.Windows.Forms.GroupBox();
            this.Healer = new System.Windows.Forms.NumericUpDown();
            this.Archer = new System.Windows.Forms.NumericUpDown();
            this.SwordMan = new System.Windows.Forms.NumericUpDown();
            this.ArcherPictureBox = new System.Windows.Forms.PictureBox();
            this.HealerPictureBox = new System.Windows.Forms.PictureBox();
            this.SwordManPictureBox = new System.Windows.Forms.PictureBox();
            this.hireButton = new System.Windows.Forms.Button();
            this.goldGroupBox = new System.Windows.Forms.GroupBox();
            this.mineLevelLabel = new System.Windows.Forms.Label();
            this.mineLevelDefaultLabel = new System.Windows.Forms.Label();
            this.minePictureBox = new System.Windows.Forms.PictureBox();
            this.cityGroupBox = new System.Windows.Forms.GroupBox();
            this.previousCity = new System.Windows.Forms.Button();
            this.nextCity = new System.Windows.Forms.Button();
            this.mineImprove = new System.Windows.Forms.Button();
            this.hireHeroButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.unitStartPositionPictureBox = new System.Windows.Forms.PictureBox();
            this.cityPictureBox = new System.Windows.Forms.PictureBox();
            this.mapContainer = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gameInfoLabel = new System.Windows.Forms.Label();
            this.goldLabel = new System.Windows.Forms.Label();
            this.goldInfo = new System.Windows.Forms.Label();
            this.swordManCost = new System.Windows.Forms.Label();
            this.archerCost = new System.Windows.Forms.Label();
            this.healerCost = new System.Windows.Forms.Label();
            this.warlordGroupBox.SuspendLayout();
            this.hireGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Healer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Archer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SwordMan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArcherPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HealerPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SwordManPictureBox)).BeginInit();
            this.goldGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minePictureBox)).BeginInit();
            this.cityGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitStartPositionPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cityPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapContainer)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // smallMenu
            // 
            this.smallMenu.Location = new System.Drawing.Point(1451, 12);
            this.smallMenu.Name = "smallMenu";
            this.smallMenu.Size = new System.Drawing.Size(75, 23);
            this.smallMenu.TabIndex = 1;
            this.smallMenu.Text = "Menu";
            this.smallMenu.UseVisualStyleBackColor = true;
            this.smallMenu.Click += new System.EventHandler(this.smallMenu_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // endTurn
            // 
            this.endTurn.Location = new System.Drawing.Point(1423, 778);
            this.endTurn.Name = "endTurn";
            this.endTurn.Size = new System.Drawing.Size(103, 47);
            this.endTurn.TabIndex = 3;
            this.endTurn.Text = "Конец хода";
            this.endTurn.UseVisualStyleBackColor = true;
            this.endTurn.Click += new System.EventHandler(this.endTurn_Click);
            // 
            // nextWarlord
            // 
            this.nextWarlord.Location = new System.Drawing.Point(15, 85);
            this.nextWarlord.Name = "nextWarlord";
            this.nextWarlord.Size = new System.Drawing.Size(103, 47);
            this.nextWarlord.TabIndex = 4;
            this.nextWarlord.Text = "Следующий ";
            this.nextWarlord.UseVisualStyleBackColor = true;
            this.nextWarlord.Click += new System.EventHandler(this.nextWarlord_Click);
            // 
            // previousWarlord
            // 
            this.previousWarlord.Location = new System.Drawing.Point(15, 21);
            this.previousWarlord.Name = "previousWarlord";
            this.previousWarlord.Size = new System.Drawing.Size(103, 47);
            this.previousWarlord.TabIndex = 5;
            this.previousWarlord.Text = "Предыдущий";
            this.previousWarlord.UseVisualStyleBackColor = true;
            this.previousWarlord.Click += new System.EventHandler(this.previousWarlord_Click);
            // 
            // warlordGroupBox
            // 
            this.warlordGroupBox.Controls.Add(this.previousWarlord);
            this.warlordGroupBox.Controls.Add(this.nextWarlord);
            this.warlordGroupBox.Location = new System.Drawing.Point(1391, 623);
            this.warlordGroupBox.Name = "warlordGroupBox";
            this.warlordGroupBox.Size = new System.Drawing.Size(135, 149);
            this.warlordGroupBox.TabIndex = 6;
            this.warlordGroupBox.TabStop = false;
            this.warlordGroupBox.Text = "Герой";
            // 
            // hireGroupBox
            // 
            this.hireGroupBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.hireGroupBox.Controls.Add(this.healerCost);
            this.hireGroupBox.Controls.Add(this.archerCost);
            this.hireGroupBox.Controls.Add(this.swordManCost);
            this.hireGroupBox.Controls.Add(this.Healer);
            this.hireGroupBox.Controls.Add(this.Archer);
            this.hireGroupBox.Controls.Add(this.SwordMan);
            this.hireGroupBox.Controls.Add(this.ArcherPictureBox);
            this.hireGroupBox.Controls.Add(this.HealerPictureBox);
            this.hireGroupBox.Controls.Add(this.SwordManPictureBox);
            this.hireGroupBox.Location = new System.Drawing.Point(47, 138);
            this.hireGroupBox.Name = "hireGroupBox";
            this.hireGroupBox.Size = new System.Drawing.Size(265, 129);
            this.hireGroupBox.TabIndex = 8;
            this.hireGroupBox.TabStop = false;
            this.hireGroupBox.Text = "Войска";
            // 
            // Healer
            // 
            this.Healer.Location = new System.Drawing.Point(209, 77);
            this.Healer.Name = "Healer";
            this.Healer.Size = new System.Drawing.Size(50, 22);
            this.Healer.TabIndex = 5;
            this.Healer.ValueChanged += new System.EventHandler(this.Healer_ValueChanged);
            // 
            // Archer
            // 
            this.Archer.Location = new System.Drawing.Point(110, 77);
            this.Archer.Name = "Archer";
            this.Archer.Size = new System.Drawing.Size(50, 22);
            this.Archer.TabIndex = 4;
            this.Archer.ValueChanged += new System.EventHandler(this.Archer_ValueChanged);
            // 
            // SwordMan
            // 
            this.SwordMan.Location = new System.Drawing.Point(6, 77);
            this.SwordMan.Name = "SwordMan";
            this.SwordMan.Size = new System.Drawing.Size(50, 22);
            this.SwordMan.TabIndex = 3;
            this.SwordMan.ValueChanged += new System.EventHandler(this.SwordMan_ValueChanged);
            // 
            // ArcherPictureBox
            // 
            this.ArcherPictureBox.Image = global::Kursach.Properties.Resources.bow;
            this.ArcherPictureBox.Location = new System.Drawing.Point(110, 21);
            this.ArcherPictureBox.Name = "ArcherPictureBox";
            this.ArcherPictureBox.Size = new System.Drawing.Size(50, 50);
            this.ArcherPictureBox.TabIndex = 2;
            this.ArcherPictureBox.TabStop = false;
            // 
            // HealerPictureBox
            // 
            this.HealerPictureBox.Image = global::Kursach.Properties.Resources.cross;
            this.HealerPictureBox.Location = new System.Drawing.Point(209, 21);
            this.HealerPictureBox.Name = "HealerPictureBox";
            this.HealerPictureBox.Size = new System.Drawing.Size(50, 50);
            this.HealerPictureBox.TabIndex = 1;
            this.HealerPictureBox.TabStop = false;
            // 
            // SwordManPictureBox
            // 
            this.SwordManPictureBox.Image = global::Kursach.Properties.Resources.sword;
            this.SwordManPictureBox.Location = new System.Drawing.Point(6, 21);
            this.SwordManPictureBox.Name = "SwordManPictureBox";
            this.SwordManPictureBox.Size = new System.Drawing.Size(50, 50);
            this.SwordManPictureBox.TabIndex = 0;
            this.SwordManPictureBox.TabStop = false;
            // 
            // hireButton
            // 
            this.hireButton.Location = new System.Drawing.Point(85, 280);
            this.hireButton.Name = "hireButton";
            this.hireButton.Size = new System.Drawing.Size(81, 43);
            this.hireButton.TabIndex = 9;
            this.hireButton.Text = "Нанять войска";
            this.hireButton.UseVisualStyleBackColor = true;
            this.hireButton.Click += new System.EventHandler(this.hireButton_Click);
            // 
            // goldGroupBox
            // 
            this.goldGroupBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.goldGroupBox.Controls.Add(this.mineLevelLabel);
            this.goldGroupBox.Controls.Add(this.mineLevelDefaultLabel);
            this.goldGroupBox.Controls.Add(this.minePictureBox);
            this.goldGroupBox.Location = new System.Drawing.Point(53, 353);
            this.goldGroupBox.Name = "goldGroupBox";
            this.goldGroupBox.Size = new System.Drawing.Size(265, 164);
            this.goldGroupBox.TabIndex = 10;
            this.goldGroupBox.TabStop = false;
            this.goldGroupBox.Text = "Добыча";
            // 
            // mineLevelLabel
            // 
            this.mineLevelLabel.Location = new System.Drawing.Point(173, 106);
            this.mineLevelLabel.Name = "mineLevelLabel";
            this.mineLevelLabel.Size = new System.Drawing.Size(60, 23);
            this.mineLevelLabel.TabIndex = 2;
            this.mineLevelLabel.Text = "0";
            this.mineLevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mineLevelDefaultLabel
            // 
            this.mineLevelDefaultLabel.Location = new System.Drawing.Point(153, 45);
            this.mineLevelDefaultLabel.Name = "mineLevelDefaultLabel";
            this.mineLevelDefaultLabel.Size = new System.Drawing.Size(106, 54);
            this.mineLevelDefaultLabel.TabIndex = 1;
            this.mineLevelDefaultLabel.Text = "Уровень золотой шахты:";
            this.mineLevelDefaultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // minePictureBox
            // 
            this.minePictureBox.Image = global::Kursach.Properties.Resources.mineL;
            this.minePictureBox.Location = new System.Drawing.Point(6, 21);
            this.minePictureBox.Name = "minePictureBox";
            this.minePictureBox.Size = new System.Drawing.Size(130, 130);
            this.minePictureBox.TabIndex = 0;
            this.minePictureBox.TabStop = false;
            // 
            // cityGroupBox
            // 
            this.cityGroupBox.Controls.Add(this.previousCity);
            this.cityGroupBox.Controls.Add(this.nextCity);
            this.cityGroupBox.Location = new System.Drawing.Point(1391, 468);
            this.cityGroupBox.Name = "cityGroupBox";
            this.cityGroupBox.Size = new System.Drawing.Size(135, 149);
            this.cityGroupBox.TabIndex = 7;
            this.cityGroupBox.TabStop = false;
            this.cityGroupBox.Text = "Город";
            // 
            // previousCity
            // 
            this.previousCity.Location = new System.Drawing.Point(15, 21);
            this.previousCity.Name = "previousCity";
            this.previousCity.Size = new System.Drawing.Size(103, 47);
            this.previousCity.TabIndex = 5;
            this.previousCity.Text = "Предыдущий";
            this.previousCity.UseVisualStyleBackColor = true;
            this.previousCity.Click += new System.EventHandler(this.previousCity_Click);
            // 
            // nextCity
            // 
            this.nextCity.Location = new System.Drawing.Point(15, 85);
            this.nextCity.Name = "nextCity";
            this.nextCity.Size = new System.Drawing.Size(103, 47);
            this.nextCity.TabIndex = 4;
            this.nextCity.Text = "Следующий ";
            this.nextCity.UseVisualStyleBackColor = true;
            this.nextCity.Click += new System.EventHandler(this.nextCity_Click);
            // 
            // mineImprove
            // 
            this.mineImprove.Location = new System.Drawing.Point(147, 527);
            this.mineImprove.Name = "mineImprove";
            this.mineImprove.Size = new System.Drawing.Size(81, 43);
            this.mineImprove.TabIndex = 11;
            this.mineImprove.Text = "Улучшить";
            this.mineImprove.UseVisualStyleBackColor = true;
            this.mineImprove.Click += new System.EventHandler(this.mineImprove_Click);
            // 
            // hireHeroButton
            // 
            this.hireHeroButton.Location = new System.Drawing.Point(196, 280);
            this.hireHeroButton.Name = "hireHeroButton";
            this.hireHeroButton.Size = new System.Drawing.Size(81, 43);
            this.hireHeroButton.TabIndex = 15;
            this.hireHeroButton.Text = "Нанять героя";
            this.hireHeroButton.UseVisualStyleBackColor = true;
            this.hireHeroButton.Click += new System.EventHandler(this.hireHeroButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Kursach.Properties.Resources._0iF9G_abLrU;
            this.pictureBox1.Location = new System.Drawing.Point(236, 74);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(26, 26);
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // unitStartPositionPictureBox
            // 
            this.unitStartPositionPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.unitStartPositionPictureBox.BackgroundImage = global::Kursach.Properties.Resources.unitStartPositionGrid;
            this.unitStartPositionPictureBox.Location = new System.Drawing.Point(15, 13);
            this.unitStartPositionPictureBox.Name = "unitStartPositionPictureBox";
            this.unitStartPositionPictureBox.Size = new System.Drawing.Size(312, 129);
            this.unitStartPositionPictureBox.TabIndex = 17;
            this.unitStartPositionPictureBox.TabStop = false;
            // 
            // cityPictureBox
            // 
            this.cityPictureBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.cityPictureBox.Location = new System.Drawing.Point(0, 100);
            this.cityPictureBox.Name = "cityPictureBox";
            this.cityPictureBox.Size = new System.Drawing.Size(400, 500);
            this.cityPictureBox.TabIndex = 7;
            this.cityPictureBox.TabStop = false;
            // 
            // mapContainer
            // 
            this.mapContainer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mapContainer.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.mapContainer.Location = new System.Drawing.Point(507, 36);
            this.mapContainer.Margin = new System.Windows.Forms.Padding(0);
            this.mapContainer.Name = "mapContainer";
            this.mapContainer.Size = new System.Drawing.Size(349, 700);
            this.mapContainer.TabIndex = 0;
            this.mapContainer.TabStop = false;
            this.mapContainer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mapContainer_MouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.unitStartPositionPictureBox);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(1184, 100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(342, 167);
            this.panel1.TabIndex = 21;
            // 
            // gameInfoLabel
            // 
            this.gameInfoLabel.AutoSize = true;
            this.gameInfoLabel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gameInfoLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gameInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gameInfoLabel.Location = new System.Drawing.Point(668, 778);
            this.gameInfoLabel.Name = "gameInfoLabel";
            this.gameInfoLabel.Size = new System.Drawing.Size(2, 27);
            this.gameInfoLabel.TabIndex = 22;
            this.gameInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // goldLabel
            // 
            this.goldLabel.AutoSize = true;
            this.goldLabel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.goldLabel.Location = new System.Drawing.Point(56, 644);
            this.goldLabel.Name = "goldLabel";
            this.goldLabel.Size = new System.Drawing.Size(117, 16);
            this.goldLabel.TabIndex = 23;
            this.goldLabel.Text = "Текущее золото:";
            // 
            // goldInfo
            // 
            this.goldInfo.AutoSize = true;
            this.goldInfo.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.goldInfo.Location = new System.Drawing.Point(172, 644);
            this.goldInfo.Name = "goldInfo";
            this.goldInfo.Size = new System.Drawing.Size(35, 16);
            this.goldInfo.TabIndex = 24;
            this.goldInfo.Text = "1000";
            // 
            // swordManCost
            // 
            this.swordManCost.BackColor = System.Drawing.SystemColors.Control;
            this.swordManCost.Location = new System.Drawing.Point(6, 102);
            this.swordManCost.Name = "swordManCost";
            this.swordManCost.Size = new System.Drawing.Size(50, 24);
            this.swordManCost.TabIndex = 6;
            this.swordManCost.Text = "100";
            this.swordManCost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // archerCost
            // 
            this.archerCost.BackColor = System.Drawing.SystemColors.Control;
            this.archerCost.Location = new System.Drawing.Point(110, 102);
            this.archerCost.Name = "archerCost";
            this.archerCost.Size = new System.Drawing.Size(50, 24);
            this.archerCost.TabIndex = 7;
            this.archerCost.Text = "200";
            this.archerCost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // healerCost
            // 
            this.healerCost.BackColor = System.Drawing.SystemColors.Control;
            this.healerCost.Location = new System.Drawing.Point(209, 102);
            this.healerCost.Name = "healerCost";
            this.healerCost.Size = new System.Drawing.Size(50, 24);
            this.healerCost.TabIndex = 8;
            this.healerCost.Text = "300";
            this.healerCost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GlobalMap
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1538, 837);
            this.ControlBox = false;
            this.Controls.Add(this.goldInfo);
            this.Controls.Add(this.goldLabel);
            this.Controls.Add(this.gameInfoLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.hireHeroButton);
            this.Controls.Add(this.mineImprove);
            this.Controls.Add(this.cityGroupBox);
            this.Controls.Add(this.goldGroupBox);
            this.Controls.Add(this.hireButton);
            this.Controls.Add(this.hireGroupBox);
            this.Controls.Add(this.cityPictureBox);
            this.Controls.Add(this.warlordGroupBox);
            this.Controls.Add(this.endTurn);
            this.Controls.Add(this.smallMenu);
            this.Controls.Add(this.mapContainer);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GlobalMap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlobalMap";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.GlobalMap_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobalMap_KeyDown);
            this.warlordGroupBox.ResumeLayout(false);
            this.hireGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Healer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Archer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SwordMan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArcherPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HealerPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SwordManPictureBox)).EndInit();
            this.goldGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minePictureBox)).EndInit();
            this.cityGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitStartPositionPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cityPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapContainer)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox mapContainer;
        private System.Windows.Forms.Button smallMenu;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button endTurn;
        private System.Windows.Forms.Button nextWarlord;
        private System.Windows.Forms.Button previousWarlord;
        private System.Windows.Forms.GroupBox warlordGroupBox;
        private System.Windows.Forms.PictureBox cityPictureBox;
        private System.Windows.Forms.GroupBox hireGroupBox;
        private System.Windows.Forms.NumericUpDown Healer;
        private System.Windows.Forms.NumericUpDown Archer;
        private System.Windows.Forms.NumericUpDown SwordMan;
        private System.Windows.Forms.PictureBox ArcherPictureBox;
        private System.Windows.Forms.PictureBox HealerPictureBox;
        private System.Windows.Forms.PictureBox SwordManPictureBox;
        private System.Windows.Forms.Button hireButton;
        private System.Windows.Forms.GroupBox goldGroupBox;
        private System.Windows.Forms.Label mineLevelDefaultLabel;
        private System.Windows.Forms.PictureBox minePictureBox;
        private System.Windows.Forms.GroupBox cityGroupBox;
        private System.Windows.Forms.Button previousCity;
        private System.Windows.Forms.Button nextCity;
        private System.Windows.Forms.Button mineImprove;
        private System.Windows.Forms.Label mineLevelLabel;
        private System.Windows.Forms.Button hireHeroButton;
        private System.Windows.Forms.PictureBox unitStartPositionPictureBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label gameInfoLabel;
        private System.Windows.Forms.Label goldLabel;
        private System.Windows.Forms.Label goldInfo;
        private System.Windows.Forms.Label healerCost;
        private System.Windows.Forms.Label archerCost;
        private System.Windows.Forms.Label swordManCost;
    }
}