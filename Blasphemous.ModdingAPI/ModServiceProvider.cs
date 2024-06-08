
namespace Blasphemous.ModdingAPI;

/// <summary>
/// Allows a mod to register custom services
/// </summary>
public class ModServiceProvider
{
    internal ModServiceProvider(BlasMod mod)
    {
        RegisteringMod = mod;
    }

    /// <summary>
    /// The mod that is registering this service
    /// </summary>
    public BlasMod RegisteringMod { get; }
}
