using NBi.Core.ResultSet;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NBi.Xml.Items.Alteration.Transform;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Scalar.Resolver
{
    public class GlobalVariableScalarResolverTest
    {
        [Test]
        public void Execute_ExistingVariable_CorrectEvaluation()
        {
            var globalVariables = new Dictionary<string, ITestVariable>()
            {
                { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10*10"))) },
                { "otherVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10+10"))) }
            };
            var args = new GlobalVariableScalarResolverArgs("myVar", globalVariables, null);
            var resolver = new GlobalVariableScalarResolver<int>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(100));
        }

        [Test]
        public void Execute_ExistingVariableWrongType_CorrectEvaluation()
        {
            var globalVariables = new Dictionary<string, ITestVariable>()
            {
                { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("(10*10).ToString()"))) },
                { "otherVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10+10"))) }
            };
            var args = new GlobalVariableScalarResolverArgs("myVar", globalVariables, null);
            var resolver = new GlobalVariableScalarResolver<int>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(100));
        }

        [Test]
        public void Execute_ExistingVariableWrongTypeDateTime_CorrectEvaluation()
        {
            var globalVariables = new Dictionary<string, ITestVariable>()
            {
                { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("\"2017-05-12\""))) },
                { "otherVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10+10"))) }
            };
            var args = new GlobalVariableScalarResolverArgs("myVar", globalVariables, null);
            var resolver = new GlobalVariableScalarResolver<DateTime>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(new DateTime(2017,5,12)));
        }

        [Test]
        public void Execute_ExistingVariableAndTransformation_CorrectEvaluation()
        {
            var globalVariables = new Dictionary<string, ITestVariable>()
            {
                { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10*10+0.0001"))) },
            };

            var transformation = new LightTransformXml()
            {
                OriginalType = ColumnType.Numeric,
                Language = LanguageType.Native,
                Code = "numeric-to-floor",
            };

            var transformer = new TransformerFactory().Instantiate(transformation);
            transformer.Initialize(transformation.Code);
            var transformers = new List<ITransformer>() { transformer };

            var args = new GlobalVariableScalarResolverArgs("myVar", globalVariables, transformers);
            var resolver = new GlobalVariableScalarResolver<int>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(100));
        }
    }
}
