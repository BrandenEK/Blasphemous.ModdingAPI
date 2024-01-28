# Items

Every mod can define custom items that can be acquired in game or through cheats

---

```cs
using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Items;
using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using UnityEngine;

namespace Blasphemous.Example;

public class ExampleMod : BlasMod
{
    public ExampleMod() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        provider.RegisterItem(new ExampleBead().AddEffect(new ExampleEffect()));
    }
}

public class ExampleBead : ModRosaryBead
{
    protected override string Id => "RB999";

    protected override string Name => Main.Example.LocalizationHandler.Localize("name");

    protected override string Description => Main.Example.LocalizationHandler.Localize("desc");

    protected override string Lore => Main.Example.LocalizationHandler.Localize("lore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => false;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        Main.Example.FileHandler.LoadDataAsSprite("bead.png", out picture);
    }
}

public class ExampleEffect : ModItemEffectOnEquip
{
    private readonly RawBonus _healthIncrease = new(100);

    protected override void ApplyEffect()
    {
        Core.Logic.Penitent.Stats.Life.AddRawBonus(_healthIncrease);
    }

    protected override void RemoveEffect()
    {
        Core.Logic.Penitent.Stats.Life.RemoveRawBonus(_healthIncrease);
    }
}
```