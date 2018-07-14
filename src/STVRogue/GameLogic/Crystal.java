package STVRogue.GameLogic;


public class Crystal extends Item {
	
	public Crystal(String iD, int charges) {
		super(iD);
	}
	
	/**
	 * Using a crystal grants superhuman speed to the player. At the next turn (and only at the next turn)
	 * the player's attack will hurt all monsters in the combat.
	 */
	@Override
	public void use(Player player) { 
		throw new UnsupportedOperationException() ; 
	}

	

}
