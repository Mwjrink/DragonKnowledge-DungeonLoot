using System.Windows.Media.Imaging;

namespace DKDG.Models
{
    public interface IDisplayable
    {
        #region Properties

        BitmapImage DisplayImage { get; }

        string Name { get; }

        #endregion Properties
    }
}
