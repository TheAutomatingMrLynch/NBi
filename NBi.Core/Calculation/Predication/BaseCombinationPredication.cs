using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predication
{
    abstract class BaseCombinationPredication : ReadOnlyCollection<IPredication>, IPredication
    {
        public abstract string Description { get; }

        internal BaseCombinationPredication(IList<IPredication> predications)
            : base(predications)
        { }

        public virtual string Describe()
        {
            var sb = new StringBuilder();
            foreach (var predication in this)
            {
                sb.Append(predication.Describe());
                sb.Append(" ");
                sb.Append(this.Description);
                sb.Append(" ");
            }
            sb.Remove(sb.Length - this.Description.Length - 2, this.Description.Length + 2);
            sb.Append(".");
            return sb.ToString();
        }

        public bool Execute(DataRow row)
        {
            var state = StartState();
            var enumerator = this.GetEnumerator();
            while (enumerator.MoveNext() && ContinueCondition(state))
                state = Calculate(state, enumerator.Current.Execute(row));
            return state;
        }

        protected abstract bool ContinueCondition(bool state);
        protected abstract bool StartState();
        protected virtual bool Calculate(bool previousState, bool currentResult) => currentResult;
    }
}
