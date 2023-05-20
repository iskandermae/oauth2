// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServerHost.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users =
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alex",
                    Password = "alex",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Oleksandr"),
                        new Claim("given_name", "Oleksandr")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "olena",
                    Password = "olena",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Olena"),
                        new Claim("given_name", "Olena")
                    }
                }
            };
    }
}