using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Armor : IItem, ISavable
    {
        #region Properties

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Bool)]
        public bool Attunement { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer, true)]
        public int Bonus { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)]
        public string Description { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Bool, false, false)]
        public bool Magical { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)] //TODO Max, something custom to save
        public (string Ability, int Amount) MinimumAbility { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public string Name { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, something custom to load
        public Rarity Rarity { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Real, true)]
        public Money Value { get; private set; } // Monetary

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Real, false)]
        public double Weight { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public ArmorType Type { get; private set; }

        public long ID { get; set; }

        public string Extension => "armr";

        #endregion Properties
    }
}
