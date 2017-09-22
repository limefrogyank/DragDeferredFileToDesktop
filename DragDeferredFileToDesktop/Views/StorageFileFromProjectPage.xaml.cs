using System;

using DragDeferredFileToDesktop.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;

namespace DragDeferredFileToDesktop.Views
{
    public sealed partial class StorageFileFromProjectPage : Page
    {
        private StorageFile storageFile;

        private StorageFileFromProjectViewModel ViewModel
        {
            get { return DataContext as StorageFileFromProjectViewModel; }
        }

        public StorageFileFromProjectPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/StoreLogo.png"));
        }

        private async void Border_DragStarting(Windows.UI.Xaml.UIElement sender, Windows.UI.Xaml.DragStartingEventArgs args)
        {
            args.Data.SetDataProvider(StandardDataFormats.StorageItems, OnDeferredStorageFileRequestedHandler);
        }

        void OnDeferredStorageFileRequestedHandler(DataProviderRequest request)
        {
            DataProviderDeferral deferral = request.GetDeferral();
            try
            {                
                request.SetData(new[] { storageFile });
            }

            catch (Exception ex)
            {
                // Handle the exception
            }

            finally
            {
                deferral.Complete();
                Debug.WriteLine("deferral complete!!!");
            }

        }


    }
}
