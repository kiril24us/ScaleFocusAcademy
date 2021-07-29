using Microsoft.Extensions.Configuration;

namespace ToDoApplication
{
    public class ConfigInitializer
    {
        public static IConfigurationRoot InitConfig()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            return builder.Build();
        }

        public static IConfigurationRoot AfterInitConfig()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings_after_initializing.json", true, true);
            return builder.Build();
        }
    }
}
