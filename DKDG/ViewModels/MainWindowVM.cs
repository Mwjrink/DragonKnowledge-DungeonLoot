using System;
using System.Windows.Input;

using DKDG.Models.Utils;
using DKDG.Views.UserControls;

using Main.Navigation;

namespace DKDG.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        #region Properties

        public ICommand ExitCmd => new RelayCommand(OnExitApp);

        public NavigationPage[] Pages { get; }

        /// <summary>
        /// Title of the application, as displayed in the top bar of the window
        /// </summary>
        public string Title => "Dragon Knowledge, Dungeon Gear";

        #endregion Properties

        //<MenuItem Header = "File" >
        //        < MenuItem Header="New Character" Command="{Binding NewCmd}" InputGestureText="Ctrl+N" />
        //        <MenuItem Header = "Open Character" Command="{Binding OpenCmd}" InputGestureText="Ctrl+O" />
        //        <MenuItem Header = "Save Character" Command="{Binding SaveCmd}" InputGestureText="Ctrl+S" />
        //        <MenuItem Header = "Save Character As" Command="{Binding SaveAsCmd}" InputGestureText="Ctrl+Shift+S"/>
        //        <Separator />
        //        <MenuItem Header = "Exit" Command="{Binding ExitCmd}" InputGestureText="Alt+F4"/>
        //    </MenuItem>

        #region Constructors

        public MainWindowVM()
        {
            Pages = new[]
            {
                new NavigationPage("Characters", new CharacterContainerPage(new CharacterContainerPageVM())),
                new NavigationPage("Note Book", new NoteBookPage(new NoteBookPageVM())),
                new NavigationPage("Items", new ItemsPage(new ItemsPageVM())),
                new NavigationPage("Spells", new SpellsPage(new SpellsPageVM())),
                new NavigationPage("Monsters", new MonstersPage(new MonstersPageVM())),
                new NavigationPage("Browser", new BrowserPage(new BrowserPageVM())),
                new NavigationPage("Rules", new RulesPage(new RulesPageVM())),
                new NavigationPage("NPC's", new NPCPage(new NPCPageVM()))
            };

            //Characters"
            //"Note Book"
            //"Items"
            //"Spells"
            //"Monsters"
            //"Browser"
            //"Rules"

            //PRINTS ALL PROPERTIES IN ALL CLASSES
            //foreach (Type ass in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass))
            //{
            //    Console.WriteLine(ass.Name + ": ");
            //    foreach (PropertyInfo prop in ass.GetProperties())
            //        Console.WriteLine(prop.Name);
            //    Console.WriteLine(Environment.NewLine);
            //}
        }

        #endregion Constructors

        #region Methods

        //Add monsters?
        private void OnExitApp()
        {
            //TODO Ask to save?
            System.Windows.Application.Current.MainWindow.Close();
        }

        internal void OnClose(object sender, EventArgs e)
        {
            //Do something maybe?
        }

        #endregion Methods

        //TODO LIST OF TODO

        //Create an Import so you can import a db into your db/combine them

        //add a database browser?

        //Export from your database so you dont have to export everything to share one thing

        //save item/char/spell as xml and allow it to be imported by app
    }
}
