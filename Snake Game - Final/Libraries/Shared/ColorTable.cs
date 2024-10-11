using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace Client.__GAME.SnakeGame.Components.SnakeComponents
{
    public static class ColorTable
    {
        public static Dictionary<int, Color> colors { get; private set; } = new Dictionary<int, Color>();

        public static void Initialize()
        {
            colors.Add(0, Color.Green);
            colors.Add(1, Color.Red);
            colors.Add(2, Color.Blue);
            colors.Add(3, Color.Yellow);
            colors.Add(4, Color.Orange);
            colors.Add(5, Color.Purple);
            colors.Add(6, Color.Cyan);
            colors.Add(7, Color.Pink);
            colors.Add(8, Color.LightGray);
            colors.Add(9, Color.Black);
            colors.Add(10, Color.Gray);
            colors.Add(11, Color.Gold);
            colors.Add(12, Color.LimeGreen);
            colors.Add(13, Color.DarkMagenta);
            colors.Add(14, Color.Violet);
            colors.Add(15, Color.Aqua);
        }
    }
}
