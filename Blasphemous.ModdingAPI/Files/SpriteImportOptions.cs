using UnityEngine;

namespace Blasphemous.ModdingAPI.Files;

/// <summary>
/// Settings used when importing a sprite
/// </summary>
public class SpriteImportOptions
{
    /// <summary> Default: 32 </summary>
    public int PixelsPerUnit { get; set; } = 32;

    /// <summary> Default: true </summary>
    public bool UsePointFilter { get; set; } = true;

    /// <summary> Default: (0.5, 0.5) </summary>
    public Vector2 Pivot { get; set; } = new Vector2(0.5f, 0.5f);

    /// <summary> Default: (0, 0, 0, 0) </summary>
    public Vector4 Border { get; set; } = Vector4.zero;

    /// <summary> Default: Tight </summary>
    public SpriteMeshType MeshType { get; set; } = SpriteMeshType.Tight;
}
