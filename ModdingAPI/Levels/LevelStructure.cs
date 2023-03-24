using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ModdingAPI.Levels
{
    [System.Serializable]
    internal class LevelStructure
    {
        public DisabledObjectsHolder DisabledObjects { get; set; }

        public List<AddedObject> AddedObjects { get; set; }

        public void CombineLevel(LevelStructure other)
        {
            if (other.DisabledObjects != null)
            {
                if (DisabledObjects == null)
                    DisabledObjects = other.DisabledObjects;
                else
                {
                    // Add disabled decoration objects
                    if (other.DisabledObjects.Decoration != null)
                    {
                        if (DisabledObjects.Decoration == null)
                            DisabledObjects.Decoration = other.DisabledObjects.Decoration;
                        else
                        {
                            foreach (string otherObject in other.DisabledObjects.Decoration)
                            {
                                if (!DisabledObjects.Decoration.Contains(otherObject))
                                    DisabledObjects.Decoration.Add(otherObject);
                            }
                        }
                    }
                    // Add disabled layout objects
                    if (other.DisabledObjects.Layout != null)
                    {
                        if (DisabledObjects.Layout == null)
                            DisabledObjects.Layout = other.DisabledObjects.Layout;
                        else
                        {
                            foreach (string otherObject in other.DisabledObjects.Layout)
                            {
                                if (!DisabledObjects.Layout.Contains(otherObject))
                                    DisabledObjects.Layout.Add(otherObject);
                            }
                        }
                    }
                    // Add disabled logic objects
                    if (other.DisabledObjects.Logic != null)
                    {
                        if (DisabledObjects.Logic == null)
                            DisabledObjects.Logic = other.DisabledObjects.Logic;
                        else
                        {
                            foreach (string otherObject in other.DisabledObjects.Logic)
                            {
                                if (!DisabledObjects.Logic.Contains(otherObject))
                                    DisabledObjects.Logic.Add(otherObject);
                            }
                        }
                    }
                }
            }

            // Add additional objects
            if (other.AddedObjects != null)
            {
                if (AddedObjects == null)
                    AddedObjects = other.AddedObjects;
                else
                {
                    foreach (AddedObject otherObj in other.AddedObjects)
                    {
                        if (!AddedObjects.Contains(otherObj))
                            AddedObjects.Add(otherObj);
                    }
                }
            }
        }
    }

    [System.Serializable]
    internal class DisabledObjectsHolder
    {
        public List<string> Decoration { get; set; }
        public List<string> Layout { get; set; }
        public List<string> Logic { get; set; }
    }

    [System.Serializable]
    internal class AddedObject
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public float XPos { get; set; }
        public float YPos { get; set; }

        public override bool Equals(object obj)
        {
            return Id == ((AddedObject)obj).Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
