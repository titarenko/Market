Inspired by NETWork (October 15, 2011) and 
Event Centric Weekend (October 22-23, 2011)

This solution will contain a set of projects 
with a different implementations of simplistic model of market. 
The goal is to compare how different architectural 
and technological approaches help us to achieve 
greater results with lower investments.

Tags: 
test, benchmark, architecture, technology, 
sql, nosql, ntier, cqrs, event sourcing

Mail me, if you have any questions or suggestions, 
to constantin.titarenko@gmail.com 
(http://about.me/constantin.titarenko).

Few notes if you've just got source code and want
to build and run the solution.

In order to build the code you should obtain 
dependencies (third-party libraries):

	1. Make sure you've installed NuGetPowerTools package
	   (if not, install it using NuGet package manager).
	2. Run Enable-PackageRestore in package manager console.

Finally, install (unless you've already done this somewhen) and
run RavenDB server as well as Redis server (check .config files 
to make sure that application talks to servers using correct
addresses and ports).