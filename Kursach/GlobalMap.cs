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

namespace Kursach
{
    

    public partial class GlobalMap : Form
    {
        private MainMenu mainMenu;
        private SmallMenu smallMenua;    

        private Map map;

        private List<Player> players;
        private int currentPlayerIndex;
        private int currentWarlordIndex;
        private int currentCityIndex;
        private Player currentPlayer;
        private Warlord currentWarlord;
        private City currentCity;

        private Stack<Tile> warlordPath;
        private Tile partOfWarlordPath;

        private XmlDocument xmlDoc;

        private Dictionary<string, int> hiringUnits;

        public bool heroLooser;

        // если на пути враг
        private Tile enemyOnPath;

        // пустые вражеские города, которые герой захватывает по пути
        private Queue<City> capturedAlongWay;

        // стаф для драг энд дропа
        private bool Dragging;
        private int xPos;
        private int yPos;
        private Point delta = new Point(60, 36), offsetEven = new Point(8, 10), offsetOdd = new Point(38, 28);
        private List<PictureBox> unitStartPositions = new List<PictureBox>();
        private Bitmap SwordManIcon = new Bitmap(@"../../Resources/swordSmall.png");
        private Bitmap ArcherIcon = new Bitmap(@"../../Resources/bowSmall.png");
        private Bitmap HealerIcon = new Bitmap(@"../../Resources/crossSmall.png");

        internal GlobalMap (MainMenu mainMenu, 
                            Map map, List<Player> players, List<string> unitTypes,
                            XmlDocument xmlDoc, 
                            int currentPlayerIndex = 0, int currentWarlordIndex = 0, int currentCityIndex = 0)
        {
            InitializeComponent();

            timer1.Interval = Animation.TIMER_INTERVAL;

            this.mainMenu = mainMenu;
            this.map = map;
            mapContainer.Size = new Size(700, 700);
            mapContainer.Location = new Point(this.Width / 2 - mapContainer.Width / 2, 0);            
            mapContainer.Image = map.CurrentVisibleMap;
            this.players = players;
            this.xmlDoc = xmlDoc;
            this.currentPlayerIndex = currentPlayerIndex;
            this.currentWarlordIndex = currentWarlordIndex;
            this.currentCityIndex = currentCityIndex;

            hiringUnits = new Dictionary<string, int>();
            // заполняем словарь найма юнитов по умолчанию
            foreach (string unitType in unitTypes)
                hiringUnits.Add(unitType, 0);

            // добавляем пикчурбоксы на форму
            foreach (Player p in players)
            {
                p.getWarlordByIndex(currentWarlordIndex).PictureBox.MouseDoubleClick += new MouseEventHandler(this.warlord_MouseDoubleClick);
                // можно попробовать не добавлять / удалять, а прятать / показывать
                if (map.IsInside(p.getWarlordByIndex(currentWarlordIndex).Position))
                    mapContainer.Controls.Add(p.getWarlordByIndex(currentWarlordIndex).PictureBox);
            }

            // расстановка начальных "указателей"
            currentPlayer = players[currentPlayerIndex];
            if (!currentPlayer.WithoutWarlords())
            {
                currentWarlord = currentPlayer.getWarlordByIndex(currentWarlordIndex);
                // подсветка первого варлорда
                map.Highlight(currentWarlord.Position);
            }
            else
                currentWarlord = null;

            if (!currentPlayer.WithoutCities())
            {
                currentCity = currentPlayer.getCityByIndex(currentCityIndex);
                mineLevelLabel.Text = currentCity.MineLevel.ToString();
                // подсветка первого варлорда
                map.Highlight(currentCity.Position);
            }
            else
                currentCity = null;

            FixUnitStartPositionPictureBox();

            // обновляем currentVisibleMap
            map.MapShift(map.CurrentMapPointer);

            smallMenua = new SmallMenu(mainMenu, map, players, unitTypes, currentPlayerIndex, currentCityIndex, currentWarlordIndex);

            capturedAlongWay = new Queue<City>();
        }

        // двойной щелчок по мэп контейнеру двигает героя к курсору
        // а как тогда в город войти??? А никак, он на той же форме, а не на отдельной
        private void mapContainer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (currentPlayer.WithoutWarlords())
                return;
            Tile userChoice = map.PixelToTile(e.Location);
            StartOfTurn(userChoice, e);
        }

        // если кликнули по пикчурбоксу героя, то нужно скакать к нему
        private void warlord_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (currentPlayer.WithoutWarlords())
                return;
            Tile currentWarlordPosition = null;
            PictureBox pic = sender as PictureBox;
            foreach (Player p in players)
                foreach (Warlord w in p.warlords)
                    if (pic == w.PictureBox)
                    {
                        currentWarlordPosition = w.Position;
                    }
            if (currentWarlordPosition == null)
                return;
            StartOfTurn(currentWarlordPosition, e);
        }

        private void StartOfTurn(Tile userChoice, MouseEventArgs e)
        {
            // если нажали на клетку воды
            if (userChoice.Landscape == Landscape.Water)
            {
                gameInfoLabel.Text = "Воевода не может плавать(";
                return;
            }
            // если нажали на клетку леса
            if (userChoice.Landscape == Landscape.Forest)
            {
                gameInfoLabel.Text = "Конь в лесу не пройдет";
                return;
            }
            // если нажали на ту же клетку, где стоит текущий варлорд
            if (userChoice == currentWarlord.Position)
                return;
            // если пытаемся встать на клетку, где уже есть союзный варлорд
            if (userChoice.Warlord != null)
                if (currentPlayer.IsWarlordInArmy(userChoice.Warlord))
                {
                    gameInfoLabel.Text = "Вдвоем на одной клетке нельзя";
                    return;
                }
            // если у варлорда уже не осталось очков передвижения
            if (currentWarlord.IsPassed())
            {
                gameInfoLabel.Text = "На этом ходу конь уже устал";
                return;
            }
            // если нажали на клетку врага, но у обоих нет армии, то можно тоже ретернать

            // получаем путь для варлорда
            warlordPath = map.WarlordPathFinding(currentWarlord.Position, userChoice);

            if (warlordPath == null)
                throw new ArgumentNullException();

            // по умолчанию на пути нет врагов
            enemyOnPath = null;
            int warlordMoveRange = 0;
            // проверяем путь на наличие противников и вражеских свободных городов
            // которые герой захватит по пути
            foreach (Tile tile in warlordPath)
            {
                // если на клетке пути есть КАКОЙ-ТО варлорд
                if (tile.Warlord != null)
                {
                    // если этот варлорд НЕ принадлежит игроку, значит, он враг
                    if (!currentPlayer.IsWarlordInArmy(tile.Warlord))
                    {
                        // помечаем клетку, где находится враг,
                        // перед ней начнется бой
                        enemyOnPath = tile;
                        // выходим из цикла, так как герйо остановится на клетке после боя
                        break;
                    }
                }
                // если на клетке пути есть КАКОЙ-ТО город
                if (tile.Landscape == Landscape.City)
                {
                    // если этот город НЕ принадлежит игроку
                    if (!currentPlayer.IsCityInPlayer(tile.City))
                    {
                        // добавляем его в список захваченных по пути городов
                        // в нем точно нет вражеского варлорда, ибо на это есть if выше
                        // если в радиусе ходьбы варлорда на ЭТОМ ходу
                        if (warlordMoveRange <= currentWarlord.MoveRange)
                            capturedAlongWay.Enqueue(tile.City);
                    }
                }
                warlordMoveRange++;
            }
            // смело прибавляем города из capturedAlongWay к варлорду после всех активностей
            // только если он не умер

            // подавляем регистрацию нажатий сдвига карты, чтобы не сломать анимацию
            KeyPreview = false;

            // стираем зеленую рамку на клетке, где находится варлорд, потому что он вот-вот с нею уйдет
            map.DeHighlight(currentWarlord.Position);
            // обновляем видимую карту
            map.MapShift(map.CurrentMapPointer);

            // извлекаем первый соседний с варлордом тайл пути
            partOfWarlordPath = warlordPath.Pop();
            // если это тот самый тайл, на котором находится враг
            if (partOfWarlordPath == enemyOnPath)
                // то запускаем бой
                if(Struggle(enemyOnPath.Warlord))
                {
                    // и если он случился
                    // в случае поражения текущего игрока подсвечиваем следующего его варлорда
                    // иначе просто подсвечиваем его, если он победил
                    if (currentWarlord != null)
                        map.Highlight(currentWarlord.Position, currentWarlord.IsPassed());
                    // карту не сдвигаем с места собитый
                    map.MapShift(map.CurrentMapPointer);
                    KeyPreview = true;
                    return;
                }
            // потенциальная опасность: если на пути несколько врагов, и первый из них без армии
            // то его проскачут, а на следующих реагировать не будут
            // возможное решение: уничтожать героя без армии вражеского и по обычной схеме останавливать
            // движение
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // делаем шажочек в направлении очередного тайла
            bool finish = currentWarlord.Step(partOfWarlordPath);
            
            // если анимация закончилась, значит, мы прошли одну клетку
            if (finish)
            {
                timer1.Stop();

                bool endOfPath = false, battleHappened = false;

                // если текущий тайл пути - это свободный город
                if (capturedAlongWay.Count > 0)
                    if (partOfWarlordPath == capturedAlongWay.Peek().Position)
                        CaptureCity(capturedAlongWay.Dequeue());

                // если еще есть клетки пути
                if (warlordPath.Count > 0)
                    // извлекаем очередной соседний с варлордом тайл пути
                    partOfWarlordPath = warlordPath.Pop();
                // иначе конец пути и надо остановить таймер
                else
                    endOfPath = true;

                // если это тот самый тайл, на котором находится враг
                if (partOfWarlordPath == enemyOnPath)
                    battleHappened = Struggle(enemyOnPath.Warlord);

                if (endOfPath || battleHappened)
                {
                    // в случае поражения текущего игрока подсвечиваем следующего его варлорда
                    // иначе просто подсвечиваем его, если он победил
                    if (currentWarlord != null)
                        map.Highlight(currentWarlord.Position, currentWarlord.IsPassed());
                    // карту не сдвигаем с места собитый
                    map.MapShift(map.CurrentMapPointer);
                    KeyPreview = true;
                }
                else
                    timer1.Start();
            }
            mapContainer.Invalidate();
        }

        // инкапсулирует все сражение в себя
        private bool Struggle (Warlord enemy)
        {
            // если армии нет у текущего варлорда
            if (currentWarlord.WithoutArmy())
            {
                heroLooser = true;
            }                      

            // если армии нет у врага
            if (enemy.WithoutArmy())
            {
                heroLooser = false;
            }            

            // если все-таки у обоих варлордов оказались юниты
            if (!currentWarlord.WithoutArmy() && !enemy.WithoutArmy())
            {
                // внутри батлформы изменится heroLooser в зависимости от того, кто выиграл
                Battle b = new Battle(currentWarlord, enemy);
                b.Owner = this;
                b.ShowDialog();
            }
            
            // если проиграл текущий варлорд
            if (heroLooser)
            {
                // заранее отвязываем пикчурбокс варлорда от мапы
                // иначе ссылка на пикурбокс сохранится в мапе после удаления
                mapContainer.Controls.Remove(currentWarlord.PictureBox);
                // перед удалением корректируем индекс текущего варлорда
                if (currentWarlordIndex == currentPlayer.AmountOfWarlords() - 1)
                {
                    currentWarlordIndex = 0;
                }
                // удаляем у текущего игрока его текущего варлорда
                currentPlayer.RemoveWarlord(currentWarlord);
                // если это был последний его варлорд
                if (currentPlayer.WithoutWarlords())
                {
                    // тогда текущего варлорда больше нет
                    currentWarlord = null;
                }
                // иначе устанавливаем нового текущего варлорда 
                else
                {
                    currentWarlord = currentPlayer.getWarlordByIndex(currentWarlordIndex);
                }
                gameInfoLabel.Text = "Ваш герой повержен, а его армия разбита";
            }
            // если проиграл враг
            else
            {
                // заранее отвязываем пикчурбокс варлорда от мапы
                // иначе ссылка на пикурбокс сохранится в мапе после удаления
                mapContainer.Controls.Remove(enemy.PictureBox);
                // ищем, какому игроку принадлежит данный варлорд
                foreach (Player p in players)
                {
                    if (p.IsWarlordInArmy(enemy))
                        p.RemoveWarlord(enemy);
                }
                gameInfoLabel.Text = "Вы блестяще разгромили войско врага";
            }
            // корректируем пикчурбоксы драг энд дроп окошка после боя
            // если чел проиграл текущий, то там должно быть пусто
            //если выиграл, то меньше картинок, если юниты погибли
            FixUnitStartPositionPictureBox();
            // возвращаем true, так как кто-то точно проиграл
            return true;
        }

        // текущий игрок захватывает город
        private void CaptureCity (City city)
        {
            if (city == null)
                throw new ArgumentNullException();
            currentPlayer.AddCity(city);
            // ищем, кому принадлежит город
            foreach (Player player in players)
                if (player.IsCityInPlayer(city) && player != currentPlayer)
                    player.RemoveCity(city);
            gameInfoLabel.Text = "Вы успешно захватили город: " + city.Name;
        }

        private void smallMenu_Click(object sender, EventArgs e)
        {
            smallMenua.ShowDialog();
        }

        // перемещение карты по wasd
        private void GlobalMap_KeyDown(object sender, KeyEventArgs e)
        {
            bool changed = true;
            switch (e.KeyCode)
            {
                case Keys.W:
                    changed = map.MapUp();
                    break;
                case Keys.S:
                    changed = map.MapDown();
                    break;
                case Keys.A:
                    changed = map.MapLeft();
                    break;
                case Keys.D:
                    changed = map.MapRight();
                    break;
            }
            FixHorses(changed);
            mapContainer.Invalidate();
        }

        // корректируем позиции всех пикчурбоксов после сдвига камеры
        private void FixHorses (bool changed)
        {
            foreach (Player p in players)
            {
                foreach (Warlord w in p.warlords)
                {
                    // фиксим почти всегда, за исключением случаев, когда карта 
                    // уперлась в край или нанят новый герой
                    if (changed)
                        w.FixPictureBoxByNumber(map.CurrentMapPointer, Map.MinMapSize);
                    // прячем пикчурбоксы, которые вышли за пределы видимой карты
                    if (!map.IsInside(w.Position))
                    {
                        mapContainer.Controls.Remove(w.PictureBox);
                    }
                    else
                    {
                        mapContainer.Controls.Add(w.PictureBox);
                    }
                }
            }   
        }

        private void endTurn_Click(object sender, EventArgs e)
        {
            gameInfoLabel.Text = "";

            // если игрок проиграл
            if (currentPlayer.IsLost())
            {
                // перед удалением корректируем индекс текущего игрока
                if (currentPlayerIndex == players.Count - 1)
                {
                    currentPlayerIndex = 0;
                }
                // удаляем текущего игрока из списка игроков
                players.Remove(currentPlayer);

                // устанавливаем нового текущего игрока 
                currentPlayer = players[currentPlayerIndex];

                string message = "Жаль, но вы проиграли";
                string caption = "Поражение";
                MessageBox.Show(message, caption, MessageBoxButtons.OK);
            }

            bool over = true;
            Player temp = null;
            int i = 0;
            // проверяем список игроков на проигравших
            while (over)
            {
                temp = players[i];
                if (temp.IsLost())
                {
                    string message = "Очередной соперник повержен";
                    string caption = "Победа!";
                    MessageBox.Show(message, caption, MessageBoxButtons.OK);

                    players.Remove(temp);
                    i--;
                }
                i++;
                if (i > players.Count - 1)
                    over = false;
            }

            // запускаем окно уведомляюще о победе
            if (players.Count == 1 && !currentPlayer.IsLost())
            {
                string message = "Полная и окончательная победа, ура, товарищ командир!";
                string caption = "Победа";
                MessageBox.Show(message, caption, MessageBoxButtons.OK);
                // прикрываем всю эту лавочку наконец
                mainMenu.Close();
                this.Close();
                return;
            }

            if (!currentPlayer.WithoutCities())
            {
                // пополняем казну ходившего игрока за все его города 
                currentPlayer.CollectTribute();
                // перестаем помечать последний его выбранный город
                map.DeHighlight(currentCity.Position);
            }

            if (!currentPlayer.WithoutWarlords())
            {
                // у всех его варлордов обновляем дистанцию ходьбы
                foreach (Warlord w in currentPlayer.warlords)
                {
                    w.ResetPassed();
                }
                // перестаем помечать последнего его выбранного варлорда 
                map.DeHighlight(currentWarlord.Position);
            }

            // увеличиваем индекс текущего игрока на 1
            currentPlayerIndex++;
            currentPlayerIndex %= players.Count;
            // переставляем "указатель" на нового игрока по индексу
            currentPlayer = players[currentPlayerIndex];

            // обнуляем индексы текущего города и варлорда, ибо у след игрока может быть меньше
            // оных
            // переставляем "указатель" на нового варлорда и город по индексу
            currentCityIndex = 0;
            if (currentPlayer.WithoutCities())
            {
                currentCity = null;
            }
            else
            {
                currentCity = currentPlayer.getCityByIndex(currentCityIndex);
                mineLevelLabel.Text = currentCity.MineLevel.ToString();
                map.Highlight(currentCity.Position);
            }

            currentWarlordIndex = 0;
            if (currentPlayer.WithoutWarlords())
            {
                currentWarlord = null;
            }
            else
            {
                currentWarlord = currentPlayer.getWarlordByIndex(currentWarlordIndex);
                map.Highlight(currentWarlord.Position);
            }

            // фиксим драг энд дроп окошко с юнитами
            FixUnitStartPositionPictureBox();

            // обновляем видимую карту после изменений
            if (currentWarlord != null)
                map.MapShift(currentWarlord.Position);
            else
                map.MapShift(currentCity.Position);
            FixHorses(true);

            goldInfo.Text = currentPlayer.Gold.ToString();

            gameInfoLabel.Text = "";

            mapContainer.Invalidate();
        }

        private void nextWarlord_Click(object sender, EventArgs e)
        {
            // если у игрока нет варлордов
            if (currentWarlord == null)
            {
                gameInfoLabel.Text = "Игрок остался без воевод!\n" +
                                     "Наймите героя в каком-нибудь городе";
                return;
            }

            map.DeHighlight(currentWarlord.Position);
            
            currentWarlordIndex++;
            currentWarlordIndex %= currentPlayer.AmountOfWarlords();
            currentWarlord = currentPlayer.getWarlordByIndex(currentWarlordIndex);

            map.Highlight(currentWarlord.Position, currentWarlord.IsPassed());

            // центрируем камеру на новом варлорде и опять пересчитываем ВСЕ((( пикчурбоксы
            map.MapShift(currentWarlord.Position);
            FixHorses(true);

            // ставим по умолчанию стартовые позиции юнитов на плашке инфа о герое
            FixUnitStartPositionPictureBox();

            mapContainer.Invalidate();
        }

        // расставляет по уже заданным позициям юнитов, стартовые дефолтные позиции назначаются в hiringUnits()
        private void FixUnitStartPositionPictureBox ()
        {
            // сначала очищаем от предыдущего варлорда
            if (unitStartPositions.Count != 0)
            {
                foreach (PictureBox position in unitStartPositions)
                    unitStartPositionPictureBox.Controls.Remove(position);
                unitStartPositions.Clear();
            }

            if (currentWarlord == null)
            {
                return;
            }

            foreach (Unit unit in currentWarlord.army)
            {
                if (unit.IsDead())
                    continue;
                Point posCoord = unit.PositionCoords;
                int yPic = posCoord.Y;
                int xPic = Math.Abs(posCoord.X - 2);
                switch (unit.Type)
                {
                    case "SwordMan":
                        unitStartPositions.Add(CreateUnitStartPositionPictureBox(SwordManIcon, yPic, xPic));
                        break;
                    case "Archer":
                        unitStartPositions.Add(CreateUnitStartPositionPictureBox(ArcherIcon, yPic, xPic));
                        break;
                    case "Healer":
                        unitStartPositions.Add(CreateUnitStartPositionPictureBox(HealerIcon, yPic, xPic));
                        break;
                }
            }
        }

        private PictureBox CreateUnitStartPositionPictureBox (Bitmap unitType, int x, int y)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(26, 26);
            int xPx, yPx;
            if (x % 2 == 0)
            {
                x /= 2;
                xPx = offsetEven.X + delta.X * x;
                yPx = offsetEven.Y + delta.Y * y;
            }
            else
            {
                x /= 2;
                xPx = offsetOdd.X + delta.X * x;
                yPx = offsetOdd.Y + delta.Y * y;
            }
            pictureBox.Location = new Point(xPx, yPx);
            pictureBox.BackColor = Color.Transparent;
            pictureBox.Image = unitType;
            // добавляем одинаковые обработчики событий для каждого новго пикчурбокса
            pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            unitStartPositionPictureBox.Controls.Add(pictureBox);
            return pictureBox;
        }

        private void previousWarlord_Click(object sender, EventArgs e)
        {
            // если у игрока нет варлордов
            if (currentWarlord == null)
            {
                gameInfoLabel.Text = "Игрок остался без воевод!\n" +
                                     "Наймите героя в каком-нибудь городе";
                return;
            }

            map.DeHighlight(currentWarlord.Position);

            currentWarlordIndex--;
            // закольцовывваем
            if (currentWarlordIndex < 0)
                currentWarlordIndex = currentPlayer.AmountOfWarlords() - 1;
            currentWarlord = currentPlayer.getWarlordByIndex(currentWarlordIndex);

            map.Highlight(currentWarlord.Position, currentWarlord.IsPassed());

            // центрируем камеру на новом варлорде и опять пересчитываем ВСЕ((( пикчурбоксы
            map.MapShift(currentWarlord.Position);
            FixHorses(true);

            FixUnitStartPositionPictureBox();

            mapContainer.Invalidate();
        }

        private void hireButton_Click(object sender, EventArgs e)
        {
            // если у игрока нет городов
            if (currentWarlord == null)
            {
                gameInfoLabel.Text = "Игрок остался без воевод!\n" +
                                     "Наймите героя в каком-нибудь городе,\n" +
                                     "чтобы нанять новых юнитов";
                return;
            }

            Warlord w = currentPlayer.GetWarlordByTile(currentCity.Position);
            // если в городе, в котором нанимают, нет героя
            if (w == null)
            {
                gameInfoLabel.Text = "В текущем городе отсутствуют воеводы\n" +
                                     "Чтобы нанять войска, подведите героя в этот город";
                return;
            }

            int wasted;
            if (!currentPlayer.CanHireUnits(hiringUnits, out wasted))
            {
                gameInfoLabel.Text = "Нужно больше золота";
                return;
            }
            else
            {
                goldInfo.Text = currentPlayer.Gold.ToString(); 
                gameInfoLabel.Text = $"Вы потратили {wasted}";
            }
                
            w.AddUnits(hiringUnits, xmlDoc);

            FixUnitStartPositionPictureBox();
        }

        private void nextCity_Click(object sender, EventArgs e)
        {
            // если у игрока нет городов
            if (currentCity == null)
            {
                gameInfoLabel.Text = "Герой остался без городов!\n" +
                                     "Захватите какой-нибудь город,\n";
                return;
            }

            map.DeHighlight(currentCity.Position);

            currentCityIndex++;
            currentCityIndex %= currentPlayer.AmountOfCities();
            currentCity = currentPlayer.getCityByIndex(currentCityIndex);

            map.Highlight(currentCity.Position);

            // центрируем камеру на новом городе и опять пересчитываем ВСЕ((( пикчурбоксы
            map.MapShift(currentCity.Position);
            FixHorses(true);

            // устанавливаем уровень шахты текущего города в окне города
            mineLevelLabel.Text = currentCity.MineLevel.ToString();

            mapContainer.Invalidate();
        }

        private void previousCity_Click(object sender, EventArgs e)
        {
            // если у игрока нет городов
            if (currentCity == null)
            {
                gameInfoLabel.Text = "Герой остался без городов!\n" +
                                     "Захватите какой-нибудь город,\n";
                return;
            }

            map.DeHighlight(currentCity.Position);

            currentCityIndex--;
            if (currentCityIndex < 0)
                currentCityIndex = currentPlayer.cities.Count - 1;
            currentCity = currentPlayer.getCityByIndex(currentCityIndex);

            map.Highlight(currentCity.Position);

            // центрируем камеру на новом городе и опять пересчитываем ВСЕ((( пикчурбоксы
            map.MapShift(currentCity.Position);
            FixHorses(true);


            // устанавливаем уровень шахты текущего города в окне города
            mineLevelLabel.Text = currentCity.MineLevel.ToString();

            mapContainer.Invalidate();
        }

        private void mineImprove_Click(object sender, EventArgs e)
        {
            if (currentCity == null)
            {
                gameInfoLabel.Text = "Герой остался без городов!\n" +
                                     "Захватите какой-нибудь город,\n";
                return;
            }

            int wasted;
            if (!currentPlayer.MineLevelIncrease(currentCity, out wasted))
            {
                gameInfoLabel.Text = "Недостаточно золота для улучшения\n" +
                                     $"Необходимо {currentCity.CostOfMineImprovement}";
                return;
            }
            else
            {
                gameInfoLabel.Text = $"Вы потратили {wasted}";
                goldInfo.Text = currentPlayer.Gold.ToString();
            }
            
            mineLevelLabel.Text = currentCity.MineLevel.ToString();
        }

        private void SwordMan_ValueChanged(object sender, EventArgs e)
        {
             hiringUnits[SwordMan.Name] = (int)SwordMan.Value;
        }

        private void Archer_ValueChanged(object sender, EventArgs e)
        {
            hiringUnits[Archer.Name] = (int)Archer.Value;
        }

        private void Healer_ValueChanged(object sender, EventArgs e)
        {
            hiringUnits[Healer.Name] = (int)Healer.Value;
        }

        private void hireHeroButton_Click(object sender, EventArgs e)
        {
            if (currentCity == null)
            {
                gameInfoLabel.Text = "Герой остался без городов!\n" +
                                     "Захватите какой-нибудь город\n" +
                                     "Чтобы нанимать новых героев";
                return;
            }

            if (currentPlayer.WithoutWarlords())
            {
                currentWarlordIndex = 0;
                currentPlayer.CreateWarlord(currentCity, xmlDoc);
                currentWarlord = currentPlayer.getWarlordByIndex(currentWarlordIndex);
                map.Highlight(currentWarlord.Position);
            }
            else
                currentPlayer.CreateWarlord(currentCity, xmlDoc);


            // корректируем пикчурбокс новоиспеченного героя
            currentPlayer.GetWarlordByTile(currentCity.Position).FixPictureBoxByNumber(map.CurrentMapPointer, Map.MinMapSize);
            currentPlayer.GetWarlordByTile(currentCity.Position).PictureBox.MouseDoubleClick += new MouseEventHandler(this.warlord_MouseDoubleClick);
            FixHorses(true);
            mapContainer.Invalidate();
        }

    // Drag and Drop для назначения стартовой позиции юнитов
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // в данном случае sender - это пикчурбокс по идее
            Control c = sender as Control;
            if (Dragging && c != null)
            {
                // оч странно работает, но прикольно
                // по идее e.X и e.Y всегда должны быть одинаковыми
                // т к это координаты внутри пикчурбокса, но они "дрожат" на единичку туда
                // сюда при перетаскивании мышкой и за счет этого дрожания обеспечивается 
                // сдвиг
                int y = e.Y + c.Top - yPos, x = e.X + c.Left - xPos;

                if (y < unitStartPositionPictureBox.Top ||
                    y + c.Height > unitStartPositionPictureBox.Top + unitStartPositionPictureBox.Height ||
                    x < unitStartPositionPictureBox.Left ||
                    x + c.Width > unitStartPositionPictureBox.Left + unitStartPositionPictureBox.Width)
                    return;

                c.Top = y;
                c.Left = x;
            }
        }

        private void GlobalMap_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            // здесь проверка на ближайший гекс и "прилипание к нему"
            int xCol = unitStartPositionPictureBox.Width / 10;  // 31,2
            int yRow = unitStartPositionPictureBox.Height / 3;  // 43
            // строка и столбец
            int x = (c.Left - unitStartPositionPictureBox.Left) / xCol;
            int y = (c.Top - unitStartPositionPictureBox.Top) / yRow;

            int yPic = Math.Abs(y - 2);
            if (((PictureBox)c).Image == SwordManIcon)
                currentWarlord.GetUnitByType("SwordMan").PositionCoords = new Point(yPic, x);
            else if (((PictureBox)c).Image == ArcherIcon)
                currentWarlord.GetUnitByType("Archer").PositionCoords = new Point(yPic, x);
            else
                currentWarlord.GetUnitByType("Healer").PositionCoords = new Point(yPic, x);

            int xPx, yPx;
            if (x % 2 == 0)
            {
                x /= 2;
                xPx = offsetEven.X + delta.X * x;
                yPx = offsetEven.Y + delta.Y * y;
            }
            else
            {
                x /= 2;
                xPx = offsetOdd.X + delta.X * x;
                yPx = offsetOdd.Y + delta.Y * y;
            }
            c.Location = new Point(xPx, yPx);
            Dragging = false;
            unitStartPositionPictureBox.Invalidate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Dragging = true;
                xPos = e.X;
                yPos = e.Y;
            }
        }    
    }
}
