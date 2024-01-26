using Blasphemous.ModdingAPI.Levels.Loaders;
using Blasphemous.ModdingAPI.Levels.Modifiers;

namespace Blasphemous.ModdingAPI.Levels;

public class ObjectCreator
{
    public ILoader Loader { get; }
    public IModifier Modifier { get; }

    public ObjectCreator(ILoader loader, IModifier modifier)
    {
        Loader = loader;
        Modifier = modifier;
    }
}
