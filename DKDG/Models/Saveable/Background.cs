using System.Collections.Generic;

using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Background : ISavable
    {
        #region Properties

        [DataMember, SQLProp(SQLPropSaveType.Link)]
        private List<Skill> Proficiencies { get; } = new List<Skill>(2);

        public string Extension => "bkg";

        [DataMember, SQLProp(SQLPropSaveType.Link)]
        public List<Language> Languages { get; } = new List<Language>(2);

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text)]
        public string Name { get; private set; }

        [DataMember, SQLProp(SQLPropSaveType.Link)]
        public List<Tool> Tools { get; } = new List<Tool>(2);

        public long ID { get; set; }

        #endregion Properties

        #region Constructors

        public Background(
            string Name,

            string proficiency1 = null,
            string proficiency2 = null,

            Tool tool1 = null,
            Tool tool2 = null,

            Language language1 = null,
            Language language2 = null)
        {
            this.Name = Name;

            if (proficiency1 != null)
                Proficiencies.Add(proficiency1);
            if (proficiency2 != null)
                Proficiencies.Add(proficiency2);

            if (tool1 != null)
                Tools.Add(tool1);
            if (tool2 != null)
                Tools.Add(tool2);

            if (language1 != null)
                Languages.Add(language1);
            if (language2 != null)
                Languages.Add(language2);
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
