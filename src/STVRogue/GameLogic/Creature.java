package STVRogue.GameLogic;

public class Creature extends GameEntity {

	public String name ;
	public int HPmax ;  // full health HP
	public int HP ;     // current HP, should never exceed HPmax
	public Node location ;
	public int AttackRating ;
	
	public Creature(String ID) { super(ID); }
	
	public void Attack(Creature foe) { 
		throw new UnsupportedOperationException() ; 
	}

}
