using System.Collections.Generic;

using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Race : IRace, ISavable
    {
        #region Properties

        public long ID { get; set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public List<Modifier> AbilityScoreIncrease { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string Age { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string Alignment { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public List<string> ArmorProficiencies { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public int DarkVision { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string DarkVisionExtraDescription { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string Description { get; private set; }

        public string Extension => "rce";

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public Dictionary<string, string> Extras { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public List<Language> Languages { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string Name
        {
            get => SelectedSubrace?.Name ?? RaceName;
            private set => RaceName = value;
        }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string RaceName { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public Race SelectedSubrace { get; set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public Size Size { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string SizeExtraDescription { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public int Speed { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public string SpeedExtraDescription { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public List<Race> SubRacesAvailable { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public CreatureType Type { get; internal set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public List<string> WeaponProficiencies { get; private set; }

        #endregion Properties

        #region Constructors

        public Race(string name)
        {
            Name = name;
        }

        #endregion Constructors

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods
    }
}

//Ability Score Increase:
//Your Constitution score increases by 2.

////Age:
//Dwarves mature at the same rate as humans, but they’re considered young until they
//reach the age of 50. On average, they live about 350 years.

////Alignment:
//Most Dwarves are lawful, believing firmly in the benefits of a well-ordered society.
//They tend toward good as well, with a strong sense of Fair Play and a belief that
//everyone deserves to share in the benefits of a just order.

////Size:
//Dwarves stand between 4 and 5 feet tall and average about 150 pounds.Your size is Medium.

////Speed:
//Your base walking speed is 25 feet.Your speed is not reduced by wearing Heavy Armor.

////Darkvision:
//Accustomed to life underground, you have superior vision in dark and dim Conditions.
//You can see in dim light within 60 feet of you as if it were bright light, and in
//Darkness as if it were dim light. You can’t discern color in Darkness, only shades of gray.

////Dwarven Resilience:
//You have advantage on Saving Throws against poison, and you have Resistance
//against poison damage.

////Dwarven Combat Training:
//You have proficiency with the Battleaxe, Handaxe, Light Hammer, and Warhammer.

////Tool Proficiency:
//You gain proficiency with the artisan’s tools of your choice: smith’s tools,
//brewer’s supplies, or mason’s tools.

////Stonecunning:
//Whenever you make an Intelligence (History) check related to the Origin of stonework,
//you are considered proficient in the History skill and add double your proficiency
//bonus to the check, instead of your normal proficiency bonus.

////Languages:
//You can speak, read, and write Common and Dwarvish.
//Dwarvish is full of hard consonants and guttural sounds,
//and those characteristics spill over into whatever other language a dwarf might speak.
