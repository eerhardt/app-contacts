using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MyContacts.Constants;
using MyContacts.Extensions;
using MyContacts.Models;
using MyContacts.Services;
using MvvmHelpers.Commands;
using Xamarin.Essentials;
using MyContacts.Shared.Models;
using MyContacts.Utils;
using MyContacts.Interfaces;

namespace MyContacts.ViewModels
{
    public class ContactViewModel : ViewModelBase
    {
        protected ContactViewModel() { }
        protected ContactViewModel(IDataSource<Contact> dataSource) : base (dataSource) { }

        AsyncCommand<string> dialNumberCommand;

        public AsyncCommand<string> DialNumberCommand => 
            dialNumberCommand ??= new AsyncCommand<string>(ExecuteDialNumberCommand);

        async Task ExecuteDialNumberCommand(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return;

            try
            {
                PhoneDialer.Open(number.SanitizePhoneNumber());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Dialogs.Alert(new AlertInfo
                {
                    Title = "Not Supported",
                    Message = "Phone calls are not supported on this device.",
                    Cancel = "OK"
                });
            }

        }

        AsyncCommand<string> messageNumberCommand;

        public AsyncCommand<string> MessageNumberCommand => 
            messageNumberCommand ??= new AsyncCommand<string>(ExecuteMessageNumberCommand);

        async Task ExecuteMessageNumberCommand(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return;

            try
            {
                await Sms.ComposeAsync(new SmsMessage(string.Empty, number.SanitizePhoneNumber()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Dialogs.Alert(new AlertInfo
                {
                    Title = "Not Supported",
                    Message = "Sms is not supported on this device.",
                    Cancel = "OK"
                });
            }
        }

        AsyncCommand<string> emailCommand;

        public AsyncCommand<string> EmailCommand =>
            emailCommand ??= new AsyncCommand<string>(ExecuteEmailCommand);

        async Task ExecuteEmailCommand(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return;

            try
            {
                await Email.ComposeAsync(string.Empty, string.Empty, email);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Dialogs.Alert(new AlertInfo
                {
                    Title = "Not Supported",
                    Message = "Email is not supported on this device.",
                    Cancel = "OK"
                });
            }
        }

        AsyncCommand<Contact> getDirectionsCommand;

        public AsyncCommand<Contact> GetDirectionsCommand =>
            getDirectionsCommand ??= new AsyncCommand<Contact>(ExecuteGetDirectionsCommand);


        async Task ExecuteGetDirectionsCommand(Contact MyContacts)
        {
            try
            {
                await Map.OpenAsync(new Placemark
                {
                    AdminArea = MyContacts.State,
                    Locality = MyContacts.City,
                    PostalCode = MyContacts.PostalCode,
                    Thoroughfare = MyContacts.AddressString
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Dialogs.Alert(new AlertInfo
                {
                    Title = "Not Supported",
                    Message = "Unable to open a map application on the device..",
                    Cancel = "OK"
                });
            }
        }
    }
}
