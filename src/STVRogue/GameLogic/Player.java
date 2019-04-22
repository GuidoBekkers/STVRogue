package STVRogue.GameLogic;

import java.util.LinkedList;
import java.util.List;

public class Player extends Creature{

	/** kill point */
	int KP = 0 ;
	int HPmax ;
    public boolean boosted  = false;
    public boolean inCombat = false ;
    public List<Item> bag = new LinkedList<Item>();

	public Player(String ID) { 
		super(ID); 
		// you need to decide how to initialize the other attributes
	    throw new UnsupportedOperationException() ;
	}
	
	@Override
	/**
	 * Attack the foe.
	 */
	public void attack(Game G, Creature foe) { 
		throw new UnsupportedOperationException() ; 
	}

	@Override
	/**
	 * Move the player to the given node. Return true if the move is
	 * successful, else false.
	 */
	public boolean move(Game G, Node nd) { 
		throw new UnsupportedOperationException() ; 
	}
	
	@Override
	/**
	 * The player flees to the given node. Return true is this is successful,
	 * else false.
	 */
	public boolean flee(Game G, Node nd) { 
		throw new UnsupportedOperationException() ; 
	}
	
}
