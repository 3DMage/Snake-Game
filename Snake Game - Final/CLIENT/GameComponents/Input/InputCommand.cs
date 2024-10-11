using Microsoft.Xna.Framework;
using Shared.Datastructures;

// Delegates for function signatures that constitute CommandPacket methods.
public delegate void PressAction(GameTime gameTime);
public delegate void ReleaseAction(GameTime gameTime);

namespace GameComponents
{
    // Represents the functionality for a key's press and release.
    public class InputCommand
    {
        // Function to use when key is pressed.
        public PressAction pressAction { get; private set; } = null;

        // Function to use when key is released.
        public ReleaseAction releaseAction { get; private set; } = null;

        // How the key's commands should be executed.
        public PressMode pressMode { get; private set; }

        // Name of the command.
        public string commandName { get; private set; }

        // Constructor.
        public InputCommand(PressMode pressMode, PressAction pressAction, ReleaseAction releaseAction, string commandName)
        {
            this.pressMode = pressMode;
            this.pressAction = pressAction;
            this.releaseAction = releaseAction;
            this.commandName = commandName;
        }
    }
}