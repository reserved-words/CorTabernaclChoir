using System.Globalization;
using System.Threading;
using CorTabernaclChoir.Common.Services;

namespace CorTabernaclChoir.Services
{
    public class CultureService : ICultureService
    {
        private const string WelshCulture = "cy-GB";
        private const string EnglishCulture = "en-GB";
        private const string WelshLanguage = "cy";
        private const string EnglishLanguage = "en";

        public bool IsCurrentCultureWelsh()
        {
            return Thread.CurrentThread.CurrentCulture.Name == WelshCulture;
        }

        public string ToggleCulture(string current = null)
        {
            var newCulture = current == null
                ? (!IsCurrentCultureWelsh() ? WelshCulture : EnglishCulture)
                : (current == WelshLanguage ? EnglishCulture : WelshCulture);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(newCulture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(newCulture);

            return IsCurrentCultureWelsh() ? WelshLanguage : EnglishLanguage;
        }

        public void ValidateCulture(string culture)
        {
            if (culture == WelshLanguage && !IsCurrentCultureWelsh() || culture != WelshLanguage && IsCurrentCultureWelsh())
            {
                ToggleCulture();
            }
        }
    }
}
