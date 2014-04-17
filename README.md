SnapshotService
===============

Simple Web API to take snapshots of given URL using PhantomJS

Set up instructions:

1. Open solution in VS 23013

2. Please change following parameter in Web.Config to set PhantomJS.exe location.
   <add key="PhantomJSDirectory" value="Folder where PhantomJS.exe is located"/>
 
3. Try to run SnapshotService Web API with following URL format:
   http://localhost/snapshotservice/Home/RenderSnapshot?url=www.google.com&imageType=jpg&imageWidth=1024&imageHeight=768

4.  You can read about PhantomJS here: http://phantomjs.org/

Technologies Used:

1. Microsoft .Net framework 4.5

2. PhantomJS version 1.9.7.0
