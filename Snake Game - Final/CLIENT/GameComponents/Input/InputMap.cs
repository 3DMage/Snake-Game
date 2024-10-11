using Microsoft.Xna.Framework.Input;
using Shared.Datastructures;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GameComponents
{
    // Holds mapping for keys and commands.
    [DataContract(Name = "InputMap")]
    public class InputMap
    {
        // Dictionary that maps input to names of commands (due to not being able to serialize delegates).
        [DataMember()]
        public Dictionary<(Context, Keys), string> keyToCommandNameMap { get; private set; }

        [DataMember()]
        public Dictionary<string, (Context, Keys)> commandNameToKeyMap { get; private set; }

        // Constructor.
        public InputMap()
        {
            keyToCommandNameMap = new Dictionary<(Context, Keys), string>();
            commandNameToKeyMap = new Dictionary<string, (Context, Keys)>();
        }





        // Registers a key and gamestate to a provided InputMapEntry.  Also specifies if the key is press-only or not.
        public void RegisterKey(Keys key, Context inputContext, string commandName)
        {
            var keyContextPair = (inputContext, key);
            keyToCommandNameMap.Add(keyContextPair, commandName);

            // Add to the reverse map as well
            commandNameToKeyMap[commandName] = keyContextPair;
        }
        

        // Unregisters a command associated with input key and gamestate.
        public void UnregisterKey(Keys key, Context inputContext)
        {
            var keyContextPair = (inputContext, key);
            if (keyToCommandNameMap.TryGetValue(keyContextPair, out string commandName))
            {
                // Remove from both maps
                keyToCommandNameMap.Remove(keyContextPair);
                commandNameToKeyMap.Remove(commandName);
            }
        }

        // Remaps a command to specified key and game state.
        public bool RemapKey(Context inputContext, Keys oldKey, Keys newKey)
        {
            var oldKeyContextPair = (inputContext, oldKey);
            if (keyToCommandNameMap.TryGetValue(oldKeyContextPair, out string commandName))
            {
                if (!keyToCommandNameMap.ContainsKey((inputContext, newKey)))
                {
                    // Do a remap
                    UnregisterKey(oldKey, inputContext);
                    RegisterKey(newKey, inputContext, commandName);
                    return true;
                }
            }
            // Can't remap if the new key is already used or old key does not exist
            return false;
        }
    }
}