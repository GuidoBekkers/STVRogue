using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace STVRogue.GameLogic
{
    [TestFixture]
    public class NTest_Player
    {

        [Test]
        public void NTest_use_onEmptyBag()
        {
            Player P = new Player();
            Assert.Throws<ArgumentException>(() => P.use(new Item()));
        }

        [Test]
        public void NTest_use_item_in_bag()
        {
            Player P = new Player();
            Item x = new HealingPotion("pot1");
            P.bag.Add(x);
            P.use(x);
            Assert.True(x.used) ;
            Assert.False(P.bag.Contains(x));
        }
    }
}
