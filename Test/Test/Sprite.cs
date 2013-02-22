using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    class Sprite
    {
        public Texture2D texture;
        private Rectangle source;
        private Rectangle size;
        private float scale = 1.0f;
        public Vector2 Position = Vector2.Zero;
        public ContentManager contentManager;

        public float Rotation { get; set; }

        public float X
        {
            get { return this.Position.X; }
            set { this.Position.X = value; }
        }

        public float Y
        {
            get { return this.Position.Y; }
            set { this.Position.Y = value; }
        }

        public Color Color { get; set; }

        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                size = new Rectangle(0, 0, (int)(source.Width * scale), (int)(source.Height * scale));
            }
        }

        public Rectangle Source
        {
            get { return this.source; }
            set
            {
                source = value;
                size = new Rectangle(0, 0, source.Width, source.Height);
            }
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            texture = theContentManager.Load<Texture2D>(theAssetName);
            Source = new Rectangle(0, 0, (int)(texture.Width * Scale), (int)(texture.Height * scale));
            size = new Rectangle(0, 0, (int)(texture.Width * Scale), (int)(texture.Height * scale));
        }

        public virtual void Draw(SpriteBatch theSpriteBatch, Vector2 origin, Vector2 position, Color color, float rotation)
        {
            theSpriteBatch.Draw(texture, Position, Source, color, rotation, origin, Scale, SpriteEffects.None, 0);
        }
    }
}