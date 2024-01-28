# Blasphemous Modding API
![Release version](https://img.shields.io/github/v/release/BrandenEK/Blasphemous-Modding-API)
![Last commit](https://img.shields.io/github/last-commit/BrandenEK/Blasphemous-Modding-API?color=important)
![Downloads](https://img.shields.io/github/downloads/BrandenEK/Blasphemous-Modding-API/total?color=success)

## Features

- Enables the console for all registered mods
- Allows loading custom skins
- Ensures compatibility between all mods
- Adds support for custom penitences and items
- Very simple process of installing new mods

## Usage

- Press 'backslash' at any time to open the debug console and enter commands
- All registered mods should be displayed in the top right corner of the main menu
- Mods can be disabled by simply moving them out of the "Modding/plugins" folder
- Keyboard input can be configured by modifying the files in the "Modding/keybindings" folder

## Save compatibility

- Make sure to back up any save files you care about before installing mods
- To prevent save corruption, don't load a save file with different mods installed than were present when the save file was created
- If corruption does happen, strange effects may take place, such as rosary beads being randomly equipped.  Removing all existing save files should fix this problem

## Documentation

Full documentation, along with examples can be found here: [Blasphemous Modding API Documentation](documentation/main.md)