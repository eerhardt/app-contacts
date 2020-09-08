using Foundation;
using Microsoft.Extensions.DependencyInjection;
using MyContacts.Interfaces;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace MyContacts.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        public AppDelegate()
        {

        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            new FormsBuilder()
                .UseForms()
                .ConfigureServices((context, services) =>
                {
                    App.ConfigureServices(services);

                    services.AddSingleton<IEnvironment, Helpers.Environment>();
                })
                .UseApplication<App>(a => LoadApplication(a))
                .Init();

            return base.FinishedLaunching(app, options);
        }  
    }
}

