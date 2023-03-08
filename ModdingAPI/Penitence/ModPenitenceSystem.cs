using Framework.Penitences;

namespace ModdingAPI
{
    [System.Serializable]
    internal class ModPenitenceSystem : IPenitence
    {
        public string Id => m_Id;
        public string m_Id;

        public bool Completed { get; set; }
        public bool Abandoned { get; set; }

        public void Activate()
        {
            Main.moddingAPI.penitenceLoader.ActivatePenitence(Id);
        }

        public void Deactivate()
        {
            Main.moddingAPI.penitenceLoader.DeactivatePenitence(Id);
        }

        public ModPenitenceSystem(string id)
        {
            m_Id = id;
        }
    }
}
