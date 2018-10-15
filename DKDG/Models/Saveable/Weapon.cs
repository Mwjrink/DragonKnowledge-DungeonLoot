using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Weapon : IItem, ISavable
    {
        #region Properties

        public long ID { get; set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public bool Attunement { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public int Bonus { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string Description { get; private set; }

        public string Extension => "wpn";

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string Family { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string GUID { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public bool Magical { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public (string Ability, int Amount) MinimumAbility { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string Name { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public Rarity Rarity { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public Money Value { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public WeaponType WeaponType { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public double Weight { get; private set; }

        #endregion Properties
    }
}
