namespace Open311.GeoReportApi.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;
    using Models;
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
                            $"{type.Name}.{prop.Name} must be decorated with a [Display(Name = \"{expected}\")].");
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
                .Where(t => t.Namespace.EndsWith(".Models")) // Skip InputModels.
                .Where(t => !t.GetCustomAttributes(typeof(DataContractAttribute), true).Any()
                            && !t.GetCustomAttributes(typeof(CollectionDataContractAttribute), true).Any());

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
                .Where(t => t.GetCustomAttributes(typeof(DataContractAttribute), true).Any()
                            || t.GetCustomAttributes(typeof(CollectionDataContractAttribute), true).Any());

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
                            $"{type.Name}.{prop.Name} must be decorated with a [DataMember(Name = \"{expected}\")].");
                    }
                }
            }

            if (errorMessage.Length > 0)
            {
                throw new Exception(errorMessage.ToString());
            }
        }

        [Fact]
        public void AllModelsWithDataContractAttributesMustDefineEnumValuesAsSnakeCase()
        {
            var modelTypes = typeof(Open311Constants).Assembly.GetTypes()
                .Where(t => t.Namespace.EndsWith("Models") && t.IsEnum)
                .Where(t => t.GetCustomAttributes(typeof(DataContractAttribute), true).Any());

            var enumTypes = modelTypes.Select(t => new
            {
                t.Name,
                EnumValues = t.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public)
            });

            var snakeCase = new SnakeCaseNamingStrategy(true, false);
            var errorMessage = new StringBuilder();

            foreach (var type in enumTypes)
            {
                foreach (var prop in type.EnumValues)
                {
                    var member =
                        prop.GetCustomAttributes(typeof(EnumMemberAttribute), true).FirstOrDefault() as EnumMemberAttribute;

                    var expected = snakeCase.GetPropertyName(prop.Name, false);
                    var actual = member?.Value;

                    if (expected != actual)
                    {
                        errorMessage.AppendLine(
                            $"{type.Name}.{prop.Name} must be decorated with a [EnumMember(Value = \"{expected}\")].");
                    }
                }
            }

            if (errorMessage.Length > 0)
            {
                throw new Exception(errorMessage.ToString());
            }
        }

        [Fact]
        public void AllModelsWithCollectionDataContractAttributeAttributesMustDefineNamespace()
        {
            var modelTypes = typeof(Open311Constants).Assembly.GetTypes()
                .Select(t => new
                {
                    t.Namespace,
                    t.Name,
                    DataContract = t.GetCustomAttributes(typeof(CollectionDataContractAttribute), true).FirstOrDefault() as CollectionDataContractAttribute
                })
                .Where(t => t.Namespace.EndsWith("Models") && t.DataContract != null);

            var errorMessage = new StringBuilder();

            foreach (var type in modelTypes)
            {
                if (type.DataContract.Namespace != Open311Constants.DefaultNamespace)
                {
                    errorMessage.AppendLine(
                        $"{type.Name} must be decorated with a namespace, ex: [DataContract(Namespace = Open311Constants.DefaultNamespace)].");
                }
            }

            if (errorMessage.Length > 0)
            {
                throw new Exception(errorMessage.ToString());
            }
        }

        [Fact]
        public void AllModelsWithDataContractAttributesMustDefineNamespace()
        {
            var modelTypes = typeof(Open311Constants).Assembly.GetTypes()
                .Select(t => new
                {
                    t.Namespace,
                    t.Name,
                    DataContract = t.GetCustomAttributes(typeof(DataContractAttribute), true).FirstOrDefault() as DataContractAttribute
                })
                .Where(t => t.Namespace.EndsWith("Models") && t.DataContract != null);

            var errorMessage = new StringBuilder();

            foreach (var type in modelTypes)
            {
                if (type.DataContract.Namespace != Open311Constants.DefaultNamespace)
                {
                    errorMessage.AppendLine(
                        $"{type.Name} must be decorated with a namespace, ex: [DataContract(Namespace = Open311Constants.DefaultNamespace)].");
                }
            }

            if (errorMessage.Length > 0)
            {
                throw new Exception(errorMessage.ToString());
            }
        }

        [Fact]
        public void AllModelsWithDataContractAttributesMustDefineSnakeCaseName()
        {
            var modelTypes = typeof(Open311Constants).Assembly.GetTypes()
                .Select(t => new
                {
                    t.Namespace,
                    t.Name,
                    DataContract = t.GetCustomAttributes(typeof(DataContractAttribute), true).FirstOrDefault() as DataContractAttribute
                })
                .Where(t => t.Namespace.EndsWith("Models") && t.DataContract != null);

            var errorMessage = new StringBuilder();
            var snakeCase = new SnakeCaseNamingStrategy(true, false);

            foreach (var type in modelTypes)
            {
                var name = type.Name;

                // exceptions:
                if (typeof(ServiceAttribute).Name == name) name = "Attribute";
                else if (typeof(ServiceRequest).Name == name) name = "Request";
                else if (typeof(ServiceRequestCreated).Name == name) name = "Request";
                else if (typeof(ServiceRequestToken).Name == name) name = "Request";
                else if (typeof(ServiceRequestStatus).Name == name) name = "Status";
                else if (typeof(ServiceType).Name == name) name = "type";

                var expected = snakeCase.GetPropertyName(name, false);

                if (type.DataContract.Name != expected)
                {
                    errorMessage.AppendLine(
                        $"{type.Name} must be decorated with a snake_case name, ex: [DataContract(Name = \"{expected}\")].");
                }
            }

            if (errorMessage.Length > 0)
            {
                throw new Exception(errorMessage.ToString());
            }
        }

        [Fact]
        public void AllModelsWithCollectionDataContractAttributesMustDefineSnakeCaseName()
        {
            var modelTypes = typeof(Open311Constants).Assembly.GetTypes()
                .Select(t => new
                {
                    t.Namespace,
                    t.Name,
                    DataContract = t.GetCustomAttributes(typeof(CollectionDataContractAttribute), true).FirstOrDefault() as CollectionDataContractAttribute
                })
                .Where(t => t.Namespace.EndsWith("Models") && t.DataContract != null);

            var errorMessage = new StringBuilder();
            var snakeCase = new SnakeCaseNamingStrategy(true, false);

            foreach (var type in modelTypes)
            {
                var name = type.Name;

                // exceptions:
                if (typeof(ServiceAttributes).Name == name) name = "Attributes";
                else if (typeof(ServiceRequests<>).Name == name) name = "ServiceRequests";

                var expected = snakeCase.GetPropertyName(name, false);
                if (type.DataContract.Name != expected)
                {
                    errorMessage.AppendLine(
                        $"{type.Name} must be decorated with a snake_case name, ex: [DataContract(Name = \"{expected}\")].");
                }
            }

            if (errorMessage.Length > 0)
            {
                throw new Exception(errorMessage.ToString());
            }
        }
    }
}
