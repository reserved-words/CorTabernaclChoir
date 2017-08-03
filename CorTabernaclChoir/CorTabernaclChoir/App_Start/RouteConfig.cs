using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using CorTabernaclChoir.Controllers;
using CorTabernaclChoir.Extensions;
using RouteLocalization.Mvc;
using RouteLocalization.Mvc.Setup;

namespace CorTabernaclChoir
{
    public class RouteConfig
    {
        private const string EnglishLanguage = "en";
        private const string WelshLanguage = "cy";

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes(Localization.LocalizationDirectRouteProvider);
            
            ISet<string> acceptedCultures = new HashSet<string> { EnglishLanguage, WelshLanguage };

            var configuration = new Configuration
            {
                DefaultCulture = EnglishLanguage,
                AcceptedCultures = acceptedCultures,
                AttributeRouteProcessing = AttributeRouteProcessing.AddAsNeutralAndDefaultCultureRoute,
                AddCultureAsRoutePrefix = true
            };

            var localization = new Localization(configuration);

            localization.TranslateInitialAttributeRoutes();

            localization.ForCulture(WelshLanguage)
                .ForController<HomeController>()
                .ForAction(x => x.Index(""))
                .AddWelshTranslation()
                .ForController<HomeController>()
                .ForAction(x => x.ToggleLanguage(""))
                .AddWelshTranslation()
                .ForController<LayoutController>()
                .ForAction(x => x.Sidebar(""))
                .AddWelshTranslation()
                .ForController<LayoutController>()
                .ForAction(x => x.Banner())
                .AddWelshTranslation()
                .ForController<AboutController>()
                .ForAction(x => x.Index(""))
                .AddWelshTranslation()
                .ForController<GalleryController>()
                .ForAction(x => x.Index(""))
                .AddWelshTranslation()
                .ForController<JoinController>()
                .ForAction(x => x.Index(""))
                .AddWelshTranslation()
                .ForController<RecordingsController>()
                .ForAction(x => x.Index(""))
                .AddWelshTranslation()
                .ForController<WorksController>()
                .ForAction(x => x.Index(""))
                .AddWelshTranslation()
                .ForController<NewsController>()
                .ForAction(x => x.Index("",0))
                .AddWelshTranslation()
                .ForController<NewsController>()
                .ForAction(x => x.Item("", 0))
                .AddWelshTranslation()
                .ForController<EventsController>()
                .ForAction(x => x.Index("", 0))
                .AddWelshTranslation()
                .ForController<EventsController>()
                .ForAction(x => x.Item("", 0))
                .AddWelshTranslation()
                .ForController<VisitsController>()
                .ForAction(x => x.Index("",0))
                .AddWelshTranslation();
        }
    }
}
