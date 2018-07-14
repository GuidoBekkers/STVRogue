package STVRogue.GameLogic;

public class HealingPotion extends Item {

    /** it can heal this many HP */
	int heal ;
	
	public HealingPotion(String iD, int heal) {
		super(iD);
		this.heal = heal ;
	}
	
	/**
	 * Using a healing potion heals the player (but not beyond his base-HP).
	 */
	@Override
	public void use(Player player) { 
		throw new UnsupportedOperationException() ; 
	}

}
