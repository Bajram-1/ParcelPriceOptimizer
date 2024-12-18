﻿using ParcelPriceOptimizer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface ITokenService
    {
        string GenerateJwtToken(ApplicationUser user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
