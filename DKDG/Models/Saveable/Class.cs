using System.Collections.Generic;

using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Class : IClass, ISavable
    {
        #region Properties

        public long ID { get; set; }

        public string Extension => "cls";

        //TODO Max, something custom (Static Dictionary of actions to save and load data with 
        // string (Type.name + . + PropertyName) as key
        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] 
        public Dictionary<int, List<bool>> SpellSlots { get; private set; } = new Dictionary<int, List<bool>>();

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Bool)]
        public bool Custom { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public Dice HitDice { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
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
