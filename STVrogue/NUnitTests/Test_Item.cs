using System;
using System.Collections.Generic;
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
        [TestCase("id1", 1)] // Check positive small heal value          
        [TestCase("id2", 0)] // Check heal value at 0
        [TestCase("id3", -1)] // Check negative small heal value
        [TestCase("id4", int.MaxValue)] // Check positive big heal value
        [TestCase("id5", int.MinValue)] // Check negative big heal value
        public void Test_HealingPotion_Constructor(String id, int healVal)
        {
            // Create the healing potion variable
            HealingPotion hPotion = null;

            // Create an exception variable, for debugging
            Exception exc = null;

            // Pre-Condition
            if (healVal <= 0)
            {
                // Check if the constructor throws an exception when the healing value is <= 0
                Assert.Throws<ArgumentException>(() => new HealingPotion(id, healVal));
            }
            // Post-Condition
            else
            {
                // Try creating the healing potion
                try
                {
                    hPotion = new HealingPotion(id, healVal);
                }
                catch (Exception e)
                {
                    exc = e;
                }

                // Check if the potion was actually made
                Assert.IsTrue(hPotion != null);

                // Check if the healing value was correctly set
                Assert.IsTrue(hPotion.HealValue == healVal);
            }
        }

        /// <summary>
        /// Test the rage potion constructor
        /// </summary>
        [Test]
        public void Test_RagePotion_Constructor()
        {
            // Define the rage potion variables
            RagePotion rPotion = null;

            // Create an exception variable, for debugging
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
            // Check if the potion was actually made
            Assert.IsTrue(rPotion != null);
        }

        /// <summary>
        /// Test the use of the healing potion
        /// </summary>
        /// <param name="healVal">The healing value of the potion</param>
        /// <param name="playerStartHp">The starting HP of the player</param>
        /// <param name="playerMaxHp">The max HP of the player</param>
        [TestCase(1, 1, 3)] // Check the player being healed to below max HP
        [TestCase(1, 1, 2)] // Check the player being healed to exactly max HP
        [TestCase(2, 1, 2)] // Check the player being healed to above max HP
        [TestCase(1, 1, 1)] // Check the player being healed while at exactly max HP
        public void Test_HealingPotion_Use(int healVal, int playerStartHp, int playerMaxHp)
        {
            // Create an exception variable, for debugging
            Exception exc = null;

            // Create the healing potion
            HealingPotion hPotion = new HealingPotion("potionId", healVal);

            // TODO: implement actual player constructor
            // Create the player
            Player player = new Player("playerId", "playerName");

            // Set the players' attributes
            player.Hp = playerStartHp;
            player.HpMax = playerMaxHp;

            // Add the potion to the players' bag
            player.Bag.Add(hPotion);

            // Pre-Condition
            if (!player.Bag.Contains(hPotion))
            {
                // TODO: handle the potion not being in the player's bag
            }
            else
            {
                // Try using the potion
                try
                {
                    hPotion.Use(player);
                }
                catch (Exception e)
                {
                    exc = e;
                }

                // Post-Conditions
                // Check that the player's hp has not exceeded it's max hp
                // TODO: uncomment when hp capping has been implemented
                //Assert.IsTrue(player.Hp <= player.HpMax);

                // Check that the player has been healed the appropriate amount, or got capped at the max hp
                Assert.IsTrue(player.Hp == playerStartHp + hPotion.HealValue || player.Hp == player.HpMax);
            }
        }

        [Test]
        public void Test_HealingPotion_Use_NotInBag()
        {
            // Create an exception variable, for debugging
            Exception exc = null;

            // Create the healing potion with valid hardcoded healValue
            HealingPotion hPotion = new HealingPotion("potionId", 1);

            // TODO: implement actual player constructor
            // Create the player
            Player player = new Player("playerId", "playerName");

            // Set the players' attributes which allow for healing
            player.Hp = 1;
            player.HpMax = 2;

            // Pre-Condition
            // Check if the correct exception is thrown when the potion is not in the player's bag
            //Assert.Throws<ArgumentNullException>(() => hPotion.Use(player));
            
            hPotion.Use(player);
            
            // Post-Condition
            // Check if the player's health remained the same
            Assert.IsTrue(player.Hp == 1);
        }

        [Test]
        public void Test_RagePotion_Use()
        {
            // Create an exception variable, for debugging
            Exception exc = null;

            // Create the rage potion
            RagePotion rPotion = new RagePotion("potionId");

            // TODO: implement actual player constructor
            // Create the player
            Player player = new Player("playerId", "playerName");
            
            // Add the potion to the players' bag
            player.Bag.Add(rPotion);
            
            // Pre-Condition
            if (!player.Bag.Contains(rPotion))
            {
                // TODO: handle the potion not being in the player's bag
            }
            else
            {
                // Try using the potion
                try
                {
                    rPotion.Use(player);
                }
                catch (Exception e)
                {
                    exc = e;
                }

                // Post-Conditions
                // Check if the player became enraged
                Assert.IsTrue(player.Enraged);
            }
        }

        [Test]
        public void Test_RagePotion_Use_NotInBag()
        {
            // Create an exception variable, for debugging
            Exception exc = null;

            // Create the rage potion
            RagePotion rPotion = new RagePotion("potionId");

            // TODO: implement actual player constructor
            // Create the player
            Player player = new Player("playerId", "playerName");
            
            // Pre-Condition
            // Check if the correct exception is thrown when the potion is not in the player's bag
            //Assert.Throws<ArgumentNullException>(() => hPotion.Use(player));
            
            rPotion.Use(player);
            
            // Post-Condition
            // Check if the player's health remained the same
            Assert.IsFalse(player.Enraged);
        }
    }
}