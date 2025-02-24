﻿using System.Collections.Generic;
using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Monster : ICreature, ISavable
    {
        #region Properties

        #region Ability Scores

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer)]
        public int Charisma { get; private set; } = 8;

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer)]
        public int Constitution { get; private set; } = 8;

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer)]
        public int Dexterity { get; private set; } = 8;

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer)]
        public int Intelligence { get; private set; } = 8;

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer)]
        public int Strength { get; private set; } = 8;

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer)]
        public int Wisdom { get; private set; } = 8;

        #endregion Ability Scores

        public long ID { get; set; }

        public string Extension => "mstr";

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer)]
        public int AC { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.MultipleParent)]
        public Alignment Alignment { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Real)]
        public double ChallengeRating { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer)]
        public int Experience { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer)]
        public int HitPointMaximum { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Integer)]
        public int HitPoints { get; private set; }

        [DataMember, SQLProp("Name", SQLSaveType.Text, CustomColumnUnique: true)] //TODO Max, something custom
        public Dictionary<string, Inventory> Inventories { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Link)]
        public List<IItem> Loot { get; private set; }

        [DataMember, SQLProp("ModifyProp", SQLSaveType.Text)] //TODO Max, something custom
        public Dictionary<string, Modifier> Modifiers { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Real)]
        public Money Money { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, unique: true)]
        public string Name { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public CreatureType Type { get; private set; }

        #endregion Properties

        //  <name>Aarakocra</name>
        //	<size>M</size>
        //	<type>humanoid(aarakocra), monster manual</type>
        //	<alignment>neutral good</alignment>
        //	<ac>12</ac>
        //	<hp>13 (3d8)</hp>
        //	<speed>20 ft., fly 50 ft.</speed>
        //	<str>10</str>
        //	<dex>14</dex>
        //	<con>10</con>
        //	<int>11</int>
        //	<wis>12</wis>
        //	<cha>11</cha>
        //	<skill>Perception +5</skill>
        //	<passive>15</passive>
        //	<languages>Auran, Aarakocra</languages>
        //	<cr>1/4</cr>
        //	<trait>
        //		<name>Dive Attack</name>
        //		<text>If the aarakocra is flying and dives at least 30 ft.straight toward a target and then hits it with a melee weapon attack, the attack deals an extra 3 (1d6) damage to the target.</text>
        //		<attack>Dive Attack||1d6</attack>
        //	</trait>
        //	<action>
        //		<name>Talon</name>
        //		<text>Melee Weapon Attack: +4 to hit, reach 5 ft., one target. Hit: 4 (1d4 + 2) slashing damage.</text>
        //		<attack>Talon|4|1d4+2</attack>
        //	</action>
        //	<action>
        //		<name>Javelin</name>
        //		<text>Melee or Ranged Weapon Attack: +4 to hit, reach 5 ft.or range 30/120 ft., one target. Hit: 5 (1d6 + 2) piercing damage.</text>
        //		<attack>Javelin|4|1d6+2</attack>
        //	</action>
        //	<action>
        //		<name>Summon Air Elemental</name>
        //		<text>Five aarakocra within 30 feet of each other can magically summon an air elemental. Each of the five must use its action and movement on three consecutive turns to perform an aerial dance and must maintain concentration while doing so (as if concentrating on a spell). When all five have finished their third turn of the dance, the elemental appears in an unoccupied space within 60 feet of them.It is friendly toward them and obeys their spoken commands.It remains for 1 hour, until it or all its summoners die, or until any of its summoners dismisses it as a bonus action.A summoner can't perform the dance again until it finishes a short rest. When the elemental returns to the Elemental Plane of Air, any aarakocra within 5 feet of it can return with it.</text>
        //	</action>
        //</monster>
        //<monster>
        //	<name>Aboleth</name>
        //	<size>L</size>
        //	<type>aberration, monster manual</type>
        //	<alignment>lawful evil</alignment>
        //	<ac>17 (natural armor)</ac>
        //	<hp>135 (18d10+36)</hp>
        //	<speed>10 ft., swim 40 ft.</speed>
        //	<str>21</str>
        //	<dex>9</dex>
        //	<con>15</con>
        //	<int>18</int>
        //	<wis>15</wis>
        //	<cha>18</cha>
        //	<save>Con +6, Int +8, Wis +6</save>
        //	<skill>History +12, Perception +10</skill>
        //	<senses>darkvision 120 ft.</senses>
        //	<passive>20</passive>
        //	<languages>Deep Speech, telepathy 120 ft.</languages>
        //	<cr>10</cr>
        //	<trait>
        //		<name>Amphibious</name>
        //		<text>The aboleth can breathe air and water.</text>
        //	</trait>
        //	<trait>
        //		<name>Mucous Cloud</name>
        //		<text>While underwater, the aboleth is surrounded by transformative mucus. A creature that touches the aboleth or that hits it with a melee attack while within 5 ft.of it must make a DC 14 Constitution saving throw. On a failure, the creature is diseased for 1d4 hours.The diseased creature can breathe only underwater.</text>
        //	</trait>
        //	<trait>
        //		<name>Probing Telepathy</name>
        //		<text>If a creature communicates telepathically with the aboleth, the aboleth learns the creature's greatest desires if the aboleth can see the creature.</text>
        //	</trait>
        //	<action>
        //		<name>Multiattack</name>
        //		<text>The aboleth makes three tentacle attacks.</text>
        //	</action>
        //	<action>
        //		<name>Tentacle</name>
        //		<text>Melee Weapon Attack: +9 to hit, reach 10 ft., one target. Hit: 12 (2d6 + 5) bludgeoning damage.If the target is a creature, it must succeed on a DC 14 Constitution saving throw or become diseased.The disease has no effect for 1 minute and can be removed by any magic that cures disease.After 1 minute, the diseased creature's skin becomes translucent and slimy, the creature can't regain hit points unless it is underwater, and the disease can be removed only by heal or another disease-curing spell of 6th level or higher. When the creature is outside a body of water, it takes 6 (1d12) acid damage every 10 minutes unless moisture is applied to the skin before 10 minutes have passed.</text>
        //		<attack>Tentacle|9|2d6+5</attack>
        //	</action>
        //	<action>
        //		<name>Tail</name>
        //		<text>Melee Weapon Attack: +9 to hit, reach 10 ft.one target.Hit: 15 (3d6 + 5) bludgeoning damage.</text>
        //		<attack>Tail|9|3d6+5</attack>
        //	</action>
        //	<action>
        //		<name>Enslave (3/day)</name>
        //		<text>The aboleth targets one creature it can see within 30 ft.of it.The target must succeed on a DC 14 Wisdom saving throw or be magically charmed by the aboleth until the aboleth dies or until it is on a different plane of existence from the target.The charmed target is under the aboleth's control and can't take reactions, and the aboleth and the target can communicate telepathically with each other over any distance.</text>
        //		<text>Whenever the charmed target takes damage, the target can repeat the saving throw. On a success, the effect ends.No more than once every 24 hours, the target can also repeat the saving throw when it is at least 1 mile away from the aboleth.</text>
        //	</action>
        //	<legendary>
        //		<name>Detect</name>
        //		<text>The aboleth makes a Wisdom (Perception) check.</text>
        //	</legendary>
        //	<legendary>
        //		<name>Tail Swipe</name>
        //		<text>The aboleth makes one tail attack.</text>
        //	</legendary>
        //	<legendary>
        //		<name>Psychic Drain (Costs 2 Actions)</name>
        //		<text>One creature charmed by the aboleth takes 10 (3d6) psychic damage, and the aboleth regains hit points equal to the damage the creature takes.</text>
        //	</legendary>
        //</monster>
        //<monster>
        //	<name>Abominable Yeti</name>
        //	<size>H</size>
        //	<type>monstrosity, monster manual</type>
        //	<alignment>chaotic evil</alignment>
        //	<ac>15 (natural armor)</ac>
        //	<hp>137 (11d12+66)</hp>
        //	<speed>40 ft., climb 40 ft.</speed>
        //	<str>24</str>
        //	<dex>10</dex>
        //	<con>22</con>
        //	<int>9</int>
        //	<wis>13</wis>
        //	<cha>9</cha>
        //	<skill>Perception +5, Stealth +4</skill>
        //	<immune>cold</immune>
        //	<senses>darkvision 60 ft.</senses>
        //	<passive>15</passive>
        //	<languages>Yeti</languages>
        //	<cr>9</cr>
        //	<trait>
        //		<name>Fear of Fire</name>
        //		<text>If the yeti takes fire damage, it has disadvantage on attack rolls and ability checks until the end of its next turn.</text>
        //	</trait>
        //	<trait>
        //		<name>Keen Smell</name>
        //		<text>The yeti has advantage on Wisdom (Perception) checks that rely on smell.</text>
        //	</trait>
        //	<trait>
        //		<name>Snow Camouflage</name>
        //		<text>The yeti has advantage on Dexterity (Stealth) checks made to hide in snowy terrain.</text>
        //	</trait>
        //	<action>
        //		<name>Multiattack</name>
        //		<text>The yeti can use its Chilling Gaze and makes two claw attacks.</text>
        //	</action>
        //	<action>
        //		<name>Claw</name>
        //		<text>Melee Weapon Attack: +11 to hit, reach 5 ft., one target. Hit: 14 (2d6 + 7) slashing damage plus 7 (2d6) cold damage.</text>
        //		<attack>Claw|11|2d6+7+2d6</attack>
        //	</action>
        //	<action>
        //		<name>Chilling Gaze</name>
        //		<text>The yeti targets one creature it can see within 30 feet of it.If the target can see the yeti, the target must succeed on a DC 18 Constitution saving throw against this magic or take 21 (6d6) cold damage and then be paralyzed for 1 minute, unless it is immune to cold damage.The target can repeat the saving throw at the end of each of its turns, ending the effect on itself on a success. If the target's saving throw is successful, or if the effect ends on it, the target is immune to this yeti's gaze for 1 hour.</text>
        //	</action>
        //	<action>
        //		<name>Cold Breath (Recharge 6)</name>
        //		<text>The yeti exhales a 30-foot cone of frigid air.Each creature in that area must make a DC 18 Constitution saving throw, taking 45 (10d8) cold damage on a failed save, or half as much damage on a successful one.</text>
        //		<attack>Cold Breath||10d8</attack>
        //	</action>
    }
}
