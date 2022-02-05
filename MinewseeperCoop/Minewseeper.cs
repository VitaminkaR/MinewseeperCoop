using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MinewseeperCoop
{
    public class Minewseeper : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        static internal Minewseeper minewseeper;

        public Map map;
        public Log baseLog;

        public GameState gameState { get; internal set; }

        SpriteFont font;


        public Minewseeper()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            minewseeper = this;

            _graphics.PreferredBackBufferWidth = Options.WIDTH;
            _graphics.PreferredBackBufferHeight = Options.HEIGHT;
        }

        protected override void Initialize()
        {
            map = new Map(this);
            map.Generate(9, 9, 10);

            baseLog = new Log();
            baseLog.Set("Logs:\n");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            map.ContentLoad(Content);
            font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            // log
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, baseLog.Get(), new Vector2(0, map.Field.GetLength(1) * 32 + 32), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
