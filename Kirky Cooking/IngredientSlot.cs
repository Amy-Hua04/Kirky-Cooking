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
    class IngredientSlot
    {
        private string type;
        private Texture2D slotImg;
        private Rectangle slotRec;
        private SpriteBatch spriteBatch;

        public IngredientSlot(string type, Texture2D slotImg, Rectangle slotRec, SpriteBatch spriteBatch)
        {
            this.type = type;
            this.slotImg = slotImg;
            this.slotRec = slotRec;
            this.spriteBatch = spriteBatch;
        }

        public void Draw()
        {
            spriteBatch.Draw(slotImg, slotRec, Color.White);
        }

        public bool Interact(Rectangle projRec)
        {
            if (projRec.Intersects(slotRec))
            {
                return true;
            }
            return false;
        }

        public string GetType()
        {
            return type;
        }
    }
}
