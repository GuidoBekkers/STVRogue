package STVRogue.TestInfrastructure;

import java.util.function.Predicate;

import STVRogue.Utils;
import STVRogue.GameLogic.Game;

public class Unless extends TemporalSpecification {

	Predicate<Game> p ;
	Predicate<Game> q ;
	public Unless(Predicate<Game> p, Predicate<Game> q) { this.p = p ; this.q = q ; }
	
	int judgement(boolean ok, boolean relevant) {
		if (ok && relevant)  return TemporalSpecification.RelevantlyValid ;
		if (ok && !relevant) return TemporalSpecification.TriviallyValid ;
		return TemporalSpecification.Invalid ;
	}
	
	@Override
	public int evaluate(GamePlay sigma) {
		sigma.reset();
		boolean relevant = false ;
		boolean ok = true ;
		boolean previous_pAndNotq ; // to keep track if p && ~q holds in the previous state
		if (sigma.atTheEnd()) return judgement(ok,relevant) ;
		// else we have at least one turn
		Game currentState = sigma.getState() ;
		previous_pAndNotq = p.test(currentState) && !q.test(currentState) ;
		relevant = relevant || previous_pAndNotq ;
		
		while (!sigma.atTheEnd()) {
			sigma.replayCurrentTurn();
			currentState = sigma.getState() ;
			if (previous_pAndNotq) ok = p.test(currentState) || q.test(currentState) ;
			else ok = true ;
			if (!ok) {
				// the predicate p is violated!
				Utils.log("violation of Unless at turn " + sigma.getTurn()) ;
				return judgement(ok,relevant) ;
			}
			previous_pAndNotq = p.test(currentState) && !q.test(currentState) ;
			relevant = relevant || previous_pAndNotq ;	
		}
		return judgement(ok,relevant) ;
	}

}
