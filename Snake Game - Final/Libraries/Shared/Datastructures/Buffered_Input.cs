using Microsoft.Xna.Framework.Input;


namespace Shared.Datastructures
{
    public class Buffered_Input
    {
        public Keys key { get; set; }
        public Context context { get; set; }
        public PressMode pressMode { get; set; }

        public Buffered_Input(Keys key, Context context, PressMode pressMode)
        {
            this.key = key;
            this.context = context;
            this.pressMode = pressMode;
        }
    }
}
