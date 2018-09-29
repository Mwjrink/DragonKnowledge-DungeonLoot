using System;
using System.Collections.Generic;

using DKDG.Models;

namespace DKDG.ViewModels
{
    public class CharacterCreationWindowVM : ViewModelBase
    {
        #region Fields

        internal Character character = new Character();

        #endregion Fields

        #region Properties

        public List<Class> AvailableClasses { get; } = new List<Class>();

        public List<Race> AvailableRaces { get; } = new List<Race>();

        public Class SelectedClass { get; set; }

        public Race SelectedRace { get; set; }

        #endregion Properties

        #region Constructors

        internal CharacterCreationWindowVM()
        {
        }

        #endregion Constructors

        #region Methods

        internal void OnClosing(object sender, EventArgs e)
        {
            //character = new Character();
        }

        #endregion Methods
    }
}
