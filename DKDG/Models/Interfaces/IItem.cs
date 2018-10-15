namespace DKDG.Models
{
    public interface IItem
    {
        #region Properties

        bool Attunement { get; }

        int Bonus { get; }

        string Description { get; }

        bool Magical { get; }

        (string Ability, int Amount) MinimumAbility { get; }

        string Name { get; }

        Rarity Rarity { get; }

        Money Value { get; }

        double Weight { get; }

        //<name>Adamantine Chain Mail</name>
        //<type>HA</type> Heavy Armor, Medium Armor, Light Armor, Martial, Simple, Ammo, Ranged, Potion, Shield, RD: Rod, RG: Ring, SC: Spell Scroll, ST: Staff, W: Amulet&Book, , WD: Wand,
        //<magic>1</magic>
        //<weight>55</weight>
        //<ac>16</ac>
        //<strength>13</strength>
        //<stealth>YES</stealth>
        //<rarity>Uncommon</rarity>
        //<text>Rarity: Uncommon</text>
        //<text>	This suit of armor is reinforced with adamantine, one of the hardest substances in existence.While you're wearing it, any critical hit against you becomes a normal hit.</text>
        //<text />
        //<text>Source: Dungeon Master's Guide, page 150</text>

        #endregion Properties
    }
}
