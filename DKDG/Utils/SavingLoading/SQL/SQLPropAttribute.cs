using System;

namespace DKDG.Utils
{
    public class SQLPropAttribute : Attribute
    {
        #region Properties

        public bool Nullable { get; private set; }

        public SQLPropSaveType SaveRelationship { get; private set; }

        public SQLSaveType SaveType { get; private set; }

        public object DefaultValue { get; private set; }

        public bool Unique { get; private set; }

        public string CustomColumnName { get; private set; }

        #endregion Properties

        #region Constructors

        public SQLPropAttribute(SQLPropSaveType saveRelationship, SQLSaveType saveType, bool nullable = false, object defaultValue = null,
            bool unique = false)
        {
            SaveRelationship = saveRelationship;
            SaveType = saveType;
            Nullable = nullable;
            DefaultValue = defaultValue;
            Unique = unique;
        }

        public SQLPropAttribute(SQLPropSaveType saveRelationship, bool nullable = false, object defaultValue = null, bool unique = false)
        {
            if (saveRelationship == SQLPropSaveType.Value)
                throw new InvalidOperationException("Cannot declare a value relationship without specifying the SaveType");
            SaveRelationship = saveRelationship;
            Nullable = nullable;
            DefaultValue = defaultValue;
            Unique = unique;
        }

        public SQLSaveType CustomColumnSaveType { get; private set; }

        public bool CustomColumnNullable { get; private set; }

        public object CustomColumnDefaultValue { get; private set; }

        public bool CustomColumnUnique { get; private set; }

        public SQLPropAttribute(string customColumnName, SQLSaveType CustomColumnSaveType, bool CustomColumnNullable = false,
            object CustomColumnDefaultValue = null, bool CustomColumnUnique = false, object defaultValue = null,
            bool unique = false)
        {
            SaveRelationship = SQLPropSaveType.Link;
            CustomColumnName = customColumnName;
            DefaultValue = defaultValue;
            Unique = unique;

            this.CustomColumnSaveType = CustomColumnSaveType;
            this.CustomColumnNullable = CustomColumnNullable;
            this.CustomColumnDefaultValue = CustomColumnDefaultValue;
            this.CustomColumnUnique = CustomColumnUnique;
        }

        #endregion Constructors
    }

    //[AnimalType(Animal.Dog)]
}
