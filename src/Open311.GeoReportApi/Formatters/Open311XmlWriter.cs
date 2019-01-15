using System;
using System.Threading.Tasks;
using System.Xml;

namespace Open311.GeoReportApi.Formatters
{
    /// <summary>
    /// Delegating <see cref="XmlWriter"/> removing xmlns attributes.
    /// </summary>
    internal class Open311XmlWriter : XmlWriter
    {
        private readonly XmlWriter _innerWriter;
        private bool _skipAttribute;

        public Open311XmlWriter(XmlWriter inner)
        {
            _innerWriter = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public override void Close()
        {
            _innerWriter.Close();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing)
            {
                _innerWriter.Dispose();
            }
        }

        public override void Flush()
        {
            _innerWriter.Flush();
        }

        public override Task FlushAsync()
        {
            return _innerWriter.FlushAsync();
        }

        public override string LookupPrefix(string ns)
        {
            return _innerWriter.LookupPrefix(ns);
        }

        public override XmlWriterSettings Settings => _innerWriter.Settings;

        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            _innerWriter.WriteBase64(buffer, index, count);
        }

        public override void WriteCData(string text)
        {
            _innerWriter.WriteCData(text);
        }

        public override void WriteCharEntity(char ch)
        {
            _innerWriter.WriteCharEntity(ch);
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            _innerWriter.WriteChars(buffer, index, count);
        }

        public override void WriteComment(string text)
        {
            _innerWriter.WriteComment(text);
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            _innerWriter.WriteDocType(name, pubid, sysid, subset);
        }

        public override void WriteEndAttribute()
        {
            if (_skipAttribute)
            {
                _skipAttribute = false;
            }
            else
            {
                _innerWriter.WriteEndAttribute();
            }
        }

        public override void WriteEndDocument()
        {
            _innerWriter.WriteEndDocument();
        }

        public override void WriteEndElement()
        {
            _innerWriter.WriteEndElement();
        }

        public override void WriteEntityRef(string name)
        {
            _innerWriter.WriteEntityRef(name);
        }

        public override void WriteFullEndElement()
        {
            _innerWriter.WriteFullEndElement();
        }

        public override void WriteProcessingInstruction(string name, string text)
        {
            _innerWriter.WriteProcessingInstruction(name, text);
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
            _innerWriter.WriteRaw(buffer, index, count);
        }

        public override void WriteRaw(string data)
        {
            _innerWriter.WriteRaw(data);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            if (prefix == "xmlns")
            {
                _skipAttribute = true;
            }
            else
            {
                _innerWriter.WriteStartAttribute(prefix, localName, ns);
            }
        }

        public override void WriteStartDocument()
        {
            _innerWriter.WriteStartDocument();
        }

        public override void WriteStartDocument(bool standalone)
        {
            _innerWriter.WriteStartDocument(standalone);
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            _innerWriter.WriteStartElement(prefix, localName, ns);
        }

        public override void WriteString(string text)
        {
            if (_skipAttribute) return;

            _innerWriter.WriteString(text);
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            _innerWriter.WriteSurrogateCharEntity(lowChar, highChar);
        }

        public override void WriteWhitespace(string ws)
        {
            _innerWriter.WriteWhitespace(ws);
        }

        public override WriteState WriteState => _innerWriter.WriteState;
    }
}
