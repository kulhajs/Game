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
        int animationLenght = 6;

        int switchFrame = 0;
        int switchAnimationLength = 24;

        Vector2 switchPosition;
        Rectangle switchSource;

        Rectangle[] sources = new Rectangle[] {
            new Rectangle(0,0,64,136),
            new Rectangle(64,0,64,136),
            new Rectangle(128,0,64,136),
            new Rectangle(0,136,64,64),
            new Rectangle(0,200,64,64),
            new Rectangle(64,136,64,64),
            new Rectangle(64,200,64,64)
        };

        public bool Switch { get; set; }

        public Vector2 SPosition { get { return this.switchPosition; } }

        public FlashDoor(Vector2 position, Vector2 switchPosition)
        {
            this.Position = position;
            this.switchPosition = switchPosition;
            this.Switch = true;
        }

        public void Update(GameTime theGameTime)
        {
            this.AnimateDoor();
            this.AnimateSwitch();
        }

        private void AnimateSwitch()
        {
            if (switchFrame < switchAnimationLength / 2)
                if (Switch) switchSource = sources[3]; else switchSource = sources[5];
            else if (switchFrame < switchAnimationLength)
                if (Switch) switchSource = sources[4]; else switchSource = sources[6];
            else
                switchFrame = 0;

            switchFrame++;
        }

        private void AnimateDoor()
        {
            if (currentFrame < animationLenght / 2)
                Source = sources[0];
            else if (currentFrame < animationLenght)
                Source = sources[1];
            else
                currentFrame = 0;
            
            currentFrame++;
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
            theSpriteBatch.Draw(_switch, switchPosition, switchSource, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            theSpriteBatch.Draw(door, Position, Switch ? Source : sources[2], Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
