package STVRogue.TestInfrastructure;

import java.io.IOException;
import STVRogue.GameLogic.Game;

/** A dummy GamePlay; for testing the specification classes */
public class DummyGamePlay extends GamePlay {
	int[] execution ;	
	Game state ;
	public DummyGamePlay(int[] execution){ 
		this.execution = execution ; 
		length = execution.length - 1 ;
		state = new Game() ;
	}
	public void reset() { turn = 0 ; state.z_ = execution[turn] ;} 
	public Game getState() { return state ; }
	public void replayTurn() { turn++ ; state.z_ = execution[turn] ; }

}
