using DKDG.Utils;

namespace DKDG.Models
{
    public interface ISavable
    {
        #region Properties

        string Extension { get; }

        [PrimaryKey]
        long ID { get; set; }

        #endregion Properties
    }
}
