using UnityEngine;

namespace Blasphemous.ModdingAPI.Levels;

public class ObjectAddition
{
    public string Type { get; }
    public string Id { get; }

    public Vector Position { get; }
    public Vector Rotation { get; }
    public Vector Scale { get; }

    public string Condition { get; }
    public string[] Properties { get; }

    public ObjectAddition(string type, string id, Vector position, Vector rotation, Vector scale, string condition, string[] properties)
    {
        Type = type;
        Id = id;
        Position = position;
        Rotation = rotation;
        Scale = scale;
        Condition = condition;
        Properties = properties;
    }
}

public class ObjectDeletion
{
    public string Scene { get; }
    public string Path { get; }

    public ObjectDeletion(string scene, string path)
    {
        Scene = scene;
        Path = path;
    }
}

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

    public static implicit operator Vector3(Vector v) => new(v.X, v.Y, v.Z);
    public static implicit operator Vector(Vector3 v) => new(v.x, v.y, v.z);
}