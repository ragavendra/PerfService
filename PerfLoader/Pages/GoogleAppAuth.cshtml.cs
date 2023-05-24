using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Authenticator;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;

namespace PerfLoader.Pages
{
    public class GoogleAppAuth : PageModel
    {
        private readonly ILogger<GoogleAppAuth> _logger;

        public string QRImageUrl;
        public string Key;
        public string ManualEntryKey;

        public string Issuer = "Raga & You";

        public string User = "ragavendra.bn@gmail.com";

        public GoogleAppAuth(ILogger<GoogleAppAuth> logger)
        {
            _logger = logger;

        }

        public void OnGet()
        {
            byte[] key = new byte[6];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(key);
            Key = Convert.ToBase64String(key);

            // User = "ragavendra.bn@gmail.com";
            // user = "user@example.com";
            // Issuer = "Raga & You";

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            SetupCode setupInfo = tfa.GenerateSetupCode(Issuer, User, Key, false, 3);
            QRImageUrl = setupInfo.QrCodeSetupImageUrl;
            ManualEntryKey = setupInfo.ManualEntryKey;
        }

        public void OnPost()
        {
            Key = Request.Form["Key"];
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            bool result = tfa.ValidateTwoFactorPIN(Key, Request.Form["txtCode"]);

            SetupCode setupInfo = tfa.GenerateSetupCode(Issuer, User, Key, false, 3);
            QRImageUrl = setupInfo.QrCodeSetupImageUrl;
            ManualEntryKey = setupInfo.ManualEntryKey;

            TempData["ValidationResult"] = $"{Request.Form["txtCode"]} is {(result ? "" : "not ")}a valid PIN at UTC time {DateTime.UtcNow.ToString()}";
        }
        
    }
}