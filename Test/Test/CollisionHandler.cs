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
        Rectangle doorRectangle;
        Rectangle switchRectangle;
        Rectangle rocketRectangle;

        const int doorDmg = 5;
        
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

        public void HandleDoorCollision(Player p, EnemyHandler e)
        {
            playerRectangle = new Rectangle((int)(p.X + 16 * p.Scale), (int)(p.Y + 1 * p.Scale), (int)(27 * p.Scale), (int)(63 * p.Scale));
            foreach(FlashDoor f in e.doors)
            {
                if(f.Switch)
                {
                    doorRectangle = new Rectangle((int)f.Position.X + 28, (int)f.Position.Y + 11, 8, 122);
                    if (doorRectangle.Intersects(playerRectangle))
                        p.Hitpoints -= doorDmg;
                }

                switchRectangle = new Rectangle((int)f.SPosition.X + 24, (int)f.SPosition.Y + 24, 16, 16);
                if (playerRectangle.Intersects(switchRectangle) && p.IntersectWithSwitch == null)
                    p.IntersectWithSwitch = f;
                else if (!playerRectangle.Intersects(switchRectangle) && p.IntersectWithSwitch != null)
                    p.IntersectWithSwitch = null;
            }
        }

        public void HandleRocketCollision(Player p, EnemyHandler e)
        {
            playerRectangle = new Rectangle((int)p.X + 17, (int)p.Y + 2, 23, 34);
            foreach(Tower t in e.towers)
                foreach(Rocket r in t.rockets)
                    if (r.Visible)
                    {
                        rocketRectangle = new Rectangle((int)r.X, (int)r.Y, 3, 3);
                        if (playerRectangle.Intersects(rocketRectangle))
                        {
                            r.Visible = false;
                            p.Color = Color.Red;
                        }
                    }
        }
    }

}
