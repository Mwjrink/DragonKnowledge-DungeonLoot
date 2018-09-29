using System.Windows.Controls;

using DKDG.ViewModels;

namespace DKDG.Views.UserControls
{
    /// <summary>
    /// Interaction logic for NoteBookPage.xaml
    /// </summary>
    public partial class NoteBookPage : UserControl
    {
        #region Constructors

        public NoteBookPage(NoteBookPageVM noteBookPageVM)
        {
            InitializeComponent();
        }

        #endregion Constructors
    }
}
