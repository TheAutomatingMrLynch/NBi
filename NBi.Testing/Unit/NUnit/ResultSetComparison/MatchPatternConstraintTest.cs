using Moq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NBi.Core.Query;
using NBi.NUnit.Query;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NBi.Testing.Unit.NUnit.ResultSetComparison
{
    public class MatchPatternConstraintTest
    {
        private class MessageWriter : TextMessageWriter
        {

            public override void DisplayDifferences(object expected, object actual, global::NUnit.Framework.Constraints.Tolerance tolerance)
            {
                throw new System.NotImplementedException();
            }

            public override void DisplayDifferences(object expected, object actual)
            {
                throw new System.NotImplementedException();
            }
            
            public override void DisplayStringDifferences(string expected, string actual, int mismatch, bool ignoreCase, bool clipping)
            {
                throw new System.NotImplementedException();
            }

            public override int MaxLineLength
            {
                get
                {
                    throw new System.NotImplementedException();
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            public override void WriteActualValue(object actual)
            {
                Actual = actual;
            }
            
            public override void WriteMessageLine(int level, string message, params object[] args)
            {
                Message += message + "\r\n";
            }
            
            public virtual void WritePredicate(string predicate)
            {
                Predicate += predicate;
            }

            public override void WriteValue(object val)
            {
                throw new System.NotImplementedException();
            }

            public object Actual { get; set; }
            public string Predicate { get; set; }

            public string Message { get; set; }
        }

        [Test]
        public void Matches_RegexCorrectlySpecified_Validated()
        {
            var cells = new FormattedResults();
            cells.Add("$185,125.12");
            cells.Add("$125.12");
            cells.Add("$125.00");

            var matchPatternConstraint = new MatchPatternConstraint();
            matchPatternConstraint = matchPatternConstraint.Regex(@"^\$?[0-9]{1,3}(?:,?[0-9]{3})*\.[0-9]{2}$");

            //Method under test
            var res = matchPatternConstraint.Matches(cells);

            //Test conclusion            
            Assert.That(res.IsSuccess, Is.True);
        }

        [Test]
        public void Matches_RegexWronglySpecified_Validated()
        {
            var cells = new FormattedResults();
            cells.Add("$185,125.12");
            cells.Add("$125.12");
            cells.Add("$125");

            var matchPatternConstraint = new MatchPatternConstraint();
            matchPatternConstraint = matchPatternConstraint.Regex(@"^\$?[0-9]{1,3}(?:,?[0-9]{3})*\.[0-9]{2}$");

            //Method under test
            var res = matchPatternConstraint.Matches(cells);

            //Test conclusion            
            Assert.That(res.IsSuccess, Is.False);
        }

        [Test]
        public void WriteDescription_OneItemHasFailed_CorrectKeywordsForPredicate()
        {
            var cells = new FormattedResults();
            cells.Add("$185,125.12");
            cells.Add("$125.12");
            cells.Add("$125");

            var matchPatternConstraint = new MatchPatternConstraint();
            matchPatternConstraint = matchPatternConstraint.Regex(@"^\$?[0-9]{1,3}(?:,?[0-9]{3})*\.[0-9]{2}$");

            //Method under test
            var res = matchPatternConstraint.Matches(cells);
       
            var msg = new MessageWriter();
            matchPatternConstraint.WriteDescriptionTo(msg);

            //Test conclusion    
            Assert.That(msg.Predicate, Does.Contain("cell")
                .And.Contain("regex"));
        }

        [Test]
        public void WriteDescription_OneItemHasFailed_CorrectKeywordsForActualValue()
        {
            var cells = new FormattedResults();
            cells.Add("$185,125.12");
            cells.Add("$125.12");
            cells.Add("$125");

            var matchPatternConstraint = new MatchPatternConstraint();
            matchPatternConstraint = matchPatternConstraint.Regex(@"^\$?[0-9]{1,3}(?:,?[0-9]{3})*\.[0-9]{2}$");

            //Method under test
            var res = matchPatternConstraint.Matches(cells);

            var msg = new MessageWriter();
            matchPatternConstraint.WriteActualValueTo(msg);

            //Test conclusion    
            Assert.That(msg.Message, Does.Contain("$125")
                .And.Contain("doesn't validate this pattern"));
        }

    }
}
