# University Menu (for Windows)

University Menu is an application that provides quick links to university (of Helsinki) services in the Windows environment. The menu contains only hostname and settings items - other items are defined using the xml file. Menu items are created when the program is started and whenever the xml file containing menu configurations is edited. Each time a menu is opened, it is checked whether the items in the menu are still active. Everything presented to the user is collected under one and the same window.

All items displayed in the menu are defined using xml files. Other additional menu items can be defined by separate xml files, which can be either user or machine-specific. The computer may have multiple additional menu item profiles at the same time.

### Popups

The pop-ups is supposed to to inform the user of any potential problems that should be addressed. Notifications can be shown using either a pop-up window or a balloon message. Only a one notification can be displayed during the same day. While the notification condition are in effect, the menu will display their own menu items for active popups, from which the popup window may always be called to display.

### Modules

Modules are freely placed items in the menu with predefined features / functions. More information about modules can be found in the full documentation.

## Documentation

The entire documentation can be read from the file 'University Menu (for Windows).pdf'. Unfortunately, at this point the documentation is only in Finnish (sorry). The entire xml syntax can be found from the file 'University Menu (for Windows).xlsx'.

## Installation

The complete Visual Studio project folder can be found in the code section but you will also need to install the following packages for the project:

 1) http://www.hardcodet.net/wpf-notifyicon
 2) https://github.com/firstfloorsoftware/mui
 3) https://github.com/aybe/Windows-API-Code-Pack-1.1
 
 ## Note
 
  - Sorry about the messy code that is a result of several different learning phases
  - Modern UI icons in the project (except the logo of University of Helsinki) are taken from http://modernuiicons.com/
