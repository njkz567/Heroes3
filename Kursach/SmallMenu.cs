using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Kursach
{
    internal partial class SmallMenu : Form
    {
        private Form parentForm;
        private Map map;
        private List<Player> players;
        private List<string> unitTypes;
        private int currentPlayerIndex, currentCityIndex, currentWarlordIndex;
        public string saveName;

        public SmallMenu(Form parentForm, Map map, List<Player> players, List<string> unitTypes,
                         int currentPlayerIndex, int currentCityIndex, int currentWarlordIndex)
        {
            InitializeComponent();

            this.parentForm = parentForm;
            this.map = map;
            this.players = players;
            this.unitTypes = unitTypes;
            this.currentPlayerIndex = currentPlayerIndex;
            this.currentCityIndex = currentCityIndex;
            this.currentWarlordIndex = currentWarlordIndex;
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveName s = new SaveName();
            s.Owner = this;
            s.ShowDialog();
            Save();
        }

        private void Save ()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = File.Open("../../Resources/Saves/" + saveName + ".bin", FileMode.Create); 

            formatter.Serialize(stream, map);
            formatter.Serialize(stream, players);
            formatter.Serialize(stream, unitTypes);

            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(currentPlayerIndex);
                writer.Write(currentCityIndex);
                writer.Write(currentWarlordIndex);
            }

            stream.Close();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            parentForm.Close();
            this.Close();
        }
    }
}
