using System;

namespace DataPersistence {

    [AttributeUsage(AttributeTargets.Field)]
    public class SaveFieldAttribute : Attribute {

        public FieldName fieldName;

        public SaveFieldAttribute(FieldName fieldName) {
            this.fieldName = fieldName;
        }
    }
}
