namespace DKDG.Models
{
    public interface IClass
    {
        #region Properties

        Dice HitDice { get; }

        string Name { get; }

        #endregion Properties

        #region Methods

        void CustomLevelAction(Character character, int level);

        #endregion Methods
    }
}
