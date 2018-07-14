package STVRogue.GameLogic;

import java.util.List;
import STVRogue.GameControl.Command;
import java.util.LinkedList;

/** This class represents the whole game state of STV-Rogue */
public class Game {
	
     public Player player ;
      
     /** the creature currently has the turn, either the player or a monster */
     public Creature turn ;
     
     public enum PlayerState {
    	 NOTinCombat,  
    	 CombatS0, CombatS1, CombatS2, CombatS3, CombatS4, CombatS5, CombatS6
     }
     
     
     public PlayerState playerstate ;
     
     /** all monsters currently live in the game. */
     public List<Monster> monsters  = new LinkedList<Monster>() ;
     public Dungeon dungeon ;
       
     public int z_ ; // no real use except for debug purposes
     
     public Game(){}
     
 	/**
 	 * Create a game with a dungeon of the specified level and capacityMultiplier.
 	 * This also creates a proper instance of Player.
 	 */
 	public Game(int level, int capacityMultiplier) {
 		throw new UnsupportedOperationException() ;
 	}
     
     /** return all monsters currently alive */
     public List<Monster> monsters() { return monsters ; }
     public List<Node> nodes() { throw new UnsupportedOperationException() ; }   // Iteration-2
     public List<Node> bridges() { throw new UnsupportedOperationException() ; } // Iteration-2
     public List<Zone> zones() { return dungeon.zones ; }
     
     /** Return the monster that still lives in the game, with the given id. 
      *  If the monster does not exist anymore, null is returned.
      */
     public Monster monster(String id) { throw new UnsupportedOperationException() ; } // Iteration-2
     public Node node(String id) { throw new UnsupportedOperationException() ; }   // Iteration-2
     public Node bridge(String id) { throw new UnsupportedOperationException() ; } // Iteration-2
     public Zone zone(String id) { throw new UnsupportedOperationException() ;   } // Iteration-2
     
     /** Check is a monster with the given id still lives in the game. */
     public boolean monsterExists(String id) { return monster(id) != null ; }
     public boolean nodeExists(String id) { return node(id) != null ; }
     public boolean bridgeExists(String id) { return bridge(id) != null ; }
     public boolean zoneExists(String id) { return zone(id) != null ; }
     
     /**
      * Update the game state by **one turn**. If the turn belongs to a monster,
      * then do that monster's turn, else interpret the given user-command to
      * do a player-turn. 
      * 
      * Keep track if the player is in a combat or not in a combat. When he is in
      * combat, keep track in which state the combat is. Use the variable playerstate
      * to keep track of this. 
      * 
      * At the end of update, you should also decide who has the next turn, and
      * what the next playerstate is (set it to NOTinCombat if the player is not
      * in combat).
      */
 	 public void update(Command usercommand) {
		throw new UnsupportedOperationException() ;
	 }
     
}
