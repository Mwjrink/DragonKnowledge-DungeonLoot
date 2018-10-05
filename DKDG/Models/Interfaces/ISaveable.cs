using DKDG.Utils;

namespace DKDG.Models
{
    public interface ISaveable
    {
        #region Properties

        string Extension { get; }

        [PrimaryKey]
        long ID { get; set; }

        #endregion Properties
    }
}
