using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

using DKDG.Utils;
using DKDG.ViewModels;
using DKDG.ViewModels.Base;
using DKDG.Views.UserControls;

namespace DKDG.Models
{
    //DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute.
    //If the type is a collection, consider marking it with the CollectionDataContractAttribute
    [DataContract, SQLSavableObject]
    public class Character : ViewModelBase, ICreature, ISavable, IDisplayable
    {
        #region Fields

        private object _Page;

        public string path = null;

        #endregion Fields

        #region Events

        public EventHandler TitleChanged;

        #endregion Events

        #region Properties

        public long ID { get; set; }

        #region Calculated Properties

        private string _title = null;

        public int AC { get; private set; } = 10;

        public string Extension => "chr";

        public List<Dice> HitDice { get; private set; } = new List<Dice>();

        public string HitDiceString => GetHitDiceString();

        public int Initiative { get; private set; }

        public int MovementSpeed { get; private set; }

        public object Page => _Page ?? (_Page = new CharacterPage(new CharacterPageVM(this)));

        public int PassivePerception { get; private set; }

        public int ProficiencyBonus => 2 + ((Level - 1) / 4);
        
        public Dictionary<string, Modifier> Modifiers { get; private set; } = new Dictionary<string, Modifier>();

        #endregion Calculated Properties

        #region DataMembers

        #region Ability Scores

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer, false)]
        public int Charisma { get; private set; } = 8;

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer, false)]
        public int Constitution { get; private set; } = 8;

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer, false)]
        public int Dexterity { get; private set; } = 8;

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer, false)]
        public int Intelligence { get; private set; } = 8;

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer, false)]
        public int Strength { get; private set; } = 8;

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer, false)]
        public int Wisdom { get; private set; } = 8;

        #endregion Ability Scores

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)]
        public Alignment Alignment { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.MultipleParent, false)]
        public Background Background { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Blob)]
        public BitmapImage DisplayImage { get; private set; }

        [DataMember, SQLProp("Attuned", SQLSaveType.Bool)] //TODO Max, something custom
        public List<(IItem item, bool attuned)> Equipped { get; } = new List<(IItem, bool)>();

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer, false)]
        public int HitPointMaximum { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer, false)]
        public int HitPoints { get; private set; }

        [DataMember, SQLProp("Name", SQLSaveType.Text)] //TODO Max, something custom
        public Dictionary<string, Inventory> Inventories { get; private set; } = new Dictionary<string, Inventory>();

        [DataMember, SQLProp(SQLPropSaveType.Link)]
        public List<Spell> KnownSpells { get; private set; } = new List<Spell>();

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer, false)]
        public int Level { get; private set; } = 0;

        [DataMember, SQLProp("Level", SQLSaveType.Integer, false, CustomColumnUnique: true)]
        public List<Class> Levels { get; private set; } = new List<Class>(App.MAX_LEVEL);

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer)]
        public int Experience { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Link)]
        public List<IItem> Loot { get; private set; } = new List<IItem>();

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Real, false)]
        public Money Money { get; private set; } = new Money();

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)]
        public string Name { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Link)] //TODO Max, from regex]
        public List<Spell> PreparedSpells { get; private set; } = new List<Spell>();

        [DataMember, SQLProp(SQLPropSaveType.MultipleParent)]
        public Race Race { get; private set; }

        [DataMember, SQLProp("Proficient", SQLSaveType.Bool)] //TODO Max, something custom
        public Dictionary<Skill, bool> Skills { get; private set; } = new Dictionary<Skill, bool>();
        
        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)] //TODO Max, do something custom
        public Dictionary<int, bool> SpellSlots { get; private set; } = new Dictionary<int, bool>(); //TODO Max, Dictionary<int, List<bool>>

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public string Title
        {
            get => _title ?? GenerateTitle();
            private set => _title = value;
        }

        public CreatureType Type => Race.Type;

        #endregion DataMembers

        #endregion Properties

        #region Constructors

        protected Character(string name)
        {
            Name = name;
        }

        public Character()
        {
        }

        public Character(string name, Alignment alignment, Background background, Race race, int level,
            int str, int dex, int con, int inl, int wis, int cha, Class cls, IEnumerable<Class> lvls = null)
        {
            Name = name;
            Alignment = alignment;
            Background = background;
            Race = race;
            Level = level;
            Strength = str;
            Dexterity = dex;
            Constitution = con;
            Intelligence = inl;
            Wisdom = wis;
            Charisma = cha;

            Levels = (lvls ?? Enumerable.Repeat(cls, Level)).ToList();
            HitPointMaximum = CalculateMaxHealth(true);
            HitPoints = HitPointMaximum;

            Skills.Add(new Skill("Athletics", Ability.Strength), false);

            Skills.Add(new Skill("Acrobatics", Ability.Dexterity), false);
            Skills.Add(new Skill("Sleight of Hand", Ability.Dexterity), false);
            Skills.Add(new Skill("Stealth", Ability.Dexterity), false);

            Skills.Add(new Skill("Arcana", Ability.Intelligence), false);
            Skills.Add(new Skill("History", Ability.Intelligence), false);
            Skills.Add(new Skill("Investigation", Ability.Intelligence), false);
            Skills.Add(new Skill("Nature", Ability.Intelligence), false);
            Skills.Add(new Skill("Religion", Ability.Intelligence), false);

            Skills.Add(new Skill("Animal Handling", Ability.Wisdom), false);
            Skills.Add(new Skill("Insight", Ability.Wisdom), false);
            Skills.Add(new Skill("Medicine", Ability.Wisdom), false);
            Skills.Add(new Skill("Perception", Ability.Wisdom), false);
            Skills.Add(new Skill("Survival", Ability.Wisdom), false);

            Skills.Add(new Skill("Deception", Ability.Charisma), false);
            Skills.Add(new Skill("Intimidation", Ability.Charisma), false);
            Skills.Add(new Skill("Performance", Ability.Charisma), false);
            Skills.Add(new Skill("Persuasion", Ability.Charisma), false);
        }

        //Strength
        //Athletics

        //Dexterity
        //Acrobatics
        //Sleight of Hand
        //Stealth

        //Intelligence
        //Arcana
        //History
        //Investigation
        //Nature
        //Religion

        //Wisdom
        //Animal Handling
        //Insight
        //Medicine
        //Perception
        //Survival

        //Charisma
        //Deception
        //Intimidation
        //Performance
        //Persuasion

        #endregion Constructors

        #region Methods

        private string GenerateTitle()
        {
            string clss = "";

            var dict = new Dictionary<Class, int>();
            foreach (Class cl in Levels)
                dict[cl] = dict.ContainsKey(cl) ? ++dict[cl] : 0;

            foreach (string name in dict.ToList().OrderByDescending(x => x.Value).Select(kv => kv.Key.Name))
                clss += name + "-";
            clss = clss.Remove(clss.Count() - 1);

            return Name + " the " + Race.Name + " " + clss;
        }

        private string GetHitDiceString()
        {
            string hitDice = "";
            foreach (IGrouping<Dice, Dice> dice in HitDice.GroupBy(x => x).ToList().OrderByDescending(x => x.Count()))
                hitDice += dice.Count() + dice.Key + ", ";
            return String.IsNullOrEmpty(hitDice) ? "" : hitDice.Remove(", ".Length);
        }

        public static int GetModifier(int abilityScore)
        {
            return (abilityScore - 10) / 2;
        }

        public int CalculateMaxHealth(bool average)
        {
            int hp = Utilities.DieMax(Levels[0].HitDice);
            foreach (Class cls in Levels.Skip(1))
                hp += (Utilities.DieMax(cls.HitDice) / 2) + 1;

            hp += Level * GetModifier(Constitution);
            return hp;
        }

        public void LevelUp(Class classChoiceForLevel)
        {
            //if (Level % 4 == 0)
            //    ProficiencyBonus++;

            Level++;
            Levels[Level] = classChoiceForLevel;

            HitDice.Add(classChoiceForLevel.HitDice);

            classChoiceForLevel.CustomLevelAction(this, Level);
        }

        public void LevelUp()
        {
            LevelUp(Levels[Level - 1]);
        }

        public void OnTitleChanged(object sender, EventArgs e)
        {
            TitleChanged.Invoke(sender, e);
        }

        #endregion Methods
    }
}
