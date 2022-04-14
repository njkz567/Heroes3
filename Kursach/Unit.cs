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
    // иерархия классов для юнитов
    [Serializable()]
    internal abstract class Unit
    {
        private string race;            // рассовая принадлежность
        private string type;            // тип юнита
        private string name;            // название юнита 
        private int hp;                 // очки здоровья
        private readonly int maxHP;     // максимальные очки здоровья
        private int attackPower;        // сила атаки
        private int quantity;           // количество однотипных юнитов в "пачке"
        private int moveRange;          // дистанция ходьбы
        private int attackRange;        // дистанция атаки
        [NonSerialized] private Hex position;           // текущая позиция на гексосетке
        private Point positionCoords;           // 
        [NonSerialized] private PictureBox pictureBox;  // контейнер для спрайта 
        [NonSerialized] private Bitmap defoltCadr;             // юнит в покое
        [NonSerialized] private Bitmap defoltLeftCadr;         // его злой отзеркаленный брат-близнец

        [NonSerialized] public Animation attackAnimation;
        [NonSerialized] public Animation attackLeftAnimation;
        [NonSerialized] public Animation moveAnimation;
        [NonSerialized] public Animation moveLeftAnimation;
        [NonSerialized] public Animation deathAnimation;
        [NonSerialized] public Animation deathLeftAnimation;
        [NonSerialized] public Animation underFireAnimation;
        [NonSerialized] public Animation underFireLeftAnimation;

        private static PictureData movePictureData = new PictureData(110, 110, 11);
        private static PictureData otherPictureData = new PictureData(110, 110, 6);

        public string Race { get { return race; } }

        public string Type { get { return type; } }

        public string Name { get { return name; } }

        public int MoveRange { get { return moveRange; } }

        public int AttackRange { get { return attackRange; } }

        public Hex Position { get { return position; } set { position = value; } }

        public Point PositionCoords { get { return positionCoords; } set { positionCoords = value; } }
        
        public PictureBox PictureBox { get { return pictureBox; } }

        public Unit (string race, string type, XmlDocument xmlDoc, int quantity = 0)
        {
            this.race = race;
            this.type = type;
            this.quantity = quantity;

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

                    // проходимся по всему внутри <race>, то есть пока что по 
                    // <warlord> и <units>
                    foreach (XmlNode unit in raceSearch.ChildNodes)
                    {
                        if (unit.Name != "units")
                            continue;

                        // проходимся по всем типам юнитов внутри <units>
                        foreach (XmlNode unitType in unit.ChildNodes)
                        {
                            XmlNode unitName = unitType.Attributes.GetNamedItem("name");

                            if (unitName.Value != type)
                                continue;

                            // проходимся по всем харкам юнита
                            foreach (XmlNode property in unitType.ChildNodes)
                            {
                                if (property.Name == "name")
                                    name = property.InnerText;
                                if (property.Name == "HP")
                                {
                                    hp = Convert.ToInt32(property.InnerText);
                                    maxHP = hp;
                                }
                                if (property.Name == "attackPower")
                                    attackPower = Convert.ToInt32(property.InnerText);
                                if (property.Name == "moveRange")
                                    moveRange = Convert.ToInt32(property.InnerText);
                                if (property.Name == "attackRange")
                                    attackRange = Convert.ToInt32(property.InnerText);

                                //  загрузка изображения юнита в покое
                                if (property.Name == "defoltCadr")
                                    defoltCadr = new Bitmap(property.InnerText);
                                if (property.Name == "defoltLeftCadr")
                                    defoltLeftCadr = new Bitmap(property.InnerText);
                                // загрузка анимации
                                if (property.Name == "moveLeftSpritePath")
                                    moveLeftAnimation = new Animation(property.InnerText, movePictureData);
                                if (property.Name == "attackLeftSpritePath")
                                    attackLeftAnimation = new Animation(property.InnerText, otherPictureData);
                                if (property.Name == "deathLeftSpritePath")
                                    deathLeftAnimation = new Animation(property.InnerText, otherPictureData);
                                if (property.Name == "underFireLeftSpritePath")
                                    underFireLeftAnimation = new Animation(property.InnerText, otherPictureData);
                                if (property.Name == "moveSpritePath")
                                    moveAnimation = new Animation(property.InnerText, movePictureData);
                                if (property.Name == "attackSpritePath")
                                    attackAnimation = new Animation(property.InnerText, otherPictureData);
                                if (property.Name == "deathSpritePath")
                                    deathAnimation = new Animation(property.InnerText, otherPictureData);
                                if (property.Name == "underFireSpritePath")
                                    underFireAnimation = new Animation(property.InnerText, otherPictureData);
                            }
                        }
                    }
                }
            }

            pictureBox = new PictureBox();
            pictureBox.BackColor = Color.Transparent;
            pictureBox.Size = new Size(movePictureData.SpriteWidth, movePictureData.SpriteHeight);
            pictureBox.Location = new Point(Convert.ToInt32(-pictureBox.Width / 2f), -pictureBox.Height);
            pictureBox.Image = defoltCadr;
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

                    // проходимся по всему внутри <race>, то есть пока что по 
                    // <warlord> и <units>
                    foreach (XmlNode unit in raceSearch.ChildNodes)
                    {
                        if (unit.Name != "units")
                            continue;

                        // проходимся по всем типам юнитов внутри <units>
                        foreach (XmlNode unitType in unit.ChildNodes)
                        {
                            XmlNode unitName = unitType.Attributes.GetNamedItem("name");

                            if (unitName.Value != type)
                                continue;

                            // проходимся по всем харкам юнита
                            foreach (XmlNode property in unitType.ChildNodes)
                            {
                                //  загрузка изображения юнита в покое
                                if (property.Name == "defoltCadr")
                                    defoltCadr = new Bitmap(property.InnerText);
                                if (property.Name == "defoltLeftCadr")
                                    defoltLeftCadr = new Bitmap(property.InnerText);
                                // загрузка анимации
                                if (property.Name == "moveLeftSpritePath")
                                    moveLeftAnimation = new Animation(property.InnerText, movePictureData);
                                if (property.Name == "attackLeftSpritePath")
                                    attackLeftAnimation = new Animation(property.InnerText, otherPictureData);
                                if (property.Name == "deathLeftSpritePath")
                                    deathLeftAnimation = new Animation(property.InnerText, otherPictureData);
                                if (property.Name == "underFireLeftSpritePath")
                                    underFireLeftAnimation = new Animation(property.InnerText, otherPictureData);
                                if (property.Name == "moveSpritePath")
                                    moveAnimation = new Animation(property.InnerText, movePictureData);
                                if (property.Name == "attackSpritePath")
                                    attackAnimation = new Animation(property.InnerText, otherPictureData);
                                if (property.Name == "deathSpritePath")
                                    deathAnimation = new Animation(property.InnerText, otherPictureData);
                                if (property.Name == "underFireSpritePath")
                                    underFireAnimation = new Animation(property.InnerText, otherPictureData);
                            }
                        }
                    }
                }
            }

            pictureBox = new PictureBox();
            pictureBox.BackColor = Color.Transparent;
            pictureBox.Size = new Size(movePictureData.SpriteWidth, movePictureData.SpriteHeight);
            pictureBox.Location = new Point(Convert.ToInt32(-pictureBox.Width / 2f), -pictureBox.Height);
            pictureBox.Image = defoltCadr;
        }

        public void AddUnit (int quantity = 1) { this.quantity += quantity; }

        public bool IsDead () { return quantity == 0; }

        public bool Death (bool left)
        {
            if (pictureBox == null || position == null)
                throw new ArgumentNullException();

            if (left)
            {
                deathLeftAnimation.NextImage(pictureBox);
                if (deathLeftAnimation.Finish())
                {
                    deathLeftAnimation.LastFrame(pictureBox);
                    return true;
                }
            }
            else
            {
                deathAnimation.NextImage(pictureBox);
                if (deathAnimation.Finish())
                {
                    deathAnimation.LastFrame(pictureBox);
                    return true;
                }
            }
            
            return false;
        }

        public void TakeDamage(int damage)
        {
            quantity -= damage / maxHP;
            hp -= damage % maxHP;
            if (hp <= 0)
            {
                hp = maxHP;
                quantity--;
            }
            if (quantity < 0)
                quantity = 0;
        }

        // возможно, virtual, чтобы какой-нить наследник мог ответный удар кинуть
        public bool UnderAttack (bool left)
        {
            if (pictureBox == null || position == null)
                throw new ArgumentNullException();

            if (left)
            {
                underFireLeftAnimation.NextImage(PictureBox);
                if (underFireLeftAnimation.Finish())
                {
                    pictureBox.Image = defoltLeftCadr;
                    return true;
                }
            }
            else
            {
                underFireAnimation.NextImage(pictureBox);
                if (underFireAnimation.Finish())
                {
                    pictureBox.Image = defoltCadr;
                    return true;
                }
            }
            
            return false;
        }

        public int GiveDamage () { return quantity * attackPower; }

        public bool Attack (Unit enemy, bool left)
        {
            if (pictureBox == null || position == null)
                throw new ArgumentNullException();

            if (!position.InRange(enemy.Position, attackRange))
                throw new Exception();

            if (left)
            {
                attackLeftAnimation.NextImage(pictureBox);
                if (attackLeftAnimation.Finish())
                {
                    pictureBox.Image = defoltLeftCadr;
                    enemy.TakeDamage(GiveDamage());
                    return true;
                }
            }
            else
            {
                attackAnimation.NextImage(pictureBox);
                if (attackAnimation.Finish())
                {
                    pictureBox.Image = defoltCadr;
                    enemy.TakeDamage(GiveDamage());
                    return true;
                }
            }
                
            return false;
        }

        public bool Step(HexGrid hexgrid, Hex goal, bool left)
        {
            if (pictureBox == null || position == null)
                throw new ArgumentNullException();

            bool finish;
            Point delta = hexgrid.NeighborToPixel(position, goal, moveAnimation.numSprites);

            if (left)
            {
                moveLeftAnimation.NextImage(pictureBox);
                finish = moveLeftAnimation.Finish();
            }
            else
            {
                moveAnimation.NextImage(pictureBox);
                finish = moveAnimation.Finish();
            }

            
            if (finish)
            {
                if (left)
                    pictureBox.Image = defoltLeftCadr;
                else
                    pictureBox.Image = defoltCadr;
                Point newLocation = hexgrid.IntHexToPixel(goal);
                newLocation.X -= Convert.ToInt32(moveAnimation.spriteWidth / 2);
                newLocation.Y -= moveAnimation.spriteHight;
                pictureBox.Location = newLocation;
                hexgrid.ReplaceUnit(this);
                position = goal;
                goal.unit = this;
                position.Locked();
            }
            else
                pictureBox.Location = new Point(pictureBox.Location.X + delta.X, pictureBox.Location.Y + delta.Y);
            return finish;
        }

        // во внешнем коде проверяем на эту функцию, и в зависимости от ее результата передаем в анимации тру или фалзе
        public bool IsLeftDirectionAnimation () { return pictureBox.Image == defoltLeftCadr; }

        public void SetLeftDirectionAnimation () { pictureBox.Image = defoltLeftCadr; }
    }

    [Serializable()]
    internal abstract class SwordMan: Unit
    {
        static public int CostOfHire = 100;

        public SwordMan (string race, XmlDocument xmlDoc, int quantity = 0)
            : base(race, "SwordMan", xmlDoc, quantity)
        {

        }
    }

    [Serializable()]
    internal abstract class Archer : Unit
    {
        static public int CostOfHire = 200;

        public Archer (string race, XmlDocument xmlDoc, int quantity = 0)
            : base(race, "Archer", xmlDoc, quantity)
        {

        }
    }

    [Serializable()]
    internal abstract class Healer : Unit
    {
        static public int CostOfHire = 300;

        public Healer (string race, XmlDocument xmlDoc, int quantity = 0)
            : base(race, "Healer", xmlDoc, quantity)
        {

        }
    }

    [Serializable()]
    internal class Soldier : SwordMan 
    {
        public Soldier (XmlDocument xmlDoc, int quantity = 0)
            : base("Human", xmlDoc, quantity)
        {

        }
    }

    [Serializable()]
    internal class Sceleton : SwordMan
    {
        public Sceleton (XmlDocument xmlDoc, int quantity = 0)
            : base("Necromant", xmlDoc, quantity)
        {

        }
    }

    [Serializable()]
    internal class Crossbowman : Archer
    {
        public Crossbowman (XmlDocument xmlDoc, int quantity = 0)
            : base("Human", xmlDoc, quantity)
        {

        }
    }

    [Serializable()]
    internal class Lich : Archer
    {
        public Lich (XmlDocument xmlDoc, int quantity = 0)
            : base("Necromant", xmlDoc, quantity)
        {

        }
    }

    [Serializable()]
    internal class Monk : Healer
    {
        public Monk (XmlDocument xmlDoc, int quantity = 0)
            : base("Human", xmlDoc, quantity)
        {

        }
    }

    [Serializable()]
    internal class Vampire : Healer
    {
        public Vampire (XmlDocument xmlDoc, int quantity = 0)
            : base("Necromant", xmlDoc, quantity)
        {

        }
    }
}
