using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Tool : ISavable
    {
        #region Properties

        public string Extension => "tool";

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public bool Proficient { get; private set; }

        #endregion Properties
    }
}
