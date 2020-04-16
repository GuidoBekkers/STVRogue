using System;
using System.Diagnostics;
using NUnit.Framework;
using STVrogue.GameLogic;

namespace NUnitTests
{
    /* Just an example of an NUnit test class to show how to write one. */
    [TestFixture]
    public class NUnitTestClass1
    {

        /* An example of a simple test. We will test the constructor of Creature. */
        [Test, Description("Test the constructor of Creature")]
        public void test1_Creature_contr()
        {
            Creature C = new Creature("smaegol",10, 1);
            Assert.IsTrue(C.Hp == 10 && C.HpMax == 10 );
            Assert.IsTrue(C.AttackRating > 0);
        }
        
        /*
         Below is an example of a parameterized test in NUnit, that allows you to reuse the
         same testcode over multiple testdata. Essentially, the idea is similar
         to property-based testing.
        */
        [TestCase(1,100)]
        [TestCase(100,100)]
        [TestCase(99,1)]
        public void parameterizedTest_Creature_contr(int hp, int ar)
        {
            // on other arguments, we want to verify these correctness properties:
            Creature C = new Creature("smaegol",hp,ar);
            Assert.IsTrue(C.Hp==hp && C.HpMax==hp);
            Assert.IsTrue(C.AttackRating > 0);
        }

        /*
         An example of generating a combinatoric test with Nunit. The test below will generate
         the full combinations of the values you give.
         Be careful with this, as this can easily explode!
         */
        [Test, Combinatorial]
        public void fullCombinatoricTest_execeptionCase_Creature_contr([Values(-1, 0, 1)] int hp, [Values(-1, 0, 1)] int ar)
        {
            TestContext.Out.WriteLine("** (" + hp + "," + ar + ")");
            Debug.WriteLine("** (" + hp + "," + ar + ")");
            if (hp <= 0 || ar <= 0)
            {
                // on negative hp or ar the constructor should throws an
                // illegal-argument-exception:
                Assert.Throws<ArgumentException>(() => new Creature("smaegol", hp, ar));
            }
        }

        /*
           An example of generating pair-wise test using Nunit. It will generate a bunch of
           tests that will give full pair coverage over the set of values you specify.
           Be mindful that Nunit use a heuristic that not necessarily produce a minimal
           test-set.
         */
        [Test, Pairwise]
        public void pairwiseTest_nonExceptionCase_Creature_contr(
            [Values("smaegol","nazgul")] string id,
            [Values(1,99,int.MaxValue)] int hp, 
            [Values(1, 99, int.MaxValue)] int ar)
        {
            Debug.WriteLine("** (" + id + "," + hp + "," + ar + ")");

            Creature C = new Creature(id,hp,ar);
            Assert.IsTrue(C.Hp==hp && C.HpMax==hp);
            Assert.IsTrue(C.AttackRating > 0);
        }
    }
}