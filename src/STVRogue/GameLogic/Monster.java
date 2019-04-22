package STVRogue.GameLogic;

public class Monster extends Creature {
	
	public Monster(String ID) { 
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
	 * Move this monster to the given node. Return true if the move is
	 * successful, else false.
	 */
	public boolean move(Game G, Node nd) { 
		throw new UnsupportedOperationException() ; 
	}
	
	@Override
	/**
	 * This monster flees to the given node. Return true is this is successful,
	 * else false.
	 */
	public boolean flee(Game G, Node nd) { 
		throw new UnsupportedOperationException() ; 
	}
	
}
