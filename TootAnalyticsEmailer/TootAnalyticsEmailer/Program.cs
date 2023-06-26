﻿namespace TootAnalyticsEmailer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var settings = Settings.Load();
            if (args.Length == 3 && args[0].ToLower() == "--set")
            {
                settings.SetValueFromArguments(args[1], args[2]);
                settings.Save();
                return;
            }
        }
    }
}