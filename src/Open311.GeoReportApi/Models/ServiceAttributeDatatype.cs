namespace Open311.GeoReportApi.Models
{
    /// <summary>
    /// Denotes the type of field used for user input.
    /// </summary>
    public enum ServiceAttributeDatatype
    {
        /// <summary>
        ///  A string of characters without line breaks. Represented in an HTML form using an &lt;input&gt; tag
        /// </summary>
        String,

        /// <summary>
        /// A numeric value. Represented in an HTML form using an &lt;input&gt; tag
        /// </summary>
        Number,

        /// <summary>
        /// The input generated must be able to transform into a valid ISO 8601 date.
        /// Represented in an HTML form using &lt;input&gt; tags
        /// </summary>
        Datetime,

        /// <summary>
        /// A string of characters that may contain line breaks.
        /// Represented in an HTML form using an &lt;textarea&gt; tag
        /// </summary>
        Text,

        /// <summary>
        /// A set of predefined values (specified in this response) where only one value may be selected.
        /// Represented in an HTML form using the &lt;select&gt; and &lt;option&gt; tags
        /// </summary>
        Singlevaluelist,

        /// <summary>
        /// A set of predefined values (specified in this response) where several values may be selected.
        /// Represented in an HTML form using the &lt;select multiple="multiple"&gt; and &lt;option&gt; tags
        /// </summary>
        Multivaluelist
    }
}
