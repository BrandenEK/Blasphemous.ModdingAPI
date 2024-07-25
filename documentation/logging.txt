# Logging

Every mod has access to a static ```ModLog``` class, which handles logging all types of messages to the console and to a file

---

```cs
    protected override void OnAllInitialized()
    {
        ModLog.Info("This text shows up in white");
        ModLog.Warn("This text shows up in yellow");
        ModLog.Error("This text shows up in red");
        ModLog.Fatal("This text shows up in dark red");
        ModLog.Debug("This text shows up in gray");
    }
```