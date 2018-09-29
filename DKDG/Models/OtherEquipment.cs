using System.Collections.Generic;

using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class OtherEquipment : ISaveable
    {
        #region Fields

        public readonly List<Modifier> Modifiers = new List<Modifier>();

        public string Extension => "eqp";

        #endregion Fields
    }
}
