using MyContacts.Util;
using MyContacts.Interfaces;
using MyContacts.Models;
using Xamarin.Forms;
using Xamarin.Essentials;
using Microsoft.Extensions.DependencyInjection;

namespace MyContacts.Styles
{
    public static class ThemeHelper
    {
        public static Theme CurrentTheme = Settings.ThemeOption;

        public static void ChangeTheme(Theme theme, bool forceTheme = false)
        {
            // don't change to the same theme
            if (theme == CurrentTheme && !forceTheme)
                return;

            //// clear all the resources
            var applicationResourceDictionary = Application.Current.Resources;

            if (theme == Theme.Default)
              theme = AppInfo.RequestedTheme == AppTheme.Dark ? Theme.Dark : Theme.Light;

#pragma warning disable IDE0007 // Use implicit type
            ResourceDictionary newTheme = theme switch
#pragma warning restore IDE0007 // Use implicit type
            {
                Theme.Light => new LightTheme(),
                Theme.Dark => new DarkTheme(),
                _ => new LightTheme(),
            };
            ManuallyCopyThemes(newTheme, applicationResourceDictionary);

            CurrentTheme = theme;

            
            var background = (Color)App.Current.Resources["PrimaryDarkColor"];
            var environment = App.Current.Services.GetService<IEnvironment>();
            environment?.SetStatusBarColor(ColorConverters.FromHex(background.ToHex()), false);
        }

        static void ManuallyCopyThemes(ResourceDictionary fromResource, ResourceDictionary toResource)
        {
            foreach (var item in fromResource.Keys)
            {
                toResource[item] = fromResource[item];
            }
        }
    }
}
