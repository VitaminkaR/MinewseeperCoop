using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.Text.Json;

namespace MinewseeperCoop
{
    public class Map : DrawableGameComponent
    {
        Random rand;
        SpriteBatch spriteBatch;
        bool isPress;
        bool isPressR;

        Texture2D[] textures;

        public int[,] Field { get; private set; }
        private int sizeW;
        private int sizeH;
        private int bombsCount;

        public Map(Game game) : base(game)
        {
            game.Components.Add(this);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            rand = new Random();
        }

        // загрузка текстур
        public void ContentLoad(ContentManager content)
        {
            textures = new Texture2D[12];
            textures[0] = content.Load<Texture2D>("close");
            textures[1] = content.Load<Texture2D>("open");
            textures[2] = content.Load<Texture2D>("cell_1");
            textures[3] = content.Load<Texture2D>("cell_2");
            textures[4] = content.Load<Texture2D>("cell_3");
            textures[5] = content.Load<Texture2D>("cell_4");
            textures[6] = content.Load<Texture2D>("cell_5");
            textures[7] = content.Load<Texture2D>("cell_6");
            textures[8] = content.Load<Texture2D>("cell_7");
            textures[9] = content.Load<Texture2D>("cell_8");
            textures[10] = content.Load<Texture2D>("bomb");
            textures[11] = content.Load<Texture2D>("flag");
        }

        // генерация поля
        public void Generate(int _w, int _h, int _b)
        {
            Field = new int[_w, _h];
            sizeW = _w;
            sizeH = _h;
            bombsCount = _b;

            for (int w = 0; w < _w; w++)
            {
                for (int h = 0; h < _h; h++)
                {
                    Field[w, h] = 9;
                }
            }

            if (Minewseeper.minewseeper.Host)
            {
                // генерация бомб
                int b = 0;
                while (b < _b)
                {
                    int w = rand.Next(0, _w);
                    int h = rand.Next(0, _h);

                    if (Field[w, h] != 10)
                    {
                        Field[w, h] = 10;
                        b++;
                    }
                }
                SendMap();
            }
        }



        // фокус ячейки (когда наводишься она подсвечивается)
        private Color Focus(int _w, int _h)
        {
            MouseState ms = Mouse.GetState();
            int borderX = 32 * sizeW;
            int borderY = 32 * sizeH;
            int mX = ms.X;
            int mY = ms.Y;
            if (mX > 0 && mX < borderX && mY > 0 && mY < borderY)
            {
                int w = mX / 32;
                int h = mY / 32;

                if (w == _w && h == _h)
                    return Color.Gray;
            }
            return Color.White;
        }

        // нажатие на ячейку (вызов открытия)
        private void SetOpen()
        {
            MouseState ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed && !isPress)
            {
                isPress = true;
                int borderX = 32 * sizeW;
                int borderY = 32 * sizeH;
                int mX = ms.X;
                int mY = ms.Y;
                if (mX > 0 && mX < borderX && mY > 0 && mY < borderY)
                {
                    int w = mX / 32;
                    int h = mY / 32;
                    if (Field[w, h] != 9 && Field[w, h] != 10 && Field[w, h] != 11 && Field[w, h] != 12 && Field[w, h] != 13)
                        Copilot(w, h);
                    if (Field[w, h] == 9 || Field[w, h] == 10 && Field[w, h] != 12 && Field[w, h] != 13)
                        Open(w, h);
                }
            }
            if (ms.LeftButton == ButtonState.Released)
                isPress = false;
        }

        // открытие клетки
        private void Open(int w, int h)
        {
            if (Field[w, h] != 10)
            {
                int bombs = BombCount(w, h);
                Field[w, h] = bombs;
                if (bombs == 0)
                {
                    for (int i = w - 1; i <= w + 1; i++)
                    {
                        for (int j = h - 1; j <= h + 1; j++)
                        {
                            if (i != w || j != h)
                            {
                                if (i >= 0 && j >= 0 && i < sizeW && j < sizeH)
                                {
                                    if (Field[i, j] == 9)
                                        Open(i, j);
                                }
                            }
                        }
                    }
                }
            }

            if (Field[w, h] == 10)
            {
                Field[w, h] = 11;
                Lose();
            }
        }

        // подсчет бомб
        private int BombCount(int w, int h)
        {
            int bombs = 0;
            for (int i = w - 1; i <= w + 1; i++)
            {
                for (int j = h - 1; j <= h + 1; j++)
                {
                    if (i != w || j != h)
                    {
                        if (i >= 0 && j >= 0 && i < sizeW && j < sizeH)
                        {
                            if (Field[i, j] == 10 || Field[i, j] == 11 || Field[i, j] == 13)
                                bombs++;
                        }
                    }
                }
            }
            return bombs;
        }

        // поставить флажок
        private void SetFlag()
        {
            MouseState ms = Mouse.GetState();
            if (ms.RightButton == ButtonState.Released)
                isPressR = false;
            if (ms.RightButton == ButtonState.Pressed && !isPressR)
            {
                isPressR = true;
                int borderX = 32 * sizeW;
                int borderY = 32 * sizeH;
                int mX = ms.X;
                int mY = ms.Y;
                if (mX > 0 && mX < borderX && mY > 0 && mY < borderY)
                {
                    int w = mX / 32;
                    int h = mY / 32;
                    if (Field[w, h] == 12)
                    {
                        Field[w, h] = 9;
                        return;
                    }
                    if (Field[w, h] == 13)
                    {
                        Field[w, h] = 10;
                        return;
                    }
                    if (Field[w, h] == 9)
                    {
                        Field[w, h] = 12;
                        return;
                    }
                    if (Field[w, h] == 10)
                    {
                        Field[w, h] = 13;
                        return;
                    }
                }
            }
        }

        // помощь игроку (октрытие клеток автоматически)
        private void Copilot(int w, int h)
        {
            int bombs = BombCount(w, h);
            int flags = 0;
            for (int i = w - 1; i <= w + 1; i++)
            {
                for (int j = h - 1; j <= h + 1; j++)
                {
                    if (i >= 0 && j >= 0 && i < sizeW && j < sizeH)
                    {
                        if (Field[i, j] == 12 || Field[i, j] == 13)
                            flags++;
                    }
                }
            }

            if (flags > bombs)
                Lose();
            if (flags == bombs)
            {
                for (int i = w - 1; i <= w + 1; i++)
                {
                    for (int j = h - 1; j <= h + 1; j++)
                    {
                        if (i >= 0 && j >= 0 && i < sizeW && j < sizeH)
                        {
                            if (Field[i, j] == 9)
                                Open(i, j);
                        }
                    }
                }
            }
        }



        // проигрыш
        private void Lose() => Minewseeper.minewseeper.gameState = GameState.lose;

        // выйгрыш
        private void CheckWin()
        {
            int cells = 0;
            for (int w = 0; w < sizeW; w++)
            {
                for (int h = 0; h < sizeH; h++)
                {
                    if (Field[w, h] != 9 && Field[w, h] != 10 && Minewseeper.minewseeper.gameState != GameState.lose)
                        cells += 1;
                }
            }
            if (cells == sizeH * sizeW)
                Minewseeper.minewseeper.gameState = GameState.win;
        }

        // рестарт
        private void Restart()
        {
            Minewseeper.minewseeper.gameState = GameState.game;
            Generate(sizeW, sizeH, bombsCount);
        }



        // отправляет пользователям карту (для оптимизации достаточно оптравить лишь позиции бомб)
        public void SendMap()
        {
            string[] bombsPos = new string[bombsCount];

            for (int w = 0; w < sizeW; w++)
            {
                for (int h = 0; h < sizeH; h++)
                {
                    if (Field[w, h] == 10)
                    {
                        for (int i = 0; i < bombsCount; i++)
                        {
                            if (bombsPos[i] == null || bombsPos[i] == "")
                            {
                                bombsPos[i] = w.ToString() + ':' + h.ToString();
                                break;
                            }
                        }
                    }
                }
            }

            string msg = "map=" + JsonSerializer.Serialize(bombsPos, typeof(string[]));
            Minewseeper.minewseeper.server?.Send(msg);
        }



        public override void Update(GameTime gameTime)
        {
            SetOpen();
            SetFlag();
            CheckWin();

            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.R))
                Restart();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            for (int w = 0; w < sizeW; w++)
            {
                for (int h = 0; h < sizeH; h++)
                {
                    Texture2D sprite = new Texture2D(GraphicsDevice, 32, 32);

                    Color color = Color.White;
                    if (Minewseeper.minewseeper.gameState == GameState.game)
                        color = Focus(w, h);
                    else if (Minewseeper.minewseeper.gameState == GameState.lose)
                        color = Color.Red;
                    else
                        color = Color.LightGreen;


                    // выбор спрайта от id
                    if (Field[w, h] == 9 || Field[w, h] == 10)
                        sprite = textures[0];
                    if (Field[w, h] == 0)
                        sprite = textures[1];
                    if (Field[w, h] == 1)
                        sprite = textures[2];
                    if (Field[w, h] == 2)
                        sprite = textures[3];
                    if (Field[w, h] == 3)
                        sprite = textures[4];
                    if (Field[w, h] == 4)
                        sprite = textures[5];
                    if (Field[w, h] == 5)
                        sprite = textures[6];
                    if (Field[w, h] == 6)
                        sprite = textures[7];
                    if (Field[w, h] == 7)
                        sprite = textures[8];
                    if (Field[w, h] == 8)
                        sprite = textures[9];
                    if (Field[w, h] == 11)
                        sprite = textures[10];
                    if (Field[w, h] == 12 || Field[w, h] == 13)
                        sprite = textures[11];

                    spriteBatch.Begin();
                    spriteBatch.Draw(sprite, new Vector2(w * 32, h * 32), color);
                    spriteBatch.End();
                }
            }

            base.Draw(gameTime);
        }
    }
}
