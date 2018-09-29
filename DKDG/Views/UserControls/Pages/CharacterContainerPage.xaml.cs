using System.Windows.Controls;
using System.Windows.Input;

using DKDG.Models;
using DKDG.ViewModels;

namespace DKDG.Views.UserControls
{
    /// <summary>
    /// Interaction logic for CharactersPage.xaml
    /// </summary>
    public partial class CharacterContainerPage : UserControl
    {
        #region Constructors

        public CharacterContainerPage()
        {
            InitializeComponent();
        }

        public CharacterContainerPage(CharacterContainerPageVM characterPageVM)
        {
            InitializeComponent();
            DataContext = characterPageVM;
        }

        #endregion Constructors

        #region Methods

        private void MenuItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (((MenuItem)sender).Name)
            {
                case "Save":
                    ((CharacterContainerPageVM)DataContext).Save((Character)((ListView)FindName("CharacterList")).SelectedItem);
                    break;

                case "SaveAs":
                    ((CharacterContainerPageVM)DataContext).SaveAs((Character)((ListView)FindName("CharacterList")).SelectedItem);
                    break;

                case "Delete":
                    ((CharacterContainerPageVM)DataContext).Delete((Character)((ListView)FindName("CharacterList")).SelectedItem);
                    break;
            }
        }

        #endregion Methods
    }
}
