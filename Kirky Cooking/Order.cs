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
    class Order
    {
        Ingredient[] ingredients;
        Texture2D img;
        Rectangle rec;

        int type;

        Timer timer;

        public Order(Ingredient[] ingredients, Texture2D img, Rectangle rec, int type)
        {
            this.ingredients = ingredients;
            this.img = img;
            this.rec = rec;

            this.type = type;
        }

        public bool CheckTimer()
        {
            if (timer.IsFinished())
            {
                return true;
            }
            return false;
        }

        public Ingredient[] GetIngr()
        {
            return ingredients;
        }

        public Rectangle GetRec()
        {
            return rec;
        }

        public Texture2D GetImg()
        {
            return img;
        }

        public int GetType()
        {
            return type;
        }
    }
}
