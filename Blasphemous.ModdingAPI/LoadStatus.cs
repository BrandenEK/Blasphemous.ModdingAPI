using Blasphemous.ModdingAPI.Helpers;

namespace Blasphemous.ModdingAPI;

/// <summary>
/// Can be used to determine if a certain type of scene is currently loaded
/// </summary>
public class LoadStatus
{
    /// <summary>
    /// Whether an actual in-game scene is loaded
    /// </summary>
    [System.Obsolete("Use the new SceneHelper instead")]
    public bool GameSceneLoaded => SceneHelper.GameSceneLoaded;

    /// <summary>
    /// Whether the main menu scene is loaded
    /// </summary>
    [System.Obsolete("Use the new SceneHelper instead")]
    public bool MenuSceneLoaded => SceneHelper.MenuSceneLoaded;

    /// <summary>
    /// Whether the main menu or an in-game scene is loaded
    /// </summary>
    [System.Obsolete("Use the new SceneHelper instead")]
    public bool AnySceneLoaded => SceneHelper.AnySceneLoaded;

    /// <summary>
    /// The name of the currently loaded scene, or ""
    /// </summary>
    [System.Obsolete("Use the new SceneHelper instead")]
    public string CurrentScene => SceneHelper.CurrentScene;
}
