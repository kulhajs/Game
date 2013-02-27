using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    class Rocket : Sprite
    {
        Vector2 velocity = new Vector2(250, 250);
        Vector2 direction;

        float delta = 0.025f;

        public Rocket(Vector2 position,Vector2 direction ,float rotation)
        {
            this.Position = position;
            this.direction = direction;
            this.Rotation = rotation;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, "rocket");
        }

        public void Update(GameTime theGameTime, Vector2 newDirection)
        {
            newDirection.Normalize();

            if (newDirection.X > direction.X)
                direction.X += delta;
             if (newDirection.X < direction.X)
                direction.X -= delta;

            if (newDirection.Y > direction.Y)
                direction.Y += delta;
            if (newDirection.Y < direction.Y)
                direction.Y -= delta;

            if (direction.X < 0)
                this.Rotation = (float)Math.Atan((double)(direction.Y / direction.X));
            else
                this.Rotation = (float)Math.Atan((double)(direction.Y / direction.X)) + (float)Math.PI;

            Position += velocity * direction * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch, Vector2.Zero, this.Position, Color.White, this.Rotation);
        }
    }
}
