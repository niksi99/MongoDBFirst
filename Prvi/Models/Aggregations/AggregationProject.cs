using Prvi.Models.Entities;
using System.Text.Json.Serialization;

namespace Prvi.Models.Aggregations
{
    public class AggregationProject
    {
        //[JsonPropertyName("plata")]
        public string stringAggProj { get; set; } = String.Empty;
        [JsonPropertyName("zaposleni")]
        public Employee? employeeAgg { get; set; }
    }
}
