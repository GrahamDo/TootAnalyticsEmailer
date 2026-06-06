// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// All properties must have public getters and setters for serialisation to work

using System.Text.Json.Serialization;

namespace TootAnalyticsEmailer.Models;

internal class MastodonId
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}