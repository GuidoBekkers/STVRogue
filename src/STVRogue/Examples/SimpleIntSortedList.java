package STVRogue.Examples;

import java.util.LinkedList;

/**
 * Just an example of some class. We use this as the target in some
 * examples of automated testing using T3.
 * 
 */
public class SimpleIntSortedList {
	
    private LinkedList<Integer> s;
    private Integer max;

    public SimpleIntSortedList() { s = new LinkedList<Integer>(); }

	public void insert(Integer x) {
    	assert x!=null : "PRE";
        int i = 0;
        for (Integer y : s) {
            if (y > x) break;
            i++;
        }
        s.add(i, x);
        // bug: should be x > max
        if (max == null || x < max) max = x;
    }

    // Retrieve the greatest element from the list, if it is not empty.
    public Integer get() {
    	assert !s.isEmpty() : "PRE";
        Integer x = max;
        s.remove(max);
        if (s.isEmpty()) max = null ;
        else max = s.getLast() ;
        assert s.isEmpty() || x >= s.getLast() : "POST";
        return x;
    }
    
    // a class invariant
    private boolean classinv__() {
    	return s.isEmpty() || s.contains(max);
    }
	
}
