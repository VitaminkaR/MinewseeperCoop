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
        internal Server server;
        internal Client client;

        public GameState gameState { get; internal set; }
        public bool Host { get; internal set; } = true;

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
            Exiting += (object sender, System.EventArgs e) => {
                server.Stop();
                client.Disconnect();
            };

            map = new Map(this);
            map.Generate(9, 9, 10);

            baseLog = new Log();
            baseLog.Set("");

            server = new Server();
            server.StartServer();

            if (Host)
            {
                server.NewClientConnectedEvent += (System.Net.Sockets.NetworkStream stream) => map.SendMap();
            }

            client = new Client();
            client.Connect();

            

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
            int y = map.Field.GetLength(1) * 32 + 32;
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, " Logs:\n", new Vector2(0, y), Color.White);
            _spriteBatch.DrawString(font, baseLog.Get(), new Vector2(0, y + 32), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
