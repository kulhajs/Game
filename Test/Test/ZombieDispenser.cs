using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Test
{
    class ZombieDispenser : Sprite
    {

        Random random = new Random();

        public List<Zombie> zombies;
        Zombie newZombie;

        Rectangle[] sources = new Rectangle[] { 
            new Rectangle(0,0,64,64),
            new Rectangle(64,0,64,64),
            new Rectangle(128,0,64,64),
            new Rectangle(192,0,64,64)
        };

        int currentFrame = 0;
        int animationLength = 16;

        public ZombieDispenser(Vector2 position)
        {
            this.Position = position;
            zombies = new List<Zombie>(25);
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, "zombie_dispenser");
            Source = sources[0];
        }

        public void Update(GameTime theGameTime, ExplosionHandler explosions, ItemHandler ih)
        {
            if(random.Next(200) == 1)
            {
                if (random.Next(2) == 0) newZombie = new Zombie(new Vector2(this.X + 8, this.Y + 48), new Vector2(-1, 1)); else newZombie = new Zombie(new Vector2(this.X + 8, this.Y + 48), new Vector2(1, 1));
                newZombie.LoadContent(contentManager);
                zombies.Add(newZombie);
                newZombie = null;
            }

            foreach (Zombie z in zombies)
                if (z.Visible)
                    z.Update(theGameTime);


            this.RemoveZombies(explosions, ih);
            this.Animate();
        }

        private void RemoveZombies(ExplosionHandler explosions, ItemHandler ih)
        {
            foreach (Zombie z in zombies)
                if (z.Y > 550 || !z.Visible)
                {
                    if (random.Next(5) == 1)
                        ih.AddFirstAid(z.Position, z.DX);
                    else if (random.Next(5) % 2 == 0)
                        ih.AddAcid(z.Position, z.DX);
                    explosions.AddExplosion(new Vector2(z.X + 20 * z.Scale, z.Y + 10 * z.Scale), contentManager, "blood", "normal", 15);
                    zombies.Remove(z);
                    break;
                }
        }

        private void Animate()
        {
            if (currentFrame < animationLength / 4)
                this.Source = sources[0];
            else if (currentFrame < 2 * animationLength / 4)
                this.Source = sources[1];
            else if (currentFrame < 3 * animationLength / 4)
                this.Source = sources[2];
            else if (currentFrame < animationLength)
                this.Source = sources[3];
            else
                currentFrame = 0;

            currentFrame++;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Zombie z in zombies)
                if (z.Visible)
                    z.Draw(theSpriteBatch);

            base.Draw(theSpriteBatch, Vector2.Zero, this.Position, Color.White, 0.0f);
        }

    }
}
