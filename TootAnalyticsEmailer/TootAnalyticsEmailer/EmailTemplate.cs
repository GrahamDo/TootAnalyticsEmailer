namespace TootAnalyticsEmailer;

internal class EmailTemplate
{
    private const string TemplateFileName = "email-template.txt";

    public static string LoadText(DateTime fromDate, DateTime toDate)
    {
        if (!File.Exists(TemplateFileName))
            throw new ApplicationException($"{TemplateFileName} not found");

        var text = File.ReadAllText(TemplateFileName);
        return text
            .Replace("{From}", fromDate.ToString("yyyy-MM-dd HH:mm:ss"))
            .Replace("{To}", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}