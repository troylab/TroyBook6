using System;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace BookStore.Domain;

public class JPac
{
    static readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        //Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        //see https://docs.microsoft.com/en-us/dotnet/api/system.text.encodings.web.javascriptencoder.unsaferelaxedjsonescaping?view=net-6.0
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        //WriteIndented = true
    };

    static readonly JsonSerializerOptions optionsWithIndented = new JsonSerializerOptions
    {
        //Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    static JPac()
    {
    }

    public static string Serialize(object obj, bool writeIndented = false)
    {
        return JsonSerializer.Serialize(obj, writeIndented ? optionsWithIndented : options);
    }

    public static (bool Success, string JsonText) SerializeSafe(object obj, bool writeIndented = false)
    {
        try
        {
            var jsonText = JsonSerializer.Serialize(obj, writeIndented ? optionsWithIndented : options);
            return (true, jsonText);
        }
        catch { }

        return (false, "");
    }
    public static T? Deserialize<T>(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return default;

        return JsonSerializer.Deserialize<T>(json, options);
    }

    public static (bool success, T obj) DeserializeSafe<T>(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return (false, default!);

        try
        {
            return (true, JsonSerializer.Deserialize<T>(json, options))!;
        }
        catch { }

        return (false, default!);
    }

    public static bool TryConvertToJsonArray<T>(string text, out IEnumerable<T> array)
    {
        try
        {
            array = Deserialize<List<T>>(text) ?? new List<T>();
            return true;
        }
        catch { }

        array = new List<T>();
        return false;
    }

    public static bool TryDeserialize<T>(string text, out T result)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            result = default!;
            return false;
        }

        try
        {
            result = Deserialize<T>(text)!;
            return true;
        }
        catch { }

        result = default!;
        return false;
    }

    public static bool TryGetPropertyValue<T>(string? jsonText, string propertyName, out T result)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(jsonText))
            {
                result = default!;
                return false;
            }

            var jd = JsonDocument.Parse(jsonText);
            if (jd.RootElement.TryGetProperty(propertyName, out var value))
            {
                var valueString = value.ToString() ?? "";
                result = (T)Convert.ChangeType(valueString, typeof(T));
                return true;
            }
        }
        catch { }
        result = default!;
        return false;
    }
}

