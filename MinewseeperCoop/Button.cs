using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MinewseeperCoop
{
    class Button : DrawableGameComponent
    {
        private Rectangle collider;
        private Vector2 pos;
        private Texture2D texture;
        private SpriteBatch spriteBatch;

        private bool focus;
        private bool press;

        public delegate void ClickD();
        public event ClickD Click;

        public Button(Game game, int x, int y, Texture2D texture) : base(game)
        {
            game.Components.Add(this);
            pos = new Vector2(x, y);
            collider = new Rectangle(x, y, texture.Width, texture.Height);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.texture = texture;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            Rectangle cursor = new Rectangle(ms.Position, new Point(2, 2));
            if (collider.Intersects(cursor))
                focus = true;
            else
                focus = false;

            if (focus && ms.LeftButton == ButtonState.Pressed && !press)
            {
                press = true;
                Click?.Invoke();
            }
            if (ms.LeftButton == ButtonState.Released)
                press = false;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Color color = Color.White;
            if (focus && !press)
                color = Color.Gray;

            spriteBatch.Begin();
            spriteBatch.Draw(texture, pos, color);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
