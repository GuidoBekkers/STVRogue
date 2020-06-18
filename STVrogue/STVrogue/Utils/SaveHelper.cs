using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using STVrogue.GameControl;
using STVrogue.GameLogic;
using static STVrogue.Utils.RandomFactory;
using CommandType = STVrogue.GameControl.CommandType;

namespace STVrogue.Utils
{
    public static class SaveHelper
    {
        private static StringBuilder sb = new StringBuilder();

        /// <summary>
        /// Record the given GameConfiguration
        /// </summary>
        /// <param name="gc">The GameConfiguration to save</param>
        public static void RecordConfig(GameConfiguration gc)
        {
            // Ensure the StringBuilder object is empty, since the GameConfiguration should always be the first item in the save file
            sb.Clear();

            // Append the values of the given GameConfiguration to the StringBuilder
            sb.Append(gc.difficultyMode.ToString()).Append(':');
            sb.Append(gc.playerName).Append('\n');
        }

        /// <summary>
        /// Load the saved GameConfiguration
        /// </summary>
        /// <param name="saveFile">The string denoting the location of the save file</param>
        /// <returns>The parsed GameConfiguration</returns>
        public static GameConfiguration LoadConfig(string saveFile)
        {
            // Create a GameConfiguration variable
            GameConfiguration gc = new GameConfiguration();
            
            // Create a StreamReader and initialize it with the given save file
            StreamReader reader = new StreamReader(saveFile);
            
            // Read the first line and check for null
            var line = reader.ReadLine();
            if (String.IsNullOrEmpty(line))
            {
                throw new ArgumentException("Given save file is empty");
            }
            
            // Separate the values
            var values = line.Split(':');
            
            // Recalculate the random variables (will be the same due to a reset of the randomizer)
            gc = Program.RandomGameConfig(gc);

            // Parse the GameConfiguration data
            Enum.TryParse(values[0], false, out gc.difficultyMode);
            gc.playerName = values[1];
            
            // Close the StreamReader
            reader.Close();
            
            // Return the GameConfiguration
            return gc;
        }

        /// <summary>
        /// Load the saved actions
        /// </summary>
        /// <param name="saveFile">The string denoting the location of the save file</param>
        /// <returns>An array of the saved actions</returns>
        public static Command[] LoadActions(string saveFile)
        {
            // Create a list containing all actions
            List<Command> actionList = new List<Command>();
            
            // Create a StreamReader and initialize it with the given save file
            StreamReader reader = new StreamReader(saveFile);

            // Read the first line to skip the GameConfiguration
            reader.ReadLine();
            
            // Read the first action
            string line = reader.ReadLine();

            // Keep parsing actions until the end of the file
            while (!String.IsNullOrEmpty(line))
            {
                // Parse the action and add it to the list
                actionList.Add(ParseAction(line));
                
                // Read the next line
                line = reader.ReadLine();
            }

            // Close the StreamReader
            reader.Close();
            
            // Return the actions as an array
            return actionList.ToArray();
        }

        /// <summary>
        /// Record the given action
        /// </summary>
        /// <param name="actionType">The type of the action</param>
        /// <param name="targetId">The target's ID</param>
        public static void RecordAction(CommandType actionType, string targetId)
        {
            // Append the given data to the StringBuilder
            sb.Append(actionType.ToString()).Append(':');
            sb.Append(targetId).Append('\n');
        }
        
        /// <summary>
        /// Parse an action
        /// </summary>
        /// <param name="input">The action to parse, in savefile format</param>
        /// <returns>A tuple containing the type of the action and the target's ID</returns>
        public static Command ParseAction(string input)
        {
            // Separate the values
            string[] values = input.Split(':');
            
            // Parse the ActionType
            CommandType type;
            Enum.TryParse(values[0], false, out type);
            
            // Return the tuple of the ActionType and the target's id
            return new Command(type, values[1]);
        }

        /// <summary>
        /// Save the currently recorded data to the given save file
        /// </summary>
        /// <param name="saveFile">The path to the save file</param>
        public static void SaveToFile(string saveFile)
        {
            // Create the StreamWriter object
            StreamWriter writer = new StreamWriter(saveFile);
            
            // Write the content of the StringBuilder to the file
            writer.Write(sb.ToString());

            // Close the StreamWriter
            writer.Close();
        }

        /// <summary>
        /// Executed when the player quits the game
        /// </summary>
        public static void Quit()
        {
            // Clear the StringBuilder
            sb.Clear();
        }
    }
}