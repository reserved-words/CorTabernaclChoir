using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using CorTabernaclChoir.Controllers;
using CorTabernaclChoir.Extensions;
using RouteLocalization.Mvc;
using RouteLocalization.Mvc.Setup;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes(Localization.LocalizationDirectRouteProvider);
            
            ISet<string> acceptedCultures = new HashSet<string> { LanguageEnglish, LanguageWelsh };

            var configuration = new Configuration
            {
                DefaultCulture = DefaultLanguage,
                AcceptedCultures = acceptedCultures,
                AttributeRouteProcessing = AttributeRouteProcessing.AddAsNeutralAndDefaultCultureRoute,
                AddCultureAsRoutePrefix = true
            };

            var localization = new Localization(configuration);

            localization.TranslateInitialAttributeRoutes();

            localization.ForCulture(LanguageWelsh)
                .ForController<HomeController>().AddTranslation(x => x.Index(""))
                .ForController<LayoutController>().AddTranslation(x => x.Sidebar(""), x => x.Banner())
                .ForController<AboutController>().AddTranslation(x => x.Index(""))
                .ForController<GalleryController>().AddTranslation(x => x.Index(""))
                .ForController<JoinController>().AddTranslation(x => x.Index(""))
                .ForController<RecordingsController>().AddTranslation(x => x.Index(""))
                .ForController<WorksController>().AddTranslation(x => x.Index(""))
                .ForController<NewsController>().AddTranslation(x => x.Index("", 0), x => x.Item("", 0))
                .ForController<EventsController>().AddTranslation(x => x.Index(""), x => x.Item("", 0))
                .ForController<VisitsController>().AddTranslation(x => x.Index("", 0), x => x.Item("", 0));
        }
    }
}
