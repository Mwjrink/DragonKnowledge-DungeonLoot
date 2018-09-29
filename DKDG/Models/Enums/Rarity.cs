using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public enum Rarity
    {
        common,
        uncommon,
        Rare,
        VeryRare,
        Legendary,
        Sentient,
        Artifact
    }
}
