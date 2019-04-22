package STVRogue.GameLogic;

/**
 * A parent class representing items in the game.
 */
public class Item extends GameEntity {

	public Item(String iD) {
		super(iD);
	}
	
	/**
	 * Implementing the logic of what happen when the player uses this item.
	 * Here it does nothing. Override this accordingly in the subclasses.
	 */
	public void use(Game G, Player player) { }

}
