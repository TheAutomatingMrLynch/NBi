using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predication
{
    class SinglePredication : IPredication
    {
        public IPredicate Predicate { get; }
        public IColumnIdentifier Operand { get; }
        protected internal RowValueExtractor Extractor { get; }

        public SinglePredication(IPredicate predicate, IColumnIdentifier operand, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
        {
            Predicate = predicate;
            Operand = operand;
            Extractor = new RowValueExtractor(aliases, expressions);
        }

        public bool Execute(DataRow row)
            => Predicate.Execute(Extractor.Execute(Operand, ref row));

        public virtual string Describe()
        {
            var sb = new StringBuilder();
            sb.Append(Operand);
            sb.Append(" ");
            sb.Append(Predicate.ToString());
            sb.Append(".");
            return sb.ToString();
        }
    }
}
