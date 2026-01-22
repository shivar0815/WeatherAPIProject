namespace Weather.Application.Services;

public interface IDateReader
{
    Task<List<DateParseResult>> ReadDatesAsync(string filePath);
}

public class DateParseResult
{
    public string OriginalValue { get; set; } = string.Empty;
    public DateTime? ParsedDate { get; set; }
    public bool IsValid => ParsedDate.HasValue;
    public string? ErrorMessage { get; set; }
}