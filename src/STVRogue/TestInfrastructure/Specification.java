package STVRogue.TestInfrastructure;

import java.util.List;
import java.util.LinkedList;

public abstract class Specification {
	
   static public final int RelevantlyValid = 2 ; /* non-trivially valid */
   static public final int TriviallyValid = 1 ;
   static public final int Invalid = 0 ;
	
   /** Evaluate this specification on a single gameplay. */
   abstract public int evaluate(GamePlay gameplay) ;
  
   /** 
    *  Evaluate this specification on a bunch of gameplays. It only returns RelevantlyValid
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
