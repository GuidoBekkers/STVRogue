package STVRogue.TestInfrastructure;

import STVRogue.HelperPredicates;
import STVRogue.GameLogic.Monster;

public class ExampleSpecifications {
	
	static public Specification example1 = new Always(G -> G.player.HP >= 0) ;
	
	static public Specification example2 = new Always(G -> HelperPredicates.forall(G.monsters(), m -> m.location.zone == m.zoneToGuard)) ;
	
	
	static public Specification example3 = new Unless(G -> G.turn == G.player, G -> G.turn instanceof Monster) ;
	
	
}
