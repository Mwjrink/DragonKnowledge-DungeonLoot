using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public enum Size
    {
        [EnumMember]
        Tiny,           //2½ by 2½ ft

        [EnumMember]
        Small,          //5  by 5 ft

        [EnumMember]
        Medium,         //5  by 5 ft

        [EnumMember]
        Large,          //10 by 10 ft

        [EnumMember]
        Huge,           //15 by 15 ft

        [EnumMember]
        Gargantuan      //20 by 20 ft
    }
}
