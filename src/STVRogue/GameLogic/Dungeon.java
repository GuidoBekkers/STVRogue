package STVRogue.GameLogic;

import java.util.List;
import java.util.LinkedList;

/**
 * A dungeon is made of a sequence of zones/levels. 
 */
public class Dungeon {
    
	List<Zone> zones = new LinkedList<Zone>() ;
    Node startnode ;
    Node exitnode = null ;
    int capacityMultiplier ;
    
	public List<Zone> getZones() { return zones; }
	public Node getStartnode() { return startnode; }
	public Node getExitnode() { return exitnode; }
    
	/**
	 * Create a dungeon with the indicated number of zones (should be at least 3). This creates
	 * the start and exit zones. All the zones in-between are dummy zones. The zones should be
	 * linked linearly to each other with bridges.
	 * 
	 * Monsters and items are spawned into the start and exit zones,
	 */
	public Dungeon(int numberOfZones, int capacityMultiplier) 
	{
		if (numberOfZones < 3) throw new IllegalArgumentException() ;
		this.capacityMultiplier = capacityMultiplier ;
		// creating the start zone:
		int numOfNodesInstartZone = -1 ; // decide how many nodes
		Zone startZone = new Zone("Z1", Zone.zoneType.STARTzone, 1, numOfNodesInstartZone) ;
		zones.add(startZone) ;
		for (Node nd : startZone.nodes) {
			if (nd.type == Node.NodeType.STARTnode) {
				startnode = nd ; break ;
			}
		}
		// adding dummy in-between zones:
		Zone previousZone = startZone ;
		for (int z=2; z<numberOfZones; z++) {
			Zone zone = new Zone("Z" + z , Zone.zoneType.DUMMYzone, 1, 1) ;
			zones.add(zone) ;
			connectWithBridge(previousZone,zone) ;
			previousZone = zone ;		
		}
		// creating the exit zone:
		int numOfNodesInExitZone = -1 ; // decide how many nodes
		Zone exitZone = new Zone("Z" + numberOfZones, Zone.zoneType.EXITzone, 1, numOfNodesInExitZone) ;
		zones.add(exitZone) ;
		connectWithBridge(previousZone,exitZone) ;

		for (Node nd : startZone.nodes) {
			if (nd.type == Node.NodeType.EXITnode) {
				exitnode = nd ; break ;
			}
		}

		// seeding monsters in the initial and exit zones:
		seedMonsters(startZone) ;
		seedItems(startZone) ;
		seedMonsters(exitZone) ;
		seedItems(exitZone) ;
	}
    
	/** Link zone1 to zone2 through a bridge. The bridge should be part of zone1 (or, you can
	 * alternatively covert a node in zone1 to become a bridge. Make sure that all paths from
	 * zone1 to zone2 MUST pass through the bridge.
	 */
	static private void connectWithBridge(Zone zone1, Zone zone2) {
		new UnsupportedOperationException() ;

	}

	/** 
	 * This will generate a new zone, if there are still dummy zones left. The first
	 * dummy zone will be replaced by a real zone. It will be connected to the rest of
	 * the dungeon and seeded with monsters and items.
	 * 
	 * Depending on the player's kill point, the new zone is either a corridor-shaped,
	 * or just some arbitrary shape.
	 */
	public void generateZone(int playerKP) {
		Zone dummyzone = getNextDummyZone() ;
		Zone newZone = null ;
		if (dummyzone == null) return ;
		if (isPrimeNumber(playerKP)) {
			int numberOfNodes = -1 ; // decide how many nodes to generate
			newZone = new Zone(dummyzone.ID,Zone.zoneType.Corridor,dummyzone.level,numberOfNodes) ;
		}
		else {
			int numberOfNodes = -1 ; // decide how many nodes to generate
			newZone = new Zone(dummyzone.ID,Zone.zoneType.InBETWEENzone,dummyzone.level,numberOfNodes) ;
		}
		replace(dummyzone,newZone) ;
		seedMonsters(newZone) ;
		seedItems(newZone) ;		
	}
	
	/** Replace a dummy zone with a real zone. */
	private void replace(Zone dummyzone, Zone newzone) {
		new UnsupportedOperationException() ;
	}
	
	private boolean isPrimeNumber(int k) {
		new UnsupportedOperationException() ;
		return false; 
	}
	
	/** return the first still dummy zone, if any. Else null. */
	private Zone getNextDummyZone() {
		for(int k = 0 ; k < zones.size() ; k++) {
			Zone zone = zones.get(k) ;
			if (zone.type == Zone.zoneType.DUMMYzone) return zone ;
		}
		return null ;
	}
	
	/** Seed/spawn monsters into the given zone. */
	private void seedMonsters(Zone zone) {
		throw new UnsupportedOperationException() ; 
	}
	
	/** Seed/spawn items into the given zone. */
	private void seedItems(Zone zone) {
		throw new UnsupportedOperationException() ; 
	}
    
}
