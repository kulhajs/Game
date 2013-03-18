using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Test
{
    class ExplosionHandler
    {
        public List<Explosion> explosions;
        Explosion newExplosion;

        public bool ShouldRemove { get; set; }

        public ExplosionHandler()
        {
            explosions = new List<Explosion>(50);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="theContentManager"></param>
        /// <param name="explosionType">"blood", "explosion", "acid"</param>
        /// <param name="animationLength">count of all frames drawn on screen (blood, explosion = 15; acid = 27)</param>
        /// <param name="frameSize">width & height of one animation frame - assuming frame is squared (blood, explosion = 32; acid = 16)</param>
        /// <param name="sourceCount">number of animation frames (blood = 4; explosion = 5; acid = 9)</param>
        public void AddExplosion(Vector2 position, ContentManager theContentManager, int sourceCount, int animationLength, string explosionType, int frameSize)
        {
            newExplosion = new Explosion(position, sourceCount, animationLength, explosionType, frameSize);
            newExplosion.LoadContent(theContentManager);
            explosions.Add(newExplosion);
            newExplosion = null;
        }

        public void Update()
        {
            if (explosions.Count > 0)
                foreach (Explosion e in explosions)
                    if (e.Visible)
                        e.Animate();
                    else
                        ShouldRemove = true;

            if (ShouldRemove)
                this.RemoveExplosion();
        }

        private void RemoveExplosion()
        {
            foreach(Explosion e in explosions)
                if(!e.Visible)
                {
                    explosions.Remove(e);
                    break;
                }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (explosions.Count > 0)
                foreach (Explosion e in explosions)
                    if (e.Visible)
                        e.Draw(theSpriteBatch);
        }
    }
}
