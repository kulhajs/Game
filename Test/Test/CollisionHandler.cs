using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Test
{
    class CollisionHandler
    {
        Rectangle blockRectangle;
        Rectangle playerRectangle;

        int WIDTH = 800;
        int HEIGHT = 500;

        public void HandleMovingCollision(Player p, Level l, int level = 0)
        {
            p.Falling = true;

            playerRectangle = new Rectangle((int)(p.X + 16 * p.Scale), (int)(p.Y + 60 * p.Scale), (int)(16 * p.Scale), (int)(4 * p.Scale));
            foreach (MacroBlock mb in l.levels[level])
                foreach (Block b in mb.blocks)
                {
                    blockRectangle = new Rectangle((int)b.X + 2, (int)b.Y + 5, 60, 8);
                    if (playerRectangle.Intersects(blockRectangle))
                    {
                        p.Falling = false;
                        break;
                    }
                }
        }
    }
}
