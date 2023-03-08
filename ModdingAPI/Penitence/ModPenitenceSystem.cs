using Framework.Penitences;

namespace ModdingAPI
{
    internal class ModPenitenceSystem : IPenitence
    {
        public string Id => m_Id;
        private string m_Id;

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
