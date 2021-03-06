﻿using System;
using System.Threading.Tasks;
using MyContacts.Constants;
using MyContacts.Models;
using MyContacts.Services;
using MyContacts.Views;
using MvvmHelpers.Commands;
using Command = Xamarin.Forms.Command;
using MyContacts.Shared.Models;
using MyContacts.Utils;

namespace MyContacts.ViewModels
{
    public class DetailViewModel : ContactViewModel
    {
        public DetailViewModel()
        {

        }
        public DetailViewModel(Contact contact)
        {
            Contact = contact;
        }

        public Contact Contact { get; private set; }

        public bool HasEmailAddress => !string.IsNullOrWhiteSpace(Contact?.Email);

        public bool HasPhoneNumber => !string.IsNullOrWhiteSpace(Contact?.Phone);

        public bool HasAddress => !string.IsNullOrWhiteSpace(Contact?.AddressString);


        AsyncCommand editCommand;

        public AsyncCommand EditCommand =>
            editCommand ??= new AsyncCommand(ExecuteEditCommand);

        Task ExecuteEditCommand() => PushAsync(new EditPage(Contact));

        AsyncCommand deleteCommand;

        public AsyncCommand DeleteCommand => deleteCommand ?? (deleteCommand = new AsyncCommand(ExecuteDeleteCommand));

        async Task ExecuteDeleteCommand()
        {
            await Dialogs.Question(new QuestionInfo
            {
                Title = string.Format("Delete {0}?", Contact.DisplayName),
                Question = null,
                Positive = "Delete",
                Negative = "Cancel",
                OnCompleted = new Action<bool>(async result =>
                {
                    if (!result) 
                        return;

                    await DataSource.RemoveItem(Contact);

                    await PopAsync();
                })
            });
        }

        public async Task DisplayGeocodingError()
        {
            /*await Dialogs.Alert(new AlertInfo
            {
                Title = "Geocoding Error",
                Message = "Please make sure the address is valid, or that you have a network connection.",
                Cancel = "OK"
            });*/

            IsBusy = false;
        }
    }
}

