using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public enum WeaponType
    {
        [EnumMember]
        SimpleMelee,

        [EnumMember]
        SimpleRanged,

        [EnumMember]
        MartialMelee,

        [EnumMember]
        MartialRanged,

        [EnumMember]
        Ammunition
    }
}
