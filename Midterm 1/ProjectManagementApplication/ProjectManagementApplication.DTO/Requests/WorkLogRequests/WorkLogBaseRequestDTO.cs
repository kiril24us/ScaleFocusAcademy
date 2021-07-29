using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ProjectManagementApplication.DTO.Requests.WorkLogRequests
{
    public abstract class WorkLogBaseRequestDTO
    {
        [Required]
        [Range(typeof(TimeSpan), "00:00", "23:59", ErrorMessage = "Time Span must be between 00:00 and 23:59")]
        [System.Text.Json.Serialization.JsonConverterAttribute(typeof(TimeSpanConverter))]
        public TimeSpan TimeSpent { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TaskId { get; set; }
    }

    public class TimeSpanConverter : System.Text.Json.Serialization.JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeSpan.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
