# The STV Rogue Project

### Needed Software

* __Visual Studio Enterprise__ You need the Enterprise edition to get the code coverage analysis tool. You'll need this tool. 
* For the Optionals assignments, and depending on offered optionals, you may need: the automated testing tool __FSCheck__ (https://fscheck.github.io/FsCheck/).


### What Is in The Project?

This provides an initial C# implementation of the logical game entities of STV Rogue.
Some methods are left unimplemented for you. You can extend the project,
but please stick to the imposed architecture and keep the signatures of the current methods.

In the directory STVRogue you can find an .sln which define a Visual Studio "solution". Open this "solution"
in VS. It will contain four projects:

  * The main project is called `STVrogue`. The folder `GameLogic` contains the game entities 
     that you need to work out in Iteration-1.

     The folder `TestInfrastructure` contains classes relevant for Iteration-2. 
     Ignore them if you are still in Iteration-1.

     Some support classes you may find useful:

      * The class `STVRogue.HelperPredicates` contains some predicates you might want to borrow, e.g. the forall and exists quantifiers you can use to write in-code specifications.

      * The class `STVRogue.PathCoverageTracker` contains a basic utility to track the coverage over a finite state machine. Check it out if you do Optional-3.5.

      * The class `STVRogue.Utils` contains few utility methods, e.g. to create a pseudo-random generator.

  * The other three projects are example projects containing unit tests, one using MS-Unit, one using
     NUnit, and one using xUnit. Choose whichever you like.
   


