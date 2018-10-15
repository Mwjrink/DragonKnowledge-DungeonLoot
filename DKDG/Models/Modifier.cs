using System.Collections.Generic;

using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Modifier : ISavable
    {
        #region Properties

        public long ID { get; set; }

        public string Extension => "";

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer)]
        public int Mod { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public string Multipler { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public string Name { get; private set; }

        #endregion Properties

        //Level: hp+1 per level

        #region Constructors

        public Modifier(string name, int mod)
        {
            Name = name;
            Mod = mod;
        }

        #endregion Constructors

        #region Methods

        public static implicit operator KeyValuePair<string, int>(Modifier mod)
        {
            return new KeyValuePair<string, int>(mod.Name, mod.Mod);
        }

        #endregion Methods
    }
}
