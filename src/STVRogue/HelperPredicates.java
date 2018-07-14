package STVRogue;

import java.util.Collection;
import java.util.LinkedList;
import java.util.List;
import java.util.function.Predicate;

import STVRogue.GameLogic.Node;
import STVRogue.GameLogic.Zone;

/** A bunch of useful helper predicates or predicate-operators */
public class HelperPredicates {
	
	/** 
	 * Forall-quantifier. Example: forall(C, x -> x>=0) checks whether all integers
	 * in the collection C are non-negative.
	 * */
	static public <T> boolean forall(Collection<T> C, Predicate<T> p) {
		for (T x : C) {
			if (! p.test(x)) return false ;
		}
		return true ;
	}

	// just for demonstrating the syntax to you:
	private void test() {
		List<Integer> C = new LinkedList<Integer>() ;
		forall(C, x -> x >= 0) ;
		forall(C, (Integer x) -> x >= 0) ; // if you need to explicitly specify the type of x
		exists(C, x -> x <0) ;
		exists(C, (Integer x) -> x <0) ;
		
	}
	
	/** 
	 * Exist-quantifier. Example: forall(C, x -> x<0) checks whether C contaisn 
	 * a negative integer.
	 */
	static public <T> boolean exists(Collection<T> C, Predicate<T> p) {
		return ! forall(C, x -> !p.test(x)) ;
	}
	
	/** Check if U = {y} */
	static private boolean isSingletonContaining(Collection<Node> U, Node y) {
		return U.size() == 1 && U.contains(y) ;
	}
	
	/** Check if U = {y1,Y2} */
	static private boolean isPairContaining(Collection<Node> U, Node y1, Node y2) {
		return U.size() == 2 && U.contains(y1)  && U.contains(y2) && y1 != y2 ;
	}
	
	/** Check if a zone has the corridor shape */
	static public boolean isCorridor(Zone zone) {
		List<Node> nodes = zone.getNodes() ;
		// check node-0:
		if (! isSingletonContaining(nodes.get(0).neighbors,nodes.get(1))) 
			return false  ;
		for (int k=1 ; k<nodes.size()-1 ; k++) {
			if (! isPairContaining(nodes.get(k).neighbors, nodes.get(k-1), nodes.get(k+1)))
					return false ;
		}
		return true ;
	}

	/** Check if a zone contains exactly one start-node. */
	static public boolean hasStartZone(Zone zone) {
		int count = 0 ;
		for (Node x : zone.getNodes()) {
			if (x.type == Node.NodeType.STARTnode) count++ ;
		}
		return count == 1 ;
	}

	/** Check if a zone contains exactly one exit-node. */
	static public boolean hasExitZone(Zone zone) {
		int count = 0 ;
		for (Node x : zone.getNodes()) {
			if (x.type == Node.NodeType.EXITnode) count++ ;
		}
		return count == 1 ;
	}
	
	/** Check if the zone is indeed fully connected. */
	static public boolean isConnected(Zone zone) {
		List<Node> nodes = zone.getNodes() ;
		for(Node x : nodes) {
			for(Node y : nodes) {
				if (! x.reachableNodes().contains(y) || ! y.reachableNodes().contains(x))
					return false ;
			}
		}
		return true ;
	}
}
