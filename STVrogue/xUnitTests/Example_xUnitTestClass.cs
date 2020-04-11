using System;
using System.Diagnostics;
using STVrogue.GameLogic;
using Xunit;

namespace xUnitTests
{
    /* Just an example of an xUnit test class to show how to write one. */
    public class xUnitTestClass1
    {
        [Fact]
        /* An example of a simple test. We will test the constructor of Creature. */
        public void test_1_Creature_contr()
        {
            Creature C = new Creature("smaegol",10, 1);
            Assert.True(C.Hp == 10 && C.HpMax == 10 );
            Assert.True(C.AttackRating > 0);
        }
        
        /*
         Below is an example of a parameterized test (also called Theory in xUnit, which is
         slightly different than Theory in nUnit, that allows you to reuse the
         same testcode over multiple testdata. Essentially, the idea is similar
        to property-based testing.
        */
        [Theory]
        [InlineData(-1,1)]
        [InlineData(0,1)]
        [InlineData(1,-1)]
        [InlineData(1,0)]
        [InlineData(-1,-1)]
        [InlineData(1,99)]
        [InlineData(99,1)]
        [InlineData(99,99)]
        public void parameterizedTest_Creature_contr(int hp, int ar)
        {
            Debug.WriteLine("** (" + hp + "," + ar + ")");
            if (hp <= 0 || ar <= 0)
            {
                // on negative hp or ar the constructor should throws an
                // illegal-argument-exception:
                Assert.Throws<ArgumentException>(() => new Creature("smaegol", hp, ar));
            }
            else
            {
                // on other arguments, we want to verify these correctness properties:
                Creature C = new Creature("smaegol",hp,ar);
                Assert.True(C.Hp==hp && C.HpMax==hp);
                Assert.True(C.AttackRating > 0);
            }
        }
        
    }
}
