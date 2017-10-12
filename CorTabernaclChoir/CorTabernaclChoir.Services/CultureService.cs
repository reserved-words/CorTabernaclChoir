using System.Globalization;
using System.Threading;
using CorTabernaclChoir.Common.Services;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir.Services
{
    public class CultureService : ICultureService
    {
        public bool IsCurrentCultureWelsh()
        {
            return Thread.CurrentThread.CurrentCulture.Name == CultureWelsh;
        }

        public string ToggleCulture(string current = null)
        {
            var newCulture = current == null
                ? (!IsCurrentCultureWelsh() ? CultureWelsh : CultureEnglish)
                : (current == LanguageWelsh ? CultureEnglish : CultureWelsh);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(newCulture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(newCulture);

            return IsCurrentCultureWelsh() ? LanguageWelsh : LanguageEnglish;
        }

        public void ValidateCulture(string culture)
        {
            if (culture == LanguageWelsh && !IsCurrentCultureWelsh() || culture != LanguageWelsh && IsCurrentCultureWelsh())
            {
                ToggleCulture();
            }
        }
    }
}
