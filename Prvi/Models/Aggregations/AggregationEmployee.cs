using Prvi.Models.Entities;
using System.Text.Json.Serialization;

namespace Prvi.Models.Aggregations
{
    public class AggregationEmployee
    {
        [JsonPropertyName("plata")]
        public int intAgg { get; set; }
        [JsonPropertyName("zaposleni")]
        public Employee? employeeAgg { get; set; }
    }
}
