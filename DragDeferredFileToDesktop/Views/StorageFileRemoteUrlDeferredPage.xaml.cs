using System;

using DragDeferredFileToDesktop.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Core;

namespace DragDeferredFileToDesktop.Views
{
    public sealed partial class StorageFileRemoteUrlDeferredPage : Page
    {
        private StorageFileRemoteUrlDeferredViewModel ViewModel
        {
            get { return DataContext as StorageFileRemoteUrlDeferredViewModel; }
        }

        public StorageFileRemoteUrlDeferredPage()
        {
            InitializeComponent();
        }

        private void Border_DragStarting(Windows.UI.Xaml.UIElement sender, Windows.UI.Xaml.DragStartingEventArgs args)
        {
            args.Data.SetDataProvider(StandardDataFormats.StorageItems, OnDeferredStorageFileRequestedHandler);
        }

        async void OnDeferredStorageFileRequestedHandler(DataProviderRequest request)
        {
            DataProviderDeferral deferral = request.GetDeferral();
            try
            {
                Task<IEnumerable<StorageFile>> task = null;
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    task = PrepareStorageFiles();
                });
                var result = await task;
                request.SetData(result);
            }

            catch (Exception ex)
            {
                // Handle the exception
                DebugTB.Text = ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace ;
            }

            finally
            {
                deferral.Complete();
                Debug.WriteLine("deferral complete!!!");
            }

        }

        async Task<IEnumerable<StorageFile>> PrepareStorageFiles()
        {
            List<StorageFile> files = new List<StorageFile>();
            var file = await StorageFile.CreateStreamedFileFromUriAsync("SampleImage.jpg", new Uri(directLink.Text), null);
            files.Add(file);
            return files;
        }

    }
}
