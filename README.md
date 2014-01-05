Assembly Buddy
=============

Synchronizes matching files from a source to a destination directory. Made for updating DLL's and can do TFS file checkouts.

Motivation
----------

While [nuget](http://www.nuget.org/) is a great solution for including libraries in projects, there are a few cases that something simpler can be helpful. For example:

1. You haven't quite set up a local nuget package yet. You've been meaning to. You'll get to it. But until then...
2. You have to copy DLL's from a build server or something. Because **_reasons_**. I really had to do this for a while.
3. When you need to customize and test a nuget library before making pull request with your change.


Number 3 is probably the most useful case for me these days. When I need to customize a library and build it locally it can be annoying to copy the output to the target projects repeatedly after every change to test it in my app. Batch files can work but are brittle. What if you add a DLL to the project but forget to edit the batch file you use to copy the files? I actually did that and kept wondering why my changes didn't seem to work...

With AssemblyBuddy you automatically see matching files and whether they differ. It will even check the destination files out from TFS first if needed.

Still don't see the point?
----------
If it still doesn't seem useful, then you probably don't need it!


Usage
----------

Open AssemblyBuddy, paste in source and destination directories, hit compare. Copy if needed.
Or make a shortcut like `AssemblyBuddy -s "C:\Source" -d "C:\Destination"`

Nuget modifying workflow
----------
When I use a nuget package and I want to modify it here is what I do. _Please let me know if you know a better way!_

* Fork and build the project
* Copy the output somewhere (I ues a folder named `lib` under my solution)
* Remove the nuget package from your projects
* Add the assembly references that you built
* Test and edit the project, using AssemblyBuddy to copy the output after each edit and build

Once you're changes are accepted in the project and their nuget package is updated, you can remove the assembly references and add back the nuget package.

License
-------

MIT license - http://www.opensource.org/licenses/mit-license.php
