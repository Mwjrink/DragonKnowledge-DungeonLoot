using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;

using DKDG.Models;
using DKDG.Models.Utils;
using DKDG.Views.CharacterCreation;

using Microsoft.Win32;

namespace DKDG.ViewModels
{
    public class CharacterContainerPageVM : ViewModelBase
    {
        #region Properties

        public ObservableCollection<Character> Characters { get; private set; } = new ObservableCollection<Character>();

        public RelayCommand NewCharacterCommand => new RelayCommand(() => NewCommand());

        public RelayCommand OpenCharacterCommand => new RelayCommand(() => Open());

        public RelayCommand SaveAllCharactersCommand => new RelayCommand(() => SaveAll());

        public object SelectedCharacterPage { get; set; }

        #endregion Properties

        #region Constructors

        public CharacterContainerPageVM()
        {
            var Rogue = new Class("Rogue", Dice.d8);
            var Ranger = new Class("Ranger", Dice.d10);
            var Cleric = new Class("Cleric", Dice.d8);
            var Barbarian = new Class("Barbarian", Dice.d12);
            var GunSlinger = new Class("GunSlinger", Dice.d8);
            var Druid = new Class("Druid", Dice.d8);
            var Sorcerer = new Class("Sorcerer", Dice.d6);
            var Bard = new Class("Bard", Dice.d8);

            Characters.Add(new Character("Vax", new Alignment(Alignment1.Chaotic, Alignment2.Good),
                new Background("Urchin"), new Race("Half-Elf"), 10, 14, 20, 10, 16, 14, 14, Rogue));

            Characters.Add(new Character("Vex", new Alignment(Alignment1.Neutral, Alignment2.Good),
                new Background("Runaway"), new Race("Half-Elf"), 10, 7, 20, 10, 14, 14, 17, Ranger));

            Characters.Add(new Character("Pike", new Alignment(Alignment1.Chaotic, Alignment2.Good),
                new Background("Sailor"), new Race("Gnome"), 10, 13, 11, 12, 13, 18, 14, Cleric));

            Characters.Add(new Character("Grog", new Alignment(Alignment1.Chaotic, Alignment2.Neutral),
                new Background("Outlander"), new Race("Goliath"), 10, 19, 15, 20, 6, 10, 13, Barbarian));

            Characters.Add(new Character("Percy", new Alignment(Alignment1.Neutral, Alignment2.Good),
                new Background("Noble"), new Race("Human"), 10, 12, 22, 14, 16, 16, 14, GunSlinger));

            Characters.Add(new Character("Keyleth", new Alignment(Alignment1.Chaotic, Alignment2.Good),
                new Background("Outlander"), new Race("Half-Elf"), 10, 14, 15, 14, 15, 22, 10, Druid));

            Characters.Add(new Character("Tiberius", new Alignment(Alignment1.Chaotic, Alignment2.Good),
                new Background("Sage"), new Race("Red Dragonborn"), 10, 12, 16, 16, 14, 4, 20, Sorcerer));

            Characters.Add(new Character("Scanlan", new Alignment(Alignment1.Chaotic, Alignment2.Good),
                new Background("Entertainer"), new Race("Forest Gnome"), 10, 13, 11, 15, 14, 7, 20, Bard));
        }

        #endregion Constructors

        #region Methods

        private Character New()
        {
            try
            {
                //Application.Current.MainWindow.Hide();
                Application.Current.MainWindow.IsEnabled = false;

                var newCharWin = new CharacterCreationWindow();
                newCharWin.ShowDialog();

                return newCharWin.GetNewCharacter();
            }
            finally
            {
                //Application.Current.MainWindow.Show();
                Application.Current.MainWindow.IsEnabled = true;
            }
        }

        private void NewCommand()
        {
            Character n = New();
            if (n != null)
                Characters.Add(n);
        }

        private void Open()
        {
            var openDialog = new OpenFileDialog();
            openDialog.Multiselect = true;
            openDialog.Title = "Open Character(s)";
            openDialog.Filter = "Save File (*.xml) | *xml";

            if (openDialog.ShowDialog() ?? false)
            {
                var ser = new DataContractSerializer(typeof(List<Character>));
                var reader = new FileStream(openDialog.FileName, FileMode.Open);

                foreach (Character v in (List<Character>)ser.ReadObject(reader))
                    Characters.Add(v);
                reader.Close();
            }
        }

        internal void Delete(Character character)
        {
            Console.WriteLine("Delete " + character.Name);
            Characters.Remove(character);
        }

        internal void Save(Character character)
        {
            Console.WriteLine("Save " + character.Name);

            if (character.path == null)
            {
                SaveAs(character);
            }
            else
            {
                var ser = new DataContractSerializer(typeof(List<Character>));
                var writer = new FileStream(character.path, FileMode.OpenOrCreate);
                ser.WriteObject(writer, new List<Character>() { character });
                writer.Close();
            }
        }

        internal void SaveAll()
        {
            Console.WriteLine("Save All");
            var saveDialog = new SaveFileDialog();
            saveDialog.Title = "Save All";
            saveDialog.AddExtension = true;
            saveDialog.DefaultExt = ".xml";
            saveDialog.FilterIndex = 0;
            saveDialog.Filter = "Save File (*.xml) | *xml";

            if (saveDialog.ShowDialog() ?? false)
            {
                var ser = new DataContractSerializer(typeof(List<Character>));
                var writer = new FileStream(saveDialog.FileName, FileMode.OpenOrCreate);
                ser.WriteObject(writer, Characters.ToList());
                writer.Close();
            }
        }

        internal void SaveAs(Character character)
        {
            Console.WriteLine("Save as" + character.Name);
            var saveDialog = new SaveFileDialog();
            saveDialog.Title = "Save as" + character.Name;
            saveDialog.AddExtension = true;
            saveDialog.DefaultExt = ".xml";
            saveDialog.FilterIndex = 0;
            saveDialog.Filter = "Save File (*.xml) | *xml";

            if (saveDialog.ShowDialog() ?? false)
            {
                var ser = new DataContractSerializer(typeof(List<Character>));
                var writer = new FileStream(saveDialog.FileName, FileMode.OpenOrCreate);
                ser.WriteObject(writer, new List<Character>() { character });
                writer.Close();
            }
        }

        #endregion Methods
    }
}
