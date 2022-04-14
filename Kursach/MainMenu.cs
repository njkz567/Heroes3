using System;
using System.Windows.Forms;

namespace Kursach
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void play_Click(object sender, EventArgs e)
        {
            this.Hide();
            new NewGameMenu(this).Show();
        }

        private void loadGameMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoadGameMenu loadGameMenu = new LoadGameMenu(this);
            loadGameMenu.Show();
        }
    }
}
