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
        /// <param name="explosionType">"blood", "explosion"</param>
        public void AddExplosion(Vector2 position, ContentManager theContentManager, string explosionType)
        {
            newExplosion = new Explosion(position, explosionType);
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
