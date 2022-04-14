using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kursach
{
    [Serializable()]
    public struct PictureData
    {
        private int spriteWidth, spriteHeight, frames;

        public int SpriteWidth { get { return spriteWidth; } }
        public int SpriteHeight { get { return spriteHeight; } }
        public int Frames { get { return frames; } }

        public PictureData(int spriteWidth, int spriteHeight, int frames)
        {
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.frames = frames;
        }
    }

    internal class Animation
    {
        public readonly int spriteWidth;
        public readonly int spriteHight;
        public readonly int numSprites;

        public static readonly int TIMER_INTERVAL = 100; // ms

        private int currentSprite = 0;
        private Image[] sprites;

        public Image[] Sprites { get { return sprites; } }

        public Animation(string path, PictureData pictureData)
        {
            this.spriteWidth = pictureData.SpriteWidth;
            this.spriteHight = pictureData.SpriteHeight;
            this.numSprites = pictureData.Frames;

            sprites = new Image[numSprites];

            Image image = Bitmap.FromFile(path); // загрузка всего спрайтлиста

            /*if (image.Width != SPRITE_WIDTH * NUM_SPRITES || image.Width != SPRITE_HEIGHT)
                throw new Exception(string.Format("File was not expected size ({0}, {1}), ({2}, {3}).", SPRITE_WIDTH * NUM_SPRITES, SPRITE_HEIGHT, image.Width, image.Height));*/

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = new Bitmap(spriteWidth, spriteHight);
                using (Graphics g = Graphics.FromImage(sprites[i]))
                    g.DrawImage
                    (
                        image,
                        new Rectangle(0, 0, spriteWidth, spriteHight),
                        new Rectangle(i * spriteWidth, 0, spriteWidth, spriteHight),
                        GraphicsUnit.Pixel
                    );
            }
        }

        public void LastFrame (PictureBox sprite)
        {
            currentSprite = numSprites - 1;
            sprite.Image = sprites[currentSprite];
        }

        public void NextImage (PictureBox sprite)
        {
            sprite.Image = sprites[currentSprite];
            currentSprite++;
            currentSprite %= numSprites;
        }

        public bool Finish () { return currentSprite == 0; }
    }
}
