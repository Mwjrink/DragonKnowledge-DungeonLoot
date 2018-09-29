using System.Windows.Controls;

using DKDG.ViewModels;

namespace DKDG.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MonstersPage.xaml
    /// </summary>
    public partial class MonstersPage : UserControl
    {
        #region Constructors

        public MonstersPage(MonstersPageVM monstersPageVM)
        {
            InitializeComponent();
        }

        #endregion Constructors
    }
}
