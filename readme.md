**JERSERN**  
jersern parses Json to C# classes.  
It is a Windows Forms project using .Net 6.  
So far it works.   

To run, clone the project down, build the project in Visual Studio and run. 
To install to pc, right click the project in VS and publish it. 
There will be a setup.exe in the published files to install to your machine.
This is obviously the default installer and needs to be polished and has no certificate. 

To use, paste json in the left pane. Classes will appear in the right pane. 
There is an example.json in the solution file to try out. 

**If you get a build error for a file with internet origin**   
Go to the EditorForm.Resx file and .ico file in File Explorer/ right click/ open properties/ and at the bottom
of the properties window click "Unblock"

MIT Licensing - View license.md
