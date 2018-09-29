using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public enum Dice
    {
        [EnumMember]
        d2,

        [EnumMember]
        d4,

        [EnumMember]
        d6,

        [EnumMember]
        d8,

        [EnumMember]
        d10,

        [EnumMember]
        d12,

        [EnumMember]
        d20,

        [EnumMember]
        d100
    }
}
