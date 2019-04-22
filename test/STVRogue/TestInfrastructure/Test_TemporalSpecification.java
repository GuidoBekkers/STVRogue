package STVRogue.TestInfrastructure;

import static org.junit.Assert.* ;
import org.junit.Test;

/** 
 * Iteration-2.
 * Few tests you can use to test the behavior of different types of specifications. 
 * Note that these are tests for the implementation of different subclasses of 
 * TemporalSpecification.
 * These are NOT tests for the STV-rogue game itself.
 * */
public class Test_TemporalSpecification {
	
	@Test
	public void test_Always() {
		int[] execution1 = { 0 , 0 , 3 , 0 } ;
		int[] execution2 = { 0 } ;
		int[] execution3 = { 0 , 0 , -1 , 0 } ;
		int[] execution4 = { -1 } ;
		TemporalSpecification spec = new Always(G -> G.z_ >= 0) ; /* [](z_ >= 0) */
		assertTrue(spec.evaluate(new DummyGamePlay(execution1)) == TemporalSpecification.RelevantlyValid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution2)) == TemporalSpecification.RelevantlyValid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution3)) == TemporalSpecification.Invalid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution4)) == TemporalSpecification.Invalid) ;
	}
	
	@Test
	public void test_Unless() {
		int[] execution1 = { 0 , 0 , 3 } ;
		int[] execution2 = { 0 , 0 , 3 , 0 , 0 , 3 , 0 } ;
		int[] execution3 = { 0 } ;
		int[] execution4 = { 3 } ;
		int[] execution5 = { 2 } ;
		int[] execution6 = { 2 , 2 , 2 } ;
		int[] execution7 = { 0 , 0 , 3 , 0 , 0 , 1 , 0 , 0 , 3} ;
		
		TemporalSpecification spec = new Unless(G -> G.z_ <= 0 , G -> G.z_ >= 3) ; /* z_ <= 0  UNLESS z_ >= 3 */
		
		assertTrue(spec.evaluate(new DummyGamePlay(execution1)) == TemporalSpecification.RelevantlyValid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution2)) == TemporalSpecification.RelevantlyValid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution3)) == TemporalSpecification.TriviallyValid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution4)) == TemporalSpecification.TriviallyValid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution5)) == TemporalSpecification.TriviallyValid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution6)) == TemporalSpecification.TriviallyValid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution7)) == TemporalSpecification.Invalid) ;
		
	}
	
	/*@Test
	public void test_Leadsto() {
		int[] execution1 = { 0 , 0 , 3 , 3 } ;
		int[] execution2 = { 0 , 0 , 3 , 0 , 1 , 3  } ;
		int[] execution3 = { 1 , 1 , 1 } ;
		int[] execution4 = { 1 } ;
		int[] execution5 = { 0 } ;
		int[] execution6 = { 0 , 1 , 1 } ;
		int[] execution7 = { 0 , 0 , 3 , 0 , 1 , 1 } ;
		
		Specification spec = new Leadsto(G -> G.z_ <= 0 , G -> G.z_ >= 3) ; // z_ <= 0  LEADSTO  z_ >= 3 
		
		assertTrue(spec.evaluate(new DummyGamePlay(execution1)) == Specification.RelevantlyValid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution2)) == Specification.RelevantlyValid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution3)) == Specification.TriviallyValid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution4)) == Specification.TriviallyValid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution5)) == Specification.Invalid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution6)) == Specification.Invalid) ;
		assertTrue(spec.evaluate(new DummyGamePlay(execution7)) == Specification.Invalid) ;	
	}*/

}
