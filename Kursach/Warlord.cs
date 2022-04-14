using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;

namespace Kursach
{
    // класс для военачальника гг и противника
    [Serializable()]
    internal /*abstract*/ class Warlord
    {
        private static readonly Pen greenPen = new Pen(Color.Green, 8);

        private string name, race;

        public List<Unit> army;

        private int moveRange, passed = 0;

        private Tile position;

        private PictureData pictureData;

        [NonSerialized] private Animation moveLeft;
        [NonSerialized] private Animation moveRight;
        [NonSerialized] private Animation moveUp;
        [NonSerialized] private Animation moveDown;

        // состояние героя в покое
        [NonSerialized] private Bitmap defoltLeftCadr;
        [NonSerialized] private Bitmap defoltRightCadr;
        [NonSerialized] private Bitmap defoltUpCadr;
        [NonSerialized] private Bitmap defoltDownCadr;
        private static readonly int defoltWidth = 100, defoltHeight = 100, frames = 8;

        [NonSerialized] public PictureBox pictureBox;

        private TileDirection orientation;

        public string Name { get { return name; } }

        public Tile Position { get { return position; } set { position = value; } }

        public PictureBox PictureBox { get { return pictureBox; } }

        public int MoveRange { get { return moveRange; } }

        public Warlord (string race, Tile position, XmlDocument xmlDoc)
        {
            name = "Генерал Шлёпа";
            this.race = race;
            this.position = position;
            position.Warlord = this;

            army = new List<Unit>();

            pictureData = new PictureData(defoltWidth, defoltHeight, frames);

            orientation = TileDirection.Down;

            // получим корневой элемент, то есть <objects></objects>
            XmlElement xmlRoot = xmlDoc.DocumentElement;
            if (xmlRoot != null)
            {
                // проходимся по всем <race></race>
                foreach (XmlNode raceSearch in xmlRoot.ChildNodes)
                {
                    XmlNode raceName = raceSearch.Attributes.GetNamedItem("name");

                    if (raceName.Value != race)
                        continue;

                    // проходимся по всем <warlord></warlord>
                    foreach (XmlNode unit in raceSearch.ChildNodes)
                    {
                        if (unit.Name != "warlord")
                            continue;

                        // проходимся по всем харкам варлорда
                        foreach (XmlNode property in unit.ChildNodes)
                        {
                            if (property.Name == "moveRange")
                                moveRange = Convert.ToInt32(property.InnerText);
                            if (property.Name == "defoltLeftCadr")
                                defoltLeftCadr = new Bitmap(property.InnerText);
                            if (property.Name == "defoltRightCadr")
                                defoltRightCadr = new Bitmap(property.InnerText);
                            if (property.Name == "defoltUpCadr")
                                defoltUpCadr = new Bitmap(property.InnerText);
                            if (property.Name == "defoltDownCadr")
                                defoltDownCadr = new Bitmap(property.InnerText);
                            if (property.Name == "moveLeft")
                                moveLeft = new Animation(property.InnerText, pictureData);
                            if (property.Name == "moveRight")
                                moveRight = new Animation(property.InnerText, pictureData);
                            if (property.Name == "moveUp")
                                moveUp = new Animation(property.InnerText, pictureData);
                            if (property.Name == "moveDown")
                                moveDown = new Animation(property.InnerText, pictureData);
                        }
                    }
                }
            }

            Highlight(moveUp);
            Highlight(moveDown);
            Highlight(moveLeft);
            Highlight(moveRight);

            pictureBox = new PictureBox();
            pictureBox.BackColor = Color.Transparent;
            pictureBox.Size = new Size(defoltWidth, defoltHeight);
            pictureBox.Location = new Point(position.X * defoltWidth, position.Y * defoltHeight);
            pictureBox.Image = defoltDownCadr;
        }

        public void SetAnimationAfterDeserialization (XmlDocument xmlDoc)
        {
            if (pictureBox != null)
                return;

            // получим корневой элемент, то есть <objects></objects>
            XmlElement xmlRoot = xmlDoc.DocumentElement;
            if (xmlRoot != null)
            {
                // проходимся по всем <race></race>
                foreach (XmlNode raceSearch in xmlRoot.ChildNodes)
                {
                    XmlNode raceName = raceSearch.Attributes.GetNamedItem("name");

                    if (raceName.Value != race)
                        continue;

                    // проходимся по всем <warlord></warlord>
                    foreach (XmlNode unit in raceSearch.ChildNodes)
                    {
                        if (unit.Name != "warlord")
                            continue;

                        // проходимся по всем харкам варлорда
                        foreach (XmlNode property in unit.ChildNodes)
                        {
                            if (property.Name == "defoltLeftCadr")
                                defoltLeftCadr = new Bitmap(property.InnerText);
                            if (property.Name == "defoltRightCadr")
                                defoltRightCadr = new Bitmap(property.InnerText);
                            if (property.Name == "defoltUpCadr")
                                defoltUpCadr = new Bitmap(property.InnerText);
                            if (property.Name == "defoltDownCadr")
                                defoltDownCadr = new Bitmap(property.InnerText);
                            if (property.Name == "moveLeft")
                                moveLeft = new Animation(property.InnerText, pictureData);
                            if (property.Name == "moveRight")
                                moveRight = new Animation(property.InnerText, pictureData);
                            if (property.Name == "moveUp")
                                moveUp = new Animation(property.InnerText, pictureData);
                            if (property.Name == "moveDown")
                                moveDown = new Animation(property.InnerText, pictureData);
                        }
                    }
                }
            }

            Highlight(moveUp);
            Highlight(moveDown);
            Highlight(moveLeft);
            Highlight(moveRight);

            pictureBox = new PictureBox();
            pictureBox.BackColor = Color.Transparent;
            pictureBox.Size = new Size(defoltWidth, defoltHeight);
            pictureBox.Location = new Point(position.X * defoltWidth, position.Y * defoltHeight);
            pictureBox.Image = defoltDownCadr;

            // теперь вызываем аналогичную функцию для всех юнитов
            foreach (Unit unit in army)
            {
                unit.SetAnimationAfterDeserialization(xmlDoc);
            }
        }

        public void MakeNullPositionRefToWarlord () { position.Warlord = null; }

        // передвижение по глобальной карте
        public bool Step (Tile goal)
        {
            if (pictureBox == null || position == null)
                throw new ArgumentNullException();

            if (passed == moveRange)
            {
                return true;
            }

            bool finish = false;

            // высчитываем смещение для pictureBox
            int x = 0, y = 0, del = 9;
            // шаг вправо
            if (goal.X > position.X)
            {
                x = defoltWidth / del;
                moveRight.NextImage(pictureBox);
                if (moveRight.Finish())
                {
                    x = 50;
                    finish = true;
                    pictureBox.Image = defoltRightCadr;
                }
            }
            // шаг влево
            if (goal.X < position.X)
            {
                x = -defoltWidth / del;
                moveLeft.NextImage(pictureBox);
                if (moveLeft.Finish())
                {
                    finish = true;
                    pictureBox.Image = defoltLeftCadr;
                }
            }
            // шаг вниз
            if (goal.Y > position.Y)
            {
                y = defoltHeight / del;
                moveDown.NextImage(pictureBox);
                if (moveDown.Finish())
                {
                    y = 50;
                    finish = true;
                    pictureBox.Image = defoltDownCadr;
                }
            }
            // шаг вверх
            if (goal.Y < position.Y)
            {
                y = -defoltHeight / del;
                moveUp.NextImage(pictureBox);
                if (moveUp.Finish())
                {
                    finish = true;
                    pictureBox.Image = defoltUpCadr;
                }
            }

            if (!finish)
            {
                pictureBox.Location = new Point(pictureBox.Location.X + x, pictureBox.Location.Y + y);
            }
            else
            {
                passed++;
                orientation = position.Orientation(goal);
                position.Warlord = null;
                position = goal;
                position.Warlord = this;
                pictureBox.Location = new Point((pictureBox.Location.X + Math.Abs(x)) / defoltWidth * defoltWidth,
                                                (pictureBox.Location.Y + Math.Abs(y)) / defoltHeight * defoltHeight);
            }

            return finish;
        }

        // задает новые координаты для пикчурбокса героя в зависимости от текущего сдвига карты
        public void FixPictureBoxByNumber (Tile center, int MinMapSize)
        {
            if (pictureBox == null || position == null)
                throw new ArgumentNullException();

            int newX, newY;
            if (center.OutOfRange(position, MinMapSize))
            {
                newX = Math.Min(Math.Abs(position.X - Math.Abs(center.X - MinMapSize / 2)),
                            Math.Abs(position.X - Math.Abs(center.X + MinMapSize / 2)));
                newY = Math.Min(Math.Abs(position.Y - Math.Abs(center.Y - MinMapSize / 2)),
                                Math.Abs(position.Y - Math.Abs(center.Y + MinMapSize / 2)));
            }
            else
            {
                newX = position.X - center.X + MinMapSize / 2;
                newY = position.Y - center.Y + MinMapSize / 2;
            }
            pictureBox.Location = new Point(newX * defoltWidth, newY * defoltHeight);
        }

        // если конь устал скакать на этом ходу
        public bool IsPassed() { return passed == moveRange; }

        public bool WithoutArmy () 
        { 
            bool without = true;
            foreach (Unit u in army)
                // если хотя бы один из юнитов живой
                if (!u.IsDead())
                    without = false;
            return without; 
        }

        // конь покушал сена, отдохнул и снова готов скакать
        public void ResetPassed () { passed = 0; }

        // рисует по контуру рамку зеленого цвета для анимаций
        private void Highlight (Animation anim)
        {
            foreach (Bitmap b in anim.Sprites)
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.DrawRectangle(greenPen, new Rectangle(0, 0, anim.spriteWidth, anim.spriteHight));   
                }
        }

        public void AddUnits (Dictionary<string, int> typeAndAmount, XmlDocument xmlDoc)
        {
            // проходимся по словарю найма
            foreach (KeyValuePair<string, int> pair in typeAndAmount)
            {
                // елси этого типа нулевое количество
                if (pair.Value == 0)
                    continue;
                // если такой тип уже есть в армии
                if (IsTypeInArmy(pair.Key))
                {
                    // просто увеличиваем quantity этого типа юнита
                    GetUnitByType(pair.Key).AddUnit(pair.Value);
                }
                // иначе подбираем нужный конструктор
                else
                {
                    switch (pair.Key)
                    {
                        case "SwordMan":
                            switch (race)
                            {
                                case "Human":
                                    army.Add(new Soldier(xmlDoc, pair.Value));
                                    break;
                                case "Necromant":
                                    army.Add(new Sceleton(xmlDoc, pair.Value));
                                    break;
                            }
                            break;
                        case "Archer":
                            switch (race)
                            {
                                case "Human":
                                    army.Add(new Crossbowman(xmlDoc, pair.Value));
                                    break;
                                case "Necromant":
                                    army.Add(new Lich(xmlDoc, pair.Value));
                                    break;
                            }
                            break;
                        case "Healer":
                            switch (race)
                            {
                                case "Human":
                                    army.Add(new Monk(xmlDoc, pair.Value));
                                    break;
                                case "Necromant":
                                    army.Add(new Vampire(xmlDoc, pair.Value));
                                    break;
                            }
                            break;
                    }
                    // задаем дефолтные PositionCoord последнему добавленному юниту
                    int x = (army.Count - 1) / 10;
                    int y = (army.Count - 1) % 10;
                    army[army.Count - 1].PositionCoords = new Point(x, y); 
                }
            }
            
        }

        // проверяет, есть ли уже такой тип войск в армии
        private bool IsTypeInArmy (string unitType)
        {
            foreach (Unit unit in army)
                if (unit.Type == unitType)
                    return true;
            return false;
        }
        
        // причем возвращает первый юнит такого типа
        public Unit GetUnitByType (string unitType)
        {
            foreach (Unit unit in army)
                if (unit.Type == unitType)
                    return unit;
            return null;
        }

        public Unit GetUnitByIndex (int index)
        {
            if (index < 0 || index >= army.Count)
                return null;
            return army[index];
        }

        public bool IsUnitInArmy (Unit unit)
        {
            if (army == null || unit == null)
                return false;
            return army.Contains(unit);
        }
    }

    /*internal class WarlordHuman : Warlord
    {

    }

    internal class WarlordComputer : Warlord
    {

    }*/
}
