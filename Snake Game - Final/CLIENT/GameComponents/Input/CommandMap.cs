using GameComponents;
using System.Collections.Generic;


namespace Client.GameComponents.Input
{
    public class CommandMap
    {
        // Stores mapping between command names and Commands.
        public Dictionary<string, InputCommand> commands { get; private set; }

        public CommandMap() 
        {
            commands = new Dictionary<string, InputCommand>();
        }

        // Maps a command name to a CommandPacket.
        public void RegisterCommand(string commandName, InputCommand command)
        {
            if (commands != null)
            {
                // Map a name to a CommandPacket.
                commands.Add(commandName, command);
            }
            else
            {
                // Initialize the commandPackets dictionary if not already initialized. Map a name to a CommandPacket.
                commands = new Dictionary<string, InputCommand>();
                commands.Add(commandName, command);
            }
        }


    }
}
