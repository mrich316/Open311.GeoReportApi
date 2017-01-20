namespace Open311.GeoReportApi.Tests
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Models;
    using Newtonsoft.Json.Serialization;
    using Xunit;

    public class XmlSerializationConstraints
    {
        [Theory, TestConventions]
        public async Task Error(Error sut)
        {
            var expected = $@"<error xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <code>{sut.Code}</code>
  <description>{sut.Description}</description>
</error>";

            var actual = await Serialize(sut);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public async Task Service(Service sut)
        {
            var expected = $@"<service xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <description>{sut.Description}</description>
  <group>{sut.Group}</group>
  <keywords>{string.Join(",", sut.Keywords)}</keywords>
  <metadata>{sut.Metadata.ToString().ToLowerInvariant()}</metadata>
  <service_code>{sut.ServiceCode}</service_code>
  <service_name>{sut.ServiceName}</service_name>
  <type>{sut.Type.ToString().ToLowerInvariant()}</type>
</service>";

            var actual = await Serialize(sut);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public async Task Services(Service sut)
        {
            var expected = $@"<services xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <service>
    <description>{sut.Description}</description>
    <group>{sut.Group}</group>
    <keywords>{string.Join(",", sut.Keywords)}</keywords>
    <metadata>{sut.Metadata.ToString().ToLowerInvariant()}</metadata>
    <service_code>{sut.ServiceCode}</service_code>
    <service_name>{sut.ServiceName}</service_name>
    <type>{sut.Type.ToString().ToLowerInvariant()}</type>
  </service>
</services>";

            var actual = await Serialize(new Services(sut));

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public async Task ServiceAttributeValue(ServiceAttributeValue sut)
        {
            var expected = $@"<value xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <key>{sut.Key}</key>
  <name>{sut.Name}</name>
</value>";

            var actual = await Serialize(sut);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public async Task ServiceAttribute(ServiceAttribute sut, ServiceAttributeValue attributeValue)
        {
            // test with only one attribute value.
            sut.Values.Clear();
            sut.Values.Add(attributeValue);

            var expected = $@"<attribute xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <code>{sut.Code}</code>
  <datatype>{sut.Datatype.ToString().ToLowerInvariant()}</datatype>
  <datatype_description>{sut.DatatypeDescription}</datatype_description>
  <description>{sut.Description}</description>
  <order>{sut.Order}</order>
  <required>{sut.Required.ToString().ToLowerInvariant()}</required>
  <values>
    <value>
      <key>{attributeValue.Key}</key>
      <name>{attributeValue.Name}</name>
    </value>
  </values>
  <variable>{sut.Variable.ToString().ToLowerInvariant()}</variable>
</attribute>";

            var actual = await Serialize(sut);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public async Task ServiceDefinition(ServiceDefinition sut, ServiceAttribute attribute, ServiceAttributeValue attributeValue)
        {
            // Only test a single attribute.
            sut.Attributes.Clear();
            sut.Attributes.Add(attribute);

            // with a single value.
            attribute.Values.Clear();
            attribute.Values.Add(attributeValue);

            var expected = $@"<service_definition xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <attributes>
    <attribute>
      <code>{attribute.Code}</code>
      <datatype>{attribute.Datatype.ToString().ToLowerInvariant()}</datatype>
      <datatype_description>{attribute.DatatypeDescription}</datatype_description>
      <description>{attribute.Description}</description>
      <order>{attribute.Order}</order>
      <required>{attribute.Required.ToString().ToLowerInvariant()}</required>
      <values>
        <value>
          <key>{attributeValue.Key}</key>
          <name>{attributeValue.Name}</name>
        </value>
      </values>
      <variable>{attribute.Variable.ToString().ToLowerInvariant()}</variable>
    </attribute>
  </attributes>
  <service_code>{sut.ServiceCode}</service_code>
</service_definition>";

            var actual = await Serialize(sut);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public async Task ServiceRequestCreated(ServiceRequestCreated sut)
        {
            var expected = $@"<request xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <account_id>{sut.AccountId}</account_id>
  <service_notice>{sut.ServiceNotice}</service_notice>
  <service_request_id>{sut.ServiceRequestId}</service_request_id>
  <token>{sut.Token}</token>
</request>";

            var actual = await Serialize(sut);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public async Task ServiceRequestsOfServiceRequestCreated(ServiceRequestCreated sut)
        {
            var expected = $@"<service_requests xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <request>
    <account_id>{sut.AccountId}</account_id>
    <service_notice>{sut.ServiceNotice}</service_notice>
    <service_request_id>{sut.ServiceRequestId}</service_request_id>
    <token>{sut.Token}</token>
  </request>
</service_requests>";

            var actual = await Serialize(new ServiceRequests<ServiceRequestCreated>(sut));

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public async Task ServiceRequestToken(ServiceRequestToken sut)
        {
            var expected = $@"<request xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <service_request_id>{sut.ServiceRequestId}</service_request_id>
  <token>{sut.Token}</token>
</request>";

            var actual = await Serialize(sut);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public async Task ServiceRequestsOfServiceRequestToken(ServiceRequestToken sut)
        {
            var expected = $@"<service_requests xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <request>
    <service_request_id>{sut.ServiceRequestId}</service_request_id>
    <token>{sut.Token}</token>
  </request>
</service_requests>";

            var actual = await Serialize(new ServiceRequests<ServiceRequestToken>(sut));

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public async Task ServiceRequest(ServiceRequest sut, SnakeCaseNamingStrategy snakeCase)
        {
            // Autofixture creates a uri with scheme+host only and JSON.NET strips the last slash on this condition.
            sut.MediaUrl = new Uri(sut.MediaUrl, "/blah/");

            var expected = $@"<request xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <address>{sut.Address}</address>
  <address_id>{sut.AddressId}</address_id>
  <agency_responsible>{sut.AgencyResponsible}</agency_responsible>
  <description>{sut.Description}</description>
  <expected_datetime>{sut.ExpectedDatetime:o}</expected_datetime>
  <lat>{sut.Lat}</lat>
  <long>{sut.Long}</long>
  <media_url>{sut.MediaUrl}</media_url>
  <requested_datetime>{sut.RequestedDatetime:o}</requested_datetime>
  <service_code>{sut.ServiceCode}</service_code>
  <service_name>{sut.ServiceName}</service_name>
  <service_notice>{sut.ServiceNotice}</service_notice>
  <service_request_id>{sut.ServiceRequestId}</service_request_id>
  <status>{snakeCase.GetPropertyName(sut.Status.ToString(), false)}</status>
  <status_notes>{sut.StatusNotes}</status_notes>
  <updated_datetime>{sut.UpdatedDatetime:o}</updated_datetime>
  <zipcode>{sut.Zipcode}</zipcode>
</request>";

            var actual = await Serialize(sut);

            Assert.Equal(expected, actual);
        }

        [Theory, TestConventions]
        public async Task ServiceRequestsOfServiceRequest(ServiceRequest sut, SnakeCaseNamingStrategy snakeCase)
        {
            // Autofixture creates a uri with scheme+host only and JSON.NET strips the last slash on this condition.
            sut.MediaUrl = new Uri(sut.MediaUrl, "/blah/");

            var expected = $@"<service_requests xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <request>
    <address>{sut.Address}</address>
    <address_id>{sut.AddressId}</address_id>
    <agency_responsible>{sut.AgencyResponsible}</agency_responsible>
    <description>{sut.Description}</description>
    <expected_datetime>{sut.ExpectedDatetime:o}</expected_datetime>
    <lat>{sut.Lat}</lat>
    <long>{sut.Long}</long>
    <media_url>{sut.MediaUrl}</media_url>
    <requested_datetime>{sut.RequestedDatetime:o}</requested_datetime>
    <service_code>{sut.ServiceCode}</service_code>
    <service_name>{sut.ServiceName}</service_name>
    <service_notice>{sut.ServiceNotice}</service_notice>
    <service_request_id>{sut.ServiceRequestId}</service_request_id>
    <status>{snakeCase.GetPropertyName(sut.Status.ToString(), false)}</status>
    <status_notes>{sut.StatusNotes}</status_notes>
    <updated_datetime>{sut.UpdatedDatetime:o}</updated_datetime>
    <zipcode>{sut.Zipcode}</zipcode>
  </request>
</service_requests>";

            var actual = await Serialize(new ServiceRequests<ServiceRequest>(sut));

            Assert.Equal(expected, actual);
        }

        private async Task<string> Serialize<T>(T value)
        {
            var serializer = new Startup().CreateXmlSerializerOutputFormatter();

            // override Startup to simplify testing.
            serializer.WriterSettings.Indent = true;
            serializer.WriterSettings.IndentChars = "  ";
            serializer.WriterSettings.OmitXmlDeclaration = true;

            var xml = new StringBuilder();

            var context = new OutputFormatterWriteContext(
                new DefaultHttpContext(),
                (stream, encoding) => new StringWriter(xml),
                typeof(T), value);

            await serializer.WriteResponseBodyAsync(context, Encoding.UTF8);

            return xml.ToString();
        }
    }
}
