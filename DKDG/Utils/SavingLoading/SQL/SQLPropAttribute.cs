using System;

namespace DKDG.Utils
{
    public class PrimaryKeyAttribute : Attribute { }

    public class SQLPropAttribute : Attribute
    {
        #region Properties

        public bool Nullable { get; private set; }

        public SQLPropSaveType SaveRelationship { get; private set; }

        public SQLSaveType SaveType { get; private set; }

        public object DefaultValue { get; private set; }

        public bool Unique { get; private set; }

        #endregion Properties

        #region Constructors

        public string CustomColumnName { get; private set; }

        public Func<object> CustomColumnValue { get; private set; }

        public SQLPropAttribute CustomPropAttributes { get; private set; }

        public SQLPropAttribute(SQLPropSaveType saveRelationship, SQLSaveType saveType, bool nullable = false, object defaultValue = null, bool unique = false)
        {
            SaveRelationship = saveRelationship;
            SaveType = saveType;
            Nullable = nullable;
            DefaultValue = defaultValue;
            Unique = unique;
        }

        public SQLPropAttribute(string customColumnName, Func<object> customColumnValue, SQLPropAttribute customPropAttributes, SQLSaveType saveType, bool nullable = false, object defaultValue = null, bool unique = false)
        {
            SaveRelationship = SQLPropSaveType.Link;
            CustomColumnName = customColumnName;
            CustomColumnValue = customColumnValue;
            SaveType = saveType;
            Nullable = nullable;
            DefaultValue = defaultValue;
            Unique = unique;
            CustomPropAttributes = customPropAttributes;
        }

        #endregion Constructors
    }

    //[AnimalType(Animal.Dog)]
}
