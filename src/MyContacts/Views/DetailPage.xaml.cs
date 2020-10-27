using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using System;
using MyContacts.ViewModels;
using Xamarin.Essentials;
using System.Linq;
using MyContacts.Models;
using MyContacts.Shared.Models;
using MyContacts.Interfaces;

namespace MyContacts.Views
{
    public partial class DetailPage : ContentPage
    {
        protected DetailViewModel ViewModel => BindingContext as DetailViewModel;

        public DetailPage(DetailViewModel detailViewModel)
        {
            InitializeComponent();
            BindingContext = detailViewModel;
        }

        public string ContactId
        {
            get;
            set;
        }

        //public DetailPage(Contact MyContacts)
        //{
        //    InitializeComponent();
        //    BindingContext = new DetailViewModel (MyContacts);
        //}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await SetupMap();
        }

        /// <summary>
        /// Sets up the map.
        /// </summary>
        /// <returns>A Task.</returns>
        async Task SetupMap()
        {
            if (ViewModel.HasAddress)
            {
                MyContactsMap.IsVisible = false;

                // set to a default position
                Location position;

                try
                {
                    position = (await Geocoding.GetLocationsAsync(ViewModel.Contact.AddressString)).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    ViewModel.DisplayGeocodingError();
                    return;
                }

                // if lat and lon are both 0, then it's assumed that position acquisition failed
                if (position == null || (position.Latitude == 0 && position.Longitude == 0))
                {
                    ViewModel.DisplayGeocodingError();
                    return;
                }

                var xfpos = new Position(position.Latitude, position.Longitude);

                if (DeviceInfo.Platform != DevicePlatform.UWP)
                {
                    var pin = new Pin()
                    {
                        Type = PinType.Place,
                        Position = xfpos,
                        Label = ViewModel.Contact.DisplayName,
                        Address = ViewModel.Contact.AddressString
                    };

                    MyContactsMap.Pins.Clear();

                    MyContactsMap.Pins.Add(pin);
                }

                MyContactsMap.MoveToRegion(MapSpan.FromCenterAndRadius(xfpos, Distance.FromMiles(10)));

                MyContactsMap.IsVisible = true;
            }
        }
    }
}

