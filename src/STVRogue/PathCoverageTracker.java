package STVRogue;

import java.util.*;


/**
 * A utility class to help you track path-coverage. Use addTargetPath to specify
 * the paths that we want to cover (target paths).
 * 
 *    - Use startPath() to signal the start of a test execution.
 *    - Use tickNode(nodeName) to signal visit to a node/state.
 *    - Use endPath() to signal the end of the test execution.
 *    - Use printSummary() etc to get information of coverage.
 */
public class PathCoverageTracker {
	
	List<String> testRerquirements = new LinkedList<String>() ;
	List<String> executed = new LinkedList<String>() ;
	String currentPath = null ;
	
	public PathCoverageTracker() {
		
	}
	
	/** 
	 * Add a path to cover. Specify it as a string in the format of node-ids separated by ":".
	 * Example: "s1:s2:s3"
	 */
	public void addTargetPath(String path) {  testRerquirements.add(path) ; }
	
	/** Start tracking a path. */
	public void startPath() { currentPath = "" ; }
	
	/** Register that we pass the given node */
	public void tickNode(String node) {
		if (currentPath == "") currentPath += node ;
		else currentPath += ":" + node ;
	}
	
	/** Signal the end of the current path. It will then be added to the set of executed path. */
	public void endPath() {
		for (String p : executed) {
			if (p.equals(currentPath)) break ;
		}
		executed.add(currentPath) ;
	}
	
	public List<String> getCoveredPaths() {
		List<String> covered = new LinkedList<String>() ;
		for (String target : testRerquirements) {
			for (String sigma : executed) {
				if(tour(sigma,target)) {
					covered.add(target) ; break ;
				}
			}
		}
		return covered ;
	}
	
	public List<String> getUncoveredPaths() {
		List<String> covered = getCoveredPaths() ;
		List<String> uncovered = new LinkedList<String>() ;
		for (String p : testRerquirements) {
			boolean cov = false ;
			for (String pcov : covered) {
				if(p.equals(pcov)) {
					cov = true ; break ;
				}
			}
			if (!cov) uncovered.add(p) ;
		}
		return uncovered ;
	}
	
	/** Return the list of paths to cover. */
	public List<String> getTestRerquirements() {
		return testRerquirements;
	}

	/** Return the current set of test paths. */
	public List<String> getTestPaths() {
		return executed;
	}

	/** Check if path1 tours path2 */
	static public boolean tour(String path1, String path2) {
		return path1.contains(path2) ;
	}
	
	public String printCovered() {
		StringBuffer out = new StringBuffer() ;
		List<String> covered = getCoveredPaths() ;
		for (int k=0; k<covered.size(); k++) {
			out.append(covered.get(k)) ;
			out.append("\n") ;
		}
		out.append("Covered: " + covered.size() + " paths.") ;
		return out.toString() ;
	}
	
	public String printUncovered() {
		StringBuffer out = new StringBuffer() ;
		List<String> uncovered = getUncoveredPaths() ;
		for (int k=0; k<uncovered.size(); k++) {
			out.append(uncovered.get(k)) ;
			out.append("\n") ;
		}
		out.append("Uncovered: " + uncovered.size() + " paths.") ;
		return out.toString() ;
	}
	
	public String printSummary() {
		int N = testRerquirements.size() ;
		int n = getCoveredPaths().size()  ;
		String out = "** The tests cover " + n + " targets out of " + N ;
		if (n>=N) out += ". Well done!" ;
		else {
			out += "\n** Covered:" ;
			for (String s : getCoveredPaths()) {
				out += "\n     " + s ;
			}
			out += "\n** Uncovered:" ;
			for (String s : getUncoveredPaths()) {
				out += "\n     " + s ;
			}
		}
        return out ;
 	}
	
	/** just for testing out */
	static public void main(String[] args) {
		PathCoverageTracker tracker = new PathCoverageTracker() ;
		tracker.addTargetPath("0:1:2");
		tracker.addTargetPath("0:1:3");
		tracker.startPath();
		tracker.tickNode("0");
		tracker.tickNode("1");
		tracker.tickNode("0");
		tracker.tickNode("1");
		tracker.tickNode("3");
		tracker.tickNode("0");
		tracker.endPath();
		//System.out.println(tracker.printCovered());
		//System.out.println(tracker.printUncovered());
		System.out.println(tracker.printSummary());
		
	}

}
