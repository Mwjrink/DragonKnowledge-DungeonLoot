using System.Windows.Controls;

using DKDG.ViewModels;

namespace DKDG.Views.UserControls
{
    /// <summary>
    /// Interaction logic for BrowserPage.xaml
    /// </summary>
    public partial class BrowserPage : UserControl
    {
        #region Constructors

        public BrowserPage(BrowserPageVM browserPageVM)
        {
            InitializeComponent();
        }

        #endregion Constructors
    }
}
