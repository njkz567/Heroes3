using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Kursach
{
    public partial class LoadGameMenu : Form
    {
        private MainMenu mainMenu;
        private List<string> savesList = new List<string>();

        private Map map;
        private List<Player> players = new List<Player>();
        private int currentPlayerIndex, currentCityIndex, currentWarlordIndex;
        private List<string> unitTypes = new List<string>();
        private XmlDocument xmlDoc;

        public LoadGameMenu(MainMenu mainMenu)
        {
            InitializeComponent();

            this.mainMenu = mainMenu;

            foreach (string save in Directory.GetFiles(@"../../Resources/Saves"))
                savesList.Add(save.Replace(@"../../Resources/Saves", ""));

            saves.Items.AddRange(savesList.ToArray());
        }

        private void close_Click(object sender, EventArgs e)
        {
            mainMenu.Show();
            this.Close();
        }

        private void load_Click(object sender, EventArgs e)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load("../../Units.xml");

            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = File.Open(@"../../Resources/Saves" + saves.Text, FileMode.Open);

            map = (Map)formatter.Deserialize(stream);
            players = (List<Player>)formatter.Deserialize(stream);
            unitTypes = (List<string>)formatter.Deserialize(stream);

            map.SetAnimationAfterDeserialization();

            foreach (Player player in players)
                player.SetAnimationAfterDeserialization(xmlDoc);

            map.ReLinking(players);

            using (BinaryReader reader = new BinaryReader(stream))
            {
                currentPlayerIndex = reader.ReadInt32();
                currentCityIndex = reader.ReadInt32();
                currentWarlordIndex = reader.ReadInt32();
            }

            stream.Close();

            GlobalMap g = new GlobalMap(mainMenu, map, players, unitTypes, xmlDoc);
            g.Owner = this;
            g.Show();
        }
    }
}
