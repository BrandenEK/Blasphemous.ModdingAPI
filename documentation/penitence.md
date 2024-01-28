# Penitence

Every mod can define custom penitences that can be activated from the Brotherhood statue in NG+

---

```cs
using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Penitence;
using Framework.Managers;
using UnityEngine;

namespace Blasphemous.Example;

public class ExampleMod : BlasMod
{
    public ExampleMod() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        provider.RegisterPenitence(new ExamplePenitence());
    }
}

public class ExamplePenitence : ModPenitence
{
    protected override string Id => "PE_EXAMPLE";

    protected override string Name => Main.Example.LocalizationHandler.Localize("name");

    protected override string Description => Main.Example.LocalizationHandler.Localize("desc");

    protected override string ItemIdToGive => "RB999";

    protected override InventoryManager.ItemType ItemTypeToGive => InventoryManager.ItemType.Bead;

    protected override void Activate() => IsActive = true;

    protected override void Deactivate() => IsActive = false;

    protected override void LoadImages(out Sprite inProgress, out Sprite completed, out Sprite abandoned, out Sprite gameplay, out Sprite chooseSelected, out Sprite chooseUnselected)
    {
        bool loaded = Main.Example.FileHandler.LoadDataAsFixedSpritesheet("menuSlots.png", new Vector2(16, 16), out Sprite[] menuSlotImages);

        inProgress = loaded ? menuSlotImages[0] : null;
        completed = loaded ? menuSlotImages[1] : null;
        abandoned = loaded ? menuSlotImages[2] : null;

        Main.Example.FileHandler.LoadDataAsSprite("gameSlot.png", out gameplay);
        Main.Example.FileHandler.LoadDataAsSprite("chooseSlotSelected.png", out chooseSelected);
        Main.Example.FileHandler.LoadDataAsSprite("chooseSlotUnselected.png", out chooseUnselected);
    }

    public static bool IsActive { get; private set; }
}
```