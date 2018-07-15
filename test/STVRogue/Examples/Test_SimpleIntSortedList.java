package STVRogue.Examples;

import java.nio.file.*;
import java.util.Iterator;

import org.junit.Test;

/**
 * An example demonstrating the use of the random testing tool T3 to test the class 
 * STVRogue.Examples.SimpleIntSortedList.
 * 
 * You need T3.jar. 
 */
public class Test_SimpleIntSortedList {
	
    /**
     * This test method will invoke T3. The first-time you run it, it will generate random sequences
     * of method calls against the target class (in this case: SimpleIntSortedList). Unlike standard
     * testing with JUnit, T3 requires you to put assertions in the form of an "assert" command,
     * in the code of the target-class itself. Additionally, you can also specify a class invariant.
     * Check out the Documentation of T3. If any of these assertions are violated, T3 will report it
     * to you.
     * 
     * We will make it so, that the generated test sequences will be saved in a so-called trace-file.
     * Next time you run the same test, we will make it so, that you will reload this trace-file,
     * and re-execute the same sequences, rather than generating fresh sequences.
     * 
     * The first time (when you invoke T3), T3 will swallow exceptions, So from the JUnit perspective,
     * the test will be "green" (no error observed) despite that T3 might report that there is actually 
     * a violation. The second time you run the test, where we then just replay the generated test
     * sequences, the replay will indeed throw an exception if some assertions are violated,
     * 
     * IMPORTANT: configure the Run configuration of T3 test classes to include -ea to enable assertion
     * checking.
     */
	@Test
	public void testC() throws Throwable {
		// Configuring T3 options:
        String dirToSaveTraceFiles = "." ;
        String onlyEnableADTtesting = "-adt true" ;
        String doNotMessWithFields = "-fup 0.0" ;
        String saveTheTraces = "-d " +  dirToSaveTraceFiles ;
        String showFirstViolatingTrace = "-sex" ;
        String targetClass = "STVRogue.Examples.SimpleIntSortedList" ;
        String T3arg = onlyEnableADTtesting + " "
        		       + doNotMessWithFields + " "
        		       + saveTheTraces + " "
        		       + showFirstViolatingTrace + " "
        		       + targetClass ;
        // Configuring T3-replayer options:
        String showViolation = "-sx" ;
        String enableBulkTraceFilesLoading = "-b " + mkRegex(targetClass) ; // regular expression to match your trace-file names
        
        // Now run either T3 or Replay-tool:
		if(! trFileExists(dirToSaveTraceFiles,targetClass))  {
	         Sequenic.T3.T3Cmd.main(T3arg) ;
	     }
		else 
			Sequenic.T3.ReplayCmd.main("-sx -b " + mkRegex(targetClass) + " " +  dirToSaveTraceFiles) ;	
	}
	
	// to check if a tracefile for the given target class exists:
	private static boolean trFileExists(String dir, String fullClassName) throws Exception {
		Path dir_ = Paths.get(dir) ;
		String pattern = "*" + fullClassName + "*.tr" ;	
		DirectoryStream<Path> stream = Files.newDirectoryStream(dir_, pattern) ; 
		return stream.iterator().hasNext() ;
	}
	
	static private String mkRegex(String fullclassName) {
		return ".*" + fullclassName.replace(".", "\\.") + ".*" ;
	}
}
