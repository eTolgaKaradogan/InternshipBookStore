using System;
using _01_AppCore.Business.Utils.Bases;
using Microsoft.Extensions.Configuration;

namespace _01_AppCore.Business.Utils
{
    public class AppSettingsUtil : AppSettingsUtilBase
    {
        public AppSettingsUtil(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
