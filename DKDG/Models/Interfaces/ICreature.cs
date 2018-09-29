using System.Collections.Generic;

namespace DKDG.Models
{
    internal interface ICreature
    {
        #region Properties

        #region Ability Scores

        int Charisma { get; }

        int Constitution { get; }

        int Dexterity { get; }

        int Intelligence { get; }

        int Strength { get; }

        int Wisdom { get; }

        #endregion Ability Scores

        int AC { get; }

        Alignment Alignment { get; }

        int HitPointMaximum { get; }

        int HitPoints { get; }

        Dictionary<string, Inventory> Inventories { get; }

        List<IItem> Loot { get; }

        Dictionary<string, Modifier> Modifiers { get; }

        Money Money { get; }

        string Name { get; }

        CreatureType Type { get; } //this is the oid, goblinoid, dragonoid, humanoid

        #endregion Properties
    }
}
