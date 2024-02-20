using UnityEngine;

namespace Blasphemous.ModdingAPI;

internal class ModdingAPI : BlasMod
{
    public ModdingAPI() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    private Font _blasFont;
    public Font BlasFont
    {
        set
        {
            if (_blasFont != null)
                return;

            _blasFont = value;
        }
    }
}
