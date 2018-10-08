using System.Collections.Generic;

using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Spell : ISavable
    {
        #region Properties

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public Dice BonusDamagePerLevel { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string CastTime { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public List<Class> Classes { get; private set; } = new List<Class>();

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string Components { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public bool Concentration { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public Roll Damage { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string Description { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string Duration { get; private set; }

        public string Extension => "spl";

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public int Level { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string Name { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public int Range { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public bool Ritual { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public List<Roll> Rolls { get; private set; } = new List<Roll>();

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public SpellSchool School { get; private set; }

        /*
        //  <spell>
        //	<name>Acid Splash</name>
        //	<level>0</level>
        //	<school>C</school>
        //	<time>1 action</time>
        //	<range>60 feet</range>
        //	<components>V, S</components>
        //	<duration>Instantaneous</duration>
        //	<classes>Sorcerer, Wizard, Fighter (Eldritch Knight), Rogue (Arcane Trickster)</classes>
        //	<text>You hurl a bubble of acid. Choose one creature within range, or choose two creatures within range that are within 5 feet of each other. A target must succeed on a Dexterity saving throw or take 1d6 acid damage.</text>
        //	<text />
        //	<text>This spells damage increases by 1d6 when you reach 5th Level (2d6), 11th level(3d6) and 17th level(4d6).</text>
        //	<roll>1d6</roll>
        //	<roll>2d6</roll>
        //	<roll>3d6</roll>
        //	<roll>4d6</roll>
        //	<ritual>YES</ritual>
        */

        #endregion Properties
    }
}
