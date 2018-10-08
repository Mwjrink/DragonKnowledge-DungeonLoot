using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Language : ISavable
    {
        #region Properties

        public string Extension => "lng";

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
