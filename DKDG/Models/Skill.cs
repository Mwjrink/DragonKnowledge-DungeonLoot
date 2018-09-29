using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Skill : ISaveable
    {
        #region Properties

        public Ability baseAbility { get; private set; }

        public string Extension => "skl";

        public string Name { get; private set; }

        #endregion Properties
    }
}
