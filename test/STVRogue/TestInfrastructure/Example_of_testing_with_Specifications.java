package STVRogue.TestInfrastructure;

import static org.junit.Assert.assertTrue;

import java.util.List;
import java.util.LinkedList;
import org.junit.Test;

import STVRogue.HelperPredicates;

/** 
 * Iteration-2.
 * Contain some examples demonstrating how to test using instances of Specification.
 * I am using dummy game-plays for this demonstration. 
 * Note that these are just example-tests, and are not actual tests for your STV-Rogue game.
 */
public class Example_of_testing_with_Specifications {
	
	/** creating a bunch of dummy gameplays for demonstration. */
	List<GamePlay> createDummyPlays() {
		int[] execution1 = { 0 , 0 , 3 , 0 } ;
		int[] execution2 = { 0 , 0 , 0 , 0 } ;
		int[] execution3 = { 0 , 0 , 3 , 0 , -1 , 0 } ;
		int[] execution4 = { 3 } ;
		List<GamePlay> testsuite = new LinkedList<GamePlay>() ;
		testsuite.add(new DummyGamePlay(execution1)) ;
		testsuite.add(new DummyGamePlay(execution2)) ;
		testsuite.add(new DummyGamePlay(execution3)) ;
		testsuite.add(new DummyGamePlay(execution4)) ;
		return testsuite ;
	}
	
	
	@Test
	public void example_test1() {
		// the game plays used as test cases: 
		List<GamePlay> plays = createDummyPlays() ;
		// the specification we want to check on the game plays:
		Specification spec = new Always(G -> G.z_ <= 3) ; /* [](z_ <= 3) */
		// checking the specifation on the gameplays. 
		// Require that the specification holds on all gameplays, and that at least 3 of them
		// are non-trivial (for "Always"-type of specification any gameplay is non-trivial).
		assertTrue(spec.evaluate(plays,3) == Specification.RelevantlyValid) ;
	}
	
	@Test
	public void example_test2() {
		// the game plays used as test cases: 
		List<GamePlay> plays = createDummyPlays() ;
		// the specification we want to check on the game plays:
		Specification spec = new Unless(G -> G.z_ <= 0, G -> G.z_ >= 3) ; /* z_ <= 0  UNLESS  z_ >= 3 */
		// checking the specifation on the gameplays. 
		// Require that the specification holds on all gameplays, and that at least 3 of them
		// are non-trivial.
		assertTrue(spec.evaluate(plays,3) == Specification.RelevantlyValid) ;
	}
	
	@Test
	public void example_test3() {
		// An example of testing a family of specifications (paremeterized specifications)
		List<GamePlay> plays = createDummyPlays() ;
		
		for(int k_ = 0 ; k_ < 5 ; k_ ++) {
			final int k = k_ ;
			/* An "unless" specification, parameterized with k:  z_ <= k  UNLESS  z_ >= 3-k */
			Specification spec = new Unless(G -> G.z_ <= k, G -> G.z_ >= 3 - k) ; 
			assertTrue(spec.evaluate(plays,3) == Specification.RelevantlyValid) ;
		}
	}
	
	// @Test  disabled, you need to create an actual GamePlaye first
	public void example_test4() {
		GamePlay sigma = null ; // well you need to create one yourself
		Specification spec = new Always(G -> HelperPredicates.forall(G.monsters(), m -> m.HP >= 0)) ;
		assertTrue(spec.evaluate(sigma) == Specification.RelevantlyValid) ;
	}
}
