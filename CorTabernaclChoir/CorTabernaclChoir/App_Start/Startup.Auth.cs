﻿using System;
using System.IO;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using CorTabernaclChoir.Data;
using Google.Apis.Auth.OAuth2;

namespace CorTabernaclChoir
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });     
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            
            app.ConfigureGoogleAuth();
        }
    }

    public static class AuthExtensionMethods
    {
        public static void ConfigureGoogleAuth(this IAppBuilder app)
        {
            ClientSecrets googleClientSecrets;

            using (var stream = new FileStream(HttpContext.Current.Server.MapPath("~/client_secret.json"), FileMode.Open, FileAccess.Read))
            {
                googleClientSecrets = GoogleClientSecrets.Load(stream).Secrets;
            }

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = googleClientSecrets.ClientId,
                ClientSecret = googleClientSecrets.ClientSecret
            });
        }
    }
}