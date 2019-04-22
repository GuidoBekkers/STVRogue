package STVRogue.TestInfrastructure;

import STVRogue.HelperPredicates;
import STVRogue.GameLogic.Monster;

public class ExampleSpecifications {
	
	static public TemporalSpecification example1 = new Always(G -> G.player.HP >= 0) ;
	
	static public TemporalSpecification example3 = new Unless(G -> G.whoHasTheTurn == G.player, G -> G.whoHasTheTurn instanceof Monster) ;
	
	
}
