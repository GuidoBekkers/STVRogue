using System;
using System.Linq;
using NUnit.Framework;
using STVrogue.GameLogic;

namespace NUnitTests
{
    [TestFixture]
    public class Test_Game
    {
        [Test]
        public void Test_GameConfiguration()
        {
            GameConfiguration gameConfiguration = new GameConfiguration();
            Assert.IsTrue(gameConfiguration.GetDifficultyMultiplier(DifficultyMode.ELITEmode) == 1);
            Assert.IsTrue(gameConfiguration.GetDifficultyMultiplier(DifficultyMode.NORMALmode) == 2);
            Assert.IsTrue(gameConfiguration.GetDifficultyMultiplier(DifficultyMode.NEWBIEmode) == 3);
        }
        
        [Test]
        public void Test_Game_Constructor()
        {
            // Create a exception variable for debug purposes
            Exception exc;
            
            // Create the game variable
            Game g;
            
            // Initialize the valid GameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 10,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 10,
                initialNumberOfHealingPots = 10,
                initialNumberOfRagePots = 10,
                difficultyMode = DifficultyMode.NORMALmode
            };

            // Try initializing the Game
            try
            {
                g = new Game(gameConfiguration);
            }
            catch (Exception e)
            {
                exc = e;
                throw;
            }
            
            // Post conditions 
            // Check if the game object is made
            Assert.IsNotNull(g);
            
            // Check if the variables are set correctly
            Assert.IsTrue(gameConfiguration.difficultyMode == g.DifficultyMode);
            Assert.IsNotNull(g.Player);
            Assert.IsNotNull(g.Dungeon);
            Assert.IsTrue(g.TurnNumber == 0);
            Assert.IsTrue(g.Player.Location == g.Dungeon.StartRoom);
            Assert.IsFalse(g.Gameover);
        }

        [Test]
        public void Test_Game_Constructor_InvalidDungeon()
        {
            // Create the game variable
            Game g;
            
            // Initialize the invalid GameConfiguration
            GameConfiguration gameConfiguration= new GameConfiguration
            {
                numberOfRooms = 1,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 10,
                initialNumberOfHealingPots = 10,
                initialNumberOfRagePots = 10,
                difficultyMode = DifficultyMode.NORMALmode
            };

            // Check if the correct exception is thrown
            Assert.Throws<ArgumentException>(() => g = new Game(gameConfiguration));
        }

        [Test]
        public void Test_Game_GetSetFunctions()
        {
            // Initialize the valid GameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 10,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 10,
                initialNumberOfHealingPots = 10,
                initialNumberOfRagePots = 10,
                difficultyMode = DifficultyMode.NORMALmode
            };
            
            // Initialize the game
            Game g = new Game(gameConfiguration);
            
            // Check if the player is not null
            Assert.IsNotNull(g.Player);
            // Check if the getter works
            Assert.IsTrue(g.Player is Player);
            
            // Check if the dungeon is not null
            Assert.IsNotNull(g.Dungeon);
            // Check if the getter works
            Assert.IsTrue(g.Dungeon is Dungeon);
            
            // Check the TurnNumber getter and setter
            int tN = 100;
            g.TurnNumber = tN;
            Assert.IsTrue(g.TurnNumber == tN);
            
            // Check the HealUsed getter and setter
            int hU = 1000;
            g.HealUsed = hU;
            Assert.IsTrue(g.HealUsed == hU);
            
            // Check the RageUsed getter and setter
            int rU = 1000;
            g.RageUsed = rU;
            Assert.IsTrue(g.RageUsed == rU);
            
            // Check if the GameOver getter works
            Assert.IsFalse(g.Gameover);
            
            // Check if the DifficultyMode getter works
            Assert.IsTrue(g.DifficultyMode == gameConfiguration.difficultyMode);
        }

        [Test]
        public void Test_Game_Move()
        {
            // Initialize the valid GameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 3,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 10,
                initialNumberOfHealingPots = 10,
                initialNumberOfRagePots = 10,
                difficultyMode = DifficultyMode.NORMALmode
            };
            
            // Initialize the game
            Game g = new Game(gameConfiguration);
            
            // Store the destination room
            Room dest = g.Player.Location.Neighbors.First();
            
            // Check if the room is indeed reachable
            Assert.IsTrue(g.Player.Location.CanReach(dest));
            
            // Try moving the player
            g.Move(g.Player, dest);
            
            // Check if the player has indeed moved to the room
            Assert.IsTrue(g.Player.Location == dest);
        }
        
        [Test]
        public void Test_Game_Move_NonNeighbour()
        {
            // Initialize the valid GameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 3,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 10,
                initialNumberOfHealingPots = 10,
                initialNumberOfRagePots = 10,
                difficultyMode = DifficultyMode.NORMALmode
            };
            
            // Initialize the game
            Game g = new Game(gameConfiguration);
            
            // Store the invalid destination room
            Room dest = g.Dungeon.ExitRoom;
            
            // Check that the destination is indeed unreachable
            Assert.IsFalse(g.Player.Location.CanReach(dest));
            
            // Check for the correct exception
            Assert.Throws<ArgumentException>(() => g.Move(g.Player, dest));
        }

        [Test]
        public void Test_Game_Attack()
        {
            // Initialize the valid GameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 3,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 10,
                initialNumberOfHealingPots = 10,
                initialNumberOfRagePots = 10,
                difficultyMode = DifficultyMode.NORMALmode
            };
            
            // Initialize the game
            Game g = new Game(gameConfiguration);

            // Initialize the foe
            Monster foe = new Monster("mId", "mName", 5, 1);
            
            // Set their location to be equal to the player's location
            foe.Location = g.Player.Location;
            
            // Execute the attack
            g.Attack(g.Player, foe);
            
            // Check if the attack was executed
            Assert.IsTrue(foe.Hp == 5 - gameConfiguration.playerBaseAr);
        }
        
        [Test]
        public void Test_Game_Attack_NotAlive()
        {
            // Initialize the valid GameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 3,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 10,
                initialNumberOfHealingPots = 10,
                initialNumberOfRagePots = 10,
                difficultyMode = DifficultyMode.NORMALmode
            };
            
            // Initialize the game
            Game g = new Game(gameConfiguration);

            // Initialize the foe
            Monster foe = new Monster("mId", "mName", 5, 1);
            
            // Set the foe to be dead
            foe.Alive = false;
            
            // Check if the correct exception is thrown
            Assert.Throws<ArgumentException>(() => g.Attack(g.Player, foe));
        }
        
        [Test]
        public void Test_Game_Attack_NotSameRoom()
        {
            // Initialize the valid GameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 3,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 10,
                initialNumberOfHealingPots = 10,
                initialNumberOfRagePots = 10,
                difficultyMode = DifficultyMode.NORMALmode
            };
            
            // Initialize the game
            Game g = new Game(gameConfiguration);

            // Initialize the foe
            Monster foe = new Monster("mId", "mName", 5, 1);
            
            // Set their location to be not equal to the player's location
            foe.Location = g.Player.Location.Neighbors.First();
            
            // Check if the correct exception is thrown
            Assert.Throws<ArgumentException>(() => g.Attack(g.Player, foe));
        }

        [Test]
        public void Test_Game_UseItem_HealingPotion()
        {
            // Initialize the valid GameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 3,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 10,
                initialNumberOfHealingPots = 10,
                initialNumberOfRagePots = 10,
                difficultyMode = DifficultyMode.NORMALmode
            };
            
            // Initialize the game
            Game g = new Game(gameConfiguration);
            
            // Set the player's health to 1
            g.Player.Hp = 1;

            // Initialize the healing potion
            HealingPotion potion = new HealingPotion("hId", 1);
            
            // Store the current healUsed
            int healUsedOld = g.HealUsed;
            
            // Up the turn number
            g.TurnNumber = 5;
            
            // Use the healing potion
            g.UseItem(potion);
            
            // Check if the healUsed is updated
            Assert.IsTrue(g.HealUsed != healUsedOld && g.HealUsed == g.TurnNumber);
            
            // Check if the healing potion was used
            Assert.IsTrue(g.Player.Hp == 2);
        }

        [Test]
        public void Test_Game_UseItem_RagePotion()
        {
            // Initialize the valid GameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 3,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 10,
                initialNumberOfHealingPots = 10,
                initialNumberOfRagePots = 10,
                difficultyMode = DifficultyMode.NORMALmode
            };
            
            // Initialize the game
            Game g = new Game(gameConfiguration);

            // Initialize the potion
            RagePotion potion = new RagePotion("rId");
            
            // Store the current healUsed
            int rageUsedOld = g.RageUsed;
            
            // Up the turn number
            g.TurnNumber = 5;
            
            // Use the rage potion
            g.UseItem(potion);
            
            // Check if the rageUsed is updated
            Assert.IsTrue(g.RageUsed != rageUsedOld && g.RageUsed == g.TurnNumber);
            
            // Check if the rage potion was used
            Assert.IsTrue(g.Player.Enraged);
        }

        [Test]
        public void Test_Game_UseItem_RagePotion_EliteFlee()
        {
            // Initialize the valid GameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 3,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 10,
                initialNumberOfHealingPots = 10,
                initialNumberOfRagePots = 10,
                difficultyMode = DifficultyMode.ELITEmode
            };
            
            // Initialize the game
            Game g = new Game(gameConfiguration);
            
            // Move the player to a room adjacent to the exit room
            g.Player.Location = g.Dungeon.ExitRoom.Neighbors.First();

            // Initialize the potion
            RagePotion potion = new RagePotion("rId");
            
            // Store the current healUsed
            int rageUsedOld = g.RageUsed;
            
            // Up the turn number
            g.TurnNumber = 5;
            
            // Use the rage potion
            g.UseItem(potion);
            
            // Check if the rageUsed is updated
            Assert.IsTrue(g.RageUsed != rageUsedOld && g.RageUsed == g.TurnNumber);
            
            // Check if the rage potion was used
            Assert.IsTrue(g.Player.Enraged);
            
            // Check if the EliteFlee bool was updated
            Assert.IsFalse(g.Player.EliteFlee);
        }

        [Test]
        public void Test_Game_Flee()
        {
            // TODO: write Game.Flee test
            throw new NotImplementedException();
        }

        [Test]
        public void Test_Game_Update()
        {
            // TODO: write Game.Update test
            throw new NotImplementedException();
        }
    }
}