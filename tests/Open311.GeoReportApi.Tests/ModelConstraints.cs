namespace Open311.GeoReportApi.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using Newtonsoft.Json.Serialization;
    using Xunit;

    public class ModelConstraints
    {
        [Fact]
        public void AllPropertiesWithValidationAttributesMustBeDecoratedWithADisplayAttribute()
        {
            // DataAnnotations, when validated are adding validation errors to a model state.
            // By default, the property name is copied verbatim, but the api expects snake_case
            // formatting.  This test ensure all validations are also backed by a DisplayAttribute.
            var modelTypes = typeof(Open311Constants).Assembly.GetTypes()
                .Where(t => t.Namespace.EndsWith("Models"));

            var typesWithValidationAttributes = modelTypes.Select(t => new
            {
                t.Name,
                Properties = t.GetProperties()
                    .Where(tprop => tprop.GetCustomAttributes(typeof(ValidationAttribute), true).Any())
            });

            var snakeCase = new SnakeCaseNamingStrategy(true, false);
            var errorMessage = new StringBuilder();

            foreach (var type in typesWithValidationAttributes)
            {
                foreach (var prop in type.Properties)
                {
                    var display =
                        prop.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;

                    var expected = snakeCase.GetPropertyName(prop.Name, false);
                    var actual = display?.Name;

                    if (expected != actual)
                    {
                        errorMessage.AppendLine(
                            $"{type.Name}.{prop.Name} must be decorated with a [Display(Name = \"{expected}\").");
                    }
                }
            }

            if (errorMessage.Length > 0)
            {
                throw new Exception(errorMessage.ToString());
            }
        }

        [Fact]
        public void AllModelsMustBeSerializableByDataContractSerializer()
        {
            var modelTypes = typeof(Open311Constants).Assembly.GetTypes()
                .Where(t => t.Namespace.EndsWith("Models"))
                .Where(t => !t.GetCustomAttributes(typeof(DataContractAttribute), true).Any());

            var errorMessage = new StringBuilder();

            foreach (var type in modelTypes)
            {
                errorMessage.AppendLine($"{type.Name} must be decorated with [DataContract].");
            }

            if (errorMessage.Length > 0)
            {
                throw new Exception(errorMessage.ToString());
            }
        }

        [Fact]
        public void AllModelsWithDataContractAttributesMustDefineMemberNamesAsSnakeCase()
        {
            var modelTypes = typeof(Open311Constants).Assembly.GetTypes()
                .Where(t => t.Namespace.EndsWith("Models"))
                .Where(t => t.GetCustomAttributes(typeof(DataContractAttribute), true).Any());

            var typesWithValidationAttributes = modelTypes.Select(t => new
            {
                t.Name,
                Properties = t.GetProperties()
                    .Where(tprop => tprop.GetCustomAttributes(typeof(DataMemberAttribute), true).Any())
            });

            var snakeCase = new SnakeCaseNamingStrategy(true, false);
            var errorMessage = new StringBuilder();

            foreach (var type in typesWithValidationAttributes)
            {
                foreach (var prop in type.Properties)
                {
                    var member =
                        prop.GetCustomAttributes(typeof(DataMemberAttribute), true).FirstOrDefault() as DataMemberAttribute;

                    var expected = snakeCase.GetPropertyName(prop.Name, false);
                    var actual = member?.Name;

                    if (expected != actual)
                    {
                        errorMessage.AppendLine(
                            $"{type.Name}.{prop.Name} must be decorated with a [DataMember(Name = \"{expected}\").");
                    }
                }
            }

            if (errorMessage.Length > 0)
            {
                throw new Exception(errorMessage.ToString());
            }
        }
    }
}
