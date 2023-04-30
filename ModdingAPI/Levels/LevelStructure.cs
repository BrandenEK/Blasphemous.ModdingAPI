using System.Collections.Generic;

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
                            DisabledObjects.Decoration.AddRange(other.DisabledObjects.Decoration);
                    }
                    // Add disabled layout objects
                    if (other.DisabledObjects.Layout != null)
                    {
                        if (DisabledObjects.Layout == null)
                            DisabledObjects.Layout = other.DisabledObjects.Layout;
                        else
                            DisabledObjects.Layout.AddRange(other.DisabledObjects.Layout);
                    }
                    // Add disabled logic objects
                    if (other.DisabledObjects.Logic != null)
                    {
                        if (DisabledObjects.Logic == null)
                            DisabledObjects.Logic = other.DisabledObjects.Logic;
                        else
                            DisabledObjects.Logic.AddRange(other.DisabledObjects.Logic);
                    }
                }
            }

            // Add additional objects
            if (other.AddedObjects != null)
            {
                if (AddedObjects == null)
                    AddedObjects = other.AddedObjects;
                else
                    AddedObjects.AddRange(other.AddedObjects);
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
        public float XSize { get; set; }
        public float YSize { get; set; }
        public float Rotation { get; set; }
        public bool FacingDirection { get; set; }
        public string ExtraData { get; set; }

        //public override bool Equals(object obj)
        //{
        //    AddedObject addedObject = (AddedObject)obj;
        //    return Type == addedObject.Type && Id == addedObject.Id;
        //}

        //public override int GetHashCode()
        //{
        //    return (Type.ToString() + Id).GetHashCode();
        //}
    }
}
