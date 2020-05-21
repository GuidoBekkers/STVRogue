using System;
using System.Diagnostics;
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
            if (rooms < 3)
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
                        // Assert.IsTrue(HelperPredicates.HasUniqueStartAndExit(dungeon));
                        // Assert.IsTrue(HelperPredicates.AllReachableFromStart(dungeon));
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
    }
}