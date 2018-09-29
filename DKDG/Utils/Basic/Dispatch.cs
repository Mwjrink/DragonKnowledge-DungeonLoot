using System.Windows;
using System.Windows.Threading;

namespace DKDG.Utils
{
    public static class Dispatch
    {
        #region Fields

        public static readonly Dispatcher er = Application.Current.Dispatcher;

        #endregion Fields
    }
}
