using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kursach
{
    internal class HexGrid
    {
        public static readonly int hexWidth = 1;
        // дефолтный цвет клеток
        private static readonly Pen brownPen = new Pen(Color.Brown, hexWidth);
        private static readonly Brush brownBrush = new SolidBrush(Color.FromArgb(150, 227, 165, 59));
        // для врагов 
        private static readonly Pen redPen = new Pen(Color.Red, hexWidth);
        private static readonly Brush redBrush = new SolidBrush(Color.FromArgb(150, 235, 26, 26));
        // для клеток хода
        private static readonly Pen greenPen = new Pen(Color.Green, hexWidth);
        private static readonly Brush greenBrush = new SolidBrush(Color.FromArgb(150, 45, 214, 26));
        // для препятствий 
        private static readonly Pen grayPen = new Pen(Color.Gray, hexWidth);
        private static readonly Brush grayBrush = new SolidBrush(Color.FromArgb(150, 119, 136, 153));

        private Hex[][] battleGrid;
        public readonly float R, r, a;
        private PointF offset;
        private int[] rowCorrection;
        private int rows, cols;

        public HexGrid (float R, int rows, int cols)
        {
            this.R = R;
            r = R * (float)Math.Cos(Math.PI / 6);
            a = 2 * R * (float)Math.Sin(Math.PI / 6);
            offset = new PointF(2 * r, R);
            rowCorrection = new int[rows];
            this.rows = rows;
            this.cols = cols;

            Point center = new Point();
            int correction = 0;

            battleGrid = new Hex[rows][];
            for (int i = 0; i < rows; i++)
            {
                if (i % 2 == 1)
                    correction--;
                rowCorrection[i] = -correction;
                center.Y = i;
                battleGrid[i] = new Hex[cols];
                center.X = correction;
                for (int j = 0; j < cols; j++)
                {
                    battleGrid[i][j] = new Hex(center);
                    center.X++;
                }
            }
        }

        // я, конечно, не космонавт, но перегрузки люблю
        public void HexToPixel (Hex hex, out float x, out float y)
        {
            if (hex.Y % 2 == 0)
                x = offset.X + offset.X * (hex.X + rowCorrection[hex.Y]);
            else
                x = offset.X / 2 + offset.X * (hex.X + rowCorrection[hex.Y]);

            y = offset.Y + offset.Y * hex.Y * 3.0f / 2;
        }

        public PointF HexToPixel (Hex hex)
        {
            PointF point = new PointF();
            if (hex.Y % 2 == 0)
                point.X = offset.X + offset.X * (hex.X + rowCorrection[hex.Y]);
            else
                point.X = offset.X / 2 + offset.X * (hex.X + rowCorrection[hex.Y]);

            point.Y = offset.Y + offset.Y * hex.Y * 3.0f / 2;

            return point;
        }

        public Point IntHexToPixel (Hex hex)
        {
            Point point = new Point();
            if (hex.Y % 2 == 0)
                point.X = Convert.ToInt32(offset.X + offset.X * (hex.X + rowCorrection[hex.Y]));
            else
                point.X = Convert.ToInt32(offset.X / 2 + offset.X * (hex.X + rowCorrection[hex.Y]));

            point.Y = Convert.ToInt32(offset.Y + offset.Y * hex.Y * 3.0f / 2);

            return point;
        }

        public Hex PixelToHex (Point mouse)
        {
            int y = (int)Math.Round((mouse.Y - offset.Y) / (3.0f / 2 * R)); 

            if (y < 0)
                y = 0;
            else if (y >= battleGrid.Length)
                y = battleGrid.Length - 1;

            int x;
            if (y % 2 == 0)
                x = (int)Math.Round((mouse.X - offset.X) / (2 * r));
            else
                x = (int)Math.Round((mouse.X - offset.X / 2) / (2 * r));

            if (x < 0)
                x = 0;
            else if (x >= battleGrid[y].Length)
                x = battleGrid[y].Length - 1;

            return battleGrid[y][x];
        }

        public void Drawgrid(Graphics g)
        {
            for (int i = 0; i < battleGrid.Length; i++)
                for (int j = 0; j < battleGrid[i].Length; j++)
                    DrawHex(g, battleGrid[i][j], brownPen, brownBrush);
        }

        private void DrawHex (Graphics g, Hex hex, Pen pen, Brush brush)
        {
            float x, y, angle = 0.523599f;
            PointF[] points = new PointF[7];
            HexToPixel(hex, out x, out y);
            PointF center = new PointF(x, y);
            for (int i = 0; i < 6; i++)
            {
                x = R * (float)Math.Cos(angle) + center.X;
                y = -R * (float)Math.Sin(angle) + center.Y;
                points[i] = new PointF(x, y);
                angle += 1.0472f;
            }
            points[6] = points[0];
            
            if (pen == redPen)
            {
                g.DrawLines(redPen, points);
                g.FillPolygon(redBrush, points);
            }
            else if (hex.IsLocked())
            {
                g.DrawLines(grayPen, points);
                g.FillPolygon(grayBrush, points);
            }
            else
            {
                g.DrawLines(pen, points);
                g.FillPolygon(brush, points);
            }
        }

        // подсвечивает клетки в радиусе, куда можно сходить
        public void HighlightDistance (Graphics g, Unit unit, List<Unit> enemies)
        {
            HashSet<Hex> reachableHexes = ReachableHexes(unit.Position, unit.MoveRange);

            // сначала помечаем все гексы зеленым и серым
            foreach (Hex hex in reachableHexes)
            {
                DrawHex(g, hex, greenPen, greenBrush);
            }

            // потом проходимся по всем врагам и если в рейндже атаки и не мертвый, то подсветим
            foreach (Unit enemy in enemies)
                if (unit.Position.InRange(enemy.Position, unit.MoveRange) && !enemy.IsDead())
                    DrawHex(g, enemy.Position, redPen, redBrush);

            // дополнительно проходимся по всем юнитам если красим гексы для лучника
            if (unit is Archer)
                foreach (Unit enemy in enemies)
                    if (unit.Position.InRange(enemy.Position, unit.AttackRange) && !enemy.IsDead())
                        DrawHex(g, enemy.Position, redPen, redBrush);
        }

        private List<Hex> Neighbors (Hex hex, int distance)
        {
            List<Hex> neighbors = new List<Hex>();

            // по оси y
            for (int i = hex.Y - distance; i <= hex.Y + distance; i++)
            {
                if (i < 0 || i >= battleGrid.Length)
                    continue;
                int a = Math.Max(-distance, -i - distance + hex.Y) + hex.X;
                int b = Math.Min(distance, -i + distance + hex.Y) + hex.X;
                // по оси x
                for (int j = a; j <= b; j++)
                    // проверка на границы гексосетки ВОЗМОЖЕН ВЫХОД ЗА ГРАНИЦЫ МАССИВА!!!
                    if (j >= -rowCorrection[i] && j < battleGrid[i].Length - rowCorrection[i])
                        if (!battleGrid[i][j + rowCorrection[i]].IsLocked())
                            neighbors.Add(battleGrid[i][j + rowCorrection[i]]);
            }
            return neighbors;
        }

        // работает в паре с братишкой Neighbors
        private HashSet<Hex> ReachableHexes (Hex start, int distance)
        {
            HashSet<Hex> visited = new HashSet<Hex>();
            visited.Add(start);

            List<Hex>[] circles = new List<Hex>[distance + 1];
            circles[0] = new List<Hex>();
            circles[0].Add(start);

            for (int i = 1; i <= distance; i++)
            {
                circles[i] = new List<Hex>();
                foreach (Hex b in circles[i-1])
                {
                    List<Hex> temp = Neighbors(b, 1);
                    foreach (Hex c in temp)
                    {
                        if (!visited.Contains(c))
                        {
                            visited.Add(c);
                            circles[i].Add(c);
                        } 
                    }
                }  
            }

            return visited;
        }

        private Dictionary<Hex, Hex> BreadthFirstSearch (Hex start, Hex goal)
        {
            Queue<Hex> frontier = new Queue<Hex>();
            frontier.Enqueue(start);

            Dictionary<Hex, Hex> cameFrom = new Dictionary<Hex, Hex>();
            cameFrom.Add(start, null);

            while (frontier.Count > 0)
            {
                Hex current = frontier.Dequeue();

                if (current == goal)
                    break;

                foreach (Hex next in Neighbors(current, 1))
                    if (!cameFrom.ContainsKey(next))
                    {
                        frontier.Enqueue(next);
                        cameFrom.Add(next, current);
                    }
            }

            return cameFrom;
        }

        public Stack<Hex> PathFinding (Hex start, Hex goal)
        {
            Dictionary<Hex, Hex> cameFrom = BreadthFirstSearch(start, goal);

            Stack<Hex> path = new Stack<Hex>();
            Hex current = goal;

            while (current != start)
            {
                path.Push(current);
                current = cameFrom[current];
            }

            return path;
        }

        public Point NeighborToPixel (Hex hex, Hex neighbor, int frames)
        {
            float x = 0, y = 0;
            Hex.Directions dir = hex.WhichNeighbor(neighbor);
            switch(dir)
            {
                case Hex.Directions.upRight:
                    x = r / frames;
                    y = -3f / 2 * R / frames;
                    break;
                case Hex.Directions.Right:
                    x = r * 2 / frames;
                    break;
                case Hex.Directions.downRight:
                    x = r / frames;
                    y = 3f / 2 * R / frames;
                    break;
                case Hex.Directions.downLeft:
                    x = -r / frames;
                    y = 3f / 2 * R / frames;
                    break;
                case Hex.Directions.Left:
                    x = -r * 2 / frames;
                    break;
                case Hex.Directions.upLeft:
                    x = -r / frames;
                    y = -3f / 2 * R / frames;
                    break;
                case Hex.Directions.None:
                    //throw new Exception(); типа крутой выбросил исключение 
                    break;
            }
            return new Point(Convert.ToInt32(x), Convert.ToInt32(y));
        }

        public void PlaceUnit (Unit unit, bool left = true) 
        {
            int x = cols - 1 - unit.PositionCoords.X;
            if (left)
                x = unit.PositionCoords.X;
            int y = unit.PositionCoords.Y;
            battleGrid[y][x].Locked();
            battleGrid[y][x].unit = unit;
            unit.Position = battleGrid[y][x];
        }

        public void ReplaceUnit (Unit unit) 
        { 
            // разлочили гекс, на котором находился юнит
            unit.Position.UnLocked();
            // убрали ссылку у гекса на этого юнита
            unit.Position.unit = null;
            // осталось сменить у юнита его привязааный гекс
            // это происходит в Unit.Step()
        }
    }
}
