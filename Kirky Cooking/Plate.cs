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
    class Plate
    {
        private Texture2D img, filledImg, overlayImg;
        private Rectangle imgRec, overlayRec;
        private bool show;
        private SpriteBatch spriteBatch;
        private List<Ingredient> ingrList;

        public Plate(Texture2D img, Texture2D filledImg, Rectangle imgRec, Texture2D overlayImg, Rectangle overlayRec, SpriteBatch spriteBatch)
        {
            this.img = img;
            this.filledImg = filledImg;
            this.overlayImg = overlayImg;
            this.imgRec = imgRec;
            this.overlayRec = overlayRec;
            this.spriteBatch = spriteBatch;

            show = true;

            ingrList = new List<Ingredient> { };
        }

        public void Clear()
        {
            ingrList.Clear();
        }

        public void AddIngr(Ingredient ingredient)
        {
            ingrList.Add(ingredient);
        }


        public void Draw()
        {
            if (ingrList.Count == 0)
            {
                spriteBatch.Draw(img, imgRec, Color.White);
            }
            else
            {
                spriteBatch.Draw(filledImg, imgRec, Color.White);
            }
                      
        }

        public Rectangle GetRec()
        {
            return imgRec;
        }

        public Texture2D GetImg()
        {
            if (ingrList.Count == 0)
            {
                return img;
            }
            else
            {
                return filledImg;
            }
        }

        public List<Ingredient> GetIngr()
        {
            return ingrList;
        }

    }
}
