using System;
using System.Drawing;

namespace Kursach
{
    // добавлю этот хекс в класс юнита
    internal class Hex
    {
        private int x, y, s;      // оси гексов
        private bool locked;      // занят ли гекс (и скорее всего занят именно препятствием, а не врагом)
        public Unit unit;

        public enum Directions : int
        {
            upRight = 0,
            Right = 1,
            downRight = 2,
            downLeft = 3,
            Left = 4,
            upLeft = 5,
            None = 6
        }

        public int X { get { return x; } }

        public int Y { get { return y; } }

        public int S { get { return s; } }

        //public bool Locked { get { return locked; } set { locked = value; } }

        public Hex (Point point, bool locked = false, Unit unit = null)
        {
            x = point.X;
            y = point.Y;
            s = -x - y;
            this.locked = locked;
            this.unit = unit;
        }

        public Hex (int x, int y, bool locked = false, Unit unit = null)
        {
            this.x = x;
            this.y = y;
            s = -x - y;
            this.locked = locked;
            this.unit = unit;
        }

        static private Hex[] directionVectors = new Hex[] { new Hex(+1, -1), new Hex(+1, 0), new Hex(0, +1),
                                                            new Hex(-1, +1), new Hex(-1, 0), new Hex(0, -1) };

        public bool InRange (Hex a, int range)
        {
            if (a == null)
                return false;

            if (Math.Abs(x - a.X) <= range && Math.Abs(y - a.Y) <= range && Math.Abs(s - a.S) <= range)
                return true;

            return false;
        }

        public Directions WhichNeighbor (Hex a)
        {
            for (int i = 0; i < directionVectors.Length; i++)
            {
                if (Add(directionVectors[i]).IsEqual(a))
                    return (Directions)i;
            }
            return Directions.None;
        }

        private Hex Add (Hex a)
        {
            return new Hex(this.X + a.X, this.Y + a.Y);
        }

        private bool IsEqual (Hex a)
        {
            if (this == a)
                return true;
            if (this.X == a.X && this.Y == a.Y) 
                return true;
            return false;
        }

        public void Locked () { locked = true; }

        public void UnLocked () { locked = false; }

        public bool IsLocked () { return locked; }

        public override string ToString()
        {
            return $"x = {x}, y = {y}";
        }
    }
}