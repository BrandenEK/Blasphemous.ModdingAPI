# Project setup

When creating a new mod, you must first install the mod template and setup your resources folder for exporting

---

### Template creation

Run these commands in a command line to create a new mod project using the template

```dotnet new install Blasphemous.Modding.Templates```

```dotnet new blas1mod -n ProjectName -m ModName -au AuthorName -ve GameVersion```

For example, to create a mod that adds a double jump relic, I would run the command:

```dotnet new blas1mod -n Blasphemous.DoubleJump -m "Double Jump" -au Damocles```

### Folder structure

Each mod must export the ExampleMod.dll in the plugins folder, and it can also export data, level, and localization files.  The exported zip file should follow this file format so that it can be extracted into the "Modding" folder.

```
ExampleMod.zip
├── data
│   ├── Mod Name
│   │   ├── dataFileOne.dat
│   │   └── dataFileTwo.dat
│   └── RequiredDLL.dll
├── levels
│   └── Mod Name
│       ├── D04Z02S01.json
│       └── D17Z01S02.json
├── localization
│   └── Mod Name.txt
└── plugins
    └── ExampleMod.dll
```