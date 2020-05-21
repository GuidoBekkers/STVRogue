using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using STVrogue.GameLogic;

namespace NUnitTests
{
    // Tests for creature class
    [TestFixture]
    public class Test_Creature
    {
     
        // Test creature move to neighboring room
        [Test]
        public void Test_CreatureMove1()
        {
            // Instantiate 2 connecting rooms with a creature in 1
            Room room1 = new Room("1",RoomType.ORDINARYroom, 5);
            Room room2 = new Room("2",RoomType.ORDINARYroom, 5);
            Creature creature = new Creature("3", "test_creature");
            creature.Location = room1;
            room1.Monsters.Add(creature);
            room1.Connect(room2);
            
            // Move the creature to the other room
            creature.Move(room2);
            
            // Check if creature is in the second room and not in the first room
            Assert.IsTrue(creature.Location == room2);
            Assert.IsFalse(creature.Location == room1);
            
            // Check if the creature is contained in the correct room
            CollectionAssert.DoesNotContain(room1.Monsters, creature);
            CollectionAssert.Contains(room2.Monsters, creature);
        }

        // Test creature move to neighboring room with max capacity
        [Test]
        public void Test_CreatureMove3()
        {
            // Instantiate 2 connecting rooms with the second room at max capacity and a creature in the first room
            Room room1 = new Room("1",RoomType.ORDINARYroom, 5);
            Room room2 = new Room("2",RoomType.ORDINARYroom, 1);
            Creature creature1 = new Creature("3", "test_creature1");
            Creature creature2 = new Creature("4", "test_creature2");
            creature1.Location = room1;
            room1.Monsters.Add(creature1);
            creature2.Location = room2;
            room2.Monsters.Add(creature2);
            room1.Connect(room2);
            
            // Check if exception is thrown when trying to move a creature to a room at max capacity
            Assert.Throws<ArgumentException>(() => creature1.Move(room2));
        }
        
        [TestCase(1,10)]  // test creature attack not killing the other creature
        [TestCase(9,10)] 
        [TestCase(10,10)] // test creature attack exactly killing the other creature
        [TestCase(15,10)] // test creature attack killing the other creature
        public void Test_CreatureAttack1(int creatureAr, int creatureHp)
        {
            // Instantiate a room with 2 creatures
            Room room1 = new Room("1",RoomType.ORDINARYroom, 5);
            Creature creature1 = new Creature("2", 10, creatureAr);
            Creature creature2 = new Creature("3", creatureHp , 10);
            creature1.Location = room1;
            creature2.Location = room1;
            
            creature1.Attack(creature2);
            
            // Check if the attacked creatures hp is correctly changed
            Assert.IsTrue(creature2.Hp == Math.Max(0, creatureHp - creatureAr));
            
            // If the attacked creature died it should not be alive
            if (creatureAr >= creatureHp)
            {
                Assert.IsFalse(creature2.Alive);
            }
        }
        
        // Test creature attacking a creature in a different room
        [Test]
        public void Test_CreatureAttack2()
        {
            // Instantiate 2 rooms with a creature in each
            Room room1 = new Room("1",RoomType.ORDINARYroom, 5);
            Room room2 = new Room("2",RoomType.ORDINARYroom, 1);
            Creature creature1 = new Creature("3", 10, 10);
            Creature creature2 = new Creature("4", 10, 10);
            creature1.Location = room1;
            creature2.Location = room2;
            
            // Check if exception is thrown when trying to attack a foe in a different room
            Assert.Throws<ArgumentException>(() => creature1.Attack(creature2));
        }
        
        // Test set HP
        [TestCase(11)]  // Test creature HP set to less then max
        [TestCase(5)]  // Test creature Hp set to more then max
        public void Test_CreatureHpSet(int healthpoints)
        {
            // Instantiate a creature 
            Creature creature = new Creature("2", 10, 1);
            creature.Hp = healthpoints;
            
            // If Hp is set to a value lower or equal to HpMax check if set correctly
            if (healthpoints <= creature.HpMax)
            {
                Assert.IsTrue(creature.Hp == healthpoints);
            }
            // If Hp is set to a value higher then HpMax check if Hp is equal to HpMax
            else
            {
                Assert.IsTrue(creature.Hp == creature.HpMax);
            }
        }
        
        // Test creature HpMax set
        [Test]
        public void Test_CreatureHpMaxSet()
        {
            Creature creature = new Creature("2", 10, 1);
            creature.HpMax = 12;
            Assert.IsTrue(creature.HpMax == 12);
        }
        
        // Test creature AttackRating set
        [Test]
        public void Test_CreatureAttackRatingSet()
        {
            Creature creature = new Creature("2", 10, 1);
            creature.AttackRating = 12;
            Assert.IsTrue(creature.AttackRating == 12);
        }
        
       
    }
}