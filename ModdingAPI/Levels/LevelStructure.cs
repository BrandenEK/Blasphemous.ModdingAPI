using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModdingAPI.Levels
{
    [System.Serializable]
    internal class LevelStructure
    {
        public DisabledObjectsHolder DisabledObjects { get; set; }

        public AddedObject[] AddedObjects { get; set; }

        public override string ToString()
        {
            string output = "New level structure";
            foreach (string item in DisabledObjects.Decoration)
                output += item + "\n";
            foreach (string item in DisabledObjects.Layout)
                output += item + "\n";
            foreach (string item in DisabledObjects.Logic)
                output += item + "\n";

            foreach (AddedObject obj in AddedObjects)
            {
                output += obj.Id + " " + obj.Type;
            }

            return output;
        }
    }

    [System.Serializable]
    internal class DisabledObjectsHolder
    {
        public string[] Decoration { get; set; }
        public string[] Layout { get; set; }
        public string[] Logic { get; set; }
    }

    [System.Serializable]
    internal class AddedObject
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public float XPos { get; set; }
        public float YPos { get; set; }
    }
}
