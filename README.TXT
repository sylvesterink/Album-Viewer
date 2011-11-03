Photo Album Viewer of the Gods
The ultimate solution to your photo viewing needs

Developers:

Zach Gardner
Cavan Crawford
Brandon Kasa
David Carver
John Verber


Table of Contents
	1. System Requirements
	2. Installation
	3. Version History
	4. Troubleshooting


1. System Requirements:

	Windows 7
	.NET Framework v4.0.30319
	Minimum 256 MB RAM
	Recommended 1GB RAM for large Albums

2. Installation:

	Copy the executable to a directory of your choice.
	When the program runs it will create an albums and pictures
	folder in the same one as the .exe.

3. Version History
	
Cycle 2 Deployment
	2.3.4324.23252 11/03/2011
			- Fixed MD5 checks on new photos
			- Added ability to store extra info of pictures, such as timestamps
			- Added ability to copy image from one album to another
			- Fixed photo viewer scaling and resizing
			- Cleaned up photo viewer interface
			- Added ability to navigate to next and previous photos in photo viewer
			- Implemented basid slideshow feature in viewer
			- Minor bugfixes (rename issue, etc)

	2.2.4322.37302 11/01/2011
			- Multiuser capability complete.  (Switching users added.)
			- Initial adjustments of the photo viewer
			- Fixed new album dialog forcing main form into background
			- Made new user and photo viewer windows start centered
			- Minor bugfixes

	2.1.4322.21999 11/01/2011
			- Added ability to create multiple users
			- Added ability to print images
			- Added ability to rotate images
			- Fixed photo deletion
			- Added confirmation on deletion of photo or album
			- Added MD5 checksum attribute for photos
			- Removed "Close Album" feature

	2.0.4313.41000 10/23/2011
            - Added default application icon
			- Added a check to make sure only one instance of the program is running
			- Added a check to make sure that .NET Framework version is 4.0.x or greater


Cycle 1 Deployment
	1.3.4 10/10/2011 - Fixed an issue with List View scroll bars not working as intended

	1.3.3 10/9/2011 - Performance optomizations when many files are loaded
	
	1.3.2 10/8/2011 - Minor changes to UI
			- Fixed crash with file extentions too long	
			- Fixed Rename picture to become disabled when an image is removed

	1.3.1 10/7/2011 - Fixed different pictures with the same name issue
	
	1.3.0 10/7/2011 - Added an update info button to update all changed information entered by the user
			- Fixed issue with highlighting an item from list or album view
			- Fixed Pictures of the same name but different images will no longer have the same image by adding (#)
			  at the end for duplicates
			- Fixed picture's filename as the caption when viewing a picture instead of the picture's name
			- Fixed Rename bug that added double the information
			- Fixed where certain features like delete, rename, ect. were not greyed out to the user
			- Removed OK button next to renaming textbox
			- Removed checkbox when changing description


	1.2.2 10/7/2011 - Bug fix of adding the first picture selected multiple times when more than
			  one picture was selected to be imported


	1.2.1 10/6/2011 - Added an OK button when renaming a pictures name
			- Added right clicking on a album in the list will display its name in the menu
			- Added right clicking on a picture in tumbnail or list view to allow the user
			  to view or remove picture
	
	1.1.1 10/1/2011 - Added feature to rename an album
			- Added feature to remove a picture
			- Added text at the bottom to show what album is open
			- Added the pictures name as a caption when viewing the picture
			- User can now double-click on an album to open
			- User can now double-click on a thumbnail to view
			- Changed APP NAME to Photo Album Viewer of the Gods
			- Fixed disposing of mempory when an album is closed
			- Removed view menu
			- Removed the ability to changed background colors
	
	1.0.2 9/26/2011 - Quickfix removing a line from GUI that was incompatable without the very latest .NET framework
	
	1.0.1 9/25/2011 - Initial Release


4.Troubleshooting:

	Error on launch: Require .NET v4.0.30319 to run

	To fix, make sure you have the latest windows updates installed
