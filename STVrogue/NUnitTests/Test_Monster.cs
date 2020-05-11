using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using STVrogue.GameLogic;

namespace NUnitTests
{
    // Tests for the monster class
    [TestFixture]
    public class Test_Monster
    {
        // Test Monster constructor
        [TestCase(1, 100)]
        [TestCase(100, 100)]
        [TestCase(99, 1)]
        public void Test_MonsterConstructor1(int hp, int ar)
        {
            Monster monster = new Monster("1", "Test_Monster", hp, ar);
            Assert.IsTrue(monster.Hp == hp && monster.HpMax == hp);
            Assert.IsTrue(monster.AttackRating > 0);
            Assert.IsTrue(monster.Name == "Test_Monster");
        }
        
        // Test monster constructor with incorrect ar, hp or id
        [TestCase("1", 10, -1)]
        [TestCase("1", -1, 10)]
        [TestCase(null, 10, 10)]
        public void Test_MonsterConstructor2(string id, int hp, int ar)
        {
            Assert.Throws<ArgumentException>(() => new Monster(id, "TestMonster", hp, ar));
        }
    }
}