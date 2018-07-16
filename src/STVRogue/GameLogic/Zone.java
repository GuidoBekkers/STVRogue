package STVRogue.GameLogic;

import java.util.List;

import STVRogue.HelperPredicates;

import java.util.LinkedList; 

/**
 * Representing a "Zone" in a dungeon. A Zone consists of nodes
 * connected to each other. The nodes in a Zone should form a connected graph.
 * That is, there is always a path in this graph from which we can go from
 * any node x to any node y in the graph.
 */
public class Zone extends GameEntity {
	
	static boolean DEBUG = true ;
	
	/** Different types of zones. */
	public enum zoneType { 
		STARTzone,  // should contain a single start node
		EXITzone,   // should contain a single exit node
		InBETWEENzone, // should not contain start nor exit nodes, has at least two nodes, and of shape except linear
		DUMMYzone, // an in-between zone with exactly one node
		Corridor  // an in-between zone, with at least two nodes connected to form a linear sequence
		}
	
	List<Node> nodes = new LinkedList<Node>();
	zoneType type ;
	int level ;

	public List<Node> getNodes() {
		return nodes;
	}

	public zoneType getType() {
		return type;
	}

	public int getLevel() {
		return level;
	}

	/**
	 * Create a zone of the specified type and number of nodes.
	 */
	public Zone(String iD, zoneType ty, int zoneLevel, int numberOfnodes) {
		super(iD);
		if (zoneLevel<1 || numberOfnodes<1) throw new IllegalArgumentException() ;
		type = ty ;
		level = zoneLevel ;
		if (ty == zoneType.STARTzone) level = 1 ;
		else level = zoneLevel ;
		
		// TODO .. the implementation here
		if (true) throw new UnsupportedOperationException() ;
		
		// check if the zone is fully connected, and is of the right shape
		if (DEBUG) {
			assert nodes.size() >= 1 ;
			assert ty == Zone.zoneType.STARTzone ? HelperPredicates.hasStartZone(this) : true ;
			assert ty == Zone.zoneType.EXITzone  ? HelperPredicates.hasExitZone(this) : true ;
			assert ty == Zone.zoneType.DUMMYzone ? nodes.size() == 1 : true ;
			assert ty == Zone.zoneType.Corridor ? HelperPredicates.isCorridor(this) : true ;
			assert HelperPredicates.isConnected(this) ;
		}
	}
	

	
	
}