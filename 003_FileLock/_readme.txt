
A Sync-App to synchronize different processes.

Actually i need a possibility to run on TFS (ICLx Project) only one job/step 'Nuget restore'

The app tries to lock a file and if the content of the file is "free" then the app writes the message "locked" into the file and releases the file

filelock -lock "text" ["10"] ["c:\temp\filelock.txt"]
filelock -free 


=====================================================
Todo
=====================================================
* Add Trace
* Trace Version

=====================================================
History
=====================================================
13.01.19 created