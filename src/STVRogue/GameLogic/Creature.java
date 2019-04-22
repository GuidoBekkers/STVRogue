package STVRogue.GameLogic;

public class Creature extends GameEntity {

	public String name="goblin" ;
	public int HP=1 ;     // current HP, should never exceed HPmax
	public Node location ;
	public int AttackRating=1 ;
	
	public Creature(String ID) { 
		super(ID); 
	}
	
	/**
	 * Attack the foe. This one just throws an exception; you need to
	 * override it in the corresponding subclasses (Monster and Player).
	 */
	public void attack(Game G, Creature foe) { 
		throw new UnsupportedOperationException() ; 
	}
	
	/**
	 * Move this creature to the given node. Return true if the move is
	 * successful, else false.
	 * This one just throws an exception; you need to
	 * override it in the corresponding subclasses (Monster and Player).
	 */
	public boolean move(Game G, Node nd) {
		throw new UnsupportedOperationException() ; 
	}
	
	/**
	 * This creature flees to the given node. Return true is this is successful,
	 * else false.
	 * This one just throws an exception; you need to
	 * override it in the corresponding subclasses (Monster and Player).
	 */
	public boolean flee(Game G, Node nd) {
		throw new UnsupportedOperationException() ; 
	}

}
