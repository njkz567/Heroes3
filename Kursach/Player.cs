using System;
using System.Collections.Generic;
using System.Xml;

namespace Kursach
{
    // класс для любого действующего игрока
    [Serializable()]
    internal class Player
    {
        public readonly string baseRace;

        public List<Warlord> warlords;
        public List<City> cities;

        private int gold = 1000;  // стартовое золото по умолчанию

        public int Gold { get { return gold; } }

        public Player(string baseRace)
        {
            this.baseRace = baseRace;
            warlords = new List<Warlord>();
            cities = new List<City>();
        }

        public void SetAnimationAfterDeserialization(XmlDocument xmlDoc)
        {
            foreach (Warlord warlord in warlords)
                warlord.SetAnimationAfterDeserialization(xmlDoc);
        }

        public Warlord getWarlordByIndex(int index) { return warlords[index]; }

        public City getCityByIndex(int index) { return cities[index]; }

        // Пополнить казну с каждого города
        public void CollectTribute ()
        {
            foreach (City c in cities)
                gold += c.PayTribute();
        }

        // функция понадобится для перехвата вражеского города
        public void AddCity(City city) { cities.Add(city); }

        public void RemoveWarlord(Warlord warlord) 
        {
            // удаление варлорда автоматически освобождает тайл, на котором он находился
            warlord.MakeNullPositionRefToWarlord();
            warlords.Remove(warlord); 
        }

        public void RemoveCity(City city) { cities.Remove(city); }

        public City GetCityByTile (Tile tile)
        {
            foreach (City city in cities)
                if (city.Position == tile)
                    return city;
            return null;
        }

        public Warlord GetWarlordByTile (Tile tile)
        {
            foreach (Warlord warlord in warlords)
                if (warlord.Position == tile)
                    return warlord;
            return null;
        }

        public void StartCity (Tile tile)
        {
            if (cities.Count != 0)
                return;
            cities.Add(new City(baseRace, tile));
        }

        public void CreateWarlord (City city, XmlDocument xmlDoc)
        {
            Warlord warlord = new Warlord(baseRace, city.Position, xmlDoc);
            city.Position.Warlord = warlord;
            warlords.Add(warlord);
        }

        public bool MineLevelIncrease (City city, out int wasted)
        {
            if (city.CostOfMineImprovement > gold)
            {
                wasted = 0;
                return false;
            }
                
            wasted = city.CostOfMineImprovement;
            gold -= city.CostOfMineImprovement;
            city.MineLevelIncrease();
            return true;
        }
        
        // проверяет, можно ли нанять столько
        public bool CanHireUnits (Dictionary<string, int> typeAndAmount, out int wasted)
        {
            int sumOfHire = 0;
            foreach (KeyValuePair<string, int> pair in typeAndAmount)
            {
                switch (pair.Key)
                {
                    case "SwordMan":
                        sumOfHire += pair.Value * SwordMan.CostOfHire;
                        break;
                    case "Archer":
                        sumOfHire += pair.Value * Archer.CostOfHire;
                        break;
                    case "Healer":
                        sumOfHire += pair.Value * Healer.CostOfHire;
                        break;
                }
                
            }
                
            wasted = sumOfHire;
            // если денег не хватает, ничего не делаем
            if (sumOfHire > gold)
                return false;
            // иначе списываем нужную сумму
            gold -= sumOfHire;
            return true;
        }

        public int AmountOfCities () { return cities.Count; }

        public int AmountOfWarlords () { return warlords.Count; }

        public bool IsWarlordInArmy (Warlord warlord) 
        {
            if (warlord == null)
                throw new ArgumentNullException();
            return warlords.Contains(warlord); 
        }

        public bool IsCityInPlayer (City city)
        {
            if (city == null)
                throw new ArgumentNullException();
            return cities.Contains(city);
        }

        // игрок проиграл, если у него больше нет городов и варлордов
        public bool IsLost ()
        {
            return cities.Count == 0 && warlords.Count == 0;
        }

        public bool WithoutWarlords () { return warlords.Count == 0; }

        public bool WithoutCities () { return cities.Count == 0; }
    }
}
