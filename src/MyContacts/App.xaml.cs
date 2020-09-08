using System;
using Microsoft.Extensions.DependencyInjection;
using MyContacts.Interfaces;
using MyContacts.Services;
using MyContacts.Shared.Models;
using MyContacts.Styles;
using MyContacts.Util;
using MyContacts.ViewModels;
using MyContacts.Views;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]
namespace MyContacts
{
    public partial class App : Application
    {
        // These 2 properties won't be needed when IServiceProvider is on the base Application class
        public IServiceProvider Services { get;  }
        public new static App Current => (App)Application.Current;

        public static bool UseLocalDataSource = true;
        public App(IServiceProvider services)
        {
            InitializeComponent();

            Services = services;

            var navPage = new NavigationPage(Services.GetService<ListPage>())
            {
                BarTextColor = Color.White
            };

            navPage.SetDynamicResource(NavigationPage.BarBackgroundColorProperty, "PrimaryColor");

            // set the MainPage of the app to the navPage
            MainPage = navPage;

        }

        protected override void OnStart()
        {
            base.OnStart();
            ThemeHelper.ChangeTheme(Settings.ThemeOption, true);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ThemeHelper.ChangeTheme(Settings.ThemeOption, true);
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            if (UseLocalDataSource)
                services.AddSingleton<IDataSource<Contact>, FileDataSource>();
            else
                services.AddSingleton<IDataSource<Contact>, AzureDataStore>();

            services.AddTransient<ListViewModel>();
            services.AddTransient<ListPage>();
        }
    }
}

