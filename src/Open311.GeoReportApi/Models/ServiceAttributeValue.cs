namespace Open311.GeoReportApi.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = Open311Constants.ModelProperties.ServiceAttributeValue, Namespace = Open311Constants.DefaultNamespace)]
    public struct ServiceAttributeValue : IEquatable<ServiceAttributeValue>
    {
        public ServiceAttributeValue(string key, string value = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            Key = key;
            Value = value;
        }

        [DataMember(Name = Open311Constants.ModelProperties.Key)]
        public string Key { get; }

        [DataMember(Name = Open311Constants.ModelProperties.Value)]
        public string Value { get; set; }

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
