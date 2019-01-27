namespace Open311.GeoReportApi
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public static class Open311Options
    {
        public static string DefaultJurisdictionId = "city.sdk";

        internal static void SetupJsonOptions(MvcJsonOptions options)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(true, false)
            };

            options.SerializerSettings.ContractResolver = contractResolver;
            options.SerializerSettings.Formatting = Formatting.Indented;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.Converters.Add(new StringEnumConverter(true));
        }
    }
}
