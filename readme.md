# The STV Rogue Project

### Needed Software

You will need an IDE that supports C# development, and additionally unit testing, and coverage analysis. Here are the options:

* Jetbrains' __Rider__ IDE. You can get a free education license for this. It is a great IDE. It has my preference.

* __Visual Studio Enterprise__. If you really insist on using Visual Studio, you can. Keep in mind that you need the __Enterprise__ edition, as smaller version would not include their code coverage tool. UU used to have Enterprise free for students, but I don't its status now. Last I checked, it is not in UU shop.


### What Is in The Project?

This provides an initial C# implementation of the logical game entities of STV Rogue as well as establishing the architecture of this game.
Some methods are left unimplemented for you. You can extend the project,
but please stick to the imposed architecture and keep the signatures of the current methods.

In the directory STVRogue you can find an .sln which define a Visual Studio "solution". Open this "solution"
in Rider or VS. It will contain four projects:

  * The main project is called `STVrogue`. The folder `GameLogic` contains the game entities
     that you need to work out in the Project's PART-1.

     The folder `TestInfrastructure` contains classes relevant for PART-2.
     Ignore them if you are still in PART-1.

     Some support classes you may find useful:

      * The class `STVRogue.HelperPredicates` contains some predicates you might want to borrow, e.g. the forall and exists quantifiers you can use to write in-code specifications.

      * The class `STVRogue.Utils.Utils` contains few utility methods, e.g. to create a pseudo-random generator.

  * The other three projects are example projects containing unit tests, one using NUnit, one using MS-Unit, and one using xUnit.
  Of the three, NUnit offers the most features such as Theory and combinatoric test. We will use NUnit.
