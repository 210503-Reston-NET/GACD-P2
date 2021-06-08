﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GACDRest
{
    public class CheckScopeAuth : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }
        public CheckScopeAuth(string scope, string issuer)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }
}
