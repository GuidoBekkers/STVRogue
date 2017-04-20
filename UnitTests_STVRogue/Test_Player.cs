﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using STVRogue.GameLogic;

namespace STVRogue.GameLogic
{

    /* An example of a test class. This one is to unit-test the
     * class Player. The test is incomplete though, as it only contains
     * two test cases. 
     */
    [TestClass]
    public class Test_Player
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_use_onEmptyBag()
        {
            Player P = new Player();
            P.use(new Item());
        }

        [TestMethod]
        public void Test_use_item_in_bag()
        {
            Player P = new Player();
            Item x = new HealingPotion("pot1");
            P.bag.Add(x);
            P.use(x);
            Assert.IsFalse(P.bag.Contains(x));
        }
    }
}
