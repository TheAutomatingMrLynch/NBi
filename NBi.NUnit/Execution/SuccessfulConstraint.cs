using System;
using System.Data;
using NBi.Core;
using NBi.Core.Query;
using NUnitCtr = NUnit.Framework.Constraints;
using NUnit.Framework.Constraints;

namespace NBi.NUnit.Execution
{
    public class SuccessfulConstraint : NBiConstraint
    {
        
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected IExecutionResult Result;

        public SuccessfulConstraint()
        {
        }


        public override ConstraintResult Matches(object actual)
        {
            if (actual is IExecution)
                return doMatch((IExecution)actual);
            else
                return new ConstraintResult(this, null, ConstraintStatus.Error);               
        }

        protected ConstraintResult doMatch(IExecution actual)
        {
            Result = actual.Run();
            return new ConstraintResult(this, Result, Result.IsSuccess);
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine();
            sb.AppendFormat("Successful execution of the etl.");
            sb.AppendLine();
            writer.WritePredicate(sb.ToString());
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            writer.WriteActualValue(string.Format("Failure during execution of the etl: {0}", Result.Message));
        }
    }
}