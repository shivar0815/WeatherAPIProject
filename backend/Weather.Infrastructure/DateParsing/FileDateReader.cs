using Weather.Application.Services;
using System.Globalization;

namespace Weather.Infrastructure.DateParsing;

public class FileDateReader : IDateReader
{
    private readonly string[] _dateFormats = new[]
    {
        "MM/dd/yyyy", "MMMM d, yyyy", "MMM-dd-yyyy", "yyyy-MM-dd", "M/d/yyyy", "d/M/yyyy"             
    };

    public async Task<List<DateParseResult>> ReadDatesAsync(string filePath)
    {
        var results = new List<DateParseResult>();

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Date file not found: {filePath}");
        }

        var lines = await File.ReadAllLinesAsync(filePath);

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            
            if (string.IsNullOrWhiteSpace(trimmedLine))
                continue;

            var parseResult = TryParseDate(trimmedLine);
            results.Add(parseResult);
        }

        return results;
    }

    private DateParseResult TryParseDate(string dateString)
    {
        var result = new DateParseResult
        {
            OriginalValue = dateString
        };

        foreach (var format in _dateFormats)
        {
            if (DateTime.TryParseExact(
                dateString,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsedDate))
            {
                if (IsValidCalendarDate(parsedDate, dateString))
                {
                    result.ParsedDate = parsedDate;
                    return result;
                }
                else
                {
                    result.ErrorMessage = "Invalid calendar date (e.g., April 31 does not exist)";
                    return result;
                }
            }
        }

        result.ErrorMessage = "Unrecognized date format";
        return result;
    }

    private bool IsValidCalendarDate(DateTime date, string original)
    {
        try
        {
            var reconstructed = new DateTime(date.Year, date.Month, date.Day);
            
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            
            return date.Day <= daysInMonth;
        }
        catch
        {
            return false;
        }
    }
}