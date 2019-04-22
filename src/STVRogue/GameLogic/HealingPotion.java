package STVRogue.GameLogic;

public class HealingPotion extends Item {

    /** it can heal this many HP */
	int HPvalue ;
	
	public HealingPotion(String iD, int heal) {
		super(iD);
		this.HPvalue = heal ;
	}
	
	/**
	 * Using a healing potion heals the player (but not beyond his HPmax).
	 */
	@Override
	public void use(Game G, Player player) { 
		throw new UnsupportedOperationException() ; 
	}

}
