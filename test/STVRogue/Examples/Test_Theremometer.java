package STVRogue.Examples;

import static org.junit.Assert.* ;
import org.junit.Test;

/**
 * An example of a test-class, containing several test cases to test the
 * class STV.Rogue.Examples.Thermometer. To run the tests, execute this
 * class using JUnit test runner. From Eclipse, you can use the "Run" button;
 * it will know that it need to invoke JUnit runner under the hood.
 */
public class Test_Theremometer {

	/** Test-case 1 to test the method value() */
	@Test
	public void test1_value() {
		Thermometer t = new Thermometer(0,100) ;
		double expected = 0 ;
		double epsilon = 1e-5 ;
		assertEquals(expected, t.value(),epsilon) ;
	}
	
	/** Another test0case to test the method value() */
	@Test
	public void test2_value() {
		Thermometer t = new Thermometer(1,0) ;
		double expected = 0 ;
		double epsilon = 1e-5 ;
		assertEquals(expected, t.value(),epsilon) ;
	}
}
