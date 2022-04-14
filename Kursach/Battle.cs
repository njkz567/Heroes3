using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace Kursach
{
    public partial class Battle : Form
    {
        private HexGrid battleField;                 // сетка из гексов
        private const float R = 40;                  // отталкиваемся от радиуса описанной окр
        private int rows = 10;                       
        private int cols = 17;

        private Unit currentUnit;
        private Unit attackGoal;
        private bool highlightDistance = true;
        private Stack<Hex> unitPath;
        private Hex partOfUnitPath;
        private Queue<Unit> unitQueue;
        private Warlord hero, enemy;
        private bool enemyInGoalTile;
        private Hex currentUnitPosition;                // позиция врага на пути
        private bool left, underFireLeft;               // признак отзеркаленной анимации

        // мой любимый конструктор 
        internal Battle(Warlord hero, Warlord enemy)
        {
            InitializeComponent();

            timer.Interval = Animation.TIMER_INTERVAL;

            FormBorderStyle = FormBorderStyle.None;

            battleField = new HexGrid(R, rows, cols);

            hexGrid.Size = new Size((int)(cols * (2 * battleField.r + 2 * HexGrid.hexWidth) + 2), (int)(rows * (3.0 / 2 * R + 2 * HexGrid.hexWidth)));
            hexGrid.Location = new Point(this.Width / 2 - hexGrid.Width / 2, 50);

            unitQueue = new Queue<Unit>();
            bool unitsRunOut = false;
            int i = 0;
            Unit tempHero, tempEnemy;
            // сопоставляем гексы и юнитов
            while (!unitsRunOut)
            {
                tempHero = hero.GetUnitByIndex(i);
                if (tempHero != null && !tempHero.IsDead())
                {
                    battleField.PlaceUnit(tempHero);
                    unitQueue.Enqueue(tempHero);
                }
                tempEnemy = enemy.GetUnitByIndex(i);
                if (tempEnemy != null && !tempEnemy.IsDead())
                {
                    battleField.PlaceUnit(tempEnemy, false);
                    unitQueue.Enqueue(tempEnemy);
                }
                i++;
                if (tempHero == null && tempEnemy == null)
                    unitsRunOut = true;

                enemyInGoalTile = false;
            }

            // загружаем пикчурбоксы героя
            Point pos;
            foreach (Unit u in hero.army)
            {
                if (u.IsDead())
                    continue;
                pos = battleField.IntHexToPixel(u.Position);
                u.PictureBox.Location = new Point(u.PictureBox.Location.X + pos.X, u.PictureBox.Location.Y + pos.Y);
                u.PictureBox.MouseClick += new MouseEventHandler(this.unit_MouseClick);
                hexGrid.Controls.Add(u.PictureBox);
                
            }
                
            // загружаем пикчурбоксы противника
            // при этом их Location надо смещать в противоположную часть карты
            foreach (Unit u in enemy.army)
            {
                if (u.IsDead())
                    continue;
                u.SetLeftDirectionAnimation();
                pos = battleField.IntHexToPixel(u.Position);
                u.PictureBox.Location = new Point(u.PictureBox.Location.X + pos.X, u.PictureBox.Location.Y + pos.Y);
                u.PictureBox.MouseClick += new MouseEventHandler(this.unit_MouseClick);
                hexGrid.Controls.Add(u.PictureBox);
            }
            
            this.hero = hero;
            this.enemy = enemy;

            currentUnit = unitQueue.Dequeue();
        }
        
        private void timer_Tick(object sender, EventArgs e)
        {
            bool finish = currentUnit.Step(battleField, partOfUnitPath, left);

            // если анимация движения на соседнюю клетку закончилась
            if (finish)
                // извлекаем новый тайл пути
                if (unitPath.Count != 0)
                {
                    if (unitPath.Count == 1 && enemyInGoalTile)
                    {
                        // вот здесь бой
                        timer.Stop();
                        highlightDistance = true;
                        enemyInGoalTile = false;
                        attackGoal = unitPath.Pop().unit;
                        attackGoal.Position.Locked();
                        underFireLeft = attackGoal.IsLeftDirectionAnimation();
                        partOfUnitPath = null;
                        attackTimer.Start();
                        hexGrid.Invalidate();
                        return;
                    }
                    else
                        partOfUnitPath = unitPath.Pop();
                }
                else 
                    partOfUnitPath = null;
            
            // вот это уже прямо полный конец пути
            if (finish && unitPath.Count == 0 && partOfUnitPath == null)
            {
                timer.Stop();
                highlightDistance = true;

                infoLabel.Text = currentUnit.Position.ToString() + "\n" +
                          attackGoal?.Position.ToString();

                // ставим нынешнего юнита в конец очереди
                unitQueue.Enqueue(currentUnit);
                // берем следующего
                currentUnit = unitQueue.Dequeue();
            }
            hexGrid.Invalidate();
        }

        private void hexGrig_Paint(object sender, PaintEventArgs e)
        {
            while (currentUnit.IsDead())
                currentUnit = unitQueue.Dequeue();
            battleField.Drawgrid(e.Graphics);
            if (highlightDistance)
            {
                if (hero.IsUnitInArmy(currentUnit))
                    battleField.HighlightDistance(e.Graphics, currentUnit, enemy.army);
                else
                    battleField.HighlightDistance(e.Graphics, currentUnit, hero.army);
            } 
        }

        // клик по пикчурбоксу героя
        private void unit_MouseClick(object sender, MouseEventArgs e)
        {
            // если какая-то из анимаций не закончилась
            if (timer.Enabled || attackTimer.Enabled || underFireTimer.Enabled || deathTimer.Enabled)
                return;

            currentUnitPosition = null;
            PictureBox pic = sender as PictureBox;

            left = currentUnit.IsLeftDirectionAnimation();

            // если ходит герой
            if (hero.IsUnitInArmy(currentUnit))
            {
                foreach (Unit u in enemy.army)
                    if (pic == u.PictureBox)
                    {
                        currentUnitPosition = u.Position;
                    }
            }
            else
                foreach (Unit u in hero.army)
                    if (pic == u.PictureBox)
                    {
                        currentUnitPosition = u.Position;
                    }

            if (currentUnitPosition == null)
            {
                return;
            }
            if (currentUnit is Archer)
            {
                attackTimer.Start();
                attackGoal = currentUnitPosition.unit;
                underFireLeft = attackGoal.IsLeftDirectionAnimation();
                return;
            }
            if (!currentUnit.Position.InRange(currentUnitPosition, currentUnit.MoveRange))
                return;

            currentUnitPosition.UnLocked();
            enemyInGoalTile = true;
            StartTurn(currentUnitPosition, e);
        }

        // тут весь процесс хода (таймер мне наврал, поэтому, видимо, только начало)
        private void StartTurn (Hex userChoice, MouseEventArgs e)
        {
            if (userChoice == null)
                return;

            if (userChoice == currentUnit.Position)
                return;

            // есди просто жмякнули по залоченной клетке
            if (userChoice.IsLocked() && userChoice.unit == null)
                return;

            // если не в рейндже ходьбы
            if (!currentUnit.Position.InRange(userChoice, currentUnit.MoveRange))
                return;

            unitPath = battleField.PathFinding(currentUnit.Position, userChoice);

            if (unitPath.Count == 1 && enemyInGoalTile)
            {
                // вот здесь бой
                enemyInGoalTile = false;
                attackGoal = unitPath.Pop().unit;
                attackGoal.Position.Locked();
                attackTimer.Start();
                //hexGrid.Invalidate();
                return;
            }

            highlightDistance = false;

            partOfUnitPath = unitPath.Pop();

            timer.Start();
        }

        // при клике пользователя по hexGrid происходит ход
        private void hexGrid_MouseDown(object sender, MouseEventArgs e)
        {
            // если какая-то из анимаций не закончилась
            if (timer.Enabled || attackTimer.Enabled || underFireTimer.Enabled || deathTimer.Enabled)
                return;
            Hex userChoice = battleField.PixelToHex(e.Location);
            if (userChoice.unit != null)
                userChoice.UnLocked();
            left = currentUnit.IsLeftDirectionAnimation();
            StartTurn(userChoice, e);
        }

        private void attackTimer_Tick(object sender, EventArgs e)
        {
            bool finish = currentUnit.Attack(attackGoal, left);

            if (finish)
            {
                attackTimer.Stop();
                if (attackGoal.IsDead())
                    deathTimer.Start();
                else
                    underFireTimer.Start();
            }
            hexGrid.Invalidate();
        }

        private void underFireTimer_Tick(object sender, EventArgs e)
        {
            bool finish = attackGoal.UnderAttack(underFireLeft);

            if (finish)
            {
                underFireTimer.Stop();
                highlightDistance = true;

                infoLabel.Text = currentUnit.Position.ToString() + "\n" +
                          attackGoal.Position.ToString();

                // ставим нынешнего юнита в конец очереди
                unitQueue.Enqueue(currentUnit);
                // берем следующего
                currentUnit = unitQueue.Dequeue();
            }
            hexGrid.Invalidate();
        }

        private void UntiePictureBox ()
        {
            foreach (Unit u in hero.army)
                hexGrid.Controls.Remove(u.PictureBox);
            foreach (Unit u in enemy.army)
                hexGrid.Controls.Remove(u.PictureBox);
        }

        private void skipTurn_Click(object sender, EventArgs e)
        {
            unitQueue.Enqueue(currentUnit);
            currentUnit = unitQueue.Dequeue();
            hexGrid.Invalidate();
        }

        private void deathTimer_Tick(object sender, EventArgs e)
        {
            bool finish = attackGoal.Death(left);
            
            if (finish)
            {
                deathTimer.Stop();

                bool heroLoose = true, enemyLoose = true;
                foreach (Unit u in hero.army)
                    // если хоть один живой юнит
                    if (!u.IsDead())
                        heroLoose = false;
                foreach (Unit u in enemy.army)
                    if (!u.IsDead())
                        enemyLoose = false;

                if (heroLoose)
                {
                    ((GlobalMap)this.Owner).heroLooser = true;
                }
                if (enemyLoose)
                {
                    ((GlobalMap)this.Owner).heroLooser = false;
                }
                // если хотя бы один проиграл
                if (heroLoose || enemyLoose)
                {
                    UntiePictureBox();
                    this.Close();
                    return;
                }

                highlightDistance = true;
                attackGoal.Position.UnLocked();
                // ставим нынешнего юнита в конец очереди
                unitQueue.Enqueue(currentUnit);
                // берем следующего, пока не окажется живой
                // заодно чистим очередь
                currentUnit = unitQueue.Dequeue();
                while (currentUnit.IsDead())
                    currentUnit = unitQueue.Dequeue();
            }
            hexGrid.Invalidate();
        }
    }
}
