using Newtonsoft.Json;

namespace TootAnalyticsEmailer
{
    internal class Settings
    {
        private const string SettingsFileName = "settings.json";

        public string InstanceUrl { get; set; }
        public string Token { get; set; }
        public string AccountName { get; set; }

        public Settings()
        {
            InstanceUrl = string.Empty;
            Token = string.Empty;
            AccountName = string.Empty;
        }

        public static Settings Load()
        {
            if (!File.Exists(SettingsFileName))
                return new Settings();

            var text = File.ReadAllText(SettingsFileName);
            return JsonConvert.DeserializeObject<Settings>(text) ??
                   throw new ApplicationException($"Your '{SettingsFileName}' appears to be empty or corrupt.");
        }

        public void Save()
        {
            var serialised = JsonConvert.SerializeObject(this);
            File.WriteAllText(SettingsFileName, serialised);
        }

        public void SetValueFromArguments(string setting, string value)
        {
            switch (setting.ToLower())
            {
                case "instanceurl":
                    InstanceUrl = value;
                    break;
                case "token":
                    Token = value;
                    break;
                case "accountname":
                    AccountName = value;
                    break;
                default:
                    throw new ApplicationException($"Invalid setting: {setting}");
            }
        }
    }
}