**JERSERN**  
jersern parses Json to generate C# class code.  

It is a Windows Forms project using .Net 6.  
So far it works.   

**To Run**  
Clone the project down.  
Build the project in Visual Studio and run.   

**To install to pc**  
Right click the project in VS, pick your path and publish it to folder.   
There will be a setup.exe in the published files to install to your machine.
This is obviously the default installer and needs to be polished and has no certificate.   

**To use**  
Paste json in the left pane. 
Classes will appear in the right pane. 
There is an example.json in the solution file to try out. 

**If you get a build error for a file with internet origin**   
Go to the EditorForm.Resx file and .ico file in File Explorer/ right click/ open properties/ and at the bottom
of the properties window click "Unblock"

MIT Licensing - View license.md
