using System.Net.Mail;
using Newtonsoft.Json;

namespace TootAnalyticsEmailer
{
    internal class Settings
    {
        private const string SettingsFileName = "settings.json";

        public string InstanceUrl { get; set; }
        public string Token { get; set; }
        public string AccountName { get; set; }
        public string SmtpServer { get; set; }
        public ushort SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string EmailSubject { get; set; }
        public string EmailFromAddress { get; set; }
        public string EmailFromDisplayName { get; set; }
        public Dictionary<string, string> ToEmailAddresses { get; set; }

        public Settings()
        {
            InstanceUrl = string.Empty;
            Token = string.Empty;
            AccountName = string.Empty;
            SmtpServer = string.Empty;
            SmtpUsername = string.Empty;
            SmtpPassword = string.Empty;
            EmailSubject = string.Empty;
            EmailFromAddress = string.Empty;
            EmailFromDisplayName = string.Empty;
            ToEmailAddresses = new Dictionary<string, string>();
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
            try
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
                    case "smtpserver":
                        SmtpServer = value;
                        break;
                    case "smtpport":
                        SmtpPort = Convert.ToUInt16(value);
                        break;
                    case "smtpusername":
                        SmtpUsername = value;
                        break;
                    case "smtppassword":
                        SmtpPassword = value;
                        break;
                    case "emailsubject":
                        EmailSubject = value;
                        break;
                    case "emailfromaddress":
                        EmailFromAddress = value;
                        break;
                    case "emailfromdisplayname":
                        EmailFromDisplayName = value;
                        break;
                    case "toemailaddresses":
                        var list = value.Split(";");
                        foreach (var item in list)
                        {
                            var parts = item.Split(',');
                            ToEmailAddresses.Add(parts[0], parts[1]);
                        }

                        break;
                    default:
                        throw new ApplicationException($"Invalid setting: {setting}");
                }
            }
            catch (FormatException)
            {
                throw new ApplicationException("Invalid value for setting");
            }
        }
    }
}