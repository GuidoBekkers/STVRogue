package STVRogue.GameLogic;

public class Monster extends Creature {
	
    public Zone zoneToGuard ;
    
	public Monster(String ID) { super(ID); }
	
	@Override
	public void Attack(Creature foe) { throw new UnsupportedOperationException() ; }

}
