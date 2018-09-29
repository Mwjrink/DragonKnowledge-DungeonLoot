using System.Windows;

namespace DKDG
{
    public partial class App : Application
    {
        #region Properties

        public static int MAX_LEVEL { get; internal set; } = 20;

        #endregion Properties

        //TODO read value from a settings.ini or .xml
    }
}
