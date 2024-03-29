# Blasphemous Modding API

<img src="https://img.shields.io/github/downloads/BrandenEK/Blasphemous.ModdingAPI/total?color=6495ED&style=for-the-badge">

---

## Features

- Ensures compatibility between all mods
- Very simple process of installing new mods
- Standard functionality for mods to take advantage of

## Usage

- All registered mods should be displayed in the top right corner of the main menu
- Mod settings can be configured by modifying the files in the "Modding/config" folder
- Keyboard input can be configured by modifying the files in the "Modding/keybindings" folder

## Save compatibility

- Make sure to back up any save files you care about before installing mods
- To prevent save corruption, don't load a save file with different mods installed than were present when the save file was created
- If corruption does happen, strange effects may take place, such as rosary beads being randomly equipped.  Removing all existing save files should fix this problem

## Development

Full documentation, along with examples can be found here: [Blasphemous Modding API Documentation](documentation/main.md)
