package STVRogue.Examples;

/**
 * Just an example of some class, to demonstrate unit testing. 
 * 
 * This is a class representing thermometers in Celcius.
 */
public class Thermometer {
	private double val ;
	private double scale ;
	private double offset ;
	
	public Thermometer(double s, double o) {
		val    = o - 273.15 ; // bug
		scale  = s ;
		offset = 0 ;
	}
	public double value() {
		return scale*val + offset ;
	}
	public double warmUp(double v) {
		val += v/scale ;
		return val ;
	}
	public double coolDown(double v) {
		val = val - v/scale ;
		return val ;
	}
}
