using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STVRogue.GameLogic
{
    public class XTest_Player
    {

        [Fact]
        public void XTest_use_onEmptyBag()
        {
            Player P = new Player();
            Assert.Throws<ArgumentException>(() => P.use(new Item()));
        }

        [Fact]
        public void XTest_use_item_in_bag()
        {
            Player P = new Player();
            Item x = new HealingPotion("pot1");
            P.bag.Add(x);
            P.use(x);
            Assert.False(P.bag.Contains(x));
        }

       // [Fact]
        public void XTest_coba()
        {
            Coba c = new Coba();
            Assert.True(c.x == 10);
        }
    }
}
