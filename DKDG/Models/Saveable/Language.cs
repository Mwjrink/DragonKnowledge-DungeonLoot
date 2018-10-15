using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Language : ISavable
    {
        #region Properties

        public long ID { get; set; }

        public string Extension => "lng";

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public string Name { get; private set; }

        #endregion Properties

        #region Constructors

        public Language(string Name)
        {
            this.Name = Name;
        }

        #endregion Constructors

        #region Methods

        public static implicit operator string(Language language)
        {
            return language.Name;
        }

        #endregion Methods
    }
}
