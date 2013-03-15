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
    class FirstAid : Sprite
    {

        public bool Falling { get; set; }

        public bool Visible { get; set; }

        public bool Slide { get; set; }

        public Vector2 direction = Vector2.Zero;
        Vector2 velocity = Vector2.Zero;

        Random random = new Random();

        const float gravity = 9.81f;

        float friction;

        public FirstAid(Vector2 position, float dx, float friction)
        {
            velocity.X = random.Next(50, 75);
            this.direction.X = dx;
            this.friction = friction;
            
            this.Slide = false;
            this.Visible = true;
            this.Falling = true;
            this.Position = position;
        }

        public virtual void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, "firstAid");
        }

        public virtual void Update(GameTime theGameTime)
        {
            if (Falling)
            {
                direction.Y = 1;
                velocity.Y += gravity;
            }
            else
            {
                direction.Y = 0;
                this.Slide = true;
            }

            if (Slide)
            {
                if (direction.X < 0.1f)
                    direction.X -= direction.X * friction;
                else if (direction.X > 0.1f)
                    direction.X -= direction.X * friction;
                else direction.X = 0;
            }

            Position += direction * velocity * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (this.Visible)
                base.Draw(theSpriteBatch, Vector2.Zero, this.Position, Color.White, 0.0f);
        }
    }
}
