// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ClassNeverInstantiated.Global
// All properties must have public getters and setters, and all classes must be concrete, for serialisation to work
namespace TootAnalyticsEmailer.Models;

internal class MastodonHashTag
{
    public string Name { get; set; } = string.Empty;
}