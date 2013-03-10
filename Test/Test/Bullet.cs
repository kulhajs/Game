using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    class Bullet : Sprite
    {

        Vector2 velocity = new Vector2(600, 600);
        Vector2 direction;

        public bool Visible { get; set; }

        public Bullet(Vector2 position, Vector2 direction, float rotation)
        {
            this.Position = position;
            this.direction = direction;
            this.Rotation = rotation;
            this.Visible = true;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, "bullet");
        }

        public void Update(GameTime theGameTime)
        {
            Position += direction * velocity * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch, Vector2.Zero, this.Position, Color.Gold, this.Rotation);
        }
    }
}
