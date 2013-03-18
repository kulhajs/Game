using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    class Block : Sprite
    {
        Rectangle[] sources = new Rectangle[]{
            new Rectangle(0,0,64,24), 
            new Rectangle(64,0,64,24),
            new Rectangle(128,0,64,24),
            new Rectangle(192,0,64,24)
        };

        int id;
        string blockType;

        public Block(int id, Vector2 initPosition, string blockType)
        {
            this.id = id;
            this.blockType = blockType;
            this.Position = initPosition;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            contentManager = theContentManager;
            base.LoadContent(contentManager, blockType);
            this.Source = sources[id];
        }

        public void Draw(SpriteBatch theSpritebatch)
        {
            base.Draw(theSpritebatch, Vector2.Zero, this.Position, Color.White, 0.0f);
        }
    }
}
