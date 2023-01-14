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
    class ProjectileManager
    {
        private Rectangle screen;
        private Projectile head;
        private Projectile prevNode;
        private Projectile curNode;

        public ProjectileManager(Rectangle screen)
        {
            this.screen = screen;
            head = null;
        }

        public void Add(Projectile projectile)
        {
            if (head == null)
            {
                head = projectile;
            }
            else
            {
                projectile.SetNext(head);
                head = projectile;
            }
        }

        public void MoveAll()
        {
            if (head != null)
            {
                curNode = head;
                while (curNode != null)
                {
                    curNode.Move();
                    if (curNode.CheckRemove(screen))
                    {

                        if (curNode == head)
                        {
                            head = head.GetNext();
                        }

                        else
                        {
                            if (curNode.GetNext() == null)
                            {
                                prevNode.SetNext(null);
                            }
                            else
                            {
                                prevNode.SetNext(curNode.GetNext());
                            }
                        }
                    }

                    prevNode = curNode;
                    curNode = curNode.GetNext();
                }                
            }            
        }

        public bool SlotInteract(IngredientSlot slot)
        {
            if (head != null)
            {
                curNode = head;
                while (curNode != null)
                {
                    if (slot.Interact(curNode.GetRec()))
                    {
                        return true;
                    }
                    curNode = curNode.GetNext();
                }
            }
            return false;
        }

        public void DrawAll()
        {
            if (head != null)
            {
                curNode = head;
                
                while (curNode != null)
                {                    
                    curNode.Draw();
                    curNode = curNode.GetNext();
                }
            }
        }

        public void Reset()
        {
            head = null;
        }

    }
}
