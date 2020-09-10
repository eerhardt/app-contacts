using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyContacts.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyContacts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell, IStartupPage
    {
        public AppShell()
        {
            InitializeComponent();
        }

        public Page Page => this;

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);
            ((this.CurrentItem.CurrentItem as IShellSectionController)
                .PresentedPage?.BindingContext as ViewModelBase)
                ?.OnNavigatedTo();
        }

    }
}