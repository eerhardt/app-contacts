using System;
using Xamarin.Forms;
using MyContacts.Util;
using MyContacts.Models;
using MyContacts.ViewModels;
using MyContacts.Shared.Models;

namespace MyContacts.Views
{
	public partial class ListPage : ContentPage
	{
		protected ListViewModel ViewModel => BindingContext as ListViewModel;

		public ListPage(ListViewModel viewModel)
		{
			InitializeComponent();
            BindingContext = viewModel;
		}

        async void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is Contact a))
                return;

            await Shell.Current.GoToAsync($"{nameof(DetailPage)}?ContactId={a.Id}");
            //await Navigation.PushAsync(new DetailPage(a));

            ((ListView)sender).SelectedItem = null;
        }
        void ItemTapped(object sender, ItemTappedEventArgs e) =>
            ((ListView)sender).SelectedItem = null;


		protected override async void OnAppearing()
		{
			base.OnAppearing();

            //Update the list if needed. View Model handles this logic.
            await ViewModel.ExecuteLoadCommand();		
		}

       
    }
}

