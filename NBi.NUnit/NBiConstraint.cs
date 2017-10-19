using NBi.Framework;
using NBi.Framework.FailureMessage;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public abstract class NBiConstraint : NUnitCtr.Constraint
    {
        protected object actual;

        public ITestConfiguration Configuration {get; set;}

        public NBiConstraint()
        {
        }

        public override NUnitCtr.ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            return Matches(actual);
        }

        public abstract NUnitCtr.ConstraintResult Matches(object actual);

        public virtual void WriteDescriptionTo(MessageWriter writer)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteActualValueTo(MessageWriter writer)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteMessageTo(MessageWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
