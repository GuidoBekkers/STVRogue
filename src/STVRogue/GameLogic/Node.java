package STVRogue.GameLogic;

import java.util.List; 
import java.util.LinkedList; 

/**
 * Representing a node in a dungeon.
 */
public class Node extends GameEntity {

	/** Different types of nodes. */
	public enum NodeType { 
		STARTnode,  // the starting node of the player. 
		EXITnode,   // representing the player's final destination.
		BRIDGE, 
		COMMONnode  // the type of the rest of the nodes. 
		}
	
	/** Neighbors node are considered connected to this node. The connection is bidirectional. */
    public List<Node>     neighbors = new LinkedList<Node>();
    public List<Creature> monsters  = new LinkedList<Creature>();
    public List<Item>     items     = new LinkedList<Item>();
    
    /** the zone to which this node belongs to: */
    public Zone zone ; 
    
    public NodeType type ;
    
    /** the capacity of this node */
    public int getCapacity() {
    	throw new UnsupportedOperationException() ;
    }

	public Node(NodeType ty, String iD) {
		super(iD);
		type = ty ;
	}

	/** To connect this node to another node. */
    public void connect(Node nd)
    {
        neighbors.add(nd); nd.neighbors.add(this);
    }

	/** To disconnect this node from the given node. */
    public void disconnect(Node nd)
    {
    	// only remove the first occurrence
        neighbors.remove(nd); nd.neighbors.remove(this);
    }
    
	/** return the set of nodes reachable from this node. */
	public List<Node> reachableNodes()
    {
		Node x = this ;
        List<Node> seen = new LinkedList<Node>();
        List<Node> todo = new LinkedList<Node>();
        todo.add(x);
        while(! todo.isEmpty()) {
        	x = todo.get(0); todo.remove(0) ;
        	seen.add(x) ;
        	for (Node y : x.neighbors) {
        		if (seen.contains(y) || todo.contains(y)) continue ;
        		todo.add(y) ;
        	}
        }
        return seen;
    }
	
	/** Check if the node nd is reachable from this node. */
	public boolean isReachable(Node nd) {
		return reachableNodes().contains(nd) ;
	}

}
