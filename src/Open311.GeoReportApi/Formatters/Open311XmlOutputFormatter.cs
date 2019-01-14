using System.Xml;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Open311.GeoReportApi.Formatters
{
    public class Open311XmlOutputFormatter : XmlDataContractSerializerOutputFormatter
    {
        public Open311XmlOutputFormatter() 
            : base(new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            OmitXmlDeclaration = true
        })
        {
            SerializerSettings.SerializeReadOnlyTypes = true;
        }
    }
}
