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

        //ui
        Texture2D startServerButtonTexture;
        Button startServerButton;
        Texture2D connectButtonTexture;
        Button connectButton;
        Texture2D restartButtonTexture;
        Button restartButton;
        Texture2D textInputTexture;
        TextInput ipInput;

        public GameState gameState { get; internal set; }
        public bool Host { get; internal set; }

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
                server?.Stop();
                client?.Disconnect();
            };

            map = new Map(this);
            map.Generate(12, 12, 10);

            baseLog = new Log();
            baseLog.Set("");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            map.ContentLoad(Content);
            font = Content.Load<SpriteFont>("font");

            //ui
            startServerButtonTexture = Content.Load<Texture2D>("start server");
            startServerButton = new Button(this, 400, 0, startServerButtonTexture);
            startServerButton.Click += () => StartServer();
            connectButtonTexture = Content.Load<Texture2D>("connect");
            connectButton = new Button(this, 400, 64, connectButtonTexture);
            connectButton.Click += () => Connect();
            restartButtonTexture = Content.Load<Texture2D>("restart");
            restartButton = new Button(this, 400, 64 * 2 + 16, restartButtonTexture);
            restartButton.Click += () => client.SendRestart();

            textInputTexture = Content.Load<Texture2D>("input");
            ipInput = new TextInput(this, 400 + 256, 0, textInputTexture, font);
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



        private void StartServer()
        {
            if(ipInput.text != null)
                server = new Server(ipInput.text);
            else
                server = new Server();
            Host = server.StartServer();
            
            if (Host)
            {
                map.Generate(12, 12, 10, false);
                server.NewClientConnectedEvent += (System.Net.Sockets.NetworkStream stream) => map.SendFMap();
            }
        }

        private void Connect()
        {
            if (!Host)
            {
                map.Generate(9, 9, 10);
            }

            client = new Client();
            if (ipInput.text != null)
                client.Connect(ipInput.text);
            else
                client.Connect();
        }
    }
}
