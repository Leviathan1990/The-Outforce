# The-Outforce

*.box archive extractor  V2.0 Console app.
======================================
*.box archive extractor.
Designed for the game "The Outforce"
======================================
Changelog history:
{V1.0}
+Support for some *.box file
---------------------------------------------------------
{V2.0}
+Full support for the *.box archive
You can extract every *.box archive files.
+Bugfixes
---------------------------------------------------------
======================================
USAGE:
======================================
Put the desired *.box archive  file to the program's
root folder and then start the program...
enter the name of the *.box file, first the 
program  will display the content of it...

All after that, type "extract" command to
extract the content of the *.box file.
No need to create folders and subfolders,
my program will do all of that for you.
======================================
KNOWING ISSUES
======================================
Have an issue? Do not hesitate, send me an email!
======================================
Contact
======================================
Moddb: https://www.moddb.com/games/the-outforce
Discord: https://discord.gg/7RbzqN9
e-mail: krisztiankispeti1990@gmail.com
======================================
Special thanks to
======================================
Krisztian Kispeti for developing the program.
And Watto for the *.box archive structure determina-
tion.
======================================
*.box archive structure
======================================

X - File Data

// Directory
  4 - Number Of Files

  // for each file
    X - Filename
    1 - null Filename Terminator
    4 - File offset
    4 - File Size

4 - Directory Offset

Endian order: Little Endian
Compression: -
Encryption: -
