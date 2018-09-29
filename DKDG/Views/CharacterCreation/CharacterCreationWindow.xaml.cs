using System.Windows;

using DKDG.Models;
using DKDG.ViewModels;

namespace DKDG.Views.CharacterCreation
{
    /// <summary>
    /// Interaction logic for CharacterCreationWindow.xaml
    /// </summary>
    public partial class CharacterCreationWindow : Window
    {
        #region Constructors

        public CharacterCreationWindow()
        {
            InitializeComponent();
            DataContext = new CharacterCreationWindowVM();
            Closed += ((CharacterCreationWindowVM)DataContext).OnClosing;
        }

        #endregion Constructors

        #region Methods

        public Character GetNewCharacter()
        {
            return ((CharacterCreationWindowVM)DataContext).character;
        }

        #endregion Methods
    }
}
