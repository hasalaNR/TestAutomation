using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestAutomationLeaveMgt.Utilities
{
        public static class ConfigReader
        {

        private static readonly string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "config.json");

        public static string? GetUrl()
        {
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"Configuration file not found at path: {configPath}");
            }

            string configContent = File.ReadAllText(configPath);
            JObject config = JObject.Parse(configContent);
            return config["system_url"]?.ToString();
        }

        public static string GetConfigValue(string key)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuration", "config.json");
            var json = File.ReadAllText(configPath);
            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (!config.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Key '{key}' not found in config. Please check your config file.");
            }

            return config[key];
        }


    }

}
