using NBi.Core.Sequence.Transformation.Aggregation.Function;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Sequence.Transformation.Aggregation.Text
{
    public class ConcatenateTextTest
    {
        [Test]
        public void Execute_Empty_CorrectValue()
        {
            var list = new List<object>() { };
            var aggregation = new ConcatenateText(" - ");
            Assert.That(aggregation.Execute(list), Is.EqualTo(""));
        }

        [Test]
        public void Execute_Single_CorrectValue()
        {
            var list = new List<object>() { "foo" };
            var aggregation = new ConcatenateText(" - ");
            Assert.That(aggregation.Execute(list), Is.EqualTo("foo"));
        }

        [Test]
        public void Execute_NotEmpty_CorrectValue()
        {
            var list = new List<object>() { "foo", "bar", "soprano" };
            var aggregation = new ConcatenateText(" - ");
            Assert.That(aggregation.Execute(list), Is.EqualTo("foo - bar - soprano"));
        }
    }
}
