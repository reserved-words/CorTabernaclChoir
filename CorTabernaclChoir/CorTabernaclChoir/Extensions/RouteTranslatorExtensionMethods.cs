using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CorTabernaclChoir.Attributes;
using RouteLocalization.Mvc;
using WebGrease.Css.Extensions;

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

        public static RouteTranslator<T> AddTranslation<T>(this RouteTranslator<T> translator, params Expression<Func<T, object>>[] expressions)
        {
            expressions.ForEach(e =>
            {
                translator = translator
                    .ForAction(e)
                    .AddTranslation(GetTranslation(typeof(T).GetMethod(translator.Action)));
            });

            return translator;
        }
    }
}