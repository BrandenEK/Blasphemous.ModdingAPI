using Blasphemous.ModdingAPI.Levels.Loaders;
using Blasphemous.ModdingAPI.Levels.Modifiers;

namespace Blasphemous.ModdingAPI.Levels;

/// <summary>
/// Handles loading and placing objects through the level editor
/// </summary>
public class ObjectCreator
{
    /// <summary>
    /// The loader used to store the base object
    /// </summary>
    public ILoader Loader { get; }
    /// <summary>
    /// The modifier used to apply properties to the base object
    /// </summary>
    public IModifier Modifier { get; }

    /// <summary>
    /// Creates a new object creator for the level editor
    /// </summary>
    public ObjectCreator(ILoader loader, IModifier modifier)
    {
        Loader = loader;
        Modifier = modifier;
    }
}
