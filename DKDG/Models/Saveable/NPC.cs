using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class NPC : Character, ISavable //TODO Max, something custom
    {
        #region Properties

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public string Description { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public string Fear { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public string Quirk { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public string Want { get; private set; }

        #endregion Properties

        #region Constructors

        public NPC(string name) : base(name)
        {
        }

        #endregion Constructors
    }
}
