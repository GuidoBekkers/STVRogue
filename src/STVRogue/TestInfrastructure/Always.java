package STVRogue.TestInfrastructure;

import java.util.function.Predicate;

import STVRogue.Utils;
import STVRogue.GameLogic.Game;

/**
 * Representing a temporal property of the form "Always p". A gameplay
 * satisfies this property if p holds on the game state through out the
 * play.
 */
public class Always extends TemporalSpecification {

	Predicate<Game> p ;
	
	public Always(Predicate<Game> p) { this.p = p ; }
	
	@Override
	public int evaluate(GamePlay sigma) {
		sigma.reset();
		// check the initial state:
		boolean ok = p.test(sigma.getState()) ;
		if (!ok) {
			// the predicate p is violated!
			Utils.log("violation of Always at turn " + sigma.getTurn()) ;
			return TemporalSpecification.Invalid ;
		}
		while(!sigma.atTheEnd()) {
			// replay the current turn (and get the next turn)
			sigma.replayCurrentTurn();
			// check if p holds on the state that resulted from replaying the turn
			ok = p.test(sigma.getState()) ;
			if (!ok) {
				// the predicate p is violated!
				Utils.log("violation of Always at turn " + sigma.getTurn()) ;
				return TemporalSpecification.Invalid ;
			}
			
		}
		// if we reach this point than p holds on every state in the gameplay:
		return TemporalSpecification.RelevantlyValid ;
	}

}
