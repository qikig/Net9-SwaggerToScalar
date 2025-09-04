using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication9
{
    public class WeatherForecast
    {
        [property:Description("��������")]
        public DateOnly Date { get; set; }
        [property: Required]
        [property: DefaultValue(0)]
        [property: Description("�¶�")]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Summary { get; set; }
    }
}
