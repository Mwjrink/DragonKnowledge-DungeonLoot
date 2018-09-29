using System.Windows.Input;

using DKDG.Models.Utils;

namespace DKDG.Models.Navigation
{
    public class DocumentationLink
    {
        #region Properties

        public string Label { get; }

        public ICommand Open { get; }

        public DocumentationLinkType Type { get; }

        public string Url { get; }

        #endregion Properties

        #region Constructors

        public DocumentationLink(DocumentationLinkType type, string url, string label = null)
        {
            Label = label ?? type.ToString();
            Url = url;
            Type = type;
            Open = new RelayCommand(Execute);
        }

        #endregion Constructors

        #region Methods

        private void Execute(object o)
        {
            System.Diagnostics.Process.Start(Url);
        }

        #endregion Methods
    }
}
