using System.Windows.Controls;

using DKDG.ViewModels;

namespace DKDG.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ItemsPage.xaml
    /// </summary>
    public partial class ItemsPage : UserControl
    {
        #region Constructors

        public ItemsPage(ItemsPageVM itemsPageVM)
        {
            InitializeComponent();
        }

        #endregion Constructors
    }
}
