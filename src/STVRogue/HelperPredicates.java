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
	 * Forall-quantifier over a collection. 
	 * Example: forall(C, x -> x>=0) checks whether all integers in the collection 
	 * C are non-negative.
	 * */
	static public <T> boolean forall(Collection<T> C, Predicate<T> p) {
		for (T x : C) {
			if (! p.test(x)) return false ;
		}
		return true ;
	}
	
	/** 
	 * Forall-quantifier over an array. Example: forall(a, x -> x>=0) checks whether 
	 * all integers in the array a are non-negative.
	 * */
	static public <T> boolean forall(T[] a, Predicate<T> p) {
		for (T x : a) {
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
	 * Exist-quantifier over a collection. Example: forall(C, x -> x<0) checks whether 
	 * the collection C contains a negative integer.
	 */
	static public <T> boolean exists(Collection<T> C, Predicate<T> p) {
		return ! forall(C, x -> !p.test(x)) ;
	}
	
	/**
	 * Check if p implies q (which is equivalent to !p or q).
	 */
	static public boolean imp(boolean p, boolean q) {
		return !p || q ;
	}
	
	/** 
	 * Exist-quantifier over an array. Example: forall(a, x -> x<0) checks whether 
	 * the array a contains a negative integer.
	 */
	static public <T> boolean exists(T[] a, Predicate<T> p) {
		return ! forall(a, x -> !p.test(x)) ;
	}

	/** Check if a zone contains exactly one start-node. */
	static public boolean hasOneStartZone(Zone zone) {
		int count = 0 ;
		for (Node x : zone.getNodes()) {
			if (x.type == Node.NodeType.STARTnode) count++ ;
		}
		return count == 1 ;
	}

	/** Check if a zone contains exactly one exit-node. */
	static public boolean hasOneExitZone(Zone zone) {
		int count = 0 ;
		for (Node x : zone.getNodes()) {
			if (x.type == Node.NodeType.EXITnode) count++ ;
		}
		return count == 1 ;
	}
	
	
	/** Check if the zone is indeed fully connected. */
	static public boolean isConnected(Zone zone) {
		throw new UnsupportedOperationException() ;
	}
}
