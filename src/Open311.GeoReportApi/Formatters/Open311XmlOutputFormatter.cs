using System.IO;
using System.Xml;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;

namespace Open311.GeoReportApi.Formatters
{
    public class Open311XmlOutputFormatter : XmlDataContractSerializerOutputFormatter
    {
        public Open311XmlOutputFormatter(ILoggerFactory loggerFactory = null)
            : base(new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            OmitXmlDeclaration = true
        }, loggerFactory)
        {
            SerializerSettings.SerializeReadOnlyTypes = true;
        }

        public override XmlWriter CreateXmlWriter(TextWriter writer, XmlWriterSettings xmlWriterSettings)
        {
            var innerWriter = base.CreateXmlWriter(writer, xmlWriterSettings);
            return new Open311XmlWriter(innerWriter);
        }
    }
}
