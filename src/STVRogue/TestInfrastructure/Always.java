package STVRogue.TestInfrastructure;

import java.util.function.Predicate;

import STVRogue.Utils;
import STVRogue.GameLogic.Game;

public class Always extends Specification {

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
			return Specification.Invalid ;
		}
		while(!sigma.atTheEnd()) {
			sigma.replayTurn();
			// check if p holds on the state of the the turn is updated:
			ok = p.test(sigma.getState()) ;
			if (!ok) {
				// the predicate p is violated!
				Utils.log("violation of Always at turn " + sigma.getTurn()) ;
				return Specification.Invalid ;
			}
			
		}
		// if we reach this point than p holds on every state in the gameplay:
		return Specification.RelevantlyValid ;
	}

}
