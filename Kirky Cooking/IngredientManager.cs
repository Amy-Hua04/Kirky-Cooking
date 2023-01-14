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
    class IngredientManager
    {
        private List<Ingredient> ingrList;

        public IngredientManager()
        {
            ingrList = new List<Ingredient> { };
        }

        public void Add(Ingredient ingr, Rectangle pipeRec)
        {
            ingrList.Insert(0, ingr);
            ingrList[0].SetLoc(pipeRec.X + 12, pipeRec.Y + 50);
        }

        public void Add(Ingredient ingr)
        {
            ingrList.Insert(0, ingr);
        }

        public Ingredient Remove(Rectangle charRec)
        {
            Ingredient tempIngr;

            for (int i = 0; i < ingrList.Count; i++)
            {
                if (charRec.Intersects(ingrList[i].GetRec()))
                {
                    tempIngr = ingrList[i];
                    ingrList.RemoveAt(i);
                    return tempIngr;
                }
            }
            return null;
        }

        public void MoveAll(Rectangle screenRec)
        {
            for (int i = 0; i < ingrList.Count; i++)
            {
                ingrList[i].Move(screenRec);
            }
        }

        public void DrawAll()
        {
            for (int i = 0; i < ingrList.Count; i++)
            {
                ingrList[i].Draw();
            }
        }

        public void Reset()
        {
            ingrList.Clear();
        }
    }
}
