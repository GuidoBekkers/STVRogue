using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using STVrogue.GameControl;
using STVrogue.GameLogic;
using STVrogue.TestInfrastructure;

namespace NUnitTests
{
    [TestFixture]
    public class Verifications
    {
        [Test, Description("Every creature alive should have HP > 0")]
        public void SAlive()
        {
            //loading gameplays:
            GamePlay g1 = new GamePlay("testSave1.txt");
            GamePlay g2 = new GamePlay("testSave2.txt");
            GamePlay g3 = new GamePlay("testSave3.txt");
            GamePlay g4 = new GamePlay("testSave4.txt");
            GamePlay g5 = new GamePlay("testSave5.txt");
            // the specification to verify:
            TemporalSpecification spec
                = new Always(G =>
                        G.livingMonsters.ToList().TrueForAll(m => m.Hp > 0))
                    .And(new Always(G => !G.Player.Alive || G.Player.Hp > 0));
            // test the specification on the plays:
            Assert.IsTrue(spec.Evaluate(3, g1, g2, g3, g4, g5) == Judgement.Valid);
        }
        
        [Test, Description("Players killpoint + number of monsters is constant " +
                           "and non-negative")]
        public void SKP()
        {
            //loading gameplays:
            GamePlay g1 = new GamePlay("testSave1.txt");
            GamePlay g2 = new GamePlay("testSave2.txt");
            GamePlay g3 = new GamePlay("testSave3.txt");
            GamePlay g4 = new GamePlay("testSave4.txt");
            GamePlay g5 = new GamePlay("testSave5.txt");
            // the specification to verify:
            TemporalSpecification spec
                = new AlwaysChange(t =>
                    t.Item1.kp + t.Item1.livingMonsters ==
                    t.Item2.Player.Kp + t.Item2.livingMonsters.Count);
            // test the specification on the plays:
            Assert.IsTrue(spec.Evaluate(3, g1, g2, g3, g4, g5) == Judgement.Valid);
        }
        
        [Test, Description("No new monster nor item during the play")]
        public void SNospawn()
        {
            //loading gameplays:
            GamePlay g1 = new GamePlay("testSave1.txt");
            GamePlay g2 = new GamePlay("testSave2.txt");
            GamePlay g3 = new GamePlay("testSave3.txt");
            GamePlay g4 = new GamePlay("testSave4.txt");
            GamePlay g5 = new GamePlay("testSave5.txt");
            // the specification to verify:
            TemporalSpecification spec
                = new AlwaysChange(t =>
                    t.Item1.livingMonsters >= t.Item2.livingMonsters.Count &&
                    t.Item1.numberOfItems == t.Item2.Dungeon.Items.Count);
            // test the specification on the plays:
            Assert.IsTrue(spec.Evaluate(3, g1, g2, g3, g4, g5) == Judgement.Valid);
        }
        
        [Test, Description("player HP should not drop for no reason")]
        public void SHPdrop()
        {
            //loading gameplays:
            GamePlay g1 = new GamePlay("testSave1.txt");
            GamePlay g2 = new GamePlay("testSave2.txt");
            GamePlay g3 = new GamePlay("testSave3.txt");
            GamePlay g4 = new GamePlay("testSave4.txt");
            GamePlay g5 = new GamePlay("testSave5.txt");
            // the specification to verify:
            TemporalSpecification spec
                = new Eventually(G => G.Dungeon.Monsters.Any(m => m.PrevAction == CommandType.ATTACK))
                    .Assuming(new Eventually(G => G.Player.Location.RoomType == RoomType.EXITroom))
                    .Assuming(new EventuallyChange(t => t.Item1.hp > t.Item2.Player.Hp));
            // test the specification on the plays:
            Assert.IsTrue(spec.Evaluate(3, g1, g2, g3, g4, g5) == Judgement.Valid);
        }

        [Test, Description("player should not be able to flee after using a healing potion")]
        public void SHealBlocksFlee()
        {
            //loading gameplays:
            GamePlay g1 = new GamePlay("testSave1.txt");
            GamePlay g2 = new GamePlay("testSave2.txt");
            GamePlay g3 = new GamePlay("testSave3.txt");
            GamePlay g4 = new GamePlay("testSave4.txt");
            GamePlay g5 = new GamePlay("testSave5.txt");
            // the specification to verify:
            TemporalSpecification spec
                = new AlwaysChange(t => t.Item1.monstersInRoom == 0 ||
                                         t.Item1.startroomNeigh ||
                                         t.Item2.TurnNumber - 1 != t.Item2.HealUsed ||
                                         !t.Item2.Player.Alive || t.Item1.location == t.Item2.Player.Location.Id)
                    .Assuming(new EventuallyChange(t => (t.Item1.monstersInRoom > 0
                                                         && !t.Item1.startroomNeigh
                                                         && t.Item1.hp < t.Item2.Player.Hp)));
            // test the specification on the plays:
            Assert.IsTrue(spec.Evaluate(3, g1, g2, g3, g4, g5) == Judgement.Valid);
        }
    } 
}