using System.Security.Cryptography;
using Google.Authenticator;

namespace PerfLoader.Pages;

public partial class AuthGoogle
{
    // [Parameter]
    // public string? Text { get; set; }

    public string Message { get; private set; } = "PageModel in C#";

    public string AccountName { get; set; } = "ragavendra.bn@gmail.com";

    public string Issuer { get; set; } = "Perf Loader";

    public TwoFactorAuthenticator TwoFactorAuthenticator { get; set; } = new TwoFactorAuthenticator();

    public string BarCode { get; set; }

    public string ManualBarCode { get; set; }

    public static string SecurityCode { get; set; } = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

    public string AuthCode { get; set; }

    // protected override void OnInitialized(  
    /// <summary>
    /// Check for any tests already running upon page initialization.
    /// </summary>
    // protected override async Task OnAfterRenderAsync(bool firstRender)
    protected override async Task OnInitializedAsync()
    {
        // if (!firstRender)
        // {
        // return;
        // }
        // SecurityCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

/*
        if(string.IsNullOrEmpty(SecurityCode))
        {
            byte[] key = new byte[6];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(key);

            SecurityCode = Convert.ToBase64String(key);
        }*/

        SetupCode setupInfo = TwoFactorAuthenticator.GenerateSetupCode(Issuer, AccountName, SecurityCode, false, 3);

        BarCode = setupInfo.QrCodeSetupImageUrl;
        ManualBarCode = setupInfo.ManualEntryKey;

        Console.WriteLine($"Generated new code - {SecurityCode}");
    }

    // protected void btnValidate_Click(object sender, EventArgs e)
    public void btnValidate_Click()
    {
        var result = TwoFactorAuthenticator.ValidateTwoFactorPIN(SecurityCode, AuthCode);

        // SetupCode setupInfo = TwoFactorAuthenticator.GenerateSetupCode(Issuer, AccountName, SecurityCode, false, 3); 
        // BarCode = setupInfo.QrCodeSetupImageUrl;
        // ManualBarCode = setupInfo.ManualEntryKey;

        if (result)
        {
            Message += "" + " is a valid PIN at UTC time " + DateTime.UtcNow.ToString();
            // this.lblValidationResult.ForeColor = System.Drawing.Color.Green;
        }
        else
        {
            Message += "" + " is not a valid PIN at UTC time " + DateTime.UtcNow.ToString();
            // this.lblValidationResult.ForeColor = System.Drawing.Color.Red;
        }

        Console.WriteLine(Message);
    }
}