# Project

**Due: Thursday, December 16, 4:00pm CDT**

This is an empty repository for your course project.  You should add your project files to this repository, fill out the readme file, and then commit and push to complete your submission.

*If you run into a technical problem with GitHub, such as an error due to a large file size, then you can alternatively submit a Google Drive link to download a zip file of your project.  In this case, you should add the link to the Submission Information below and commit the readme file to the repository .* 

## Submission Information

You should fill out this information before submitting your project.

**Team Members**:

Jarod Pivovar - pivov004@umn.edu

**Third Party Assets**:

The scene uses assets from the [Playground Low Poly](https://assetstore.unity.com/packages/3d/environments/playground-low-poly-191533) package from the Unity Asset Store.

The base used for this project was Assignment 4, which was originally set up by [Evan Suma Rosenberg](https://illusioneering.umn.edu/), see License below.

Example code for sorting an Array in C# was adpated from the [.NET Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/system.array.sort?redirectedfrom=MSDN&view=net-6.0#System_Array_Sort__1___0___System_Collections_Generic_IComparer___0__)

Details on how to activate single pass instanced rendereding for use in VR shaders were drawn from the [Unity Docs](https://docs.unity3d.com/Manual/SinglePassInstancing.html?_ga=2.124393730.967674592.1638645914-1632545405.1631661852)

**Project Description**:

This project introduces a WIM (World in Minuature) object into the scene, but instead of representing an entire world at once, it lets you change the scale and center focus of the WIM. To make this more usable, only a cubic portion at the center of the WIM is visible, and anything outside of that is not rendered/interactable. This allows the user to interact naturally with very small objects by scaling the WIM up, or conversely with very large objects by scaling the WIM down. The exact method of scaling and repositioning can be changed easily to any desired method. This project implemented simple pointing to determine the new WIM focus location, and it uses the distance between the hands to determine the new scale of the WIM. This project is tested on the Oculus Quest, but is designed for any general VR system.

**Instructions**:

The WIM sits just a little in front of the user when loading up, so use the joystick to walk up to it if needed. Manipulating objects in the WIM will manipulate them in the larger world, and vice-versa. To change the center of focus of the WIM, hold down the secondary button on either controller (B or Y). This will display a ray out the front of the controller and also display a preview of what portion of the world the WIM will focus on. Point at the desired location in the world, and then change the distance between the controllers to change the scale of the WIM focus. This scaling uses polynomial scaling, so the user can create very small focuses, and very large ones as well. Once the desired scale is chosen, simply release the button again and the WIM will change it's focus.

## Rubric

Graded out of 80 points. 

1. UI Contribution (25).  How significant is the UI contribution of the project? Is the project simply an application, or does it have a substantial focus on new UI technology? How novel and creative is the system?
2. Technical Complexity (25). Projects should represent a substantial implementation effort.
3. Usability (25).  Does the project work as intended?  Are there any serious usability problems or bugs?  
4. Documentation (5).  Are all the functions sufficiently described in the documentation?  Are the instructions clear?

Make sure to document all third party assets in your readme file. ***Be aware that points will be deducted for using third party assets that are not properly documented.***

## Submission

You will need to check out and submit the project through GitHub classroom.  **Make sure your APK file is in the root folder.** Do not remove the `.gitignore` or `README.md` files.

Please test that your submission meets these requirements.  For example, after you check in your final version of the assignment to GitHub, check it out again to a new directory and make sure everything opens and runs correctly.  You can also test your APK file by installing it manually using [SideQuest](https://sidequestvr.com/).

If your project is intended for use on a PC with Oculus Link, then you should include a Google Drive link to download a zip file of your build folder instead of an APK file.

## License

Material for [CSCI 5619 Fall 2021](https://canvas.umn.edu/courses/268490) by [Evan Suma Rosenberg](https://illusioneering.umn.edu/) is licensed under a [Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License](http://creativecommons.org/licenses/by-nc-sa/4.0/).

The intent of choosing CC BY-NC-SA 4.0 is to allow individuals and instructors at non-profit entities to use this content.  This includes not-for-profit schools (K-12 and post-secondary). For-profit entities (or people creating courses for those sites) may not use this content without permission (this includes, but is not limited to, for-profit schools and universities and commercial education sites such as Coursera, Udacity, LinkedIn Learning, and other similar sites).   