using System;

using System.Runtime.Serialization;
using System.Windows.Input;
using DKDG.Models.Utils;
using DKDG.Utils;
using DKDG.ViewModels;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Roll : ViewModelBase
    {
        #region Fields

        private Random random = new Random();

        #endregion Fields

        #region Commands

        public ICommand RollCommand => new RelayCommand(param => { RollValue = Throw(); OnPropertyChanged(nameof(RollValue)); });

        #endregion Commands

        #region Properties

        public bool? Advantage { get; private set; }

        public bool? Crit { get; private set; }

        public Dice DieType { get; private set; }

        public int Modifier { get; private set; }

        public int NumberOfDice { get; private set; }

        public int RollValue { get; private set; }

        #endregion Properties

        #region Constructors

        public Roll(int numberOfDice, Dice dieType, bool? advantage = null, bool? Crit = null, bool half = false, params int[] modifiers)
        {
            NumberOfDice = numberOfDice;
            DieType = DieType;
            foreach (int mod in modifiers)
                Modifier += mod;

            this.Crit = Crit;
            if (half)
                if (Crit ?? false)
                    Crit = null;
                else
                    Crit = false;

            Advantage = advantage;
        }

        #endregion Constructors

        #region Methods

        private int RollInternal()
        {
            int value = 0;
            for (int i = 0; i < NumberOfDice; i++)
                value += (int)random.NextDouble() * Utilities.DieMax(DieType) + 1;

            return value;
        }

        public int Throw()
        {
            int value = RollInternal();

            if ((Advantage != null) || (Crit != null))
            {
                int value2 = RollInternal();

                if (Advantage ?? false)
                    value = value > value2 ? value : value2;
                else if (Crit ?? false)
                    value = value + value2;
                else if (!(Advantage ?? true))
                    value = value < value2 ? value : value2;
            }

            value += Modifier;

            if (!(Crit ?? true))
                return value / 2;

            return value;
        }

        #endregion Methods
    }
}
