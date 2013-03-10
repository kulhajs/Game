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
        Rectangle doorRectangle_L;
        Rectangle doorRectangle_R;
        Rectangle switchRectangle;
        Rectangle rocketRectangle;
        Rectangle coinRectangle;
        Rectangle zombieRectangle;
        Rectangle eolRectangle;

        const int doorDmg = 5;
        const int rocketDmg = 10;

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

        public void HandleZombiesMovingCollision(EnemyHandler e, Level l, int level = 0)
        {
            foreach(ZombieDispenser zd in e.zombies)
                foreach(Zombie z in zd.zombies)
                {
                    z.Falling = true;

                    zombieRectangle = new Rectangle((int)(z.X + 18 * z.Scale), (int)(z.Y + 60 * z.Scale), (int)(28 * z.Scale), (int)(4 * z.Scale));
                    foreach(MacroBlock mb in l.levels[level])
                        if(zombieRectangle.Intersects(mb.GetRectangle()))
                        {
                            z.Falling = false;
                            break;
                        }
                }
        }

        public void HandleZombiePlayerCollision(Player p, EnemyHandler e)
        {
            playerRectangle = p.Crouching ? new Rectangle((int)(p.X + 24 * p.Scale), (int)(p.Y + 9 * p.Scale), (int)(9 * p.Scale), (int)(40 * p.Scale))
                                          : new Rectangle((int)(p.X + 24 * p.Scale), (int)p.Y, (int)(9 * p.Scale), (int)(60 * p.Scale));

            foreach(ZombieDispenser zd in e.zombies)
                foreach(Zombie z in zd.zombies)
                {
                    zombieRectangle = new Rectangle((int)(z.X + 24 * z.Scale), (int)(z.Y + 9 * z.Scale), (int)(16 * z.Scale), (int)(56 * z.Scale));
                    if (playerRectangle.Intersects(zombieRectangle))
                    {
                        z.Collide = true;
                        if (z.CanBite)
                        {
                            if (z.DX < 0 && p.DX >= 0)
                            {
                                p.Push = true;
                                p.DX = -3;
                            }
                            else if (z.DX > 0 && p.DX <= 0)
                            {
                                p.Push = true;
                                p.DX = 3;
                            }

                            p.Hitpoints -= 2;
                            z.realoadTime = 0.0f;
                        }

                        if (z.DX < 0 && p.DX < 0 && !p.Push)
                            z.X = p.X - 15 * z.Scale;
                        else if (z.DX > 0 && p.DX > 0 && !p.Push)
                            z.X = p.X + 12 * z.Scale;
                    }
                    else
                        z.Collide = false;
                }
            
        }

        public void HandleDoorCollision(Player p, EnemyHandler e)
        {
            playerRectangle = new Rectangle((int)(p.X + 16 * p.Scale), (int)(p.Y + 1 * p.Scale), (int)(27 * p.Scale), (int)(63 * p.Scale));
            foreach (FlashDoor f in e.doors)
            {
                if (f.Switch)
                {
                    doorRectangle_L = new Rectangle((int)f.Position.X + 28, (int)f.Position.Y + 11, 4, 122); //left side of door laser beam
                    doorRectangle_R = new Rectangle((int)f.Position.X + 32, (int)f.Position.Y + 11, 4, 122); //right side of door laser beam
                    if (doorRectangle_L.Intersects(playerRectangle))
                    {
                        p.Hitpoints -= doorDmg;
                        p.DX = -4; //push to the left
                        p.Push = true;
                    }
                    else if (doorRectangle_R.Intersects(playerRectangle))
                    {
                        p.Hitpoints -= doorDmg;
                        p.DX = 4; //push to the right
                        p.Push = true;
                    }
                }

                switchRectangle = new Rectangle((int)f.SPosition.X + 24, (int)f.SPosition.Y + 24, 16, 16);
                if (playerRectangle.Intersects(switchRectangle) && p.IntersectWithSwitch == null)
                    p.IntersectWithSwitch = f;

                if (!playerRectangle.Intersects(switchRectangle) && p.IntersectWithSwitch == f)
                    p.IntersectWithSwitch = null;
            }
        }

        public void HandleRocketCollision(Player p, EnemyHandler e)
        {
            if (p.Jumping || p.Falling)
                playerRectangle = new Rectangle((int)(p.X + 24 * p.Scale), (int)p.Y, (int)(9 * p.Scale), (int)(48 * p.Scale));
            else
            {
                playerRectangle = p.Crouching ? new Rectangle((int)(p.X + 24 * p.Scale), (int)(p.Y + 9 * p.Scale), (int)(9 * p.Scale), (int)(40 * p.Scale))
                                             : new Rectangle((int)(p.X + 24 * p.Scale), (int)p.Y, (int)(9 * p.Scale), (int)(60 * p.Scale));

            }
            foreach (Tower t in e.towers)
                foreach (Rocket r in t.rockets)
                    if (r.Visible)
                    {
                        rocketRectangle = new Rectangle((int)r.X, (int)r.Y, 3, 3);
                        if (playerRectangle.Intersects(rocketRectangle))
                        {
                            r.Visible = false;
                            p.Hitpoints -= rocketDmg;
                        }
                    }
        }

        public void HandleItemCollision(Player p, ItemHandler i)
        {
            if (p.Jumping || p.Falling)
                playerRectangle = new Rectangle((int)(p.X + 24 * p.Scale), (int)p.Y, (int)(9 * p.Scale), (int)(48 * p.Scale));
            else
            {
                playerRectangle = p.Crouching ? new Rectangle((int)(p.X + 24 * p.Scale), (int)(p.Y + 9 * p.Scale), (int)(9 * p.Scale), (int)(40 * p.Scale))
                                              : new Rectangle((int)(p.X + 24 * p.Scale), (int)p.Y, (int)(9 * p.Scale), (int)(60 * p.Scale));

            }
            foreach (Coin c in i.coins)

                if (c.Visible)
                {
                    coinRectangle = new Rectangle((int)c.X, (int)c.Y, 16, 16);
                    if (coinRectangle.Intersects(playerRectangle))
                    {
                        c.Visible = false;
                        p.Score += 5;
                    }
                }
        }

        public void HandleEndLevel(Game1 game, Player p, Level l)
        {
            playerRectangle = new Rectangle((int)(p.X + 24 * p.Scale), (int)p.Y, (int)(9 * p.Scale), (int)(60 * p.Scale));
            eolRectangle = new Rectangle((int)(l.endOflevel.X + 42), (int)l.endOflevel.Y, 10, 64);
            if (playerRectangle.Intersects(eolRectangle))
                game.ChangeLevel = true;
        }
    }
}
