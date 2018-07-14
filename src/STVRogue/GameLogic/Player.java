package STVRogue.GameLogic;

import java.util.LinkedList;
import java.util.List;

public class Player extends Creature{

	/** kill point */
	int KP = 0 ;
	
    public Dungeon dungeon;
    public boolean accelerated = false;
    public boolean inCombat = false ;
    public List<Item> bag = new LinkedList<Item>();

	public Player(String ID) { super(ID); }
	
	@Override
	public void Attack(Creature foe) { throw new UnsupportedOperationException() ; }

}
