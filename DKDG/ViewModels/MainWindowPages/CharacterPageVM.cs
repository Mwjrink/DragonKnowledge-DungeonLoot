using DKDG.Models;

namespace DKDG.ViewModels.Base
{
    public class CharacterPageVM : ViewModelBase
    {
        #region Properties

        public Character Character { get; }

        #endregion Properties

        #region Constructors

        public CharacterPageVM(Character character)
        {
            Character = character;
        }

        #endregion Constructors
    }
}
