using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;

namespace Kirky_Cooking
{
    class Button
    {
        private SpriteBatch spriteBatch;

        private bool hover;
        private Rectangle rec;
        private Texture2D img;
        private Texture2D imgHov;

        public Button(Rectangle rec, Texture2D img, Texture2D imgHov, SpriteBatch spriteBatch)
        {
            this.rec = rec;
            this.img = img;
            this.imgHov = imgHov;

            this.spriteBatch = spriteBatch;

            hover = false;
        }

        public void CheckHover(Point mouseLoc)
        {
            if (rec.Contains(mouseLoc))
            {
                hover = true;
            }
            else
            {
                hover = false;
            }
        }

        public void DrawButtons()
        {
            if (hover == true)
            {
                spriteBatch.Draw(imgHov, rec, Color.White);
            }
            else
            {
                spriteBatch.Draw(img, rec, Color.White);
            }
        }

    }
}
