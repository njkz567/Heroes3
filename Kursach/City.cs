using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kursach
{
    [Serializable()]
    internal class City
    {
        static private int defaultGold = 1000; // добыча золота за каждый уровень
        private string race;
        private string name;
        private int mineLevel = 0;
        private int costOfMineImprovement = 1000, coef = 2;
        private Tile position;

        public int MineLevel { get { return mineLevel; } }
        public int CostOfMineImprovement { get { return costOfMineImprovement; } }

        public Tile Position { get { return position; } set { position = value; } }

        public string Name { get { return name; } }

        public City (string race, Tile tile) 
        { 
            this.race = race; 
            position = tile;
            position.SetCity(this);
        }

        public void MineLevelIncrease () 
        { 
            mineLevel++; 
            costOfMineImprovement *= coef; 
        }

        // выплата золота городом в конце каждого хода
        public int PayTribute () { return mineLevel * defaultGold; }
    }
}
