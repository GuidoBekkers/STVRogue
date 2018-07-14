package STVRogue.GameControl;

import java.io.Serializable;

/** Representing a user/player command. */
public class Command implements Serializable {
	
	public enum CommandType { DoNOTHING, MOVE, ATTACK, USE, FLEE }
	
	CommandType name ;
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
