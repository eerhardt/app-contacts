using System;
using System.Threading.Tasks;
using MyContacts.Util;
using MyContacts.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using MyContacts.Shared.Models;
using MyContacts.Interfaces;

namespace MyContacts.ViewModels
{
    public class ListViewModel : ContactViewModel
    {
        public DateTime LastUpdate { get; set; }
        public ListViewModel(IDataSource<Contact> dataSource) : base(dataSource)
        {
        }


        public ObservableRangeCollection<Contact> Contacts { get; } = new ObservableRangeCollection<Contact>();

        AsyncCommand loadCommand;
        public AsyncCommand LoadCommand => loadCommand ??=
            new AsyncCommand(ExecuteLoadCommand);

        public async Task ExecuteLoadCommand()
        { 
            if (Contacts.Count < 1 || LastUpdate < Settings.LastUpdate)
                await FetchContacts();
        }

        AsyncCommand refreshCommand;
        public AsyncCommand RefreshCommand => refreshCommand ??=
            new AsyncCommand(ExecuteRefreshCommand);

        async Task ExecuteRefreshCommand()
        {
            await FetchContacts();
        }

        async Task FetchContacts()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            await Task.Delay(1000);
            var items = await DataSource.GetItems();

            Contacts.ReplaceRange(items);

            LastUpdate = DateTime.UtcNow;

            IsBusy = false;
        }

        AsyncCommand newCommand;
        public AsyncCommand NewCommand => newCommand ??=
            new AsyncCommand(ExecuteNewCommand);
        Task ExecuteNewCommand() => PushAsync(new EditPage());

        AsyncCommand showSettingsCommand;
        public AsyncCommand ShowSettingsCommand => showSettingsCommand ??=
            new AsyncCommand(ExecuteShowSettingsCommand);

        Task ExecuteShowSettingsCommand() => PushModalAsync(new SettingsPage());
    }
}

