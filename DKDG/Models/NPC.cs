using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class NPC : Character, ISavable
    {
        #region Properties

        public string Description { get; private set; }

        public string Fear { get; private set; }

        public string Quirk { get; private set; }

        public string Want { get; private set; }

        #endregion Properties

        #region Constructors

        public NPC(string name) : base(name)
        {
        }

        #endregion Constructors
    }
}
