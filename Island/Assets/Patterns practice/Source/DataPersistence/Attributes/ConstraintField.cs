using System;

namespace DataPersistence {

    [AttributeUsage(AttributeTargets.Field)]
    public class ConstraintFieldAttribute : Attribute {

        public Constraints<int> constraintsInt { get; }
        public Constraints<float> constraintsFloat { get; }

        public ConstraintFieldAttribute(int min, int max) {
            constraintsInt = new Constraints<int>(min, max);
        }

        public ConstraintFieldAttribute(float min, float max) {
            constraintsFloat = new Constraints<float>(min, max);
        }
    }

    public class Constraints<T> where T : struct {
        public T minValue, maxValue;

        public Constraints(T minValue, T maxValue) {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }
}
