using Newtonsoft.Json;
using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels;

public class LevelEdit
{
    [JsonProperty] public readonly ObjectData[] additions = [];
    [JsonProperty] public readonly ObjectData[] modifications = [];
    [JsonProperty] public readonly ObjectData[] deletions = [];
}

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

//public class ObjectAddition
//{
//    [JsonProperty] public readonly string type; // Required
//    [JsonProperty] public readonly string id; // Required

//    [JsonProperty] public readonly Vector position = Vector.Zero;
//    [JsonProperty] public readonly Vector rotation = Vector.Zero;
//    [JsonProperty] public readonly Vector scale = Vector.One;

//    [JsonProperty] public readonly string condition = string.Empty;
//    [JsonProperty] public readonly string[] properties = [];
//}

//public class ObjectModification
//{
//    [JsonProperty] public readonly string scene; // Required
//    [JsonProperty] public readonly string path; // Required

//    [JsonProperty] public readonly string type; // Required
//    [JsonProperty] public readonly string id; // Required

//    [JsonProperty] public readonly Vector position = Vector.Zero;
//    [JsonProperty] public readonly Vector rotation = Vector.Zero;
//    [JsonProperty] public readonly Vector scale = Vector.One;

//    [JsonProperty] public readonly string condition = string.Empty;
//    [JsonProperty] public readonly string[] properties = [];
//}

//public class ObjectDeletion
//{
//    [JsonProperty] public readonly string scene; // Required
//    [JsonProperty] public readonly string path; // Required

//    [JsonProperty] public readonly string condition = string.Empty;
//}

public readonly record struct Vector
{
    public float X { get; }
    public float Y { get; }
    public float Z { get; }

    public Vector(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString() => $"({X}, {Y}, {Z})";

    public static Vector Zero => new(0, 0, 0);
    public static Vector One => new(1, 1, 1);

    public static implicit operator Vector3(Vector v) => new(v.X, v.Y, v.Z);
    public static implicit operator Vector(Vector3 v) => new(v.x, v.y, v.z);
}