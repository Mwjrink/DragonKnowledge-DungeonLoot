using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Skill : ISavable
    {
        #region Properties

        public long ID { get; set; }

        public Ability BaseAbility { get; private set; }

        public string Extension => "skl";

        public string Name { get; private set; }

        #endregion Properties

        #region Constructors

        public Skill(string name, Ability baseAbility)
        {
            Name = name;
            BaseAbility = baseAbility;
        }

        #endregion Constructors
    }
}
