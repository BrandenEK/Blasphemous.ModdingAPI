using Framework.Penitences;

namespace Blasphemous.ModdingAPI.Penitence;

internal class ModPenitenceSystem : IPenitence
{
    public string id;
    public string Id => id;

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

    public ModPenitenceSystem(string id)
    {
        this.id = id;
    }
}
