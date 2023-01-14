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
    class OrderQueue
    {
        private const int SIDESPACING = 60;
        private const int ORDERSPACING = 10;
        private const int SLIPSPACING = 5;
        
        private List<Order> orderList;
        private int size;
        private SpriteBatch spriteBatch;
        private Texture2D orderBg, orderSlip;
        private Rectangle orderBgRec, orderSlipRec;


        public OrderQueue(SpriteBatch spriteBatch, Texture2D orderBg, Rectangle orderBgRec, Texture2D orderSlip, Rectangle orderSlipRec)
        {
            this.spriteBatch = spriteBatch;

            this.orderBg = orderBg;
            this.orderSlip = orderSlip;
            this.orderBgRec = orderBgRec;
            this.orderSlipRec = orderSlipRec;

            orderList = new List<Order> { };
        }

        public void Enqueue(Order order)
        {
            orderList.Add(order);
            size++;
        }

        public bool Dequeue(Order order)
        {
            for (int i = 0; i < size; i++)
            {
                if (orderList[i].GetType() == order.GetType())
                {
                    orderList.RemoveAt(i);
                    size--;
                    return true;
                }
            }

            return false;

        }

        public void CheckDequeue()
        {
            for (int i = 0; i < size; i++)
            {
                if (orderList[i].CheckTimer())
                {
                    orderList.RemoveAt(i);
                    size--;
                }
            }
        }

        public void Reset()
        {
            orderList = new List<Order> { };
            size = 0;
        }

        public bool Remove(List<Ingredient> ingrList)
        {
            for (int i = 0; i < orderList.Count; i++)
            {
                if (ingrList.Count == orderList[i].GetType())
                {
                    orderList.RemoveAt(i);
                    size--;
                    return true;
                }
            }
            return false;
            
        }


        public void DrawAll()
        {
            Ingredient[] ingr;
            Rectangle rec;
            for (int i = 0; i < size; i++)
            {
                orderBgRec.X = i * (orderBgRec.Width + ORDERSPACING) + SIDESPACING;
                ingr = orderList[i].GetIngr();

                orderSlipRec.Y = orderBgRec.Y + orderBgRec.Height;
                orderSlipRec.X = orderBgRec.X + SLIPSPACING;
                for (int j = 0; j < ingr.Length; j++)
                {                    
                    spriteBatch.Draw(orderSlip, orderSlipRec, Color.White);
                    rec = new Rectangle(orderSlipRec.X + orderSlipRec.Width / 2 - ingr[j].GetRec().Width / 2 + 5, orderSlipRec.Y + orderSlipRec.Height / 2 - ingr[j].GetRec().Height / 2 + 5, (int)(ingr[j].GetRec().Width * 0.7), (int)(ingr[j].GetRec().Height * 0.7));

                    spriteBatch.Draw(ingr[j].GetImg(), rec, Color.White);

                    orderSlipRec.X += orderSlipRec.Width + SLIPSPACING;
                    
                }

                rec = orderList[i].GetRec();

                rec.X = orderBgRec.X + orderBgRec.Width / 2 - rec.Width / 2;
                rec.Y = orderBgRec.Y + orderBgRec.Height / 2 - rec.Height / 2;

                spriteBatch.Draw(orderBg, orderBgRec, Color.White);
                spriteBatch.Draw(orderList[i].GetImg(), rec, Color.White);
            }
        }

        public int GetSize()
        {
            return size;
        }
    }
}
