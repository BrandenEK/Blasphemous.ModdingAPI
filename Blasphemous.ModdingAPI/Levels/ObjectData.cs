using Newtonsoft.Json;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels;

internal class LevelEdit
{
    [JsonProperty] public readonly ObjectData[] additions = [];
    [JsonProperty] public readonly ObjectData[] modifications = [];
    [JsonProperty] public readonly ObjectData[] deletions = [];
}

/// <summary>
/// Stores information about a level addition, modification, or deletion
/// </summary>
public class ObjectData
{
    /// <summary> Required for modifications and deletions to locate them in the scene </summary>
    [JsonProperty] public readonly string scene;
    /// <summary> Required for modifications and deletions to locate them in the scene </summary>
    [JsonProperty] public readonly string path;

    /// <summary> Required for additions and modifications to determine their properties </summary>
    [JsonProperty] public readonly string type;
    /// <summary> Required for additions and modifications to determine their properties </summary>
    [JsonProperty] public readonly string id;

    /// <summary> Used by additions and modifications to place the object </summary>
    [JsonProperty] public readonly Vector position = Vector.Zero;
    /// <summary> Used by additions and modifications to place the object </summary>
    [JsonProperty] public readonly Vector rotation = Vector.Zero;
    /// <summary> Used by additions and modifications to place the object </summary>
    [JsonProperty] public readonly Vector scale = Vector.One;

    /// <summary> Used by everything to determine if this object should be skipped </summary>
    [JsonProperty] public readonly string condition = string.Empty;
    /// <summary> Used by additions and modifications to provide extra information </summary>
    [JsonProperty] public readonly string[] properties = [];
}

/// <summary>
/// Serializable representation of a Vector3
/// </summary>
public readonly record struct Vector
{
    /// <summary> The X coordinate </summary>
    public float X { get; }
    /// <summary> The Y coordinate </summary>
    public float Y { get; }
    /// <summary> The Z coordinate </summary>
    public float Z { get; }

    /// <summary>
    /// Creates a new Vector with the specified properties
    /// </summary>
    public Vector(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Formats the vector
    /// </summary>
    public override string ToString() => $"({X}, {Y}, {Z})";

    /// <summary>
    /// (0, 0, 0)
    /// </summary>
    public static Vector Zero => new(0, 0, 0);
    /// <summary>
    /// (1, 1, 1)
    /// </summary>
    public static Vector One => new(1, 1, 1);

    /// <summary>
    /// Converts to a Vector3
    /// </summary>
    public static implicit operator Vector3(Vector v) => new(v.X, v.Y, v.Z);
    /// <summary>
    /// Converts to a SerializeableVector
    /// </summary>
    public static implicit operator Vector(Vector3 v) => new(v.x, v.y, v.z);
}