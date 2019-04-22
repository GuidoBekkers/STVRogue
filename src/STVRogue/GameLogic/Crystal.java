package STVRogue.GameLogic;


public class Crystal extends Item {
	
	public Crystal(String iD, int charges) {
		super(iD);
	}
	
	/**
	 * Using a crystal during a combat temporarily doubles the player's 
	 * attack rating. The effect is gone once the combat ends.
	 * Using a crystal while not in combat has no effect.
	 */
	@Override
	public void use(Game G, Player player) { 
		throw new UnsupportedOperationException() ; 
	}

	

}
