package STVRogue;

import java.util.Random;
import java.util.logging.Logger;

public class Utils {
	
	/**
	 * To write s to the log. Modify this if you want more sophisticated logging.
	 */
	static public void log(String s) {
		System.err.println("** " + s);
	}
	
	/** Return a pseudo-random generator, using the given seed, */
	static public Random mkRandom(long seed) {
		return new Random(seed) ;
	}
}
