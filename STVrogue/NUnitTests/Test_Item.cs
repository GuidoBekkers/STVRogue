using System;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using NUnit.Framework;
using STVrogue.GameLogic;

namespace NUnitTests
{
    [TestFixture]
    public class Test_Item
    {
        /// <summary>
        /// Test the healing potion constructor
        /// </summary>
        /// <param name="id">the given id</param>
        /// <param name="healVal">the given healing value</param>
        [TestCase("id1", 1)]
        [TestCase("id2", 0)]
        [TestCase("id3", -1)]
        [TestCase("id4", int.MaxValue)]
        [TestCase("id5", int.MinValue)]
        public void Test_HealingPotion_Constructor(String id, int healVal)
        {
            HealingPotion hPotion = null;
            Exception exc = null;

            // Try creating the healing potion
            try
            {
                hPotion = new HealingPotion(id, healVal);
            }
            catch (Exception e)
            {
                exc = e;
            }

            // Pre-Condition
            if (healVal < 0)
            {
                Assert.Throws<ArgumentException>(() => new HealingPotion(id, healVal));
            }
            // Post-Condition
            else
            {
                Assert.IsTrue(hPotion != null);
                Assert.IsTrue(hPotion.HealValue1 == healVal);
            }
        }

        /// <summary>
        /// Test the rage potion constructor
        /// </summary>
        /// <param name="id">the given id</param>
        [Test]
        public void Test_RagePotion_Constructor()
        {
            RagePotion rPotion = null;
            Exception exc = null;

            // Try creating the healing potion
            try
            {
                rPotion = new RagePotion("id");
            }
            catch (Exception e)
            {
                exc = e;
            }

            // Post-Condition
            Assert.IsTrue(rPotion != null);
        }
    }
}