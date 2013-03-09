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
    class Zombie : Sprite
    {

        Vector2 direction;
        Vector2 velocity = new Vector2(75, 0);

        Texture2D body;

        Facing currentFacing;

        Rectangle[] sources = new Rectangle[] { 
            new Rectangle(0,0,64,64),
            new Rectangle(64,0,64,64),
            new Rectangle(128,0,64,64),
        };

        int currentFrame = 0;
        int animationLength = 32;

        public float realoadTime = 0.0f;
        
        float initReloadTime = 0.5f;

        const float gravity = 8f;

        public bool Falling { get; set; }

        public bool Collide { get; set; }

        public bool CanBite { get; set; }

        public float DX { get { return direction.X; } set { direction.X = value; } }

        public float DY { get { return direction.Y; } set { direction.Y = value; } }

        public Zombie(Vector2 position, Vector2 direction)
        {
            this.Position = position;
            this.direction = direction;
            this.Falling = true;
            this.CanBite = true;
            this.Collide = false;
            this.Scale = 0.667f;
            if (direction.X < 0) currentFacing = Facing.Left; else currentFacing = Facing.Right;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            body = contentManager.Load<Texture2D>("zombie");
            Source = sources[0];
        }

        public void Update(GameTime theGameTime)
        {
            if (realoadTime < initReloadTime)
                realoadTime += (float)theGameTime.ElapsedGameTime.TotalSeconds;

            CanBite = realoadTime >= initReloadTime ? true : false;

            if (Falling)
            {
                DY = 1f;
                velocity.Y += gravity;
            }
            else
                velocity.Y = 0;

            Position += direction * velocity * (float)theGameTime.ElapsedGameTime.TotalSeconds;

            this.Animate();
        }

        private void Animate()
        {
            if (currentFrame < animationLength / 4)
                this.Source = sources[2];
            else if (currentFrame < 2 * animationLength / 4)
                this.Source = sources[1];
            else if (currentFrame < 3 * animationLength / 4)
                this.Source = sources[0];
            else
                currentFrame = 0;

            currentFrame++;
        }

        public void Draw(SpriteBatch theSpritebatch)
        {
            theSpritebatch.Draw(body, this.Position, this.Source, Color.White, 0.0f, Vector2.Zero, this.Scale, currentFacing == Facing.Left ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
        }
    }
}
