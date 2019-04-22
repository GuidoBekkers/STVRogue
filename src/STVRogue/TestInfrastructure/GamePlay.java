package STVRogue.TestInfrastructure;

import java.io.IOException;
import STVRogue.GameLogic.Game;

public class GamePlay {

	int turn = 0 ;
	int length ;
	
	GamePlay() { }
	
	public GamePlay(String savefile) throws IOException {
		throw new UnsupportedOperationException() ; 
	}
	
	/** reset the gameplay to turn 0 */
	public void reset() { throw new UnsupportedOperationException() ; }
	
	/** return the current game state */
	public Game getState() { throw new UnsupportedOperationException() ; }
	
	/** return the current turn number. */
	public int getTurn() { return turn ; }
	
	/** true if the gameplay is at the end, hence has no more turn to do. */
	public boolean atTheEnd() { return turn >= length ; }
	
	/** 
	 * Replay the current turn, thus updating the game state.
	 * This also increases the turn nr, thus shifting the current turn to the next one. 
	 */
	public void replayCurrentTurn() { throw new UnsupportedOperationException() ; }
	
}
