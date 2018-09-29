using System.Collections.ObjectModel;

using DKDG.Models;

namespace DKDG.ViewModels
{
    public class NPCPageVM : ViewModelBase
    {
        #region Properties

        private ObservableCollection<NPC> NPCs { get; } = new ObservableCollection<NPC>();

        #endregion Properties

        #region Methods

        public void Add(NPC npc)
        {
            NPCs.Add(npc);
        }

        #endregion Methods
    }
}
