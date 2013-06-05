﻿using System;
using System.IO;
using System.Linq;

namespace NBi.Service.RunnerConfig
{
    class NUnitRunnerConfigBuilder : AbstractRunnerConfigBuilder
    {
        public NUnitRunnerConfigBuilder()
            : base("NBi.config", "NUnit.nunitproj")
        {

        }

        public NUnitRunnerConfigBuilder(IFilePersister filePersister)
            : this()
        {
            base.filePersister = filePersister;
        }

        protected override string CalculateRunnerProjectFullPath()
        {
            return base.CalculateRunnerProjectFullPath() + ".nunitproj";
        }

        protected override string CalculateConfigFullPath()
        {
            return BasePath + TestSuite + Path.GetFileNameWithoutExtension(this.Filename) + ".config";
        }

    }
}