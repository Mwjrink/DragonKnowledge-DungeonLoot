using System.Collections.Generic;

using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class OtherEquipment : IItem, ISavable
    {
        #region Properties
        
        public long ID { get; set; }

        public string Extension => "eqp";

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Bool)]
        public bool Attunement { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer, true)]
        public int Bonus { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public string Description { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Bool)]
        public bool Magical { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public (string Ability, int Amount) MinimumAbility { get; private set; } //TODO Max, something custom

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, unique: true)]
        public string Name { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public Rarity Rarity { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Real)]
        public Money Value { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Real)]
        public double Weight { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public List<Modifier> Modifiers { get; private set; } = new List<Modifier>();

        #endregion Properties
    }
}
