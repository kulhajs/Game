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
        SoundEffect pickUp;

        public void LoadContent(ContentManager theContentManager)
        {
            explosion = theContentManager.Load<SoundEffect>("Sounds/explosion");
            hurt = theContentManager.Load<SoundEffect>("Sounds/hurt");
            pickUp = theContentManager.Load<SoundEffect>("Sounds/pick");
        }

        public void PlayExplosion(Player p, Rocket r)
        {
            float volume = (-(FAbs(r.X - p.X) / 400f) + 1) / 5;
            explosion.Play(volume > 0.0f ? volume : 0.0f, 0.0f, r.X - p.X > 0 ? 0.25f : -0.25f);
        }

        public void PlayHurt(Player p, Zombie z=null)
        {
            if (z != null)
                hurt.Play(0.25f, 0.0f, z.X - p.X > 0 ? 0.20f : -0.20f);
            else
                hurt.Play(0.15f, 0.0f, 0.0f);
        }

        public void PlayPickUp()
        {
            pickUp.Play(0.25f, 0.0f, 0.0f);
        }

        private float FAbs(float x)
        {
            return (float)Math.Abs((double)x);
        }
    }
}
