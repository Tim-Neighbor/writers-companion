//Ty Larson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Ty Larson
/// </summary>
namespace TheWritersCompanion
{
    interface IAuthorizer
    {
        bool Login(string username, string password);
    }
}
