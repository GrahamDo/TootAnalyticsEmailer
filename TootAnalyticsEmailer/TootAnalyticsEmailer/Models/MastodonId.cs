// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// All properties must have public getters and setters for serialisation to work
namespace TootAnalyticsEmailer.Models;

internal class MastodonId
{
    public string Id { get; set; } = string.Empty;
}