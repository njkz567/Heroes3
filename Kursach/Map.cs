using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Kursach
{
    // нужен чисто для Дийкстры и не используется вне генератора карты
    internal class PriorityQueue<TPriority, TItem> : IEnumerable<TItem>, IEnumerable<KeyValuePair<TPriority, TItem>>
    {
        private readonly SortedDictionary<TPriority, Queue<TItem>> _storage;

        public PriorityQueue() : this(Comparer<TPriority>.Default)
        {

        }

        public PriorityQueue(IComparer<TPriority> comparer)
        {
            _storage = new SortedDictionary<TPriority, Queue<TItem>>(comparer);
        }

        public int Count
        {
            get;
            private set;
        }

        public void Enqueue(TPriority priority, TItem item)
        {
            if (!_storage.TryGetValue(priority, out var queue))
                _storage[priority] = queue = new Queue<TItem>();
            queue.Enqueue(item);

            Count++;
        }

        public TItem Dequeue()
        {
            if (Count == 0)
                throw new InvalidOperationException("Queue is empty");

            var queue = _storage.First();
            var item = queue.Value.Dequeue();

            if (queue.Value.Count == 0)
                _storage.Remove(queue.Key);

            Count--;
            return item;
        }

        public IEnumerator<KeyValuePair<TPriority, TItem>> GetEnumerator()
        {
            var items = from pair in _storage
                        from item in pair.Value
                        select new KeyValuePair<TPriority, TItem>(pair.Key, item);

            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        {
            var items = _storage.SelectMany(pair => pair.Value);
            return items.GetEnumerator();
        }
    }

    [Serializable()]
    internal class Map //: IDisposable
    {
        private static readonly Pen greenPen = new Pen(Color.Green, 5);
        private static readonly Pen redPen = new Pen(Color.Red, 5);

        public static readonly int MinMapSize = 7;          // минимальный размер карты 

        private int rows, cols;                             // игреки и иксы соответственно
        private Tile[,] tiles;                              // вся карта состоит из тайлов
        int[,] randoms;                                     // массив рандомных коэф, на которых строятся дороги
        [NonSerialized] private Random rand;                // вроде нужен будет для много чего 

        [NonSerialized] private Bitmap mapImage;            // хранит всю карту
        [NonSerialized] private Bitmap currentVisibleMap;   // хранит текущий отображемый кусок карты
        private Tile currentMapPointer;                             // по центру видимой карты
        public readonly int tileSize = 100;                 // размеры в пикселях одного тайла

        // похоже на временное решение
        private static Dictionary<Landscape, string> imagePaths = new Dictionary<Landscape, string>()
        {
             {Landscape.Road, @"..\..\Resources\MapObjects\road.png" } ,
             {Landscape.Water, @"..\..\Resources\MapObjects\water.png" } ,
             {Landscape.Forest, @"..\..\Resources\MapObjects\forest.png" } ,
             {Landscape.City, @"..\..\Resources\MapObjects\castle.png" } ,
        };
        // хранит базовые изображения, из которых клеится карта 
        [NonSerialized] private Dictionary<Landscape, Bitmap> tilesImages;

        public Bitmap CurrentVisibleMap { get { return currentVisibleMap; } }

        public Tile CurrentMapPointer { get { return currentMapPointer; } }

    // начало генерации карты
        public Map (int rows, int cols, List<Player> players, XmlDocument xmlDoc)
        {
            if (players.Count == 0)
                throw new ArgumentException("Player's amount mustn't be equal to 0");

            this.rows = rows;
            this.cols = cols;
             
            // инициализация вспомогательного массива
            randoms = new int[cols, rows];
            rand = new Random();
            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                    randoms[i, j] = rand.Next(10000);
            // начальная инициализация по умолчанию
            tiles = new Tile[cols, rows];
            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                    tiles[i, j] = new Tile(i, j);

            // расставляем города
            List<Tile> cities = CitiesStart(players, xmlDoc);

            // прокладываем дороги между городами
            ConnectCities(cities);

            // в оставшиеся незанятые клетки назначаем воду или лес
            WoodAndWater();

            // на этом этапе у нас готов бекэнд карты, приступаем к картинкам
            mapImage = new Bitmap(tileSize * cols, tileSize * rows);

            // заполняем словарь базовыми битмапамы
            LoadTileImages();

            // заполняем пустую mapImage мозаикой tileImages
            GluingPictures();

            currentVisibleMap = new Bitmap(tileSize * MinMapSize, tileSize * MinMapSize);

            // устанавливаем стартовую отображаемую часть карты = город первого игрока
            // last, потому что города расставлялись в обратном порядке
            MapShift(cities.Last());

            // корректировка пикчурбоксов на старте
            foreach (Player p in players)
                p.getWarlordByIndex(0).FixPictureBoxByNumber(currentMapPointer, MinMapSize);

        }

        private void LoadTileImages ()
        {
            // словарь содержит разные изображения-"кусочки" в зависимости от типа ландшафта
            tilesImages = new Dictionary<Landscape, Bitmap>();

            // загрузка изображений тайлов
            Bitmap temp;
            Landscape land;
            string[] landscapes = Enum.GetNames(typeof(Landscape));
            foreach (string landscape in landscapes)
            {
                if (landscape == "None")
                    continue;
                Enum.TryParse(landscape, out land);
                temp = (Bitmap)Bitmap.FromFile(imagePaths[land]);
                tilesImages.Add(land, temp);
            }
        }

        private void GluingPictures ()
        {
            // создаем графикс для глобальной карты
            using (Graphics g = Graphics.FromImage(mapImage))
            {
                // проходимся по каждому тайлу в мапе
                foreach (var tile in tiles)
                {
                    // рисуем на глобальной карте нужный кусочек в нужном месте
                    g.DrawImage
                    (
                        tilesImages[tile.Landscape],
                        new Rectangle(tile.X * tileSize, tile.Y * tileSize, tileSize, tileSize)
                    );
                }
            }  
        }

        // для нахождения кратчайших путей во взвешенной матрице
        private Dictionary<Tile, Tile> Dijkstra (Tile start, Tile goal)
        {
            PriorityQueue<int, Tile> frontier = new PriorityQueue<int, Tile>();
            frontier.Enqueue(0, start);
            Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
            cameFrom.Add(start, null);
            Dictionary<Tile, int> costSoFar = new Dictionary<Tile, int>();
            costSoFar.Add(start, 0);

            while (frontier.Count > 0)
            {
                Tile current = frontier.Dequeue();
                if (current == goal)
                    break;
                foreach (Tile next in Neighbors(current))
                {
                    int newCost = costSoFar[current] + randoms[next.X, next.Y];
                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        frontier.Enqueue(newCost, next);
                        cameFrom.Add(next, current);
                    }
                }
            }

            return cameFrom;
        }

        // для "развертывания" пути из Дийкстры
        private Stack<Tile> PathFinding(Tile start, Tile goal)
        {
            Dictionary<Tile, Tile> cameFrom = Dijkstra(start, goal);

            Stack<Tile> path = new Stack<Tile>();
            Tile current = goal;

            while (current != start)
            {
                path.Push(current);
                current = cameFrom[current];
            }

            return path;
        }

        private List<Tile> Neighbors (Tile tile)
        {
            List<Tile> neighbors = new List<Tile>();
            Point[] points = tile.Neighbors();
            foreach (Point point in points)
            {
                if (point.X >= 0 && point.Y >= 0 && point.X < cols && point.Y < rows)
                    neighbors.Add(tiles[point.X, point.Y]);
            }
            return neighbors;
        }

        private List<Tile> CitiesStart(List<Player> players, XmlDocument xmlDoc)
        {
            List<Tile> cities = new List<Tile>();
            int len = players.Count; // по городу на каждого игрока
            int range = 1;   // минимальное расстояние между городами, которое должно быть
            int i, j;
            bool possible;   // возможность поставить город в данной точке

            while (len > 0)
            {
                i = rand.Next(rows);
                j = rand.Next(cols);
                possible = true;

                foreach (var city in cities)
                    if (city == tiles[i, j] || !tiles[i, j].OutOfRange(city, range))
                    {
                        possible = false;
                        break;
                    }     
                
                if (possible)
                {
                    cities.Add(tiles[i, j]);
                    tiles[i, j].Landscape = Landscape.City;
                    // при создании стартового города перекрестные ссылки тайла на город и обратно 
                    // устанавливаются в конструкторе City
                    players[len - 1].StartCity(tiles[i, j]);
                    players[len - 1].CreateWarlord(players[len - 1].GetCityByTile(tiles[i, j]), xmlDoc);
                    len--;
                }
            }
            return cities;
        }

        private void ConnectCities (List<Tile> cities)
        {
            for (int i = 0; i < cities.Count; i++)
                for (int j = i + 1; j < cities.Count; j++)
                {
                    Stack<Tile> path = PathFinding(cities[i], cities[j]);
                    Tile tile;
                    while (path.Count > 1)
                    {
                        tile = path.Pop();
                        if (tile.Landscape != Landscape.City)
                            tile.Landscape = Landscape.Road;
                    }
                }
        }

        private void WoodAndWater ()
        {
            foreach (Tile tile in tiles)
                if (tile.Landscape == Landscape.None)
                    tile.Landscape = (rand.Next(100) >= 75) ? Landscape.Water : Landscape.Forest;
        }

    // конец генерации карты

        public void SetAnimationAfterDeserialization()
        {
            if (mapImage != null)
                return;

            // на этом этапе у нас готов бекэнд карты, приступаем к картинкам
            mapImage = new Bitmap(tileSize * cols, tileSize * rows);

            // заполняем словарь базовыми битмапами
            LoadTileImages();

            // заполняем пустую mapImage мозаикой tileImages
            GluingPictures();

            currentVisibleMap = new Bitmap(tileSize * MinMapSize, tileSize * MinMapSize);
        }

        public void ReLinking (List<Player> players)
        {
            foreach (Player p in players)
            {
                foreach (City city in p.cities)
                {
                    city.Position = tiles[city.Position.X, city.Position.Y];
                    city.Position.SetCity(city);
                }
                foreach (Warlord warlord in p.warlords)
                {
                    warlord.Position = tiles[warlord.Position.X, warlord.Position.Y];
                    warlord.Position.SetWarlord(warlord);
                } 
            }
        }

     // начало перемещения камеры по карте

        // корректирует currentMapPointer, чтобы не вышел за границы карты
        private bool OutOfMapRange ()
        {
            bool changed = true;
            int x = currentMapPointer.X, y = currentMapPointer.Y;
            // лево
            if (currentMapPointer.X - MinMapSize / 2 < 0)
                x += Math.Abs(currentMapPointer.X - MinMapSize / 2);
            // право
            if (currentMapPointer.X + MinMapSize / 2 >= cols)
                x -= Math.Abs(currentMapPointer.X + MinMapSize / 2 - cols + 1);
            // вверх
            if (currentMapPointer.Y - MinMapSize / 2 < 0)
                y += Math.Abs(currentMapPointer.Y - MinMapSize / 2); 
            // вниз
            if (currentMapPointer.Y + MinMapSize / 2 >= rows)
                y -= Math.Abs(currentMapPointer.Y + MinMapSize / 2 - rows + 1);

            if (x != currentMapPointer.X || y != currentMapPointer.Y)
                changed = false;
            currentMapPointer = tiles[x, y];
            return changed;
        }

        // по совместительству работает, как рефреш CurrentVisibleMap
        public bool MapShift (Tile center)
        {
            currentMapPointer = center;

            bool changed = OutOfMapRange();

            // создаем графикс для текущего куска карты
            using (Graphics g = Graphics.FromImage(currentVisibleMap))
            {
                // вырезаем из глобальной карты новый кусок
                g.DrawImage
                (
                    mapImage,
                    new Rectangle(0, 0, currentVisibleMap.Width, currentVisibleMap.Height),
                    new Rectangle((currentMapPointer.X - MinMapSize / 2) * tileSize, 
                                  (currentMapPointer.Y - MinMapSize / 2) * tileSize, 
                                  currentVisibleMap.Width, currentVisibleMap.Height),
                    GraphicsUnit.Pixel
                );
            }
            return changed;
        }

        public bool MapUp ()
        {
            return MapShift(tiles[currentMapPointer.X, currentMapPointer.Y - 1]);
        }

        public bool MapDown ()
        {
            return MapShift(tiles[currentMapPointer.X, currentMapPointer.Y + 1]);
        }

        public bool MapLeft ()
        {
            return MapShift(tiles[currentMapPointer.X - 1, currentMapPointer.Y]);
        }

        public bool MapRight ()
        {
            return MapShift(tiles[currentMapPointer.X + 1, currentMapPointer.Y]);
        }

     // конец перемещения камеры по карте

        // для считывания координат клика игрока
        public Tile PixelToTile (Point mouse)
        {
            int y = mouse.Y / tileSize + currentMapPointer.Y - MinMapSize / 2;
            int x = mouse.X / tileSize + currentMapPointer.X - MinMapSize / 2;
            
            return tiles[x, y];
        }

    // начало поиска пути для варлордов

        private Dictionary<Tile, Tile> BreadthFirstSearch (Tile start, Tile goal)
        {
            Queue<Tile> frontier = new Queue<Tile>();
            frontier.Enqueue(start);

            Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
            cameFrom.Add(start, null);

            while (frontier.Count > 0)
            {
                Tile current = frontier.Dequeue();

                if (current == goal)
                    break;

                foreach (Tile next in Neighbors(current))
                {
                    if (next.Landscape != Landscape.Road && next.Landscape != Landscape.City)
                        continue;

                    if (!cameFrom.ContainsKey(next))
                    {
                        frontier.Enqueue(next);
                        cameFrom.Add(next, current);
                    }
                }  
            }

            return cameFrom;
        }

        public Stack<Tile> WarlordPathFinding (Tile start, Tile goal)
        {
            Dictionary<Tile, Tile> cameFrom = BreadthFirstSearch(start, goal);

            Stack<Tile> path = new Stack<Tile>();
            Tile current = goal;

            while (current != start)
            {
                path.Push(current);
                current = cameFrom[current];
            }

            return path;
        }

    // конец поиска пути для варлордов

        public bool IsInside (Tile tile)
        {
            if (tile.X < currentMapPointer.X - MinMapSize / 2 || tile.X > currentMapPointer.X + MinMapSize / 2
                || tile.Y < currentMapPointer.Y - MinMapSize / 2 || tile.Y > currentMapPointer.Y + MinMapSize / 2)
                return false;
            return true;
        }

        // подсветка клеток (текущего города и варлорда)
        public void Highlight(Tile tile, bool passed = false)
        {
            using (Graphics g = Graphics.FromImage(mapImage))
            {
                if (passed)
                    g.DrawRectangle(redPen, 
                        new Rectangle(tile.X * tileSize + 2, tile.Y * tileSize + 2, tileSize - 5, tileSize - 5)
                        );
                else
                    g.DrawRectangle(greenPen,
                        new Rectangle(tile.X * tileSize + 2, tile.Y * tileSize + 2, tileSize - 5, tileSize - 5)
                        );
            }
        }

        public void DeHighlight(Tile tile)
        {
            using (Graphics g = Graphics.FromImage(mapImage))
            {
                g.DrawImage
                   (
                       tilesImages[tile.Landscape],
                       new Rectangle(tile.X * tileSize, tile.Y * tileSize, tileSize, tileSize)
                   );
            }
        }
    }
}
