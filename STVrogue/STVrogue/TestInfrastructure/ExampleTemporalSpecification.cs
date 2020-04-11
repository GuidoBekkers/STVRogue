using System;
using STVrogue.GameLogic;

namespace STVrogue.TestInfrastructure
{
    public class ExampleTemporalSpecification
    {
        static public TemporalSpecification example1 = new Always(G=>G.Player.Hp >= 0);
        static public TemporalSpecification example2 = new Always(G=>G.Player.Kp >= 0);
    }
}
