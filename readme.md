# The STV Rogue Project

This provides an initial implementation of the logical game entities of STV Rogue.
Some methods are left unimplemented for you. You can extend the project, but
please stick to the imposed architecture and keep the signatures of the current
methods.

Load the solution file (.sln) in Visual Studio. The solution contains the following projects:
   * STVRogue: contains an initial implementation of the game entities.
   * STVRogue_Main: a dummy console application for running the game.
   * MSunitTests: an example a test project that uses Miscrosoft's own Visual Studio testing framework.
   * NunitTests: an example a test project that uses NUnit testing framework.
   * XunitTests: an example a test project that uses XUnit testing framework.
   
When extending the game logic, stick to pure C#. Avoid adding XNA classes to it, for example,
since XNA does not interact well with the above unit testing frameworks (you will then
need to write your own tetsing framework).
   
## Which VS to use?
   
You will need the Enterprise edition of Visual Studio to be able to measure the 
 coverage of your tests. The Enterprise edition also allows you to obtain
 code metrics from your solition (the size of every class, its complexity, etc).

## Which unit testing framework to use?

There are three popular unit testing framework: Miscrosoft's own Visual Studio testing framework,
the NUnit framework, and the XUnit framework. They all support the same basic functionalities,
but there are small differences in their usage. I have included an example test project
for each in the above solution.

If you want to try [NUnit](https://nunit.org/) or [XUnit](https://xunit.github.io/), visit their website for instructions on how to 
 install them in your solution. Install their respective VS runner too; this will allow
 you to run them from VS (rather than from Console). NUnit has beter documentation, but
 XUnit claims to be more modern :)

## Mutation Tests (an optional part of your project)

Mutation test is NOT a test against your System Under Test. Instead, it is
a test to measure the strength of your test suite, so it is also quite useful.
In fact, it should actually be a standard tool to support unit testing.
There are a number of mutation testing tool for C#, unfortunately none
seem to work anymore, except perhaps one.

It's fun to do, and would be a useful experience too, if you manage to get
the tool sufficiently working for you. For now, we will just do this for 
experiment, and simply hope that the maturity of the tooling would become
beter in the future.

One tool that works is [VisualMutator] (https://visualmutator.github.io/web/). Do not
install the standard distribution, install from this fork instead: https://github.com/pavzaj/visualmutator/releases

The fork works with VS 2013 and VS 2015. I haven't checked 2017. However,
**it only works in combination with NUnit tests**.

The manual should tell you how to start and configure a mutation test. There is
**one annoying quirk** of VisualMutator: after the mutation test, it retains the lock
to classes under test (the classes you let it mutate), so you can't save changes into
those files. Since the idea is to test your test suites, in principle you don't
want to change the classes under test. The lock is realeased if you quite VS.
It could be that this is a phenomenon that only happens on my Windows-7 machine :|


 


