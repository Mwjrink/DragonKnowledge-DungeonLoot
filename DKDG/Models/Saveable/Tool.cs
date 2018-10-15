using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Tool : ISavable
    {
        #region Properties

        public long ID { get; set; }

        public string Extension => "tool";

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Bool)]
        public bool Proficient { get; private set; }

        #endregion Properties
    }
}
