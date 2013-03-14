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

        Vector2 velocity = Vector2.Zero;
        Vector2 direction = Vector2.Zero;

        public bool Exploded { get; set; }

        public Acid(Vector2 position, float dx, float friction)
            : base(position, dx, friction)
        {
            this.Exploded = false;
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
            if (this.direction.X > 0)
                this.Rotation += direction.X * 3;
            else if (this.direction.X < 0)
                this.Rotation -= direction.X * 3;
            
            if (Exploded)
                this.Source = new Rectangle(16, 0, 16, 16);
        }

        public override void Draw(SpriteBatch theSpritebatch)
        {
            base.Draw(theSpritebatch);
        }
    }
}
