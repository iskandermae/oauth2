// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace Alex.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            { };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {new Client{
                    ClientName = "YesNo Game",
                    ClientId = SettingsCore.Settings.Site_ClientId,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string> {
                        $"{SettingsCore.Settings.Site_applicationUrl}/signin-oidc"
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                    RequirePkce = false,
                    ClientSecrets = {new Secret(SettingsCore.Settings.Site_secret.Sha256())},
                    RequireConsent = true  //todo: delete
                }

            };
    }
}