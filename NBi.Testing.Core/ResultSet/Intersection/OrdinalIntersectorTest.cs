using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Data;
using NBi.Core.ResultSet.Intersection;

namespace NBi.Testing.Core.ResultSet.Intersection
{
    public class OrdinalIntersectorTest
    {
        protected DataTable BuildDataTable(IEnumerable<List<object>> rows)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            for (int i = 0; i < rows.Max(x => x.Count()); i++)
                dt.Columns.Add($"Column{i}");

            foreach (var row in rows)
            {
                var dr = dt.NewRow();
                int i = 0;
                foreach (var value in row)
                {
                    dr.SetField<object>(i, value);
                    i++;
                }
                dt.Rows.Add(dr);
            }
            
            return dt;
        }

        [Test]
        public void Execute_NoIntersection_True()
        {
            var x = BuildDataTable(
                new List<List<object>>()
                {
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "c", 140 },
                    new List<object>() {"a" , "d", 150 },
                });

            var y = BuildDataTable(
                new List<List<object>>()
                {
                    new List<object>() {"x" , "b", 120 },
                    new List<object>() {"a" , "y", 140 },
                    new List<object>() {"a" , "d", -20 },
                });

            var intersector = new OrdinalIntersector();
            var result = intersector.Execute(x,y);
            Assert.That(result, Is.EqualTo(ResultIntersectionRows.Empty));
        }

        [Test]
        public void Execute_OneIntersection_False()
        {
            var x = BuildDataTable(
                new List<List<object>>()
                {
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "c", 140 },
                    new List<object>() {"a" , "d", 150 },
                });

            var y = BuildDataTable(
                new List<List<object>>()
                {
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "y", 140 },
                    new List<object>() {"a" , "d", -20 },
                });

            var intersector = new OrdinalIntersector();
            var result = intersector.Execute(x, y);
            Assert.That(result, Is.Not.EqualTo(ResultIntersectionRows.Empty));
            Assert.That(result.Rows.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_TwoIntersection_False()
        {
            var x = BuildDataTable(
                new List<List<object>>()
                {
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "c", 140 },
                    new List<object>() {"a" , "d", 150 },
                });

            var y = BuildDataTable(
                new List<List<object>>()
                {
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "c", 140 },
                    new List<object>() {"a" , "d", -20 },
                });

            var intersector = new OrdinalIntersector();
            var result = intersector.Execute(x, y);
            Assert.That(result, Is.Not.EqualTo(ResultIntersectionRows.Empty));
            Assert.That(result.Rows.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Execute_DuplicateIntersection_TwoOccurences()
        {
            var x = BuildDataTable(
                new List<List<object>>()
                {
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "c", 140 },
                    new List<object>() {"a" , "b", 120 },
                });

            var y = BuildDataTable(
                new List<List<object>>()
                {
                    new List<object>() {"a" , "b", 120 },
                    new List<object>() {"a" , "c", 140 },
                    new List<object>() {"a" , "d", -20 },
                });

            var intersector = new OrdinalIntersector();
            var result = intersector.Execute(x, y);
            Assert.That(result, Is.Not.EqualTo(ResultIntersectionRows.Empty));
            Assert.That(result.Rows.Count(), Is.EqualTo(2));
        }
    }
}
