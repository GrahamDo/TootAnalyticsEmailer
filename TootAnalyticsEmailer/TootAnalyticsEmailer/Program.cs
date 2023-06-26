namespace TootAnalyticsEmailer
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                var settings = Settings.Load();
                if (args.Length == 3 && args[0].ToLower() == "--set")
                {
                    settings.SetValueFromArguments(args[1], args[2]);
                    settings.Save();
                    return;
                }

                if (!VerifyArguments(args, out var fromDate, out var toDate))
                {
                    throw new ApplicationException("Invalid arguments: required --from <date> --to <date>");
                }

                var apiClient = new MastodonApiClient();
                var getter = new MastodonStatusGetter(settings, apiClient);
                var statuses = await getter.GetStatuses(fromDate, toDate);
                var csvGenerator = new CsvGenerator();
                var csv = csvGenerator.GenerateFromStatuses(statuses);
                var emailTemplate = EmailTemplate.Load();
                var emailer = new CsvEmailer(settings, emailTemplate);
                emailer.Send(csv);
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled Exception: {ex}");
            }
        }

        private static bool VerifyArguments(string[] args, out DateTime fromDate, out DateTime toDate)
        {
            fromDate = default;
            toDate = default;

            var result = args.Length == 4 &&
                args[0].ToLower() == "--from" &&
                DateTime.TryParse(args[1], out fromDate) &&
                args[2].ToLower() == "--to" &&
                DateTime.TryParse(args[3], out toDate);

            return result;
        }
    }
}