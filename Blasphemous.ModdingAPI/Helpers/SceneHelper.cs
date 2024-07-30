
namespace Blasphemous.ModdingAPI.Helpers;

/// <summary>
/// Provides information about the currently loaded scene
/// </summary>
public static class SceneHelper
{
    /// <summary>
    /// Whether an actual in-game scene is loaded
    /// </summary>
    public static bool GameSceneLoaded => AnySceneLoaded && !MenuSceneLoaded;

    /// <summary>
    /// Whether the main menu scene is loaded
    /// </summary>
    public static bool MenuSceneLoaded => Main.ModLoader.CurrentScene == "MainMenu";

    /// <summary>
    /// Whether the main menu or an in-game scene is loaded
    /// </summary>
    public static bool AnySceneLoaded => Main.ModLoader.CurrentScene.Length > 0;

    /// <summary>
    /// The name of the currently loaded scene, or ""
    /// </summary>
    public static string CurrentScene => Main.ModLoader.CurrentScene;
}
