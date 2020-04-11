using System;
using STVrogue.GameLogic;

namespace STVrogue.TestInfrastructure
{
    /// <summary>
    /// For PART-2.
    /// Representing a recorded and replayable gameplay. 
    /// </summary>
    public class GamePlay
    {
        protected int turn = 0;

        protected int length;

        public GamePlay(){ }

        /// <summary>
        /// The current turn number in the play.
        /// </summary>
        public int Turn => turn;

        /// <summary>
        /// The length (in terms of how many turns) of the play recorded by
        /// this instance of GamePlay.
        /// </summary>
        public int Length => length;


        /// <summary>
        /// For saving the recorded play represented by this instance of GamePlay into
        /// a file.
        /// </summary>
        public GamePlay(String Savefile) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reset the recorded gameplay to turn 0.
        /// </summary>
        public virtual void Reset() { throw new NotImplementedException(); }

        /// <summary>
        /// True if the gameplay is at the end, hence has no more turn to do.
        /// </summary>
        public bool AtTheEnd() { return turn >= length; }

        /// <summary>
        /// Return the current state of the gameplay.
        /// </summary>
        public virtual Game GetState()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Replay the current turn, thus updating the game state.
        /// This also increases the turn nr, thus shifting the current turn to the next one. 
        /// </summary>
        public virtual void ReplayCurrentTurn() { throw new NotImplementedException(); }

    }

    /// <summary>
    /// A dummy GamePlay; for testing the specification classes.
    /// </summary>
    public class DummyGamePlay : GamePlay
    {
        int[] execution;
        Game state = new Game();
        public DummyGamePlay(params int[] execution)
        {
            this.execution = execution;
            length = execution.Length - 1;
            state = new Game();
        }
        public override void Reset() { turn = 0; 
            state.z_ = execution[0]; }
        public override Game GetState() { return state; }
        public override void ReplayCurrentTurn() { turn++; state.z_ = execution[turn]; }
    }
}
