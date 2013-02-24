using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Test
{
    class CollisionHandler
    {
        Rectangle playerRectangle;
        
        public void HandleMovingCollision(Player p, Level l, int level = 0)
        {
            p.Falling = true;

            playerRectangle = new Rectangle((int)(p.X + 24 * p.Scale), (int)(p.Y + 60 * p.Scale), (int)(16 * p.Scale), (int)(4 * p.Scale));
            foreach (MacroBlock mb in l.levels[level])
            {
                if (playerRectangle.Intersects(mb.GetRectangle()))
                {
                    p.Falling = false;
                    break;
                }
            }
        }
    }

}
