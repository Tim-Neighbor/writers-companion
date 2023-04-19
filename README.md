# TheWritersCompanion
Team 1

# How to Run
1. Clone and build the master branch of this repo in Visual Studio 2019.
2. If using a database for project backup, tunnel into the PHPMyAdmin server using local host and port 3306.
3. When prompted while tunneling, use "team1" for the database username and "********" for the database password.
4. When prompted in the Writers Companion app, use "team1" for the database username and "********" for the database password.
5. Run TheWritersCompanion.exe.
6. See the Project Startup Notes section for tips on using the startup window.
7. Take some awesome notes and be happy :)

# Possible Alternative Database Method
It may be possible to use your own database for project backup, but this feature is not guaranteed to work as of yet. If you wish to try it:
1. Use databasesetup.sql (can be found in the same directory as this README on your machine) to create the needed tables in your MySql database on the PHPMyAdmin server.
2. Tunnel into your database's server using local host and port 3306. NOTE: Your database's name must match your username.

# Project Startup Notes
- When creating a new project in the startup window, an empty directory must be selected. This is the directory in which this project will be stored
- When opening an existing project in the startup window, you must select the same directory that was selected when the project was created. This is the project directory. It immediately contains folders for categories and a TWCconfig.txt file.
- When importing a project from a database in the startup window, you should select a folder that will contain the downloaded project directory. This folder does not need to be empty. The project directory will be downloaded into this folder, and the project directory will be named after the project.

# Keyboard Shortcuts
When in the main text box (containing your note text) in the main editor window, the following keyboard shortcuts are available:
- Ctrl + I : Toggle italics
- Ctrl + B : Toggle bold
- Ctrl + Q : Toggle bullet points
- Ctrl + F : Change font
- Ctrl + S : Save current note
- Ctrl + N : Create new note
- Ctrl + Shift + N : Create new category
- Ctrl + Z : Undo
- Ctrl + Y : Redo
- Ctrl + A : Select all


