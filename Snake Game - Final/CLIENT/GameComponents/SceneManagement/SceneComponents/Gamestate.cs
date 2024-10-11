using Microsoft.Xna.Framework;
using Shared.Datastructures;

namespace GameComponents
{
    public delegate void UpdateMethod(GameTime gameTime);
    public delegate void RenderMethod();

    public class Gamestate
    {
        public Context context { get; private set; }
        public InputMode inputMode { get; private set; }
        public UpdateMethod updateMethod { get; private set; }
        public RenderMethod renderMethod { get; private set; }

        public Gamestate(Context context, InputMode inputMode, UpdateMethod updateMethod, RenderMethod renderMethod)
        {
            this.context = context;
            this.inputMode = inputMode;
            this.updateMethod = updateMethod;
            this.renderMethod = renderMethod;
        }

        public virtual void OnEnterState() { }
        public virtual void OnExitState() { }
    }
}