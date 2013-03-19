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
    class Acid : FirstAid
    {
        Random random = new Random();

        public bool Exploded { get; set; }

        public bool Visible { get; set; }

        float lifeTime;

        public Acid(Vector2 position, float dx, float friction)
            : base(position, dx, friction)
        {
            this.Exploded = false;
            this.Rotation = 0.0f;
            this.lifeTime = (float)(random.Next(30, 50) / 10);
            this.Visible = true;
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, "acid_ball");
            this.Source = new Rectangle(0, 0, 16, 16);
        }

        public override void Update(GameTime theGameTime)
        {
            base.Update(theGameTime);

            if (direction.X > 0 && direction.Y == 0)
                this.Rotation += direction.X;
            else if (base.direction.X < 0 && direction.Y == 0)
                this.Rotation += direction.X;
            
            if (Exploded)
            {
                this.Source = new Rectangle(16, 0, 16, 16);
                this.Rotation = 0.0f;
                if (lifeTime > 0.0f)
                    lifeTime -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
                else
                    this.Visible = false;
            }
        }

        public void Draw(SpriteBatch theSpritebatch)
        {
            theSpritebatch.Draw(this.texture, new Vector2(this.X, this.Y), this.Source, Color.White, this.Rotation, new Vector2(8, 8), 1.0f, SpriteEffects.None, 0f);
        }
    }
}
