using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Kursach
{
    public partial class NewGameMenu : Form
    {
        private MainMenu mainMenu;

        private List<string> races, unitType;

        private List<ComboBox> playersComboBox;
        private int comboBoxHeight;

        private XmlDocument xmlDoc;

        private Map map = null;
        private List<Player> players = new List<Player>();

        // Знакомтесь, чудовище Франкенштейна, оставил, чисто как памятник
        Dictionary<string, Dictionary<string, List<string>>> unitData;

        public NewGameMenu(MainMenu mainMenu)
        {
            InitializeComponent();

            mapHeightNumericUpDown.Minimum = 7;

            races = new List<string>();
            unitType = new List<string>();

            xmlDoc = new XmlDocument();
            xmlDoc.Load("../../Units.xml");

            // получим корневой элемент, то есть <objects>
            XmlElement xmlRoot = xmlDoc.DocumentElement;
            if (xmlRoot != null)
            {
                bool typesReceived = false;
                // проходимся по всем <race>
                foreach (XmlNode race in xmlRoot.ChildNodes)
                {
                    XmlNode raceName = race.Attributes.GetNamedItem("name");
                    races.Add(raceName.Value);

                    // проходимся по всему внутри <race>, то есть по <warlord> и <units>
                    foreach (XmlNode unit in race.ChildNodes)
                    {
                        if (unit.Name != "units" || typesReceived)
                            continue;

                        // проходимся по всем <type>
                        foreach (XmlNode type in unit.ChildNodes)
                        {
                            XmlNode typeName = type.Attributes.GetNamedItem("name");
                            unitType.Add(typeName.Value);
                        }
                        typesReceived = true;
                    }
                }
            }

            comboBoxHeight = playersPanel.Height / Convert.ToInt32(playerNumericUpDown.Maximum);

            playersComboBox = new List<ComboBox>();
            SetComboBox(playersComboBox, playersPanel);
            playerNumericUpDown.Value = 1;

            this.mainMenu = mainMenu;
        }

        private void SetComboBox (List<ComboBox> comboBoxes, Panel start)
        {
            ComboBox temp = new ComboBox();
            temp.FormattingEnabled = true;
            temp.Items.AddRange(races.ToArray());
            temp.Text = races.FirstOrDefault();
            temp.Size = new Size(start.Width, comboBoxHeight);
            temp.Location = new Point(0, comboBoxHeight * comboBoxes.Count);
            start.Controls.Add(temp);
            comboBoxes.Add(temp);
        }

        private void back_Click(object sender, EventArgs e)
        {
            mainMenu.Show();
            this.Close();
        }

        private void play_Click(object sender, EventArgs e)
        {
            if (map == null)
                return;

            GC.Collect();
            
            GlobalMap g = new GlobalMap(mainMenu, map, players, unitType, xmlDoc);
            g.Owner = this;
            g.Show();
        }      

        private void mapMakeButton_Click(object sender, EventArgs e)
        {
            if (players.Count != 0)
                players.Clear();

            foreach (ComboBox comboBox in playersComboBox)
                players.Add(new Player(comboBox.Text));

            map = new Map((int)mapHeightNumericUpDown.Value, (int)mapHeightNumericUpDown.Value, players, xmlDoc);
        }

        private void playerNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (map != null)
            {
                map = null;
                GC.Collect();
            }
                

            if (playersComboBox.Count > playerNumericUpDown.Value)
                for (int i = playersComboBox.Count; i > playerNumericUpDown.Value; i--)
                {
                    playersComboBox.RemoveAt(i-1);
                    playersPanel.Controls.RemoveAt(i-1);
                }
            else if (playersComboBox.Count < playerNumericUpDown.Value)
                for (int i = playersComboBox.Count; i < playerNumericUpDown.Value; i++)
                {
                    SetComboBox(playersComboBox, playersPanel);
                }
        }

    }
}
