namespace Open311.GeoReportApi.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = Open311Constants.ModelProperties.ServiceAttributeValue,
        Namespace = Open311Constants.DefaultNamespace)]
    public struct ServiceAttributeValue : IEquatable<ServiceAttributeValue>
    {
        private readonly string _key;

        public ServiceAttributeValue(string key, string value = null)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            Name = value;
        }

        [DataMember(Name = Open311Constants.ModelProperties.Key)]
        public string Key
        {
            get { return _key; }

#if NETSTANDARD
            internal set
            {
                throw new NotSupportedException(
                    "Enabled only because DataContractSerializer does not honor SerializeReadOnlyTypes in netstandard, see https://github.com/mrich316/Open311.GeoReportApi/issues/1");
            }
#endif
        }

        [DataMember(Name = Open311Constants.ModelProperties.Name)]
        public string Name { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                return Key.GetHashCode() * 397;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;

            return obj.GetType() == GetType()
                   && Equals((ServiceAttributeValue) obj);
        }

        public bool Equals(ServiceAttributeValue other)
        {
            return string.Equals(Key, other.Key);
        }

        public override string ToString()
        {
            return Key;
        }
    }
}
