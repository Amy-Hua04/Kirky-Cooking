using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;

namespace Kirky_Cooking
{
    class Ingredient
    {
        private const int GRAVITY = 1;

        private bool cut;
        private bool grounded = false;
        private string type;
        private Texture2D img;
        private Texture2D cutImg;
        private Rectangle imgRec;
        private Rectangle cutImgRec;
        private int ground;
        private int speedX = 0;
        private int speedY = 0;

        private Ingredient ingredient;

        private SpriteBatch spriteBatch;

        public Ingredient(Ingredient ingredient)
        {
            this.ingredient = ingredient;
        }

        public Ingredient(string type, Texture2D img, Rectangle imgRec, SpriteBatch spriteBatch, int ground)
        {
            this.type = type;
            this.img = img;
            this.imgRec = imgRec;

            this.ground = ground;

            this.spriteBatch = spriteBatch;
        }

        public void Move(Rectangle screenRec)
        {
            if (grounded == false)
            {
                imgRec.Y -= speedY;

                speedY -= GRAVITY;

                if (grounded == false && imgRec.Y > ground)
                {
                    imgRec.Y = ground;
                    speedY = 0;
                }

            }
        }

        
        public void SetLoc(int x, int y)
        {
            imgRec.X = x;
            imgRec.Y = y;
        }

        public void Draw()
        {
            spriteBatch.Draw(img, imgRec, Color.White);
        }

        public void SetRec(Rectangle imgRec)
        {
            this.imgRec = imgRec;
        }
        public Rectangle GetRec()
        {
            return imgRec;
        }

        public Texture2D GetImg()
        {
            return img;
        }
    }
}
