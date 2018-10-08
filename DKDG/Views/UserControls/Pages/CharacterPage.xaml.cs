using System;
using System.Windows.Controls;

using DKDG.ViewModels.Base;

using MaterialDesignThemes.Wpf;

namespace DKDG.Views.UserControls
{
    /// <summary>
    /// Interaction logic for CharacterPage.xaml
    /// </summary>
    public partial class CharacterPage : UserControl
    {
        #region Constructors

        public CharacterPage()
        {
            InitializeComponent();
        }

        public CharacterPage(CharacterPageVM characterVM)
        {
            InitializeComponent();
            DataContext = characterVM;
        }

        private void NotesCard_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("SAMPLE 1: Closing dialog with parameter: " + (eventArgs.Parameter ?? ""));

            //you can cancel the dialog close:
            //eventArgs.Cancel();

            if (!Equals(eventArgs.Parameter, true))
                return;

            if (!String.IsNullOrWhiteSpace(FruitTextBox.Text))
                NotesListBox.Items.Add(FruitTextBox.Text.Trim());

            FruitTextBox.Text = String.Empty;
        }

        #endregion Constructors
    }
}
