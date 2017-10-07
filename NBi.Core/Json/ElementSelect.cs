using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Json
{
    public class ElementSelect : AbstractSelect
    {
        internal ElementSelect(string xpath)
            : base(xpath)
        {
        }
    }
}
