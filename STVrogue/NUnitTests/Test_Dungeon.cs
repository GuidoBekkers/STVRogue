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

        //NUnit Theory testing for the SeedMonstersAndItems method
        [DatapointSource]
        public int[] int_values = new int[] {0, 1, 3, 5, 10};

        [Theory]
        // Theory-1 checks if the method returns false if the preconditions are not met for: numberOfMonsters,
        //numberOfHealingPotions and numberOfRagePotions.
        public void SeedMonstersAndItems_Theory1(int numberOfMonster, int numberOfHealingPotions, int numberOfRagePotions,
            int numberOfRooms, int maximumCapacity)
        {
            //Make sure that the dungeon has always valid inputs
            Assume.That(numberOfRooms > 2 && maximumCapacity > 0);
            Dungeon dungeon = new Dungeon(DungeonShapeType.LINEARshape, numberOfRooms, maximumCapacity);
            int maxMonsterSeeds = MaxSeeds(maximumCapacity, dungeon);
            
            //Cases when pre-conditions are not met
            Assume.That(numberOfMonster > maxMonsterSeeds || numberOfHealingPotions < 1 
                                                          || numberOfRagePotions < 1 || numberOfMonster < 1);
            //Check if the function returns false in all cases
            Assert.IsFalse(dungeon.SeedMonstersAndItems(numberOfMonster, numberOfHealingPotions, numberOfRagePotions));
        }
        
        [Theory]
        //Theory-2 checks if the post-conditions are met, when the pre-conditions are met
        public void SeedMonstersAndItems_Theory2(int numberOfMonster, int numberOfHealingPotions, int numberOfRagePotions,
            int numberOfRooms, int maximumCapacity)
        {
            //Make sure that the dungeon has always valid inputs
            Assume.That(numberOfRooms > 2 && maximumCapacity > 0);
            Dungeon dungeon = new Dungeon(DungeonShapeType.LINEARshape, numberOfRooms, maximumCapacity);
            int maxMonsterSeeds = MaxSeeds(maximumCapacity, dungeon);

            //Cases when the pre-conditions are met
            Assume.That(numberOfMonster <= maxMonsterSeeds && numberOfHealingPotions >= 1 
                                                           && numberOfRagePotions >= 1 && numberOfMonster > 0);
            //Check is the function returns true
            Assert.IsTrue(dungeon.SeedMonstersAndItems(numberOfMonster, numberOfHealingPotions, numberOfRagePotions));
            
            //Check if every seeded monster has HP > 0 and AR > 0
            foreach (Monster m in dungeon.Monsters)
            {
                Assert.IsTrue(m.Hp > 0 && m.AttackRating > 0);
            }
            
            //lowest amount of monsters in exit.Neighbors:
            int lowestMonstersAmount = LowestMonstersAmount(maximumCapacity, dungeon.ExitRoom.Neighbors);
            int emptyRooms = 0; //amount of rooms without items

            foreach (Room r in dungeon.Rooms)
            {
                //Check if every room not neighbor of exit room has less monsters than
                //lowest amount of monsters in exitRoom.Neighbors
                if (!r.Neighbors.Contains(dungeon.ExitRoom) && r != dungeon.StartRoom)
                    Assert.IsTrue(r.Monsters.Count < lowestMonstersAmount);
                
                //Check if number of monsters in every room is less than its maximum capacity
                Assert.IsTrue(r.Capacity >= r.Monsters.Count);
                
                if (r.Items.Count == 0)
                {
                    emptyRooms++;
                }
            }
            
            //Check if at least N/2 rooms have no item at all
            Assert.IsTrue(emptyRooms >= dungeon.Rooms.Count / 2);
            
            bool hpItem = false; //is there a hp item?
            bool rpItem = false; //is there an ar item?
            //Check if there is at least one healing and one rage potion in startRoom.Neigbors
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
            
            Assert.IsTrue(hpItem);
            Assert.IsTrue(rpItem);
        }
        
        //Returns the maximum monsters that can be seeded
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

            int nonExitRooms = dungeon.Rooms.Count - dungeon.ExitRoom.Neighbors.Count;
            //This is a check for random dungeon shape:
            nonExitRooms -= dungeon.ExitRoom.Neighbors.Contains(dungeon.StartRoom) ? 1 : 2; 

            res += nonExitRooms * (lowestCap - 1);
            return res;
        }

        //Returns the lowest amount of monsters in a hash set of rooms
        private int LowestMonstersAmount(int maxCap, HashSet<Room> rooms)
        {
            int lowestMonstersAmount = maxCap; 
            foreach (Room r in rooms)
            {
                if (r.Monsters.Count < lowestMonstersAmount)
                {
                    lowestMonstersAmount = r.Monsters.Count;
                }
            }
            
            return lowestMonstersAmount;
        }
    }
}