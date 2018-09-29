using System.Windows.Controls;

using DKDG.ViewModels;

namespace DKDG.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SpellsPage.xaml
    /// </summary>
    public partial class SpellsPage : UserControl
    {
        #region Constructors

        public SpellsPage(SpellsPageVM spellsPageVM)
        {
            InitializeComponent();
        }

        #endregion Constructors
    }
}
