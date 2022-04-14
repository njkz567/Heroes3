using System;
using System.Drawing;

namespace Kursach
{
    public enum Landscape 
    {
        None = 0,
        Road = 1,
        Forest = 2,
        Water = 3,
        City = 4,
    }

    public enum TileDirection
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
    }

    [Serializable()]
    internal class Tile
    {
        private int x, y;
        private Landscape landscape;
        // содержит ссылку на варлорда если есть
        [NonSerialized] private Warlord warlord;
        // содержит ссылку на город если есть
        [NonSerialized] private City city;

        public int X { get { return x; } private set { x = value; } }
        public int Y { get { return y; } private set { y = value; } }
        public Landscape Landscape { get { return landscape; } set { landscape = value; } }
        public City City { get { return city; } }
        public Warlord Warlord { get { return warlord; } set { warlord = value; } }

        public Tile (int x, int y, Warlord warlord = null, City city = null)
        {
            this.x = x;
            this.y = y;
            landscape = Landscape.None;
            this.warlord = warlord;
            this.city = city;
        }

        // установка города срабатывает только один раз
        public void SetCity (City city)
        {
            if (city == null)
                throw new ArgumentNullException();
            // если еще не присваивали никакйо город тому тайлу
            this.city = city;
        }

        public void SetWarlord (Warlord warlord) { this.warlord = warlord; }

        // эта функция выдает координаты всех соседей
        public Point[] Neighbors ()
        {
            Point[] result = new Point[4]; // у каждого тайла только 4 соседа 
            result[0] = new Point(x, y - 1);
            result[1] = new Point(x + 1, y);
            result[2] = new Point(x, y + 1);
            result[3] = new Point(x - 1, y);
            return result;
        }

        // для проверки близости городов друг к другу
        public bool OutOfRange (Tile tile, int range)
        {
            return Math.Abs(x - tile.X) > range || Math.Abs(y - tile.Y) > range;
        }

        public TileDirection Orientation (Tile goal)
        {
            TileDirection result = TileDirection.None;
            if (x < goal.X)
                result = TileDirection.Right;
            else
                result = TileDirection.Left;
            if (y < goal.Y)
                result = TileDirection.Down;
            else
                result |= TileDirection.Up;
            return result;
        }

        // через простое равно сравнивается идентичность объектов (один и тот же)
        // а здесь только равенство координат
        public bool IsEqualCoordinates (Tile tile)
        {
            if (x == tile.X && y == tile.Y)
                return true;
            return false;
        }
    }
}
