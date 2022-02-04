using System;

namespace MinewseeperCoop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Minewseeper())
                game.Run();
        }
    }
}
