package STVRogue.GameControl;

import java.io.Serializable;

/** Representing a command. */
public class Command implements Serializable {
	
	public enum CommandType { DoNOTHING, MOVE, ATTACK, USE, FLEE }
	
	CommandType name ;
	
	/**
	 * Some commands have arguments. For example, "USE" should specify
	 * what item to use (e.g. a healing potion). You should decide the format
	 * of the arguments.
	 */
	String[] args ;
	
	public Command(CommandType name, String[] args) {
		this.name = name ;
		this.args = args ;
	}
	
	@Override
	public String toString() {
		String o = "" + name ;
		if (args != null && args.length > 0) {
			o += "(" ;
			for (int i=0 ; i<args.length ; i++) {
				if (i>0) o += "," ;
				o += args[i] ;
			}
			o += ")" ;
		}
		return o ;
	}
   
}
