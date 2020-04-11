using System;
namespace STVrogue.GameControl
{
    [Serializable()]
    public enum CommandType { DoNOTHING, MOVE, ATTACK, USE, FLEE }

    
    
    /// <summary>
    /// Representing a player command/action. 
    /// </summary>
    [Serializable()]
    public class Command
    {
        CommandType name;

        /// <summary>
        /// Some commands have arguments. For example, "USE" should specify which item to use,
        /// and "ATTACK" should specify which moster to attack. The item/room/monster targeted by
        /// the command is specified through their unique ID. It is essential that you use the
        /// ID to allow series of commands to be saved.
        /// </summary>
        string[] args;

        public Command(CommandType name, string[] args)
        {
            this.name = name;
            this.args = args;
        }
        
        // well.. add the getters yourself
        
        public override string ToString()
        {
            String o = "" + name;
            if (args != null && args.Length > 0)
            {
                o += "(";
                for (int i = 0; i < args.Length; i++)
                {
                    if (i > 0) o += ",";
                    o += args[i];
                }
                o += ")";
            }
            return o;
        }
    }
}
