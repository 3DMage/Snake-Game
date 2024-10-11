using Client.GameComponents.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shared.Datastructures;

using System.Collections.Generic;

namespace GameComponents
{
    // Handles the setup and execution of user input and commands.  Also handles typing.
    public static class InputManager
    {
        // Holds the mapping of keys and commands.
        public static InputMap inputMap;
        public static CommandMap commandMap;

        

        // Persistant storage management of inputs.
        public static InputMap_IO inputMap_IO { get; private set; }

        // Flag indicating if an input map was loaded in by InputMap_IO.
        public static bool loadedPreexistingInputMap = false;

        // Flag to indicate if the user is done typing.
        public static bool doneTyping = false;

        // The current context from which inputs are selected from.
        public static Context currentInputContext { get; private set; }

        // Stores the current and previous states of the keyboard.
        public static KeyboardState currentState { get; private set; }
        public static KeyboardState previousState { get; private set; }

        // Collects keys that are pressed to then execute.
        public static Keys[] keysBuffer { get; private set; }

        // String used to store typing output.
        public static string workingString { get; set; } = "";
        private static int characterLimit = 15;

        // Characters obtained by holding Shift key and a number key.
        private static string[] shiftNumberCharacters = { ")", "!", "@", "#", "$", "%", "^", "&", "*", "(" };

        // A flag that globally marks if any key was pressed.
        private static bool globalWasPressed = false;

        // The latest key to be pressed.
        public static Keys latestPressedKey;





        // Constructor.
        public static void Initialize()
        {
            inputMap = null;
            commandMap = new CommandMap();
        }

        // Try loading an InputMap from persistent storage.  Initialize a new one if no InputMap could be loaded.
        public static void TryLoadingInputMap()
        {
            inputMap_IO = new InputMap_IO();

            inputMap_IO.LoadInputMap();

            if (inputMap != null)
            {
                // InputMap was successfully loaded.
                loadedPreexistingInputMap = true;
            }
            else
            {
                // No InputMap was found.  Initialize a new one. 
                loadedPreexistingInputMap = false;
                inputMap = new InputMap();
            }
        }









        // Processes commands associated with pressed keys.
        public static void ProcessCommands(GameTime gameTime)
        {
            //List<Buffered_Input> bufferedInputs = new List<Buffered_Input>();

            // Updates current keyboard state.
            currentState = Keyboard.GetState();

            // Grab pressed keys.
            keysBuffer = currentState.GetPressedKeys();


            // Process commands based on stored pressed keys.
            for (int i = 0; i < keysBuffer.Length; i++)
            {
                // Grab a pressed key.
                Keys key = keysBuffer[i];

                // Check if command has entry in key map.
                if (inputMap.keyToCommandNameMap.ContainsKey((currentInputContext, key)))
                {
                    // Grab the inputMapEntry from input map.
                    InputCommand command = commandMap.commands[inputMap.keyToCommandNameMap[(currentInputContext, key)]];

                    // Check if command is press-only/
                    if (command.pressMode == PressMode.PRESS_ONLY && command.pressAction != null)
                    {
                        // Key is press-only.

                        // Check if key can be pressed.
                        if (CanPressKey(key))
                        {
                            // Execute command.
                            command.pressAction(gameTime);
                            //Buffered_Input bufferedInput = new Buffered_Input(key, currentInputContext, PressMode.PRESS_ONLY, gameTime);
                            //bufferedInputs.Add(bufferedInput);
                        }
                    }
                    else if (command.pressMode == PressMode.PRESS_REGULAR && command.pressAction != null)
                    {
                        // Key is not press-only.

                        // Check if key is currently down.
                        if (currentState.IsKeyDown(key))
                        {
                            // Execute command.
                            command.pressAction(gameTime);
                            //Buffered_Input bufferedInput = new Buffered_Input(key, currentInputContext, PressMode.PRESS_REGULAR, gameTime);
                            //bufferedInputs.Add(bufferedInput);
                        }
                    }
                }
            }

            // Process RELEASE ONLY keys.
            foreach (var key in previousState.GetPressedKeys())
            {
                if (inputMap.keyToCommandNameMap.ContainsKey((currentInputContext, key)))
                {
                    // Grab the command from input map.
                    InputCommand command = commandMap.commands[inputMap.keyToCommandNameMap[(currentInputContext, key)]];

                    if (command.releaseAction != null)
                    {
                        // Check if key was released.

                        if (WasReleasedKey(key))
                        {
                            // Execute command.                
                            command.releaseAction(gameTime);
                            //Buffered_Input bufferedInput = new Buffered_Input(key, currentInputContext, PressMode.RELEASE_ONLY, gameTime);
                            //bufferedInputs.Add(bufferedInput);
                        }
                    }
                }
            }

            // Mark globalWasPressed flag.
            if (currentState.GetPressedKeyCount() > 0)
            {
                // Some keys are pressed.
                globalWasPressed = true;
            }
            else
            {
                globalWasPressed = false;
            }

            // Updates previous keyboard state.
            previousState = currentState;

            //if (bufferedInputs.Count > 0)
            //{
            //    BufferedInputDataPacket dataPacket = new BufferedInputDataPacket(bufferedInputs);
            //    MSG_BufferedInput message = new MSG_BufferedInput();
            //    message.Inject(dataPacket);
            //    _Client.SendMessageWithID(message);
            //}
        }







        // Processes commands associated with pressed keys.
        public static void ProcessBufferedCommands(List<Buffered_Input> bufferedInputs, GameTime gameTime)
        {
            // Process commands based on stored pressed keys.
            for (int i = 0; i < bufferedInputs.Count; i++)
            {
                Buffered_Input currentBufferedInput = bufferedInputs[i];

                if (inputMap.keyToCommandNameMap.ContainsKey((currentBufferedInput.context, currentBufferedInput.key)))
                {

                    // Grab the inputMapEntry from input map.
                    InputCommand command = commandMap.commands[inputMap.keyToCommandNameMap[(currentBufferedInput.context, currentBufferedInput.key)]];
                    
                    if(currentBufferedInput.pressMode == PressMode.PRESS_REGULAR || currentBufferedInput.pressMode == PressMode.PRESS_ONLY) 
                    {
                        command.pressAction(gameTime);
                    }
                    else if (currentBufferedInput.pressMode == PressMode.RELEASE_ONLY)
                    {
                        command.releaseAction(gameTime);
                    }
                }
            }
        }








        // Updates an entry in input map based on pressed key.
        public static InputConfigState ConfigKey(Context inputContext, Keys oldKey)
        {
            // Updates current keyboard state.
            currentState = Keyboard.GetState();

            // Grab pressed keys.
            keysBuffer = currentState.GetPressedKeys();

            // Only process if no keys are pressed previously and some key is pressed.
            if (!globalWasPressed && keysBuffer.Length > 0)
            {
                // Get the first key that was pressed in keysBuffer.
                Keys firstPressedKey = keysBuffer[0];

                // Update latestPressedKey.
                latestPressedKey = firstPressedKey;

                // Update globalWasPressed flag.
                globalWasPressed = true;

                // See if input map entry exists for pressed key and is not Escape.
                if (!inputMap.keyToCommandNameMap.ContainsKey((inputContext, latestPressedKey)) && latestPressedKey != Keys.Escape)
                {
                    // Remap the key if the pressed key was not mapped previously.
                    inputMap.RemapKey(inputContext, oldKey, latestPressedKey);

                    return InputConfigState.VALID;
                }
                else
                {
                    if (latestPressedKey == oldKey)
                    {
                        // Key was already pressed previously.
                        return InputConfigState.ITSELF;
                    }
                    else if (latestPressedKey == Keys.Escape)
                    {
                        // Key is Escape.  InputManager indicates canceling.
                        return InputConfigState.CANCELED;
                    }
                    else
                    {
                        // Key was already mapped elsewhere.
                        return InputConfigState.ALREADY_MAPPED;
                    }
                }
            }

            // Update globalWasPressed to false if no keys are currently pressed.
            if (currentState.GetPressedKeyCount() <= 0)
            {
                globalWasPressed = false;
            }

            // Updates previous keyboard state.
            previousState = currentState;

            return InputConfigState.NOTHING;
        }

        
        
        
        
        // Records keys while typing.
        public static void GetTypedCharacters()
        {
            // Updates current keyboard state.
            currentState = Keyboard.GetState();

            // Get pressed keys.
            keysBuffer = currentState.GetPressedKeys();

            // Check if shift and caps lock are pressed or enabled respectively.
            bool shiftDown = currentState.IsKeyDown(Keys.LeftShift) || currentState.IsKeyDown(Keys.RightShift);
            bool capitalMode = shiftDown || currentState.CapsLock;

            // Go through each key and insert pressed button into the working string.
            for (int i = 0; i < keysBuffer.Length; i++)
            {
                // Grab current key.
                Keys key = keysBuffer[i];

                // Check if key can be pressed.
                if (CanPressKey(key) && !doneTyping)
                {
                    // Check if backspace is pressed and string is non-empty.
                    if (key == Keys.Back && workingString.Length > 0)
                    {
                        // Backspace pressed. Remove last character from string.
                        workingString = workingString.Substring(0, workingString.Length - 1);
                    }
                    else if (key == Keys.Enter)
                    {
                        // Indicate typing is done.
                        doneTyping = true;
                    }
                    else
                    {
                        // Check if the string's length does not exceed character limit.
                        if (workingString.Length < characterLimit)
                        {
                            // Add character to string.
                            workingString += KeyToString(key, shiftDown, capitalMode);
                        }
                    }
                }
            }

            // Updates previous keyboard state.
            previousState = currentState;
        }

        // Sets limit of how many characters can be typed.
        public static void SetCharacterLimit(int characterLimit)
        {
            InputManager.characterLimit = characterLimit;
        }

        // Converts pressed key into a string character.  Also checks for shift and capitalization.
        private static string KeyToString(Keys key, bool shift, bool capitalMode)
        {
            // Check if key is in number row.
            if (key >= Keys.D0 && key <= Keys.D9)
            {
                // Check if shift is held.
                if (shift)
                {
                    // Grab shift character from number row corresponding to number pressed.
                    return shiftNumberCharacters[key - Keys.D0];
                }

                // Grab the number from number row.
                return ((int)key - (int)Keys.D0).ToString();
            }

            // Check if key is a letter
            if (key >= Keys.A && key <= Keys.Z)
            {
                // Determine if letter is captalized or not.
                if (!capitalMode)
                {
                    return key.ToString().ToLower();
                }
                else
                {
                    return key.ToString().ToUpper();
                }
            }

            // Characters and puncuation.
            switch (key)
            {
                case Keys.Space: return " ";
                case Keys.OemTilde: return shift ? "~" : "`";
                case Keys.OemSemicolon: return shift ? ":" : ";";
                case Keys.OemQuotes: return shift ? "\"" : "'";
                case Keys.OemQuestion: return shift ? "?" : "/";
                case Keys.OemPlus: return shift ? "+" : "=";
                case Keys.OemPipe: return shift ? "|" : "\\";
                case Keys.OemPeriod: return shift ? ">" : ".";
                case Keys.OemOpenBrackets: return shift ? "{" : "[";
                case Keys.OemCloseBrackets: return shift ? "}" : "]";
                case Keys.OemMinus: return shift ? "_" : "-";
                case Keys.OemComma: return shift ? "<" : ",";
                case Keys.OemClear: return shift ? "Clear" : "Clear";
            }

            // No valid key was pressed.
            return "";
        }








        public static bool IsKeyPressed(Keys key)
        {
            return currentState.IsKeyDown(key);
        }







        // Updates the currentInputContext.
        public static void TransitionInputContext(Context inputContext)
        {
            currentInputContext = inputContext;
        }

        // Clears keyboard states.
        public static void ResetKeyboardState()
        {
            currentState = Keyboard.GetState();
            previousState = currentState;
        }

        // Checks if a key can be pressed or not.
        public static bool CanPressKey(Keys key)
        {
            return currentState.IsKeyDown(key) && previousState.IsKeyUp(key);
        }

        // Check if key was released.
        public static bool WasReleasedKey(Keys key)
        {
            return currentState.IsKeyUp(key) && previousState.IsKeyDown(key);
        }
    }
}