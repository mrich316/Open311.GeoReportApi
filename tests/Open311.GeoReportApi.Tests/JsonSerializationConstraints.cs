namespace Open311.GeoReportApi.Tests
{
    using System;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Xunit;

    public class JsonSerializationConstraints
    {
        [Theory, TestConventions]
        public void Error(JsonSerializerSettings serializerSettings, Error sut)
        {
            var expected = $@"{{
  ""code"": {sut.Code},
  ""description"": ""{sut.Description}""
}}";
            var actual = JsonConvert.SerializeObject(sut, serializerSettings);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public void Errors(JsonSerializerSettings serializerSettings, Error sut)
        {
            var expected = $@"[
  {{
    ""code"": {sut.Code},
    ""description"": ""{sut.Description}""
  }}
]";
            var actual = JsonConvert.SerializeObject(new Errors(sut), serializerSettings);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public void Service(JsonSerializerSettings serializerSettings, Service sut, SnakeCaseNamingStrategy snakeCase)
        {
            var expected = $@"{{
  ""service_code"": ""{sut.ServiceCode}"",
  ""service_name"": ""{sut.ServiceName}"",
  ""description"": ""{sut.Description}"",
  ""metadata"": {snakeCase.GetPropertyName(sut.Metadata.ToString(), false)},
  ""type"": ""{snakeCase.GetPropertyName(sut.Type.ToString(), false)}"",
  ""keywords"": ""{sut.Keywords}"",
  ""group"": ""{sut.Group}""
}}";
            var actual = JsonConvert.SerializeObject(sut, serializerSettings);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public void Services(JsonSerializerSettings serializerSettings, Service sut, SnakeCaseNamingStrategy snakeCase)
        {
            var expected = $@"[
  {{
    ""service_code"": ""{sut.ServiceCode}"",
    ""service_name"": ""{sut.ServiceName}"",
    ""description"": ""{sut.Description}"",
    ""metadata"": {snakeCase.GetPropertyName(sut.Metadata.ToString(), false)},
    ""type"": ""{snakeCase.GetPropertyName(sut.Type.ToString(), false)}"",
    ""keywords"": ""{sut.Keywords}"",
    ""group"": ""{sut.Group}""
  }}
]";
            var actual = JsonConvert.SerializeObject(new Services(sut), serializerSettings);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public void ServiceAttributeValue(JsonSerializerSettings serializerSettings,
            ServiceAttributeValue sut, SnakeCaseNamingStrategy snakeCase)
        {
            var expected = $@"{{
  ""key"": ""{sut.Key}"",
  ""value"": ""{sut.Value}""
}}";
            var actual = JsonConvert.SerializeObject(sut, serializerSettings);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public void ServiceAttribute(JsonSerializerSettings serializerSettings, ServiceAttribute sut,
            ServiceAttributeValue attributeValue, SnakeCaseNamingStrategy snakeCase)
        {
            // with a single value.
            sut.Values.Clear();
            sut.Values.Add(attributeValue);

            var expected = $@"{{
  ""variable"": {snakeCase.GetPropertyName(sut.Variable.ToString(), false)},
  ""code"": ""{sut.Code}"",
  ""datatype"": ""{snakeCase.GetPropertyName(sut.Datatype.ToString(), false)}"",
  ""required"": {snakeCase.GetPropertyName(sut.Required.ToString(), false)},
  ""datatype_description"": ""{sut.DatatypeDescription}"",
  ""order"": {sut.Order},
  ""description"": ""{sut.Description}"",
  ""values"": [
    {{
      ""key"": ""{attributeValue.Key}"",
      ""value"": ""{attributeValue.Value}""
    }}
  ]
}}";
            var actual = JsonConvert.SerializeObject(sut, serializerSettings);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public void ServiceDefinition(JsonSerializerSettings serializerSettings, ServiceDefinition sut,
            ServiceAttribute attribute, ServiceAttributeValue attributeValue, SnakeCaseNamingStrategy snakeCase)
        {
            // Only test a single attribute.
            sut.Attributes.Clear();
            sut.Attributes.Add(attribute);

            // with a single value.
            attribute.Values.Clear();
            attribute.Values.Add(attributeValue);

            var expected = $@"{{
  ""service_code"": ""{sut.ServiceCode}"",
  ""attributes"": [
    {{
      ""variable"": {snakeCase.GetPropertyName(attribute.Variable.ToString(), false)},
      ""code"": ""{attribute.Code}"",
      ""datatype"": ""{snakeCase.GetPropertyName(attribute.Datatype.ToString(), false)}"",
      ""required"": {snakeCase.GetPropertyName(attribute.Required.ToString(), false)},
      ""datatype_description"": ""{attribute.DatatypeDescription}"",
      ""order"": {attribute.Order},
      ""description"": ""{attribute.Description}"",
      ""values"": [
        {{
          ""key"": ""{attributeValue.Key}"",
          ""value"": ""{attributeValue.Value}""
        }}
      ]
    }}
  ]
}}";
            var actual = JsonConvert.SerializeObject(sut, serializerSettings);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public void ServiceRequestCreated(JsonSerializerSettings serializerSettings, ServiceRequestCreated sut,
            SnakeCaseNamingStrategy snakeCase)
        {
            var expected = $@"{{
  ""service_request_id"": ""{sut.ServiceRequestId}"",
  ""token"": ""{sut.Token}"",
  ""service_notice"": ""{sut.ServiceNotice}"",
  ""account_id"": ""{sut.AccountId}""
}}";
            var actual = JsonConvert.SerializeObject(sut, serializerSettings);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public void ServiceRequestsOfServiceRequestCreated(JsonSerializerSettings serializerSettings, ServiceRequestCreated sut,
            SnakeCaseNamingStrategy snakeCase)
        {
            var expected = $@"[
  {{
    ""service_request_id"": ""{sut.ServiceRequestId}"",
    ""token"": ""{sut.Token}"",
    ""service_notice"": ""{sut.ServiceNotice}"",
    ""account_id"": ""{sut.AccountId}""
  }}
]";
            var actual = JsonConvert.SerializeObject(new ServiceRequests<ServiceRequestCreated>(sut), serializerSettings);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public void ServiceRequestToken(JsonSerializerSettings serializerSettings, ServiceRequestToken sut,
            SnakeCaseNamingStrategy snakeCase)
        {
            var expected = $@"{{
  ""service_request_id"": ""{sut.ServiceRequestId}"",
  ""token"": ""{sut.Token}""
}}";
            var actual = JsonConvert.SerializeObject(sut, serializerSettings);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public void ServiceRequestsOfServiceRequestToken(JsonSerializerSettings serializerSettings, ServiceRequestToken sut,
            SnakeCaseNamingStrategy snakeCase)
        {
            var expected = $@"[
  {{
    ""service_request_id"": ""{sut.ServiceRequestId}"",
    ""token"": ""{sut.Token}""
  }}
]";
            var actual = JsonConvert.SerializeObject(new ServiceRequests<ServiceRequestToken>(sut), serializerSettings);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public void ServiceRequest(JsonSerializerSettings serializerSettings, ServiceRequest sut,
            SnakeCaseNamingStrategy snakeCase)
        {
            // Autofixture creates a uri with scheme+host only and JSON.NET strips the last slash on this condition.
            sut.MediaUrl = new Uri(sut.MediaUrl, "/blah/");

            // by default, JSON.NET adds ".0" to float and double values.
            // that's why you see lat/long with the format F1 (meaning 1 fixed decimal).

            var expected = $@"{{
  ""service_request_id"": ""{sut.ServiceRequestId}"",
  ""status"": ""{snakeCase.GetPropertyName(sut.Status.ToString(), false)}"",
  ""status_notes"": ""{sut.StatusNotes}"",
  ""service_name"": ""{sut.ServiceName}"",
  ""service_code"": ""{sut.ServiceCode}"",
  ""description"": ""{sut.Description}"",
  ""agency_responsible"": ""{sut.AgencyResponsible}"",
  ""service_notice"": ""{sut.ServiceNotice}"",
  ""requested_datetime"": {JsonConvert.SerializeObject(sut.RequestedDatetime, serializerSettings)},
  ""updated_datetime"": {JsonConvert.SerializeObject(sut.UpdatedDatetime, serializerSettings)},
  ""expected_datetime"": {JsonConvert.SerializeObject(sut.ExpectedDatetime, serializerSettings)},
  ""address"": ""{sut.Address}"",
  ""address_id"": ""{sut.AddressId}"",
  ""zipcode"": ""{sut.Zipcode}"",
  ""lat"": {sut.Lat:F1},
  ""long"": {sut.Long:F1},
  ""media_url"": ""{sut.MediaUrl}""
}}";
            var actual = JsonConvert.SerializeObject(sut, serializerSettings);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public void ServiceRequestsOfServiceRequest(JsonSerializerSettings serializerSettings, ServiceRequest sut,
            SnakeCaseNamingStrategy snakeCase)
        {
            // Autofixture creates a uri with scheme+host only and JSON.NET strips the last slash on this condition.
            sut.MediaUrl = new Uri(sut.MediaUrl, "/blah/");

            // by default, JSON.NET adds ".0" to float and double values.
            // that's why you see lat/long with the format F1 (meaning 1 fixed decimal).

            var expected = $@"[
  {{
    ""service_request_id"": ""{sut.ServiceRequestId}"",
    ""status"": ""{snakeCase.GetPropertyName(sut.Status.ToString(), false)}"",
    ""status_notes"": ""{sut.StatusNotes}"",
    ""service_name"": ""{sut.ServiceName}"",
    ""service_code"": ""{sut.ServiceCode}"",
    ""description"": ""{sut.Description}"",
    ""agency_responsible"": ""{sut.AgencyResponsible}"",
    ""service_notice"": ""{sut.ServiceNotice}"",
    ""requested_datetime"": {JsonConvert.SerializeObject(sut.RequestedDatetime, serializerSettings)},
    ""updated_datetime"": {JsonConvert.SerializeObject(sut.UpdatedDatetime, serializerSettings)},
    ""expected_datetime"": {JsonConvert.SerializeObject(sut.ExpectedDatetime, serializerSettings)},
    ""address"": ""{sut.Address}"",
    ""address_id"": ""{sut.AddressId}"",
    ""zipcode"": ""{sut.Zipcode}"",
    ""lat"": {sut.Lat:F1},
    ""long"": {sut.Long:F1},
    ""media_url"": ""{sut.MediaUrl}""
  }}
]";
            var actual = JsonConvert.SerializeObject(new ServiceRequests<ServiceRequest>(sut), serializerSettings);

            Assert.Equal(expected, actual);
        }

        // This test DateTimeOffset serializations, because DateTimeOffset.ToString("o")
        // can be zero-padded, but JSON.NET serialization removes this padding.
        //                                      ↓
        // Ex: Expected: ···2-28T06:37:23.1508190-05:00" // DateTimeOffset.ToString("o")
        //     Actual:   ···2-28T06:37:23.150819-05:00"  // JsonConvert.Serialize(...)
        //                                      ↑
        [Theory, TestConventions]
        public void SerializerSettingsHasIsoDate(JsonSerializerSettings serializerSettings)
        {
            Assert.Equal(DateTimeZoneHandling.RoundtripKind, serializerSettings.DateTimeZoneHandling);
            Assert.Equal(DateFormatHandling.IsoDateFormat, serializerSettings.DateFormatHandling);
        }
    }
}