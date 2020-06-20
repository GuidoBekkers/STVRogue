using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using STVrogue.GameLogic;
using STVrogue.TestInfrastructure;

namespace NUnitTests
{
    public class Verifications
    {
        [Test, Description("Every creature alive should have HP > 0")]
        public void SAlive()
        {
            //loading gameplays:
            
            // the specification to verify:
            TemporalSpecification spec
                = new Always(G => 
                    G.livingMonsters.ToList().TrueForAll(m => m.Hp > 0) && 
                    G.Player.Hp > 0 && G.Player.Alive);
            // test the specification on the plays:
            Assert.IsTrue(spec.Evaluate(3, ) == Judgement.Valid);
        }
        
        [Test, Description("Players killpoint + number of monsters is constant " +
                           "and non-negative")]
        public void SKP()
        {
            //loading gameplays:
            
            // the specification to verify:
            TemporalSpecification spec
                = new AlwaysChange(t =>
                    t.Item1.Player.Kp + t.Item1.livingMonsters.Count ==
                    t.Item2.Player.Kp + t.Item2.livingMonsters.Count);
            // test the specification on the plays:
            Assert.IsTrue(spec.Evaluate(3, ) == Judgement.Valid);
        }
        
        [Test, Description("No new monster nor item during the play")]
        public void SNospawn()
        {
            //loading gameplays:
            
            // the specification to verify:
            TemporalSpecification spec
                = new AlwaysChange(t =>
                    t.Item1.Dungeon.Monsters.Count == t.Item2.Dungeon.Monsters.Count &&
                    t.Item1.Dungeon.Items.Count == t.Item2.Dungeon.Items.Count);
            // test the specification on the plays:
            Assert.IsTrue(spec.Evaluate(3, ) == Judgement.Valid);
        }
        
        [Test, Description("player HP should not drop for no reason")]
        public void SHPdrop()
        {
            //loading gameplays:
            
            // the specification to verify:
            TemporalSpecification spec
                = new Eventually(G =>
                    
            // test the specification on the plays:
            Assert.IsTrue(spec.Evaluate(3, ) == Judgement.Valid);
        }
    }
}