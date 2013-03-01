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
        Vector2 velocity = new Vector2(450, 450);
        Vector2 direction;

        Random random = new Random();

        float delta = 0.015f; //0.025f

        float lifeTime;

        const int animationLenght = 9;
        int currentFrame = 0;
        
        Rectangle[] sources = new Rectangle[] {
            new Rectangle(0,0,16,5),
            new Rectangle(0,5,16,5),
            new Rectangle(0,10,16,5)
        };

        public bool Visible { get; set; }

        public Rocket(Vector2 position, Vector2 direction, float rotation)
        {
            this.Position = position;
            this.direction = direction;
            this.Rotation = rotation;
            this.Visible = true;
            lifeTime = (float)(random.NextDouble() * 2 + 1.0);
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, "rocket");
        }

        public void Update(GameTime theGameTime, Vector2 newDirection)
        {
            if (lifeTime > 0.0f)
                lifeTime -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
            else
                Visible = false;

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
                this.Rotation = this.FAtan(direction.Y / direction.X);
            else
                this.Rotation = this.FAtan(direction.Y / direction.X) + this.FPI;

            Position += velocity * direction * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            
            this.Animate();
        }

        private void Animate()
        {
            if (currentFrame < 2 * animationLenght / 3)
                Source = sources[0];
            else if (currentFrame < animationLenght / 3)
                Source = sources[1];
            else if (currentFrame < animationLenght)
                Source = sources[2];
            else
                currentFrame = 0;

            currentFrame++;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch, Vector2.Zero, this.Position, Color.White, this.Rotation);
        }
    }
}
