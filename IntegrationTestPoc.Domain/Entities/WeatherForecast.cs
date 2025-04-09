namespace IntegrationTestPoc.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("WeatherForecast")]
public class WeatherForecast
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column(TypeName = "int")]
    public int TemperatureC { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? Summary { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? SecondaryId { get; set; }

    [NotMapped]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
