using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Intersection
{
    public class IntersectorFactory
    {
        public Intersector Instantiate(ISettingsResultSet settings)
        {
            switch(settings)
            {
                case SettingsOrdinalResultSet o: return new OrdinalIntersector(o);
                case SettingsNameResultSet n: return new NameIntersector(n);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
