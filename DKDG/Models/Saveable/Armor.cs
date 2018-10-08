using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Armor : IItem, ISavable
    {
        #region Properties

        public bool Attunement { get; private set; }

        public int Bonus { get; private set; }

        public string Description { get; private set; }

        public string Extension => "armr";

        public string GUID { get; private set; }

        public bool Magical { get; private set; }

        public (string Ability, int Amount) MinimumAbility { get; private set; }

        public string Name { get; private set; }

        public Rarity Rarity { get; private set; }

        public int Value { get; private set; }

        public double Weight { get; private set; }

        public long ID { get; set; }

        #endregion Properties
    }
}
