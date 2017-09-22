using System;

using DragDeferredFileToDesktop.Services;
using DragDeferredFileToDesktop.Views;

using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

namespace DragDeferredFileToDesktop.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<ShellViewModel>();
            Register<StorageFileFromProjectViewModel, StorageFileFromProjectPage>();
            Register<StorageFileRemoteUrlDeferredViewModel, StorageFileRemoteUrlDeferredPage>();
            Register<StorageFileHackViewModel, StorageFileHackPage>();
        }

        public StorageFileHackViewModel StorageFileHackViewModel => ServiceLocator.Current.GetInstance<StorageFileHackViewModel>();

        public StorageFileRemoteUrlDeferredViewModel StorageFileRemoteUrlDeferredViewModel => ServiceLocator.Current.GetInstance<StorageFileRemoteUrlDeferredViewModel>();

        public StorageFileFromProjectViewModel StorageFileFromProjectViewModel => ServiceLocator.Current.GetInstance<StorageFileFromProjectViewModel>();

        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
