# Mod class

Every mod utilizing the API must have a class that derives from 'BlasMod'.  This allows it to be registered with the mod loader and implement common mod functionality.

---

### Logging

Every mod has four different methods of logging information

```cs
// Logs a message in white to the console
public void Log(object message)

// Logs a message in yellow to the console
public void LogWarning(object warning)

// Logs a message in red to the console
public void LogError(object error)

// Displays a message with a UI text box
public void LogDisplay(string message)
```

### Events

Every mod can choose to implement any of these methods called by the API

```cs
// Called when starting the game, at the same time as other managers
protected internal virtual void OnInitialize()

// Called when starting the game, after all other mods have been initialized
protected internal virtual void OnAllInitialized()

// Called when exiting the game, at the same time as other managers
protected internal virtual void OnDispose()

// Called every frame after initialization
protected internal virtual void OnUpdate()

// Called at the end of every frame after initialization
protected internal virtual void OnLateUpdate()

// Called when a new level is about to be loaded, including the main menu
protected internal virtual void OnLevelPreloaded(string oldLevel, string newLevel)

// Called when a new level is loaded, including the main menu
protected internal virtual void OnLevelLoaded(string oldLevel, string newLevel)

// Called when an old level is unloaded, including the main menu
protected internal virtual void OnLevelUnloaded(string oldLevel, string newLevel)

// Called when starting a new game on the main menu, after data is reset
protected internal virtual void OnNewGame()

// Called when loading an existing game on the main menu, after data is reset
protected internal virtual void OnLoadGame()

// Called when quiting a game, after returning to the main menu
protected internal virtual void OnExitGame()
```