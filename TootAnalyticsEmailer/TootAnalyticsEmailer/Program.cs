﻿namespace TootAnalyticsEmailer
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
                using var zip = new ZipFileCreator("Statuses");
                var zipFileName = zip.Create(csv);
                var emailText = EmailTemplate.LoadText(fromDate, toDate);
                var emailer = new ZipFileEmailer(settings);
                emailer.Send(emailText, zipFileName);
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

        private static bool VerifyArguments(IReadOnlyList<string> args, out DateTime fromDate, out DateTime toDate)
        {
            fromDate = default;
            toDate = default;

            var result = args.Count == 4 &&
                         args[0].ToLower() == "--from" &&
                         DateTime.TryParse(args[1], out fromDate) &&
                         args[2].ToLower() == "--to" &&
                         DateTime.TryParse(args[3], out toDate);

            if (result)
                toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);

            return result;
        }
    }
}