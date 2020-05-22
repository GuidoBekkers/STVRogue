using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using STVrogue.GameLogic;
using STVrogue.Utils;

namespace NUnitTests
{
   [TestFixture]
    public class Test_Dungeon
    {
        /// <summary>
        /// Test the dungeon constructor
        /// </summary>
        /// <param name = "shape">the given shape of the dungeon</param>
        /// <param name = "rooms">the given number of rooms to generate</param>
        /// <param name = "cap">the given maximum capacity of a room</param>
        
        //Linear shape
        [TestCase(DungeonShapeType.LINEARshape, 2, 2)] //not enough rooms 
        [TestCase(DungeonShapeType.LINEARshape, 3, 4)] //exactly enough rooms
        [TestCase(DungeonShapeType.LINEARshape, 4, 6)] //just enough rooms
        [TestCase(DungeonShapeType.LINEARshape, 20, 15)] //many rooms
        
        //Tree shape
        [TestCase(DungeonShapeType.TREEshape, 2, 2)] //not enough rooms for tree shape
        [TestCase(DungeonShapeType.TREEshape, 3, 5 )] //exactly enough rooms
        [TestCase(DungeonShapeType.TREEshape, 4, 2)] //just enough rooms
        [TestCase(DungeonShapeType.TREEshape, 19, 2)] //many rooms
        
        //Random shape
        [TestCase(DungeonShapeType.RANDOMshape, 2, 2)] //not enough rooms 
        [TestCase(DungeonShapeType.RANDOMshape, 3, 2)] //exactly enough rooms
        [TestCase(DungeonShapeType.RANDOMshape, 4, 2)] //just enough rooms
        [TestCase(DungeonShapeType.RANDOMshape, 20, 2)] //many rooms
        
        //capacity test
        [TestCase(DungeonShapeType.RANDOMshape, 4, 0)] //not enough capacity
        [TestCase(DungeonShapeType.RANDOMshape, 4, 1)] //exactly enough capacity
        
        public void Test_Dungeon_Constructor(DungeonShapeType shape, int rooms, int cap)
        {
            // Create the dungeon variable
            Dungeon dungeon = null;
            Exception exc = null;
            
            //Pre-Condition
            if (rooms < 3 || cap < 1)
            {
                //Check if constructor throws an exception when pre-condition is not met
                Assert.Throws<ArgumentException>(() => new Dungeon(shape, rooms, cap));
            }
            //Post-Condition
            else
            {
                //Try creating the dungeon
                try
                {
                    dungeon = new Dungeon(shape, rooms, cap);
                }
                catch(Exception e)
                {
                    exc = e;
                }
                
                Assert.IsTrue(dungeon != null); 
                Assert.IsTrue(HelperPredicates.AllIdsAreUnique(dungeon)); //Check if all ids are unique
                Assert.IsTrue(dungeon.Rooms.Count == rooms); //Check the number of rooms
                Assert.IsTrue(HelperPredicates.Forall(dungeon.Rooms, r => r.Capacity <= cap)); //Check the capacity of each room

                switch (shape)
                {
                    case DungeonShapeType.LINEARshape:
                        Assert.IsTrue(HelperPredicates.IsLinear(dungeon));
                        break;
                    case DungeonShapeType.TREEshape:
                        Assert.IsTrue(HelperPredicates.IsTree(dungeon));
                        break;
                    case DungeonShapeType.RANDOMshape:
                        Assert.IsTrue(HelperPredicates.HasUniqueStartAndExit(dungeon));
                        Assert.IsTrue(HelperPredicates.AllReachableFromStart(dungeon));
                        Assert.IsFalse(HelperPredicates.IsTree(dungeon));
                        Assert.IsFalse(HelperPredicates.IsLinear(dungeon));
                        break;
                }
            }
        }

        /// <summary>
        /// Test the SeedMonstersAndItems methods with NUnit Theory. The Datapoint Source values
        /// are chosen because of the following reasons:
        /// -- 0 for invalid input for monsters, healPotions, ragePotions
        /// -- 1 for minimum valid input for monsters, healPotions, ragePotions and cap
        /// -- 5 and 10 for higher amount
        /// For the shape of the dungeon, RandomShape is chosen because in this case, the exitRoom can have
        /// more then one neighbor
        /// </summary>
        /// <param name = "monsters">the given number of monsters to seed</param>
        /// <param name = "healPotions">the given number heal potions to seed</param>
        /// <param name = "ragePotions">the given number of rage potions to seed</param>
        /// <param name = "rooms">the given number of rooms to generate</param>
        /// <param name = "cap">the given maximum capacity of a room</param>
        /// <param name = "shape">the given shape of the dungeon</param>
        
        [DatapointSource]
        public int[] int_values = new int[] {0, 1, 5, 10};

        [Theory]
        // Theory-1 checks if the method returns false if the preconditions are not met for: numberOfMonsters,
        //numberOfHealingPotions and numberOfRagePotions.
        public void SeedMonstersAndItems_Theory1(int monsters, int healPotions, int ragePotions,
            int rooms, int cap)
        {
            //Make sure that the dungeon has always valid inputs
            Assume.That(rooms > 2 && cap > 0);
            Dungeon dungeon = new Dungeon(DungeonShapeType.RANDOMshape, rooms, cap);
            int maxMonsterSeeds = MaxSeeds(cap, dungeon);
            
            //Cases when pre-conditions are not met
            Assume.That(monsters > maxMonsterSeeds || healPotions < 1 || ragePotions < 1 || monsters < 1 ||
                        monsters < dungeon.ExitRoom.Neighbors.Count);
            //Check if the function returns false in all cases
            Assert.IsFalse(dungeon.SeedMonstersAndItems(monsters, healPotions, ragePotions));
        }
        
        [Theory]
        //Theory-2 checks if the post-conditions are met, when the pre-conditions are met
        public void SeedMonstersAndItems_Theory2(int monsters, int healPotions, int ragePotions,
            int rooms, int cap)
        {
            //Make sure that the dungeon has always valid inputs
            Assume.That(rooms > 2 && cap > 0);
            Dungeon dungeon = new Dungeon(DungeonShapeType.RANDOMshape, rooms, cap);
            int maxMonsterSeeds = MaxSeeds(cap, dungeon);

            //Cases when the pre-conditions are met
            Assume.That(monsters <= maxMonsterSeeds && healPotions >= 1 
                                                           && ragePotions >= 1 && monsters >= 1
                                                           && monsters >= dungeon.ExitRoom.Neighbors.Count);
            //Check if the function returns true
            Assert.IsTrue(dungeon.SeedMonstersAndItems(monsters, healPotions, ragePotions));
            
            //Check if every seeded monster has HP > 0 and AR > 0
            foreach (Monster m in dungeon.Monsters)
            {
                Assert.IsTrue(m.Hp > 0 && m.AttackRating > 0);
            }
            
            //lowest amount of monsters in exit.Neighbors:
            int lowestMonstersAmount = LowestMonstersAmount(cap, dungeon);
            int emptyRooms = 0; //amount of rooms without items
            int monsterCount = 0; 
            int hpCount = 0; 
            int rpCount = 0;

            foreach (Room r in dungeon.Rooms)
            {
                //Check if every room not neighbor of exit room has less monsters than
                //lowest amount of monsters in exitRoom.Neighbors
                if (!r.Neighbors.Contains(dungeon.ExitRoom) && r != dungeon.StartRoom)
                    Assert.IsTrue(r.Monsters.Count < lowestMonstersAmount);

                if (r.Capacity < r.Monsters.Count)
                {
                    int x = 0;
                }
                //Check if number of monsters in every room is less than its maximum capacity
                Assert.IsTrue(r.Capacity >= r.Monsters.Count);
                
                if (r.Items.Count == 0)
                {
                    emptyRooms++;
                }

                monsterCount += r.Monsters.Count;

                foreach (Item potion in r.Items)
                {
                    if (potion is HealingPotion)
                        hpCount++;
                    else if (potion is RagePotion)
                        rpCount++;
                }
            }
            
            //Check if at least N/2 rooms have no item at all
            Assert.IsTrue(emptyRooms >= dungeon.Rooms.Count / 2);
            //Check amount of monsters
            Assert.IsTrue(monsterCount == monsters);
            //Check amount of hp and rp potions
            Assert.IsTrue(hpCount == healPotions && rpCount == ragePotions);
            
            bool hpItem = false; //is there a hp item?
            bool rpItem = false; //is there an ar item?
            //Check if there is at least one healing and one rage potion in startRoom.Neighbors
            foreach (Room r in dungeon.StartRoom.Neighbors)
            {
                foreach (Item i in r.Items)
                {
                    if (i is HealingPotion)
                        hpItem = true;
                    if (i is RagePotion)
                        rpItem = true;

                    if (hpItem && rpItem)
                        break;
                }
                if (hpItem && rpItem)
                    break;
            }
            
            Assert.IsTrue(hpItem && rpItem);
        }

        //Returns the maximum monsters that can be seeded in a dungeon
        private int MaxSeeds(int maxCap, Dungeon dungeon)
        {
            int lowestCap = maxCap;
            int res = 0;
            foreach (Room r in dungeon.ExitRoom.Neighbors)
            {
                res += r.Capacity;
                if (r.Capacity < lowestCap)
                {
                    lowestCap = r.Capacity;
                }
            }

            foreach (Room r in dungeon.Rooms
                .Where(x => !x.Neighbors.Contains(dungeon.ExitRoom)))
            {
                res += r.Capacity >= lowestCap ? lowestCap - 1 : r.Capacity;
            }
            return res;
        }

        //Returns the lowest amount of monsters in the neighbors of dungeon
        private int LowestMonstersAmount(int maxCap, Dungeon dungeon)
        {
            int lowestMonstersAmount = maxCap; 
            foreach (Room r in dungeon.ExitRoom.Neighbors.Where(r => r != dungeon.StartRoom))
            {
                if (r.Monsters.Count < lowestMonstersAmount)
                {
                    lowestMonstersAmount = r.Monsters.Count;
                }
            }
            
            return lowestMonstersAmount;
        }
        
        [Test]
        //Getters and setters tests
        public void Test_RoomType()
        {
            Room r = new Room("test", RoomType.ORDINARYroom, 3);
            r.RoomType = RoomType.STARTroom;
            Assert.IsTrue(r.RoomType == RoomType.STARTroom);
        }
        
        [Test]
        public void Test_Room()
        {
            Dungeon dungeon = new Dungeon(DungeonShapeType.LINEARshape, 5, 3);
            dungeon.Rooms = new HashSet<Room>();
            Assert.IsTrue(dungeon.Rooms.Count == 0);
        }
        
    }
}