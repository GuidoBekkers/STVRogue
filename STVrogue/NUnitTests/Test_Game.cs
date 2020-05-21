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

        // Test Game Flee with player Combinatorial
        [Test, Combinatorial]
        public void Test_Game_Flee_Player(
            [Values(1, 2, 3)] int roomType, // room nextToStart = 1, room nextToExit = 2, normal room = 3
            [Values(1, 9)] int healPotUsed, // Last turn heal potion was used 
            [Values(true, false)] bool playerEnraged, 
            [Values(DifficultyMode.NEWBIEmode, DifficultyMode.NORMALmode, DifficultyMode.ELITEmode)] DifficultyMode mode, 
            [Values(true, false)] bool eliteFlee) // Only relevant if next to exit
        {
            // Initialize correct gameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 5,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 1,
                initialNumberOfHealingPots = 1,
                initialNumberOfRagePots = 1,
                difficultyMode = mode
            };
            // Initialize the game
            Game g = new Game(gameConfiguration);
            
            // Set turn number to 10
            g.TurnNumber = 10;
            
            // Set healUsed
            g.HealUsed = healPotUsed;
            
            // Set player.enraged
            g.Player.Enraged = playerEnraged;

            // Move the player to a room with the correct neighbours
            switch (roomType)
            {
                case 1:
                    g.Player.Location = g.Dungeon.StartRoom.Neighbors.First();
                    break;
                case 2:
                    g.Player.Location = g.Dungeon.ExitRoom.Neighbors.First();
                    // Also set elite flee to the correct value if next to exit node
                    g.Player.EliteFlee = eliteFlee;
                    break;
                default:
                    foreach (Room r in g.Dungeon.StartRoom.Neighbors.First().Neighbors)
                    {
                        if (r.RoomType == RoomType.ORDINARYroom)
                        {
                            g.Player.Location = r;
                            break;
                        }
                    }
                    break;
            }
            // Initialize a monster and add it to the room player is in
            Monster m = new Monster("mId", "TestMonster", 10, 10);
            m.Location = g.Player.Location;
            g.Player.Location.Monsters.Add(m);
            
            // If room is next to start room player should always be able to flee
            if (roomType == 1)
            {
                // Check that 
                Assert.IsTrue(g.Flee(g.Player));
                Assert.IsFalse(m.Location == g.Player.Location);
                return;
            }
            
            // If heal potion is used at turn t-1 player should not be able to flee
            if (g.TurnNumber <= g.HealUsed + 1)
            {
                Assert.IsFalse(g.Flee(g.Player));
                Assert.IsTrue(m.Location == g.Player.Location);
                return;
            }

            // If difficulty mode is not newbie mode and player is enraged, player should not be able to flee
            // If in elite mode and player entered room neighboring exit room enraged, player should not be able to flee
            if (g.DifficultyMode != DifficultyMode.NEWBIEmode && g.Player.Enraged ||( g.DifficultyMode == DifficultyMode.ELITEmode && (roomType == 2 && !eliteFlee)))
            {
                Assert.IsFalse(g.Flee(g.Player));
                Assert.IsTrue(m.Location == g.Player.Location);
                return;
            }

            // In all other cases player should be able to flee
            Assert.IsTrue(g.Flee(g.Player));
            Assert.IsFalse(m.Location == g.Player.Location);
        }

        // Test game flee player trying to flee to exit room
        [Test]
        public void Test_Game_Flee_Player_ToExitRoom()
        {
            // Initialize gameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 5,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 1,
                initialNumberOfHealingPots = 1,
                initialNumberOfRagePots = 1,
                difficultyMode = DifficultyMode.NORMALmode
            };
            // Initialize the game
            Game g = new Game(gameConfiguration);
                
            // Initialize a room with a monster and a player connecting to an exit room
            Room room1 = new Room("1",RoomType.ORDINARYroom, 5);
            Room room2 = new Room("2",RoomType.EXITroom, 5);
            room1.Connect(room2);
            g.Player.Location = room1;
            Monster m = new Monster("mId", "TestMonster", 10, 10);
            m.Location = room1;
            room1.Monsters.Add(m);

            // Check if player fleeing to exit room returns false
            Assert.IsFalse(g.Flee(g.Player));
            Assert.IsTrue(g.Player.Location == room1);
        }
        
        // Test game flee with monster to neighboring room not at max capacity
        [Test]
        public void Test_Game_Flee_Monster()
        {
            // Initialize correct gameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 5,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 1,
                initialNumberOfHealingPots = 1,
                initialNumberOfRagePots = 1,
                difficultyMode = DifficultyMode.NORMALmode
            };
            
            // Initialize the game
            Game g = new Game(gameConfiguration);
            
            // Initialize a room with a monster and a player connecting to a room not at max capacity
            Room room1 = new Room("1",RoomType.ORDINARYroom, 5);
            Room room2 = new Room("2",RoomType.ORDINARYroom, 5);
            room1.Connect(room2);
            g.Player.Location = room1;
            Monster m = new Monster("mId", "TestMonster", 10, 10);
            m.Location = room1;
            room1.Monsters.Add(m);
            
            // Check if monster trying to flee returns true
            Assert.IsTrue(g.Flee(m));
            Assert.IsTrue(m.Location == room2);
        }

        // Test game flee with monster with all neighboring rooms at max capacity
        [Test]
        public void Test_Game_Flee_Monster_MaxCapacity()
        {
            // Initialize correct gameConfiguration
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                numberOfRooms = 5,
                maxRoomCapacity = 5,
                dungeonShape = DungeonShapeType.LINEARshape,
                initialNumberOfMonsters = 1,
                initialNumberOfHealingPots = 1,
                initialNumberOfRagePots = 1,
                difficultyMode = DifficultyMode.NORMALmode
            };
            
            // Initialize the game
            Game g = new Game(gameConfiguration);
            
            // Initialize a room with a monster and a player connecting to a room 
            Room room1 = new Room("1",RoomType.ORDINARYroom, 2);
            Room room2 = new Room("2",RoomType.EXITroom, 2);
            room1.Connect(room2);
            g.Player.Location = room1;
            Monster m1 = new Monster("3", "TestMonster", 10, 10);
            Monster m2= new Monster("4", "TestMonster", 10, 10);
            Monster m3 = new Monster("5", "TestMonster", 10, 10);
            m1.Location = room1;
            room1.Monsters.Add(m1);
            m2.Location = room2;
            room2.Monsters.Add(m2);
            m3.Location = room2;
            room2.Monsters.Add(m3);
            
            // Check if monster trying to flee returns false
            Assert.IsFalse(g.Flee(m1));
            Assert.IsFalse(m1.Location == room2);
        }

        // Test player and monster not in combat fleeing
        
        
        [Test]
        public void Test_Game_Update()
        {
            // TODO: write Game.Update test
            throw new NotImplementedException();
        }
    }
}