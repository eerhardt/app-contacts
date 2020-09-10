using MvvmHelpers;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using MyContacts.Interfaces;
using MyContacts.Models;
using MyContacts.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MyContacts.ViewModels
{
    public abstract class ViewModelBase : BaseViewModel, INavigation
    {
        IDataSource<Contact> dataSource;
        public IDataSource<Contact> DataSource =>
            dataSource ??= App.Current.Services.GetService<IDataSource<Contact>>();

        protected ViewModelBase()
        {
        }

        protected ViewModelBase(IDataSource<Contact> dataSource)
        {
            this.dataSource = dataSource;
        }

        INavigation Navigation => Application.Current?.MainPage?.Navigation;

        public virtual void OnNavigatedTo()
        {          
        }

        #region INavigation implementation

        public void RemovePage(Page page)
		{
			Navigation?.RemovePage(page);
		}

		public void InsertPageBefore(Page page, Page before)
		{
			Navigation?.InsertPageBefore(page, before);
		}

		public async Task PushAsync(Page page)
		{
			var task = Navigation?.PushAsync(page);
			if (task != null)
				await task;
		}

		public async Task<Page> PopAsync()
		{
			var task = Navigation?.PopAsync();
			return task != null ? await task : await Task.FromResult(null as Page);
		}

		public async Task PopToRootAsync()
		{
			var task = Navigation?.PopToRootAsync();
			if (task != null)
				await task;
		}

		public async Task PushModalAsync(Page page)
		{
			var task = Navigation?.PushModalAsync(page);
			if (task != null)
				await task;
		}

		public async Task<Page> PopModalAsync()
		{
			var task = Navigation?.PopModalAsync();
			return task != null ? await task : await Task.FromResult(null as Page);
		}

		public async Task PushAsync(Page page, bool animated)
		{
			var task = Navigation?.PushAsync(page, animated);
			if (task != null)
				await task;
		}

		public async Task<Page> PopAsync(bool animated)
		{
			var task = Navigation?.PopAsync(animated);
			return task != null ? await task : await Task.FromResult(null as Page);
		}

		public async Task PopToRootAsync(bool animated)
		{
			var task = Navigation?.PopToRootAsync(animated);
			if (task != null)
				await task;
		}

		public async Task PushModalAsync(Page page, bool animated)
		{
			var task = Navigation?.PushModalAsync(page, animated);
			if (task != null)
				await task;
		}

		public async Task<Page> PopModalAsync(bool animated)
		{
			var task = Navigation?.PopModalAsync(animated);
			return task != null ? await task : await Task.FromResult(null as Page);
		}

		public IReadOnlyList<Page> NavigationStack => Navigation?.NavigationStack;

	    public IReadOnlyList<Page> ModalStack => Navigation?.ModalStack;

	    #endregion
	}
}

