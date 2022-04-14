using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursach
{
    public partial class SaveName : Form
    {
        public SaveName()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (saveNameTextBox.Text == "")
                return;
            ((SmallMenu)this.Owner).saveName = saveNameTextBox.Text;
            this.Close();
        }

        private void SaveName_Load(object sender, EventArgs e)
        {
            ((SmallMenu)this.Owner).saveName = saveNameTextBox.Text;
        }
    }
}
