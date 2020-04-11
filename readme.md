# The STV Rogue Project

### Needed Software

You will need an IDE that supports C# development, and additionally unit testing, and coverage analysis. Here are the options:

* Jetbrains' [__Rider__](https://www.jetbrains.com/rider/) .NET IDE. You can get a [free education license](https://www.jetbrains.com/community/education/#students) for this. It is a great IDE (fast, powerful refactoring, works on windows/mac/linux). It has my preference.

* __Visual Studio Enterprise__. If you really insist on using Visual Studio, you can. Keep in mind that you need the __Enterprise__ edition, as smaller version would not include their code coverage tool. UU used to have Enterprise free for students, but I don't its status now. Last I checked, it is not in UU shop.


### What Is in The Project?

This provides an initial C# implementation of the logical game entities of __STV Rogue__ as well as establishing the architecture of this game.
Some methods are left unimplemented for you. You can extend the project,
but please stick to the imposed architecture and keep the signatures of the current methods.

In the directory `STVRogue` you can find an `.sln` file that describes the configuration of the project.
Open this "solution"-file
in Rider or VS. It will contain four projects:

  * The main project is called `STVrogue`. The folder `GameLogic` contains the classes that comprise the logic of the STV Rogue game. These are the classes
     that you need to work out in the Project's PART-1.

     The folder `TestInfrastructure` contains classes relevant for PART-2.
     Ignore them if you are still in PART-1.

     Some support classes you may find useful:

      * The class `STVRogue.HelperPredicates` contains some predicates you might want to borrow, e.g. the forall and exists quantifiers you can use to write in-code specifications.

      * The class `STVRogue.Utils.Utils` contains few utility methods, e.g. to create a pseudo-random generator.

  * The other three projects are example projects containing unit tests, one using NUnit, one using MS-Unit (v2), and one using xUnit.

   NUnit, MSUnit, and xUnit are well known unit testing frameworks for C# and are part of .Net Core.
   If you use Rider IDE there is no need to install them.

   Of the three, [NUnit](https://nunit.org/) offers the most features such as Theory and combinatoric test. We will use NUnit. Finding tutorial for NUnit might be a bit challenging. I will list some below:

       1. From the lectures you should already know the key concepts of unit testing. The examples  provided in STV Rogue itself are the shortest route to learn NUnit.

       2. If you prefer formal documentation, check [Microsoft doc on NUnit](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-nunit), though this doc is pretty verbose.

       3. There is an __old__ [NUnit Quick Start tutorial](https://nunit.org/docs/2.5.9/quickStart.html) which is still useful to learn its main concepts.
      For the more advanced stuffs, either look at
      the examples in STV Rogue or consult [NUnit reference doc](https://github.com/nunit/docs/wiki/NUnit-Documentation), but this reference might be a bit dry (lack of examples) for new users.
