using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication9
{
    public class WeatherForecast
    {
        [property:Description("创建日期")]
        public DateOnly Date { get; set; }
        [property: Required]
        [property: DefaultValue(0)]
        [property: Description("温度")]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
