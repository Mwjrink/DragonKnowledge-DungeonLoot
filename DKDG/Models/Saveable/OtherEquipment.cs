using System.Collections.Generic;

using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class OtherEquipment : ISavable
    {
        #region Fields

        public readonly List<Modifier> Modifiers = new List<Modifier>();

        public string Extension => "eqp";

        #endregion Fields

        #region Properties

        public long ID { get; set; }

        #endregion Properties
    }
}
