namespace Fanex.Athena.UnitTest.Common.Constraints
{
    using NUnit.Framework.Constraints;
    using Utility;

    public class SamePropertiesConstraint : Constraint
    {
        private readonly object expected;

        public SamePropertiesConstraint(object expected) : base(new[]
        {
            expected
        })
        {
            this.expected = expected;
        }

        public override string Description
        {
            get
            {
                return "same as " + this.expected;
            }
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            bool isSuccess = ObjectDumper.Dump(this.expected) == ObjectDumper.Dump(actual);
            return new ConstraintResult(this, actual, isSuccess);
        }
    }
}