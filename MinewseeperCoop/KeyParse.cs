using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MinewseeperCoop
{
    static class KeyParse
    {
        public static string Get(Keys k)
        {
            string result = "";

            switch (k)
            {
                case Keys.Space:
                    result = " ";
                    break;
                case Keys.D0:
                    result = "0";
                    break;
                case Keys.D1:
                    result = "1";
                    break;
                case Keys.D2:
                    result = "2";
                    break;
                case Keys.D3:
                    result = "3";
                    break;
                case Keys.D4:
                    result = "4";
                    break;
                case Keys.D5:
                    result = "5";
                    break;
                case Keys.D6:
                    result = "6";
                    break;
                case Keys.D7:
                    result = "7";
                    break;
                case Keys.D8:
                    result = "8";
                    break;
                case Keys.D9:
                    result = "9";
                    break;
                case Keys.A:
                    result = k.ToString().ToLower();
                    break;
                case Keys.B:
                    result = k.ToString().ToLower();
                    break;
                case Keys.C:
                    result = k.ToString().ToLower();
                    break;
                case Keys.D:
                    result = k.ToString().ToLower();
                    break;
                case Keys.E:
                    result = k.ToString().ToLower();
                    break;
                case Keys.F:
                    result = k.ToString().ToLower();
                    break;
                case Keys.G:
                    result = k.ToString().ToLower();
                    break;
                case Keys.H:
                    result = k.ToString().ToLower();
                    break;
                case Keys.I:
                    result = k.ToString().ToLower();
                    break;
                case Keys.J:
                    result = k.ToString().ToLower();
                    break;
                case Keys.K:
                    result = k.ToString().ToLower();
                    break;
                case Keys.L:
                    result = k.ToString().ToLower();
                    break;
                case Keys.M:
                    result = k.ToString().ToLower();
                    break;
                case Keys.N:
                    result = k.ToString().ToLower();
                    break;
                case Keys.O:
                    result = k.ToString().ToLower();
                    break;
                case Keys.P:
                    result = k.ToString().ToLower();
                    break;
                case Keys.Q:
                    result = k.ToString().ToLower();
                    break;
                case Keys.R:
                    result = k.ToString().ToLower();
                    break;
                case Keys.S:
                    result = k.ToString().ToLower();
                    break;
                case Keys.T:
                    result = k.ToString().ToLower();
                    break;
                case Keys.U:
                    result = k.ToString().ToLower();
                    break;
                case Keys.V:
                    result = k.ToString().ToLower();
                    break;
                case Keys.W:
                    result = k.ToString().ToLower();
                    break;
                case Keys.X:
                    result = k.ToString().ToLower();
                    break;
                case Keys.Y:
                    result = k.ToString().ToLower();
                    break;
                case Keys.Z:
                    result = k.ToString().ToLower();
                    break;
                case Keys.OemPeriod:
                    result = ".";
                    break;
            }

            return result;
        }
    }
}
