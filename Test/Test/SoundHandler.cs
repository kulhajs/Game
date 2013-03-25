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
    class SoundHandler
    {
        SoundEffect explosion;
        SoundEffect hurt;

        public void LoadContent(ContentManager theContentManager)
        {
            explosion = theContentManager.Load<SoundEffect>("Sounds/explosion");
            hurt = theContentManager.Load<SoundEffect>("Sounds/hurt");
        }

        public void PlayExplosion(Player p, Rocket r)
        {
            float volume = (-(FAbs(r.X - p.X) / 400f) + 1) / 4;
            float pan = ((r.X - p.X) / 400f) / 3.33f;
            explosion.Play(volume > 0.0f ? volume : 0.0f, 0.0f, pan);
        }

        public void PlayHurt(Player p, Zombie z)
        {
            hurt.Play(0.25f, 0.0f, z.X - p.X > 0 ? 0.33f : -0.33f);
        }

        private float FAbs(float x)
        {
            return (float)Math.Abs((double)x);
        }
    }
}
