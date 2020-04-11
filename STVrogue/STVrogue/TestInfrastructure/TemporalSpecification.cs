using System;
using System.Collections.Generic;
using STVrogue.GameLogic;
using static STVrogue.Utils.Utils ;

namespace STVrogue.TestInfrastructure
{

    /// <summary>
    /// Representing three types of judgement of a specification.
    /// </summary>
    public enum Judgement {
        Valid, DontKnow, Invalid
    }

    /// <summary>
    /// This is for PART-2.
    /// A "temporal specification" represents a correctness property that is judged over
    /// an entire gameplay.
    /// </summary>
    /* ITERATION 2.
     * 
     */
    public abstract class TemporalSpecification
    {

        /// <summary>
        /// Check if this specification holds on the given gameplay. It returns a Judgement
        /// with the following meaning:
        /// 
        ///    Invalid : the gameplay violates this specification (in other words this spec
        ///              is invalid on the gameplay).
        /// 
        ///    DontKnow : the gameplay does not violate this specification, but it does so
        ///               by violating the assumption of this specification, and therefore
        ///               trivially satisfying it.
        ///               This verdict is only applicable for a specification that has a
        ///               concept of "assumption".
        ///
        ///    Valid:     the gameplay does not violate this specification, nor its
        ///               assumption, if this specification has a concept of "assumption".
        ///
        /// </summary>
        abstract public Judgement Evaluate(GamePlay gameplay);

        /// <summary>
        /// Evaluate this specification on a bunch of gameplays. It returns three types of
        /// verdict:
        ///     Invalid: if the specification is invalid on one of the gameplay.
        ///     Valid: if no gameplay results in Invalid, and there are enough gameplays
        ///            that give Valid verdict. The needed number is specified by the
        ///            threshold parameter.
        ///     DontKnow: if none of the above two cases hold.
        /// </summary>
        public Judgement Evaluate(int threshold, params GamePlay[] gameplays)
        {
            int countRelevantlyValid = 0;
            for (int k = 0; k < gameplays.Length; k++)
            {
                Judgement verdict = Evaluate(gameplays[k]);
                if (verdict == Judgement.Invalid) return Judgement.Invalid;
                if (verdict == Judgement.Valid) countRelevantlyValid++;
            }
            if (countRelevantlyValid >= threshold) return Judgement.Valid;
            return Judgement.DontKnow;
        }

        public Judgement Evaluate(int threshold, List<GamePlay> gameplays)
        {
            return Evaluate(threshold, gameplays.ToArray());
        }
    }

    /// <summary>
    /// Representing a temporal property of the form "Always p", where p is a
    /// state-predicate. A gameplay satisfies this property if the predicate p
    /// holds on every game state through out the play.
    /// </summary>
    public class Always : TemporalSpecification
    {
        Predicate<Game> p;

        public Always(Predicate<Game> p) { this.p = p; }
       
        public override Judgement Evaluate(GamePlay sigma)
        {
            sigma.Reset();
            // check the initial state:
            Boolean ok = p(sigma.GetState());
            if (!ok)
            {
                // the predicate p is violated!
                Log("violation of Always at turn " + sigma.Turn);
                return Judgement.Invalid;
            }
            while (!sigma.AtTheEnd())
            {
                // replay the current turn (and get the next turn)
                sigma.ReplayCurrentTurn();
                // check if p holds on the state that resulted from replaying the turn
                ok = p(sigma.GetState());
                if (!ok)
                {
                    // the predicate p is violated!
                    Log("violation of Always at turn " + sigma.Turn);
                    return Judgement.Invalid;
                }

            }
            // if we reach this point than p holds on every state in the gameplay:
            return Judgement.Valid;
        }
    }
    
}
