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
    class Projectile
    {
        private const double GRAVITY = 0.4;

        private SpriteBatch spriteBatch;
        private bool gravity;
        private Texture2D projImg;
        private Rectangle projRec;
        private double speedX;
        private double speedY;

        private Projectile next = null;

        public Projectile(bool gravity, Texture2D img, Rectangle imgRec, Point currentLoc, Point destLoc, int speed, SpriteBatch spriteBatch)
        {
            double rise = currentLoc.Y - destLoc.Y;
            double run = destLoc.X - currentLoc.X;

            this.gravity = gravity;
            this.projImg = img;
            this.projRec = imgRec;
            this.spriteBatch = spriteBatch;

            this.projRec.X = (int)currentLoc.X;
            this.projRec.Y = (int)currentLoc.Y;

            try
            {
                speedX = speed * Math.Cos(Math.Atan2(rise, run));
                speedY = speed * Math.Sin(Math.Atan2(rise, run));

            }
            catch (DivideByZeroException)
            {
                speedX = 0;

                if (currentLoc.Y - destLoc.Y > 0)
                {
                    speedY = speed;
                }
                else
                {
                    speedY = -speed;
                }
            }

            if (rise <= 0)
            {
                this.projRec.X = -100;
                this.projRec.Y = -100;
            }
        }

        public void Move()
        {
            projRec.X += (int)speedX;
            projRec.Y -= (int)speedY;
            if (gravity == true)
            {
                speedY -= GRAVITY;
            }
        }

        public bool CheckRemove(Rectangle screen)
        {
            if (screen.Intersects(projRec))
            {
                return false;
            }
            return true;
        }

        public void SetNext(Projectile next)
        {
            this.next = next;
        }

        public void Draw()
        {
            spriteBatch.Draw(projImg, projRec, Color.White);
        }

        public Projectile GetNext()
        {
            return next;
        }

        public Rectangle GetRec()
        {
            return projRec;
        }        
    }
}
