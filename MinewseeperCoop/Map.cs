using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace MinewseeperCoop
{
    public class Map : DrawableGameComponent
    {
        Random rand;
        SpriteBatch spriteBatch;

        Texture2D[] textures;

        public int[,] Field { get; private set; }

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

            for (int w = 0; w < _w; w++)
            {
                for (int h = 0; h < _h; h++)
                {
                    Field[w, h] = 9;
                }
            }

            // генерация бомб
            int b = 0;
            while(b < _b) 
            {
                int w = rand.Next(0, _w);
                int h = rand.Next(0, _h);

                if(Field[w, h] != 10)
                {
                    Field[w, h] = 10;
                    b++;
                }
            } 
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            for (int w = 0; w < Field.GetLength(0); w++)
            {
                for (int h = 0; h < Field.GetLength(1); h++)
                {
                    Texture2D sprite = new Texture2D(GraphicsDevice, 32, 32);

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
                    spriteBatch.Draw(sprite, new Vector2(w * 32, h * 32), Color.White);
                    spriteBatch.End();
                }
            }

            base.Draw(gameTime);
        }
    }
}
