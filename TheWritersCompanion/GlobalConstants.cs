using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWritersCompanion
{
    /// <summary>
    /// Singleton data object for storing project-specific global constants
    /// </summary>
    public class GlobalConstants
    {
        // extension used for all Note files
        public static readonly string FILE_EXTENSION = ".txt";

        // name of config file used to identify a directory as a valid project
        public static readonly string NAME_OF_CONFIG_FILE = "TWCconfig";

        // content that each config file should contain
        public static readonly string CONTENT_OF_CONFIG_FILE = "DO NOT DELETE THIS FILE - " +
            "It is necessary to identify projects of The Writers Companion";
    }
}
