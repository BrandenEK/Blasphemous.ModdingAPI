using Framework.Managers;

namespace ModdingAPI
{
    /// <summary>
    /// A serializable class that contains persistent data related to this mod
    /// </summary>
    [System.Serializable]
    public abstract class ModPersistentData : PersistentManager.PersistentData
    {
        /// <summary>
        /// Creates new persistent data for this mod
        /// </summary>
        /// <param name="persistentId">The same persistent id as the persistent mod</param>
        public ModPersistentData(string persistentId) : base(persistentId) { }
    }
}
