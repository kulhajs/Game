using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Test
{
    class Camera
    {
        public Matrix transform;
        public Vector2 origin;
        Viewport view;

        public Camera(Viewport view)
        {
            this.view = view;
            origin = new Vector2(view.Width / 2, view.Height / 2);
        }

        public void Update(Player player)
        {
            if (player.X < 400)
                origin.X = 0;
            else
                origin.X = player.X - 400;

            if (player.Y < 64)
                origin.Y = player.Y - 64;
            else
                origin.Y = 0;

            transform = Matrix.CreateTranslation(new Vector3(-origin.X, -origin.Y, 0));
        }
    }
}
