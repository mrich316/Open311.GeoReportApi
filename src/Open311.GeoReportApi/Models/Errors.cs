namespace Open311.GeoReportApi.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [CollectionDataContract(Name = "errors", ItemName = "error")]
    public class Errors : List<Error>
    {
        public Errors()
        {
        }

        public Errors(int capacity)
            : base(capacity)
        {
        }

        public Errors(IEnumerable<Error> errors)
            : base(errors)
        {
        }

        public Errors(params Error[] errors)
            : base(errors)
        {
        }

        public void Add(int code, string description)
        {
            Add(new Error {Code = code, Description = description});
        }
    }
}
