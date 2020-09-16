using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Harmonogram.Helper
{
    class AppSettingsHelper
    {
        Configuration config;

        public AppSettingsHelper()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        //Get connection string from App.Config file
        public string GetConnectionString(string key)
        {
            return config.ConnectionStrings.ConnectionStrings[key].ConnectionString;
        }

        //Save connection string to App.config file
        public void SaveConnectionString(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["HarmonogramDBEntities"].ConnectionString = value;
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");

        }
    }
}
