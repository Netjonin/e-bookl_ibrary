using E_BOOKLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Services
{
    public interface IJWTService
    {
        string GenerateToken(AppUser user, List<string> userRoles, IList<Claim> claims);
    }
}
