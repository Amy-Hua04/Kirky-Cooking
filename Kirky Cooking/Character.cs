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
    class Character
    {
        const int MOVEMENTSPEED = 6;
        const int JUMPSPEED = 15;
        const int GRAVITY = 1;
        const int OBJECTSPACINGX = 30;
        const int OBJECTSPACINGY = 10;

        private SpriteBatch spriteBatch;
        private Ingredient ingr;
        private Plate plate;
        private Texture2D charImg;
        private Rectangle characterDefaultRec, characterRec;
        private int ground;
        private bool charGrounded;
        private int charSpeed;
        private Rectangle objectRec;

        public Character(Texture2D charImg, Rectangle characterDefaultRec, SpriteBatch spriteBatch)
        {
            this.charImg = charImg;
            this.characterDefaultRec = characterDefaultRec;
            ground = characterDefaultRec.Y;
            this.spriteBatch = spriteBatch;
            Reset();
        }

        public void Jump()
        {
            charGrounded = false;
            charSpeed = JUMPSPEED;
        }

        public void MoveLeft()
        {
            characterRec.X -= MOVEMENTSPEED;
        }

        public void MoveRight()
        {
            characterRec.X += MOVEMENTSPEED;
        }

        public void ApplyGravity()
        {
            characterRec.Y -= charSpeed;
            charSpeed -= GRAVITY;
            if (characterRec.Y > ground)
            {
                characterRec.Y = ground;
                charGrounded = true;
            }
        }

        public void AddPlate(Plate plate)
        {
            this.plate = plate;
        }


        public void AddIngr(Ingredient ingr)
        {
            this.ingr = ingr;
        }

        public Ingredient RemoveIngr()
        {
            Ingredient tempIngr = ingr;
            ingr = null;
            tempIngr.SetRec(objectRec);
            return tempIngr;
        }

        public void MoveObject()
        {            
            if (ingr != null)
            {
                objectRec = ingr.GetRec();
                objectRec.X = characterRec.X + OBJECTSPACINGX;
                objectRec.Y = characterRec.Y + OBJECTSPACINGY;
            }
        }

        public void Reset()
        {
            characterRec = characterDefaultRec;
            charSpeed = 0;
            charGrounded = true;
        }

        public void Draw()
        {
            spriteBatch.Draw(charImg, characterRec, Color.White);
            if (ingr != null)
            {
                spriteBatch.Draw(ingr.GetImg(), objectRec, Color.White);
            }
            else if (plate != null)
            {
                spriteBatch.Draw(plate.GetImg(), objectRec, Color.White) ;
            }
        }

        public Rectangle GetRec()
        {
            return characterRec;
        }

        public bool CheckIngr()
        {
            if (ingr == null)
            {
                return false;
            }
            return true;
        }

        public bool IsGrounded()
        {
            return charGrounded;
        }

        public bool IsHolding()
        {
            if (ingr != null || plate != null)
            {
                return true;
            }

            return false;
        }
    }
}
