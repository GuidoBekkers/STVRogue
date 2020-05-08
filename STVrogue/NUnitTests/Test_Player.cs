using System;
using System.Diagnostics;
using NUnit.Framework;
using STVrogue.GameLogic;

namespace NUnitTests
{
    //tests for player class
    [TestFixture]
    public class Test_Player
    {
        // Test Player constructor
        [TestCase(1, 100)]
        [TestCase(100, 100)]
        [TestCase(99, 1)]
        public void Test_PlayerConstructor1(int hp, int ar)
        {
            Player player = new Player("1", "Test_player", hp, ar);
            Assert.IsTrue(player.Hp == hp && player.HpMax == hp);
            Assert.IsTrue(player.AttackRating > 0);
            Assert.IsTrue(player.Name == "Test_player");
        }
        
        //Test player move to neighboring room 
        [Test]
        public void Test_PlayerMove1()
        {
            // Instantiate 2 connecting rooms with a player in 1
            Room room1 = new Room("1",RoomType.ORDINARYroom, 5);
            Room room2 = new Room("2",RoomType.ORDINARYroom, 5);
            Player player = new Player("3", "TestPlayer", 10, 10);
            player.Location = room1;
            room1.Connect(room2);
            
            // Move player to the other room
            player.Move(room2);
            
            // Check if players location is in the second room
            Assert.IsTrue(player.Location == room2);
            Assert.IsFalse(player.Location == room1);
        }
        
        // Test player move to non neighboring room
        [Test]
        public void Test_PlayerMove2()
        {
            // Instantiate 2 connecting rooms with a player in 1
            Room room1 = new Room("1",RoomType.ORDINARYroom, 5);
            Room room2 = new Room("2",RoomType.ORDINARYroom, 5);
            Player player = new Player("3", "TestPlayer", 10, 10);
            player.Location = room1;
                
            // Check if exception in thrown when rooms are not connected
            Assert.Throws<ArgumentException>(() => player.Move(room2));
        }
        
        // Test player attack killing foe and increasing kill points
        [Test]
        public void Test_PlayerAttack()
        {
            // Instantiate a room with a player and a creature
            Room room = new Room("1",RoomType.ORDINARYroom, 5);
            Creature creature = new Creature("2", 1, 10);
            Player player = new Player("3","TestPlayer", 10 , 10);
            creature.Location = room;
            player.Location = room;
            
            // Player attacks and kills the creature
            player.Attack(creature);
            
            // Check if kill points are increased when creature is slain
            Assert.IsTrue(player.Kp == 1);
        }
        
        // TODO: player test use
    }
}