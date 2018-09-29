using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public struct Alignment
    {
        #region Fields

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)]
        private readonly Alignment1 al1;

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        private readonly Alignment2 al2;

        #endregion Fields

        #region Constructors

        public Alignment(Alignment1 al1, Alignment2 al2)
        {
            this.al1 = al1;
            this.al2 = al2;
        }

        #endregion Constructors

        #region Methods

        public override string ToString()
        {
            return al1.ToString() == al2.ToString() ? al1.ToString() : al1.ToString() + " " + al2.ToString();
        }

        #endregion Methods
    }

    [DataContract, SQLSavableObject]
    public enum Alignment1
    {
        [EnumMember]
        Chaotic,

        [EnumMember]
        Neutral,

        [EnumMember]
        Lawful
    }

    [DataContract, SQLSavableObject]
    public enum Alignment2
    {
        [EnumMember]
        Evil,

        [EnumMember]
        Neutral,

        [EnumMember]
        Good
    }
}
