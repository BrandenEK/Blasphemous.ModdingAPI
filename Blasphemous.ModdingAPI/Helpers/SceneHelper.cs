
namespace Blasphemous.ModdingAPI.Helpers;

/// <summary>
/// Provides information about the currently loaded scene
/// </summary>
public static class SceneHelper
{
    /// <summary>
    /// The name of the currently loaded scene, or ""
    /// </summary>
    public static string CurrentScene { get; internal set; } = string.Empty;

    /// <summary>
    /// Whether the main menu or a gameplay scene is loaded
    /// </summary>
    public static bool AnySceneLoaded => CurrentScene.Length > 0;

    /// <summary>
    /// Whether the main menu scene is loaded
    /// </summary>
    public static bool MenuSceneLoaded => CurrentScene == "MainMenu";

    /// <summary>
    /// Whether a gameplay scene is loaded
    /// </summary>
    public static bool GameSceneLoaded => AnySceneLoaded && !MenuSceneLoaded;
}
