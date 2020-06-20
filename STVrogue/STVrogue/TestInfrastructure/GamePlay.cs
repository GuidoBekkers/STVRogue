using System;
using STVrogue.GameControl;
using STVrogue.GameLogic;
using STVrogue.Utils;

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

        protected Command[] actions;

        protected Game simulatedGame;

        public string saveFileLocation;

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
        /// Load the data from the save file and initialize the simulatedGame
        /// </summary>
        private void LoadSimGame()
        {
            // Instance the game according to the save file
            simulatedGame = Program.CreateGame(SaveHelper.LoadConfig(saveFileLocation));
            
            // Load the recorded actions
            actions = SaveHelper.LoadActions(saveFileLocation);
        }

        /// <summary>
        /// For saving the recorded play represented by this instance of GamePlay into
        /// a file.
        /// </summary>
        public GamePlay(string saveFile)
        {
            // Reset the id factory
            IdFactory.ResetIdFactory();
            
            // Store the save file location
            saveFileLocation = saveFile;

            // Load the data from the save file
            LoadSimGame();

            // Store the length of the game recorded in this save file
            length = actions.Length;
        }

        /// <summary>
        /// Get the fully simulated game
        /// </summary>
        public Game GetLoadedGame()
        {
            // Execute all saved actions
            while (!AtTheEnd())
            {
                ReplayCurrentTurn();
            }
            
            // Return the fully simulated game
            return simulatedGame;
        }

        /// <summary>
        /// Reset the recorded gameplay to turn 0.
        /// </summary>
        public virtual void Reset()
        {
            // Reset the turn number
            turn = 0;
            
            // Reset the id factory
            IdFactory.ResetIdFactory();
            
            simulatedGame.ResetRandom();
            
            // Reset the game and action list
            LoadSimGame();
        }

        /// <summary>
        /// True if the gameplay is at the end, hence has no more turn to do.
        /// </summary>
        public bool AtTheEnd() { return turn >= length; }

        /// <summary>
        /// Return the current state of the gameplay.
        /// </summary>
        public virtual Game GetState()
        {
            return simulatedGame;
        }

        /// <summary>
        /// Replay the current turn, thus updating the game state.
        /// This also increases the turn nr, thus shifting the current turn to the next one. 
        /// </summary>
        public virtual void ReplayCurrentTurn()
        {
            // Execute this turns action and update the turn number
            simulatedGame.Update(actions[turn++]);
        }

    }

    /*
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
    */
}
