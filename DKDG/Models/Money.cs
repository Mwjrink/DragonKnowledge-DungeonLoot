using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public struct Money
    {
        #region Properties

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public int Copper { get; set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public int Gold { get; set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public int Platinum { get; set; }

        [DataMember, SQLProp(SQLPropSaveType.Value, SQLSaveType.Text, false)] //TODO Max, from regex]
        public int Silver { get; set; }

        #endregion Properties

        #region Constructors

        public Money(int P = 0, int G = 0, int S = 0, int C = 0)
        {
            Platinum = P;
            Gold = G;
            Silver = S;
            Copper = C;
        }

        public Money(double money)
        {
            Platinum = (int)money / 10;
            Gold = (int)(money % 10);
            Silver = (int)(money % 1 * 10);
            Copper = (int)(money % 0.1 * 100);
        }

        #endregion Constructors

        #region Methods

        public static implicit operator double(Money money)
        {
            return (money.Platinum * 10) + money.Gold + (money.Silver * 0.1) + (money.Copper * 0.01);
        }

        public static implicit operator Money(double money)
        {
            return new Money(money);
        }

        #endregion Methods
    }
}
