using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Rest;
using NBi.Xml.Items.Rest;

namespace NBi.NUnit.Builder
{
    abstract class AbstractExecutionBuilder : AbstractTestCaseBuilder
    {
        protected ExecutionXml SystemUnderTestXml { get; set; }

        protected override void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(sutXml is ExecutionXml))
                throw new ArgumentException("System-under-test must be a 'ExecutionXml'");

            SystemUnderTestXml = (ExecutionXml)sutXml;
        }

        protected override void BaseBuild()
        {
            SystemUnderTest = InstantiateSystemUnderTest(SystemUnderTestXml);
        }

        protected virtual object InstantiateSystemUnderTest(ExecutionXml executionXml)
        {
            if (executionXml.BaseItem is RestXml)
            {
                var restXml = (RestXml)executionXml.BaseItem;
                var factory = new RestClientFactory();
                var restClient = factory.Instantiate(restXml.Location.ContentType, restXml.Location.BaseAddress, restXml.Credentials.Type);

                var cmd = new RestCommand(restClient);
                cmd.Uri = restXml.Path;
                foreach (var param in restXml.Parameters)
                    cmd.Parameters.Add(param.Name, param.StringValue);

                return cmd;
            }
            else
            {
                var commandBuilder = new CommandBuilder();

                var connectionString = executionXml.Item.GetConnectionString();
                var commandText = (executionXml.Item as QueryableXml).GetQuery();

                IEnumerable<IQueryParameter> parameters = null;
                IEnumerable<IQueryTemplateVariable> variables = null;
                int timeout = 0;
                if (executionXml.BaseItem is QueryXml)
                {
                    parameters = ((QueryXml)executionXml.BaseItem).GetParameters();
                    variables = ((QueryXml)executionXml.BaseItem).GetVariables();
                    timeout = ((QueryXml)executionXml.BaseItem).Timeout;
                }
                if (executionXml.BaseItem is ReportXml)
                {
                    parameters = ((ReportXml)executionXml.BaseItem).GetParameters();
                }
                var cmd = commandBuilder.Build(connectionString, commandText, parameters, variables, timeout);

                if (executionXml.BaseItem is ReportXml)
                {
                    cmd.CommandType = ((ReportXml)executionXml.BaseItem).GetCommandType();
                }

                return cmd;
            }
        }


    }
}
