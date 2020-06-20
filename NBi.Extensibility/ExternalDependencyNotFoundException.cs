using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.Extensibility
{
    public class ExternalDependencyNotFoundException : NBiException
    {
        public ExternalDependencyNotFoundException(string filename) 
            : base (string.IsNullOrEmpty(filename)
                  ? $"This test is in error because a path to a file has not been set to an empty string"
                  : $"This test is in error because the following dependency has not been found '{Path.GetFullPath(filename)}'.")
        { }


    }
}
