package STVRogue.TestInfrastructure;

import java.util.List;
import java.util.LinkedList;

/**
 * A "temporal specification" represents a correctness property that should hold over
 * an entire gameplay.
 */
public abstract class TemporalSpecification {
	
   static public final int RelevantlyValid = 2 ; /* non-trivially valid */
   static public final int TriviallyValid = 1 ;
   static public final int Invalid = 0 ;
	
   /** 
    * Check if this specification holds on the given gameplay. It returns an integer
    * with the following meaning:
    *    0 : the gameplay violates this specification (in other words this spec
    *        is invalid on the gameplay).
    *    1 : the gameplay satisfies this specification, but only because it does not
    *        meet this specification's assumptions (if there are any). Therefore
    *        the specification is only trivially valid on the gameplay.
    *    2 : the gameplay satisfies this specification's assumptions and its claim.
    *        Therefore it is relevantly valid.
    */
   abstract public int evaluate(GamePlay gameplay) ;
  
   /** 
    *  Evaluate this specification on a bunch of gameplays. It only returns RelevantlyValid (2)
    *  if there is no violation and furthermore the number of gameplays on which this
    *  specification is relevantly valid is at least the specified threshold.
    */
   public int evaluate(GamePlay[] gameplays, int threshold) {
	   int countRelevantlyValid = 0  ;
	   for (int k=0; k<gameplays.length ; k++) {
		   int verdict = evaluate(gameplays[k]) ;
		   if (verdict == Invalid) return Invalid ;
		   if (verdict == RelevantlyValid) countRelevantlyValid++ ;
	   }
	   if (countRelevantlyValid >= threshold) return RelevantlyValid ;
	   return TriviallyValid ;
   }
   
   /** Evaluate this specification on a bunch of gameplays. */
   public int evaluate(List<GamePlay> gameplays, int threshold) { 
	   GamePlay[] dummy = {} ;
	   return evaluate(gameplays.toArray(dummy),threshold) ;
   }
   
}
