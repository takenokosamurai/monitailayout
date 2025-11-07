using System;
using System.Windows.Controls;

namespace monitailayout.Services
{
    public class NavigationService
    {
        private static NavigationService _instance;
        private Frame _mainFrame;

        public static NavigationService Instance => _instance ?? (_instance = new NavigationService());

        public void Initialize(Frame frame)
        {
            _mainFrame = frame;
        }

        public void NavigateTo(Type pageType)
        {
            if (_mainFrame != null)
            {
                var page = Activator.CreateInstance(pageType);
                _mainFrame.Navigate(page);
            }
        }

        public void NavigateTo<T>() where T : Page, new()
        {
            if (_mainFrame != null)
            {
                _mainFrame.Navigate(new T());
            }
        }

        public bool CanGoBack => _mainFrame?.CanGoBack ?? false;

        public void GoBack()
        {
            if (_mainFrame?.CanGoBack == true)
            {
                _mainFrame.GoBack();
            }
        }
    }
}