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
        Rectangle zombieHeadRectangle;
        Rectangle eolRectangle;
        Rectangle bulletRectangle;
        Rectangle firstAidRectangle;
        Rectangle acidRectangle;

        Random random = new Random();

        const int minRocketDmg = 30;
        const int maxRocketDmg = 50;

        const int minDoorDmg = 3;
        const int maxDoorDmg = 8;

        const int minZombieDmg = 5; 
        const int maxZombieDmg = 15; 

        const int firstAidHealth = 20;

        const int bulletDmg = 20;

        public void HandleMovingCollision(Player p, Level l, Camera c)
        {
            p.Falling = true;

            playerRectangle = new Rectangle((int)(p.X + 24 * p.Scale), (int)(p.Y + 58 * p.Scale), (int)(16 * p.Scale), (int)(6 * p.Scale));
            foreach (MacroBlock mb in l.levelBlocks)
                if (mb.IsOnScreen(c))
                {
                    if (playerRectangle.Intersects(mb.GetRectangle()))
                    {
                        p.Falling = false;
                        break;
                    }
                }
        }

        public void HandleZombiesMovingCollision(EnemyHandler e, Level l)
        {
            foreach (ZombieDispenser zd in e.zombies)
                foreach (Zombie z in zd.zombies)
                {
                    z.Falling = true;

                    zombieRectangle = new Rectangle((int)(z.X + 18 * z.Scale), (int)(z.Y + 60 * z.Scale), (int)(28 * z.Scale), (int)(6 * z.Scale));
                    foreach (MacroBlock mb in l.levelBlocks)
                        if (zombieRectangle.Intersects(mb.GetRectangle()))
                        {
                            z.Falling = false;
                            break;
                        }
                }
        }

        public void HandleZombiePlayerCollision(Player p, EnemyHandler e, ExplosionHandler explosions, SoundHandler sounds)
        {
            playerRectangle = p.Crouching ? new Rectangle((int)(p.X + 24 * p.Scale), (int)(p.Y + 9 * p.Scale), (int)(9 * p.Scale), (int)(40 * p.Scale))
                                          : new Rectangle((int)(p.X + 24 * p.Scale), (int)p.Y, (int)(9 * p.Scale), (int)(60 * p.Scale));

            foreach (ZombieDispenser zd in e.zombies)
                foreach (Zombie z in zd.zombies)
                    if (z.OnScreen)
                    {
                        zombieRectangle = new Rectangle((int)(z.X + 24 * z.Scale), (int)(z.Y + 9 * z.Scale), (int)(16 * z.Scale), (int)(56 * z.Scale));
                        if (playerRectangle.Intersects(zombieRectangle))
                        {
                            z.Collide = true;
                            if (z.CanBite)
                            {
                                if (z.DX < 0 && p.DX >= 0)
                                {
                                    explosions.AddExplosion(new Vector2(p.X - 9 * p.Scale, p.Y), p.contentManager, 4, 15, "blood", 32);
                                    p.DX = -4f;
                                    p.Push = true;
                                    p.Hitpoints -= random.Next(minZombieDmg, maxZombieDmg);
                                    sounds.PlayHurt(p, z);
                                    z.realoadTime = 0.0f;
                                }
                                else if (z.DX > 0 && p.DX <= 0)
                                {
                                    explosions.AddExplosion(new Vector2(p.X + 9 * p.Scale, p.Y), p.contentManager, 4, 15, "blood", 32);
                                    p.DX = 4f;
                                    p.Push = true;
                                    p.Hitpoints -= random.Next(minZombieDmg, maxZombieDmg);
                                    sounds.PlayHurt(p, z);
                                    z.realoadTime = 0.0f;
                                }
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
                        p.Hitpoints -= random.Next(minDoorDmg, maxDoorDmg);
                        p.DX = -4; //push to the left
                        p.Push = true;
                    }
                    else if (doorRectangle_R.Intersects(playerRectangle))
                    {
                        p.Hitpoints -= random.Next(minDoorDmg, maxDoorDmg);
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
                playerRectangle = p.Crouching ? new Rectangle((int)(p.X + 24 * p.Scale), (int)(p.Y + 9 * p.Scale), (int)(9 * p.Scale), (int)(45 * p.Scale))
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
                            p.Push = true;

                            if(r.DX > 0)
                                p.DX = 3.5f;
                            else
                                p.DX = -3.5f;
                            
                            p.Hitpoints -= random.Next(minRocketDmg, maxRocketDmg);
                        }

                        foreach(ZombieDispenser zd in e.zombies)
                            foreach(Zombie z in zd.zombies)
                                if (z.Visible && r.Visible)
                                {
                                    zombieRectangle = new Rectangle((int)(z.X + 24 * z.Scale), (int)z.Y, (int)(16 * z.Scale), (int)(60 * z.Scale));
                                    if(rocketRectangle.Intersects(zombieRectangle))
                                    {
                                        r.Visible = false;
                                        z.Hitpoints = 0;
                                    }
                                }
                    }
        }

        public void HandleBulletZombieCollision(Player p, EnemyHandler e)
        {
            foreach (Bullet b in p.bullets)
                if (b.Visible)
                {
                    bulletRectangle = new Rectangle((int)b.X, (int)b.Y, 2, 1);
                    foreach (ZombieDispenser zd in e.zombies)
                        foreach (Zombie z in zd.zombies)
                            if (z.Visible)
                            {
                                zombieRectangle = new Rectangle((int)(z.X + 18 * z.Scale), (int)(z.Y + 19 * z.Scale), (int)(28 * z.Scale), (int)(45 * z.Scale));
                                zombieHeadRectangle = new Rectangle((int)(z.X + 18 * z.Scale), (int)(z.Y + 4 * z.Scale), (int)(22 * z.Scale), (int)(15 * z.Scale));
                                if (bulletRectangle.Intersects(zombieRectangle))
                                {
                                    z.Hitpoints -= bulletDmg;
                                    b.SprayBlood = true;
                                    b.Visible = false;
                                }
                                else if(bulletRectangle.Intersects(zombieHeadRectangle))
                                {
                                    z.Hitpoints = 0;
                                    p.Score += 5;
                                    b.SprayBlood = true;
                                    b.Visible = false;
                                }
                            }
                }
        }

        public void HandleBulletTurretCollision(Player p, EnemyHandler e, ExplosionHandler eh)
        {
            foreach(Bullet b in p.bullets)
            {
                bulletRectangle = new Rectangle((int)b.X, (int)b.Y, 2, 1);
                foreach(Tower t in e.towers)
                    if(bulletRectangle.Intersects(t.GetRectangle()))
                    {
                        b.Visible = false;
                        eh.AddExplosion(b.Position, t.contentManager, 5, 15, "explosion", 32, 0.33f);
                    }
            }
        }

        public void HandleItemCollision(Player p, ItemHandler i, ExplosionHandler explosions, Level l)
        {
            if (p.Jumping || p.Falling)
                playerRectangle = new Rectangle((int)(p.X + 24 * p.Scale), (int)p.Y, (int)(9 * p.Scale), (int)(48 * p.Scale));
            else
            {
                playerRectangle = p.Crouching ? new Rectangle((int)(p.X + 24 * p.Scale), (int)(p.Y + 9 * p.Scale), (int)(9 * p.Scale), (int)(50 * p.Scale))
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

            
            foreach (FirstAid f in i.firstAids)
            {
                f.Falling = true;
                firstAidRectangle = new Rectangle((int)f.X + 1, (int)f.Y + 3, 14, 12);
                foreach (MacroBlock mb in l.levelBlocks)
                {
                    if (firstAidRectangle.Intersects(mb.GetRectangle()))
                        f.Falling = false;
                }
                if(playerRectangle.Intersects(firstAidRectangle))
                {
                    if (p.Hitpoints + firstAidHealth <= 100)
                        p.Hitpoints += firstAidHealth; 
                    else
                        p.Hitpoints = 100;
                    f.Visible = false;
                }
            }

            foreach (Acid a in i.acidBalls)
            {
                a.Falling = true;
                acidRectangle = new Rectangle((int)a.X - 6, (int)a.Y - 6, 10, 10);
                foreach (MacroBlock mb in l.levelBlocks)
                {
                    if (acidRectangle.Intersects(mb.GetRectangle()))
                        a.Falling = false;
                }
                if(playerRectangle.Intersects(acidRectangle) && !a.Exploded)
                {
                    p.Hitpoints -= random.Next(minZombieDmg, maxZombieDmg) / 2;
                    explosions.AddExplosion(new Vector2(a.Position.X - 8, a.Position.Y - 8), a.contentManager, 9, 27, "acid", 16);
                    a.Exploded = true;
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
