using PersonsMVC.Interfaces;

namespace PersonsMVC.Tools
{
    public class AppSetting : IDBSettings
    {
        private readonly IConfiguration _configuration;

        public AppSetting(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string DBConnection => _configuration["Settings:DBConnection"];
        public string AppDir => _configuration["Settings:AppDir"];

    }
}
