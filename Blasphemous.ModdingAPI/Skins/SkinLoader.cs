using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Skins;

internal class SkinLoader
{
    public bool AllowedSkinButtons { get; private set; }

    private readonly Dictionary<string, SkinInfo> _customSkins = new();
    private readonly List<GameObject> _skinButtons = new();

    public void AddSkinButton(GameObject obj)
    {
        _skinButtons.Add(obj);
    }

    public List<GameObject> AllowSkinButtons()
    {
        AllowedSkinButtons = true;
        return _skinButtons;
    }

    public SkinInfo GetSkinInfo(string id)
    {
        return _customSkins.TryGetValue(id, out var info) ? info : null;
    }

    public IEnumerable<SkinInfo> GetAllSkinInfos()
    {
        return _customSkins.Values;
    }

    public void LoadCustomSkins()
    {
        Dictionary<string, Sprite> skinData = Main.ModdingAPI.FileHandler.LoadSkins();

        foreach (string skinText in skinData.Keys)
        {
            SkinInfo skinInfo = JsonConvert.DeserializeObject<SkinInfo>(skinText);
            if (_customSkins.ContainsKey(skinInfo.id))
            {
                Main.ModdingAPI.LogWarning($"Rejecting duplicate skin: {skinInfo.id}");
                continue;
            }

            skinInfo.texture = skinData[skinText];
            _customSkins.Add(skinInfo.id, skinInfo);
            Main.ModdingAPI.Log($"Loading custom skin: {skinInfo.id} by {skinInfo.author}");
        }
    }
}
