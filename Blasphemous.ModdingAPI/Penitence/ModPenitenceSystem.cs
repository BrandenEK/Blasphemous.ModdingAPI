using Framework.Penitences;

namespace Blasphemous.ModdingAPI.Penitence;

internal class ModPenitenceSystem(string id) : IPenitence
{
    public string Id { get; } = id;

    public bool Completed { get; set; }
    public bool Abandoned { get; set; }

    public void Activate()
    {
        Main.ModdingAPI.PenitenceHandler.ActivatePenitence(Id);
    }

    public void Deactivate()
    {
        Main.ModdingAPI.PenitenceHandler.DeactivatePenitence(Id);
    }
}
