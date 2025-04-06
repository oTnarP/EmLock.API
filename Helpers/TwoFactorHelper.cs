using OtpNet;
using System;
using System.Web;

namespace EmLock.API.Helpers
{
    public static class TwoFactorHelper
    {
        public static string GenerateSecretKey()
        {
            var secret = KeyGeneration.GenerateRandomKey(20); // 160-bit key
            return Base32Encoding.ToString(secret); // Base32 string
        }

        public static string GenerateProvisioningUri(string email, string secretKey, string issuer = "EmLock")
        {
            var encodedIssuer = HttpUtility.UrlEncode(issuer);
            var encodedEmail = HttpUtility.UrlEncode(email);
            return $"otpauth://totp/{encodedIssuer}:{encodedEmail}?secret={secretKey}&issuer={encodedIssuer}&digits=6";
        }

        public static bool VerifyCode(string secretKey, string userInputCode)
        {
            var bytes = Base32Encoding.ToBytes(secretKey);
            var totp = new Totp(bytes);
            return totp.VerifyTotp(userInputCode, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
        }
    }
}
