using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace MinewseeperCoop
{
    class TextInput : DrawableGameComponent
    {
        private Rectangle collider;
        private Vector2 pos;
        private Texture2D texture;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Timer writeInterval;

        private bool focus;
        private bool input;
        private bool press;
        public string text;
        public bool pressK;
        private bool isWrite = true;

        public TextInput(Game game, int x, int y, Texture2D texture, SpriteFont font) : base(game)
        {
            game.Components.Add(this);
            pos = new Vector2(x, y);
            collider = new Rectangle(x, y, texture.Width, texture.Height);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.texture = texture;
            this.font = font;
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
                input = true;
            }
            else if (ms.LeftButton == ButtonState.Pressed && !press && !focus)
            {
                press = true;
                input = false;
            }
                
            if (ms.LeftButton == ButtonState.Released)
                press = false;


            KeyboardState ks = Keyboard.GetState();
            if (input)
            {
                if(ks.GetPressedKeys().Length > 0 && !ks.IsKeyDown(Keys.LeftControl) && isWrite)
                {
                    Keys key = ks.GetPressedKeys()[0];
                    text = KeyParse.Get(key);

                    isWrite = false;
                    writeInterval = new Timer(new TimerCallback((obj) => { isWrite = true; writeInterval.Dispose(); }), null, 100, 0);
                }
                    
                if (ks.IsKeyDown(Keys.Back))
                    text = "";

                if(ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.V) && !pressK)
                {
                    text = System.Windows.Forms.Clipboard.GetText();
                    pressK = true;
                }
            }

            if (pressK)
                if (ks.GetPressedKeys().Length == 0)
                    pressK = false;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Color color = Color.White;
            if (input || focus && !press)
                color = Color.Gray;

            spriteBatch.Begin();
            spriteBatch.Draw(texture, pos, color);
            if(text != "" && text != null)
                spriteBatch.DrawString(font, text, new Vector2(pos.X + 8, pos.Y + texture.Height / 3), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
