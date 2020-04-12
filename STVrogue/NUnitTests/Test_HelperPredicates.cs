using NUnit.Framework;
using STVrogue;
using STVrogue.GameLogic;

namespace NUnitTests
{
    class DummyDungeon : Dungeon
    {
        
    }
    [TestFixture]
    public class Test_HelperPredicates
    {
        [Test]
        public void Test_IsLinear()
        {
            var start = new Room("start", RoomType.STARTroom, 0);
            var r1 = new Room("r1", RoomType.ORDINARYroom, 10);
            var r2 = new Room("r2", RoomType.ORDINARYroom, 10);
            var exit = new Room("exit", RoomType.EXITroom, 0);
            start.Connect(r1);
            r1.Connect(r2);
            r2.Connect(exit);
            var dungeon = new DummyDungeon();
            // start -- r1 -- r2 -- exit
            dungeon.Rooms.Add(start);
            dungeon.Rooms.Add(r1);
            dungeon.Rooms.Add(r2);
            dungeon.Rooms.Add(exit);
            dungeon.StartRoom = start;
            dungeon.ExitRoom = exit;
            Assert.IsTrue(HelperPredicates.IsLinear(dungeon));

            // start -- r1 -- r2 -- exit
            //           |___________|
            r1.Connect(exit);
            Assert.IsFalse(HelperPredicates.IsLinear(dungeon));
            
            // start -- r1 ------ exit
            //           |___r2
            r1.Disconnect(r2);
            r2.Disconnect(exit);
            start.Connect(r2);
            Assert.IsFalse(HelperPredicates.IsLinear(dungeon));
        }
    }
}