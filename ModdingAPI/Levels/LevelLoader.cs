using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModdingAPI.Levels
{
    internal class LevelLoader
    {
        public void LevelLoaded(string level)
        {

        }

        public void LoadLevelEdits()
        {
            Main.LogWarning(Main.MOD_NAME, "Loading level edits from levels folder");
        }
    }
}
