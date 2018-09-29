using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;

using DKDG.Models.Navigation;
using DKDG.Models.Utils;
using DKDG.Utils;

namespace Main.Navigation
{
    [DataContract, SQLSavableObject]
    public class NavigationPage : INotifyPropertyChanged
    {
        #region Fields

        private object _content;
        private ScrollBarVisibility _horizontalScrollBarVisibilityRequirement;
        private Thickness _marginRequirement = new Thickness(16);
        private string _name;
        private ScrollBarVisibility _verticalScrollBarVisibilityRequirement;

        #endregion Fields

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public object Content
        {
            get => _content;
            set => this.MutateVerbose(ref _content, value, RaisePropertyChanged());
        }

        public IEnumerable<DocumentationLink> Documentation { get; }

        public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement
        {
            get => _horizontalScrollBarVisibilityRequirement;
            set => this.MutateVerbose(ref _horizontalScrollBarVisibilityRequirement, value, RaisePropertyChanged());
        }

        public Thickness MarginRequirement
        {
            get => _marginRequirement;
            set => this.MutateVerbose(ref _marginRequirement, value, RaisePropertyChanged());
        }

        public string Name
        {
            get => _name;
            set => this.MutateVerbose(ref _name, value, RaisePropertyChanged());
        }

        public ScrollBarVisibility VerticalScrollBarVisibilityRequirement
        {
            get => _verticalScrollBarVisibilityRequirement;
            set => this.MutateVerbose(ref _verticalScrollBarVisibilityRequirement, value, RaisePropertyChanged());
        }

        #endregion Properties

        #region Constructors

        public NavigationPage(string name, object content, IEnumerable<DocumentationLink> documentation)
        {
            _name = name;
            Content = content;
            Documentation = documentation;
        }

        public NavigationPage(string name, object content)
        {
            _name = name;
            Content = content;
        }

        #endregion Constructors

        #region Methods

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        #endregion Methods
    }
}
