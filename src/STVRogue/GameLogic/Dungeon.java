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
	 * the start and exit zones. The zones should be linked linearly to each other with bridges.
	 */
	public Dungeon(int numberOfZones, int capacityMultiplier) 
	{
		if (numberOfZones < 3) throw new IllegalArgumentException() ;
		this.capacityMultiplier = capacityMultiplier ;
		// creating the start zone:
		int numOfNodesInstartZone = -1 ; // FIX THIS! Decide how many nodes
		Zone startZone = new Zone("Z1", Zone.zoneType.STARTzone, 1, numOfNodesInstartZone) ;
		zones.add(startZone) ;
		for (Node nd : startZone.nodes) {
			if (nd.type == Node.NodeType.STARTnode) {
				startnode = nd ; break ;
			}
		}
		// adding in-between zones:
		Zone previousZone = startZone ;
		for (int z=2; z<numberOfZones; z++) {
			int numOfNodes = -1 ; // FIX THIS! Decide how many nodes
			Zone zone = new Zone("Z" + z , Zone.zoneType.InBETWEENzone, 1, numOfNodes) ;
			zones.add(zone) ;
			connectWithBridge(previousZone,zone) ;
			previousZone = zone ;		
		}
		// creating the exit zone:
		int numOfNodesInExitZone = -1 ; // FIX THIS! decide how many nodes
		Zone exitZone = new Zone("Z" + numberOfZones, Zone.zoneType.EXITzone, 1, numOfNodesInExitZone) ;
		zones.add(exitZone) ;
		connectWithBridge(previousZone,exitZone) ;

		for (Node nd : exitZone.nodes) {
			if (nd.type == Node.NodeType.EXITnode) {
				exitnode = nd ; break ;
			}
		}
	}
    
	/**
	 * Drop monsters and items into the dungeon.
	 */
	public void seedMonstersAndItems() {
		throw new UnsupportedOperationException() ; 
	}
	
	
	/** Link zone1 to zone2 through a bridge. The bridge should be part of zone1 (or, you can
	 * alternatively convert a node in zone1 to become a bridge. Make sure that all paths from
	 * zone1 to zone2 MUST pass through the bridge.
	 */
	static private void connectWithBridge(Zone zone1, Zone zone2) {
		new UnsupportedOperationException() ;

	}
    
}
