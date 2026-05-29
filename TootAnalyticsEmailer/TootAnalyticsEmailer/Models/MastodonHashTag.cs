// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// All properties must have public getters and setters for serialisation to work
namespace TootAnalyticsEmailer.Models;

internal abstract class MastodonHashTag
{
    public string Name { get; set; } = string.Empty;
}