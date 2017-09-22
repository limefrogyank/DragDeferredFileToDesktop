using System;

using DragDeferredFileToDesktop.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.Storage;
using System.Diagnostics;
using Windows.UI.Core;

namespace DragDeferredFileToDesktop.Views
{
    public sealed partial class StorageFileHackPage : Page
    {
        private StorageFileHackViewModel ViewModel
        {
            get { return DataContext as StorageFileHackViewModel; }
        }

        public StorageFileHackPage()
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
                DebugTB.Text = ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace;
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

            //HACK HERE:  forces download of file.  Side-effect: It locks the Desktop until download is complete.
            //  Also, if drop is cancelled (i.e. dropped onto bad target), download has wasted bandwidth.
            var props = await file.GetBasicPropertiesAsync();
            
            files.Add(file);
            return files;
        }

    }
}
