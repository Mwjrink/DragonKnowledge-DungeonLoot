using System;

namespace DKDG.Utils
{
    public class SQLSavableObject : Attribute
    {
        #region Properties

        public string SaveName { get; }

        #endregion Properties

        #region Constructors

        public SQLSavableObject(string SaveName = null)
        {
            this.SaveName = SaveName;
        }

        #endregion Constructors
    }
}
