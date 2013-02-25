using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    class FlashDoor : Sprite
    {
        Texture2D _switch;
        Texture2D door;

        int currentFrame = 0;
        int animationLenght = 16;

        Vector2 switchPosition;

        Rectangle[] sources = new Rectangle[] {
            new Rectangle(0,0,64,192),
            new Rectangle(64,0,64,192),
            new Rectangle(0,192,64,64),
            new Rectangle(64,192,64,64)
        };

        public bool Switch { get; set; }

        public FlashDoor(Vector2 position, Vector2 switchPosition)
        {
            this.Position = position;
            this.switchPosition = switchPosition;
            this.Switch = true;
        }

        public void Update(GameTime theGameTime)
        {
            this.Animate();
        }

        private void Animate()
        {
            throw new NotImplementedException();
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            _switch = contentManager.Load<Texture2D>("flashDoor");
            door = contentManager.Load<Texture2D>("flashDoor");
            Source = sources[0];
        }
        
        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(_switch, switchPosition, Switch ? sources[2] : sources[3], Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            theSpriteBatch.Draw(door, Position, Source, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
