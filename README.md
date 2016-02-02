# WebSiteToJpg

This is a guide to setup correctly your Visual Studio and Debug the solution:

First the solution is to run the main project of this repository named **WebSiteToJpg** and is located in the folder with name **GetWebSitesToJPG**

####WARNING:
To debug the solution currently you need to enable IIS in the machine.

#####Steps to run the app
 1. Enter in Visual Studio with Administrator permissions.
 2. Load the solution.
 3. Right click on project named **ServerUsersOwinAuth**, select Properties, in the options of the Properties menu select Web.
 4. On the Servers configurations, change to Local IIS and set the Project Url field to point to your server name and a name for the instance for the virtual directory that will be created.  
 _URL Example:_ http://JRBSAG-THINKPC/ServerUsersOwinAuth
 5. Click on Create Virtual Directory button.
 6. Checks the file IcommktAuthenticationHandler.cs located in **OtherIcommktOwinAuth** see the lines: 18,19,20 and 88 pointing to the virtual directory for create the bridge between the projects. Replace if necessary for your virtual directory URL.
 7. Run the solution
 
Any doubt or problem please let me know.
