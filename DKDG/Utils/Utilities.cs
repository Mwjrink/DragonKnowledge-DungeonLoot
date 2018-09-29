using System;
using System.Collections.Generic;

using System.Runtime.Serialization;

using DKDG.Models;

namespace DKDG.Utils
{
    [DataContract, SQLSavableObject]
    internal static class Utilities
    {
        #region Properties

        public static Dictionary<string, string> AbilityNames { get; } = new Dictionary<string, string>()
        {{"Strength", "STR"},
        {"Dexterity", "DEX"},
        {"Constitution", "CON"},
        {"Intelligence", "INT"},
        {"Wisdom", "WIS"},
        {"Charisma", "CHA"},

        {"STR", "Strength"},
        {"DEX", "Dexterity"},
        {"CON", "Constitution"},
        {"INT", "Intelligence"},
        {"WIS", "Wisdom"},
        {"CHA", "Charisma"}};

        #endregion Properties

        #region Methods

        public static int DieMax(Dice dieType)
        {
            return Int32.Parse(dieType.ToString().Trim('d'));
        }

        #endregion Methods
    }
}
