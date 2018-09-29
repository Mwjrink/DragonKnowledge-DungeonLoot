using System.Collections.Generic;

using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Class : IClass, ISaveable
    {
        #region Fields

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public Dictionary<int, List<int>> SpellSlots = new Dictionary<int, List<int>>();

        #endregion Fields

        #region Properties

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public bool Custom { get; private set; }

        public string Extension => "cls";

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public Dice HitDice { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string Name { get; private set; }

        #endregion Properties

        #region Constructors

        public Class(string name, Dice hitDice)
        {
            Name = name;
            HitDice = hitDice;
        }

        #endregion Constructors

        #region Methods

        public void CustomLevelAction(Character character, int level)
        {
            throw new System.NotImplementedException();
        }

        #endregion Methods
    }
}
