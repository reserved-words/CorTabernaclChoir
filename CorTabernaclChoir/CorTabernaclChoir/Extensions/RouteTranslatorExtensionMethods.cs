using System.Linq;
using System.Reflection;
using CorTabernaclChoir.Attributes;
using RouteLocalization.Mvc;

namespace CorTabernaclChoir.Extensions
{
    public static class RouteTranslatorExtensionMethods
    {
        private static string GetTranslation(MethodInfo method)
        {
            return method
                .GetCustomAttributes(typeof(WelshRouteAttribute), false)
                .OfType<WelshRouteAttribute>()
                .SingleOrDefault()?.Route;
        }

        public static RouteTranslator<T> AddWelshTranslation<T>(this RouteTranslator<T> translator)
        {
            return translator.AddTranslation(GetTranslation(typeof(T).GetMethod(translator.Action)));
        }
    }
}