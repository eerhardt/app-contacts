using System;
using System.Linq;
using MyContacts.Interfaces;
using MyContacts.Models;
using UIKit;
using Xamarin.Forms;

namespace MyContacts.iOS.Helpers
{
    public class Environment : IEnvironment
    {
        public void SetStatusBarColor(System.Drawing.Color color, bool darkStatusBarTint)
        {
        }
    }
}