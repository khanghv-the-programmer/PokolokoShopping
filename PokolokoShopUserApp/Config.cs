// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace PokolokoShopUserApp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource("roles", "Your roles", new List<string>(){"role"}),
                //new IdentityResource("countries", "Your country", new List<string>(){"country"}),
                //new IdentityResource("levels", "Your level", new List<string>(){"level"}),
               
            };

        

        public static IEnumerable<ApiResource> ApiResource =>
            new ApiResource[]
            { 
                 new ApiResource("productapi", "Product API", new List<string>(){ "role"})
            };

        public static IEnumerable<Client> Clients =>
            new Client[] 
            { 
                new Client
                {
                    //IdentityTokenLifetime = //second, default 5 mins
                    //AuthorizationCodeLifetime =
                    AccessTokenLifetime = 60*5, //1 hour default
                    AllowOfflineAccess = true,
                    UpdateAccessTokenClaimsOnRefresh = true, //30days
                    ClientName = "PokolokoShop",
                    ClientId = "pokolokoshop",
                    RequirePkce = true,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:5001/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:5001/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "productapi",
                        //"country",
                        //"level"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
            };
    }
}